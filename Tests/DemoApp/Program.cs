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
            var clock = new Clock();
            var w = new EffectDemo.EffectDemoWindow();
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
