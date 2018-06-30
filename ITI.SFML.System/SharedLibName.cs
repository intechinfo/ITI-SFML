using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SFML.System
{
    public class CSFML
    {
        private static string decorate(String libname)
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return $"{libname}-2.dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return $"lib{libname}.so";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return $"lib{libname}.dylib";
            }
            else
            {
                throw new Exception("Unknown OS cannot match Shared Library");
            }

        }

#if   _WINDOWS_
        public const string Audio       = "csfml-audio-2.dll";
        public const string Graphics    = "csfml-graphics-2.dll";
        public const string System      = "csfml-system-2.dll";
        public const string Window      = "csfml-window-2.dll";
#elif _OSX_
        public const string Audio       = "libcsfml-audio.dylib";
        public const string Graphics    = "libcsfml-graphics.dylib";
        public const string System      = "libcsfml-system.dylib";
        public const string Window      = "libcsfml-window.dylib";
#elif _LINUX_
        public const string Audio       = "libcsfml-audio.so";
        public const string Graphics    = "libcsfml-graphics.so";
        public const string System      = "libcsfml-system.so";
        public const string Window      = "libcsfml-window.so";
#endif


    }
}
