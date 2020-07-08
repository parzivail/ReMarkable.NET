using System;
using System.Collections.Generic;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using RmEmulator.Framebuffer;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RmEmulator
{
    internal class RefreshTask
    {
        private Image<Rgb24> _image;
        private Image<Rgb24> _previousImage;
        private int _intervals;
        private DateTime _nextRefresh;

        public Rectangle Region { get; }
        public WaveformMode Mode { get; }
        public bool Running { get; private set; }

        public RefreshTask(Rectangle region, WaveformMode mode)
        {
            EmulatedDevices.Display.Framebuffer.ConstrainRectangle(ref region);

            Region = region;
            Mode = mode;
        }

        public void Run()
        {
            _image = EmulatedDevices.Display.Framebuffer.Read(Region);
            _image.Mutate(g => g.Crop(new Rectangle(Point.Empty, Region.Size)));

            _previousImage = EmulatedFramebuffer.FrontBuffer.Clone(g => g.Crop(Region));

            Running = true;
            _nextRefresh = DateTime.Now;
        }

        public void Poll(Queue<ImageUploadTask> imageSwapQueue)
        {
            if (DateTime.Now < _nextRefresh) return;

            _intervals++;

            if (_intervals > 5)
            {
                Running = false;
                EmulatedFramebuffer.FrontBuffer.Mutate(g => g.DrawImage(EmulatedFramebuffer.BackBuffer.Clone(g2 => g2.Crop(Region)), Region.Location, 1));
                EmulatedFramebuffer.FrontBuffer.Save("E:\\colby\\Desktop\\temp2\\output.png");
            }
            else
            {
                switch (_intervals)
                {
                    case 1:
                    {
                        // new, inverted, edge detected, edges and fill removed from old
                        var newImage = _previousImage.Clone(g =>
                        {
                            g.DrawImage(_image.Clone(g2 => g2.Invert()), PixelColorBlendingMode.Add, 1);
                            g.DrawImage(_image.Clone(g2 => g2.DetectEdges(EdgeDetectionOperators.Laplacian3x3)),
                                PixelColorBlendingMode.Subtract, 1);
                        });
                        imageSwapQueue.Enqueue(new ImageUploadTask(newImage, Region.Location));
                        _nextRefresh = DateTime.Now + TimeSpan.FromMilliseconds(175);
                        break;
                    }
                    case 2:
                    {
                        // new, inverted rectangle
                        var newImage = _image.Clone(g =>
                        {
                            g.Invert();
                            g.DrawImage(_previousImage.Clone(g2 => g2.DetectEdges(EdgeDetectionOperators.Laplacian3x3)),
                                PixelColorBlendingMode.Add, 0.25f);
                        });
                        imageSwapQueue.Enqueue(new ImageUploadTask(newImage, Region.Location));
                        _nextRefresh = DateTime.Now + TimeSpan.FromMilliseconds(100);
                        break;
                    }
                    case 3:
                    {
                        // new, inverted, edge detected, edges removed from old
                        var newImage = _previousImage.Clone(g =>
                        {
                            g.DrawImage(_image.Clone(g2 => g2.DetectEdges(EdgeDetectionOperators.Laplacian3x3)), PixelColorBlendingMode.Add, 1);
                        });
                        imageSwapQueue.Enqueue(new ImageUploadTask(newImage, Region.Location));
                        _nextRefresh = DateTime.Now + TimeSpan.FromMilliseconds(75);
                        break;
                    }
                    case 4:
                    {
                        // old, black removed from black on new
                        var newImage = _image.Clone(g =>
                        {
                            g.DrawImage(_previousImage.Clone(g2 => g2.DetectEdges(EdgeDetectionOperators.Laplacian3x3).Invert()), PixelColorBlendingMode.Add, 1);
                        });
                        imageSwapQueue.Enqueue(new ImageUploadTask(newImage, Region.Location));
                        _nextRefresh = DateTime.Now + TimeSpan.FromMilliseconds(120);
                        break;
                    }
                    case 5:
                    {
                        imageSwapQueue.Enqueue(new ImageUploadTask(_image, Region.Location));
                        break;
                    }
                }
            }
        }
    }
}