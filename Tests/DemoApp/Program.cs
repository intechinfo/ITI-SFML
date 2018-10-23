using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DemoApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            SFML.GraphicsNative.Load();
            SFML.AudioNative.Load();
            var w = new EffectDemo.EffectDemoWindow();
            // Start the game loop
            var clock = new Clock();
            while( w.IsOpen )
            {
                // Process events
                w.DispatchEvents();
                w.Update( clock );
                w.Draw();
            }
        }

    }
}
