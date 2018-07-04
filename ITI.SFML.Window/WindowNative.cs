using System;
using System.Collections.Generic;
using System.Text;

namespace SFML
{
    public static class WindowNative
    {
        static bool _loaded;

        /// <summary>
        /// Ensures that the native <see cref="System.CSFML.System"/> and <see cref="System.CSFML.Window"/>
        /// are loaded.
        /// </summary>
        public static void Load()
        {
            SystemNative.Load();
            if( !_loaded ) System.CSFML.LoadNative( typeof( WindowNative ).Assembly, System.CSFML.Window );
            _loaded = true;
        }
    }
}
