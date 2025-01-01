﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using NLog;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Common.Input;
using OpenToolkit.Windowing.Desktop;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using ReMarkable.NET.Util;
using RmEmulator.Framebuffer;
using RmEmulator.Shader;
using SixLabors.ImageSharp;

namespace RmEmulator
{
    public class EmulatorWindow : GameWindow
    {
        private static readonly DebugProc DebugCallback = OnGlMessage;

        private static Logger _logger;
        private static Logger _glLogger;

        public int ScreenVao { get; set; }
        public ShaderProgram ShaderScreen { get; set; }
        public int ScreenTexture { get; set; }

        private readonly Rgb24TextureEncoder _textureEncoder = new Rgb24TextureEncoder();

        private readonly Queue<RefreshTask> _refreshQueue = new Queue<RefreshTask>();
        private readonly Queue<ImageUploadTask> _imageUploadQueue = new Queue<ImageUploadTask>();

        private Thread _appThread;

        private string _assemblyFileName;

        public EmulatorWindow(string assemblyName) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            _assemblyFileName = assemblyName;
            Thread.CurrentThread.Name = "EmuWindow";

            Load += WindowLoad;
            Resize += WindowResize;

            RenderFrame += WindowRender;
            UpdateFrame += WindowUpdate;

            Closing += WindowClosing;

            WindowBorder = WindowBorder.Fixed;

            _logger = Lumberjack.CreateLogger("RmEmulator");
            _glLogger = Lumberjack.CreateLogger("OpenGL");
        }

        private void WindowLoad()
        {
            _logger.Info("Setting up OpenGL");

            // Set up caps
            GL.Enable(EnableCap.RescaleNormal);
            GL.Enable(EnableCap.DebugOutput);
            GL.DebugMessageCallback(DebugCallback, IntPtr.Zero);
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            GL.PixelStore(PixelStoreParameter.PackAlignment, 1);

            // Set background color
            GL.ClearColor(1, 1, 1, 1);

            ShaderScreen = new ShaderProgram(
                "#version 330 core\nout vec4 FragColor;\nin vec2 TexCoords;\nuniform sampler2D img;\nvoid main(){FragColor=vec4(texture(img,vec2(TexCoords.x,1-TexCoords.y)).rgb,1.0);}",
                "#version 330 core\nlayout (location=0) in vec2 aPos;\nlayout (location=1) in vec2 aTexCoords;\nout vec2 TexCoords;\nvoid main()\n{\ngl_Position=vec4(aPos.x,aPos.y,0.0,1.0);\nTexCoords=aTexCoords;\n} "
                );
            ShaderScreen.Uniforms.SetValue("img", 0);

            CreateScreenVao();

            _logger.Info("Creating emulated devices");
            EmulatedDevices.Init(this);

            _logger.Info("Creating screen buffer");
            ScreenTexture = GL.GenTexture();

            var w = EmulatedDevices.Display.VisibleWidth;
            var h = EmulatedDevices.Display.VisibleHeight;

            var pixels = Populate(new byte[w * h * 3], 0xFF);

            GL.BindTexture(TextureTarget.Texture2D, ScreenTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, pixels);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.ClampToEdge);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            MouseDown += EmulatedDevices.Touchscreen.ConsumeMouseDown;
            MouseUp += EmulatedDevices.Touchscreen.ConsumeMouseUp;
            MouseMove += EmulatedDevices.Touchscreen.ConsumeMouseMove;

            KeyUp += EmulatedDevices.PhysicalButtons.ConsumeKeyUp;
            KeyDown += EmulatedDevices.PhysicalButtons.ConsumeKeyDown;

            KeyDown += args =>
            {
                if (args.Key != Key.S)
                    return;

                Directory.CreateDirectory("Screenshots");
                var filename = $"Screenshots/screenshot-{DateTime.Now.Ticks}.png";
                EmulatedFramebuffer.FrontBuffer.Save(filename);
                _logger.Info($"Saved screenshot as {filename}");
            };

            Size = new Vector2i(EmulatedDevices.Display.VisibleWidth / 2, EmulatedDevices.Display.VisibleHeight / 2);
            
            _logger.Info("Loading application assembly");

            //var assemblyFile = "Sandbox.dll";
            var appAssy = Assembly.LoadFrom(this._assemblyFileName);

            var appEntry = appAssy.EntryPoint;

            Environment.SetEnvironmentVariable("RM_EMULATOR", "1");
            _appThread = new Thread(() => { appEntry.Invoke(null, new object[] {new string[0]}); })
            {
                Name = "EmuApp"
            };

            _logger.Info("Spawning application thread");
            _appThread.Start();
        }

        public static byte[] Populate(byte[] arr, byte value)
        {
            for (var i = 0; i < arr.Length; i++) arr[i] = value;
            return arr;
        }

        private void WindowClosing(CancelEventArgs obj)
        {
            // Without this the app thread would never abort.
            Environment.Exit(0);
        }

        private static void OnGlMessage(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userparam)
        {
            if (severity == DebugSeverity.DebugSeverityNotification)
                return;

            var msg = Marshal.PtrToStringAnsi(message, length);
            _glLogger.Debug(msg);
        }

        private void CreateScreenVao()
        {
            float[] quadVertices = { -1, 1, 0, 1, -1, -1, 0, 0, 1, -1, 1, 0, -1, 1, 0, 1, 1, -1, 1, 0, 1, 1, 1, 1 };

            ScreenVao = GL.GenVertexArray();
            var screenVbo = GL.GenBuffer();
            GL.BindVertexArray(ScreenVao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, screenVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, quadVertices.Length * sizeof(float), quadVertices,
                BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.BufferData(BufferTarget.ArrayBuffer, quadVertices.Length * sizeof(float), quadVertices,
                BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        private void DrawFullscreenQuad()
        {
            GL.BindVertexArray(ScreenVao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        private void WindowResize(ResizeEventArgs obj)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        private void WindowUpdate(FrameEventArgs e)
        {
            if (_refreshQueue.TryPeek(out var refreshTask))
            {
                if (!refreshTask.Running)
                {
                    _logger.Debug($"Refreshing region [Location=({refreshTask.Region.X},{refreshTask.Region.Y}) Size=({refreshTask.Region.Width},{refreshTask.Region.Height})] with {refreshTask.Mode} waveform");
                    refreshTask.Run();
                }
                else
                {
                    refreshTask.Poll(_imageUploadQueue);
                    if (!refreshTask.Running)
                        _refreshQueue.Dequeue();
                }
            }

            if (_imageUploadQueue.TryDequeue(out var imageUploadTask))
            {
                _logger.Debug($"Bitting region [Location=({imageUploadTask.DestPoint.X},{imageUploadTask.DestPoint.Y}) Size=({imageUploadTask.Image.Width},{imageUploadTask.Image.Height})]");

                GL.BindTexture(TextureTarget.Texture2D, ScreenTexture);

                var image = imageUploadTask.Image;

                using var ms = new MemoryStream();
                _textureEncoder.Encode(image, ms);
                var pixels = ms.GetBuffer();

                GL.TexSubImage2D(TextureTarget.Texture2D, 0, imageUploadTask.DestPoint.X, imageUploadTask.DestPoint.Y, imageUploadTask.Image.Width, imageUploadTask.Image.Height,
                    PixelFormat.Rgb, PixelType.UnsignedByte, pixels);

                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }

        private void WindowRender(FrameEventArgs e)
        {
            const ClearBufferMask bits = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit;
            // Reset the view
            GL.Clear(bits);

            GL.BindTexture(TextureTarget.Texture2D, ScreenTexture);
            ShaderScreen.Use();
            DrawFullscreenQuad();
            ShaderScreen.Release();

            // Swap the graphics buffer
            SwapBuffers();
        }

        public void RefreshRegion(Rectangle region, WaveformMode mode, DisplayTemp displayTemp, UpdateMode updateMode)
        {
            _refreshQueue.Enqueue(new RefreshTask(region, mode, displayTemp, updateMode));
        }
    }
}
