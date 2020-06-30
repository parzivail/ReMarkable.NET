using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;
using RmEmulator.Framebuffer;

namespace RmEmulator
{
    public class EmulatorWindow : GameWindow
    {
        private static readonly DebugProc DebugCallback = OnGlMessage;

        public int ScreenVao { get; set; }
        public ShaderProgram ShaderScreen { get; set; }
        public int ScreenTexture { get; set; }

        public static bool RefreshFlag { get; set; }
        private Rgb24TextureEncoder _textureEncoder;

        private Thread _appThread;

        public EmulatorWindow() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            Load += WindowLoad;
            Resize += WindowResize;

            RenderFrame += WindowRender;
            UpdateFrame += WindowUpdate;

            Closing += WindowClosing;
            
            WindowBorder = WindowBorder.Fixed;
        }

        private void WindowLoad()
        {
            // Set up caps
            GL.Enable(EnableCap.RescaleNormal);
            GL.Enable(EnableCap.DebugOutput);
            GL.DebugMessageCallback(DebugCallback, IntPtr.Zero);
            GL.ActiveTexture(TextureUnit.Texture0);

            // Set background color
            GL.ClearColor(1, 1, 1, 1);

            ShaderScreen = new ShaderProgram(
                "#version 330 core\nout vec4 FragColor;\nin vec2 TexCoords;\nuniform sampler2D tex;\nvoid main(){FragColor=vec4(texture(tex,vec2(TexCoords.x,1-TexCoords.y)).rgb,1.0);}",
                "#version 330 core\nlayout (location=0) in vec2 aPos;\nlayout (location=1) in vec2 aTexCoords;\nout vec2 TexCoords;\nvoid main()\n{\ngl_Position=vec4(aPos.x,aPos.y,0.0,1.0);\nTexCoords=aTexCoords;\n} "
                );
            ShaderScreen.Uniforms.SetValue("tex", 0);

            CreateScreenVao();

            ScreenTexture = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, ScreenTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.ClampToEdge);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            var assemblyFile = "Sandbox.dll";
            var appAssy = Assembly.LoadFrom(assemblyFile);
            
            var appEntry = appAssy.EntryPoint;
            
            Environment.SetEnvironmentVariable("RM_EMULATOR", "1");
            _appThread = new Thread(() =>
            {
                appEntry.Invoke(null, new object[] { new string[0] });
            });

            Devices.Init(this);

            MouseDown += Devices.Touchscreen.ConsumeMouseDown;
            MouseUp += Devices.Touchscreen.ConsumeMouseUp;
            MouseMove += Devices.Touchscreen.ConsumeMouseMove;

            KeyUp += Devices.PhysicalButtons.ConsumeKeyUp;
            KeyDown += Devices.PhysicalButtons.ConsumeKeyDown;

            _textureEncoder = new Rgb24TextureEncoder(Devices.Display.VisibleWidth, Devices.Display.VisibleHeight);

            Size = new Vector2i(Devices.Display.VisibleWidth / 2, Devices.Display.VisibleHeight / 2);
            
            _appThread.Start();
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
            Console.WriteLine($"OpenGL: {msg}");
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
            GL.Viewport(new Size(Size.X, Size.Y));
        }

        private void WindowUpdate(FrameEventArgs e)
        {
        }

        private void WindowRender(FrameEventArgs e)
        {
            const ClearBufferMask bits = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit;
            // Reset the view
            GL.Clear(bits);

            if (RefreshFlag)
            {
                GL.BindTexture(TextureTarget.Texture2D, ScreenTexture);

                var buf = EmulatedFramebuffer.BackBuffer;

                using var ms = new MemoryStream();
                _textureEncoder.Encode(buf, ms);
                var pixels = ms.GetBuffer();

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, buf.Width,
                    buf.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, pixels);

                GL.BindTexture(TextureTarget.Texture2D, 0);

                RefreshFlag = false;
            }

            GL.BindTexture(TextureTarget.Texture2D, ScreenTexture);
            ShaderScreen.Use();
            DrawFullscreenQuad();
            ShaderScreen.Release();

            // Swap the graphics buffer
            SwapBuffers();
        }
    }
}
