using System;
using System.Collections.Generic;
using System.Text;
using SFML;
using SFML.Window;
using SFML.Graphics;

namespace TopDownGame
{
    static class Window
    {
        public static RenderWindow window;

        public static bool IsCreated { get { return window != null; } }

        public static bool IsOpen { get { return window.IsOpen; } }

        public static void CreateWindow(uint width, uint height, string title)
        {
            window = new RenderWindow(new VideoMode(width, height), title, Styles.Close);

            window.Closed += (s, a) => window.Close();
        }

        public static void DispatchEvents()
        {
            window.DispatchEvents();
        }

        public static void Clear(Color color)
        {
            window.Clear(color);
        }

        public static void Display()
        {
            window.Display();
        }

        public static void Draw(Drawable drawable)
        {
            window.Draw(drawable);
        }

        public static void SetSize(SFML.System.Vector2f size)
        {
            FloatRect visibleArea = new FloatRect(new Vector2(), size);
            window.SetView(new View(visibleArea));
            window.Size = new SFML.System.Vector2u((uint)size.X, (uint)size.Y);
        }
    }
}
