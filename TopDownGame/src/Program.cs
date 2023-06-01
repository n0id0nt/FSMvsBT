using System;
using SFML.Graphics;
using SFML.Window;

namespace TopDownGame
{
    class Program
    {
        static void OnKeyRealeased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Space)
            {
                Game.Debug = !Game.Debug;
            }
        }

        static void Main(string[] args)
        {
            Game.Init();

            Window.window.KeyReleased += new EventHandler<KeyEventArgs>(OnKeyRealeased);


            while (Window.IsOpen)
            {
                Window.DispatchEvents();

                // update
                Game.Update();

                // render
                Window.Clear(Color.Black);

                Game.Render();

                Window.Display();
            }

            // on close
            Game.OnClose();
        }
    }
}
