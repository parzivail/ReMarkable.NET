using Graphite;
using Graphite.Controls;
using Graphite.Typography;
using ReMarkable.NET.Unix.Driver;
using SixLabors.ImageSharp;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var screen = OutputDevices.Display;

            var w = new Window(screen.VisibleWidth, screen.VisibleHeight);
            w.Update += WindowUpdate;

            var f = Fonts.SegoeUi;

            var mainPage = w.CreatePage();
            mainPage.Content.Add(new Button
            {
                Bounds = new Rectangle(50, 50, 200, 100),
                Text = "Button"
            });

            w.Forward(mainPage);

            while (true) { }
        }

        private static void WindowUpdate(object sender, WindowUpdateEventArgs e)
        {
            OutputDevices.Display.Draw(e.Buffer, e.UpdatedArea, e.UpdatedArea.Location);
        }
    }
}
