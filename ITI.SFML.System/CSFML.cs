using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SFML.System
{
    public static class CSFML
    {
        public const string Audio = "csfml-audio-2.4.0";
        public const string Graphics = "csfml-graphics-2.4.0";
        public const string System = "csfml-system-2.4.0";
        public const string Window = "csfml-window-2.4.0";

        static readonly string _executingAssemblyDirectory;

        static CSFML()
        {
            _executingAssemblyDirectory = GetExecutingAssemblyDirectory();
        }

        static bool _system;
        static bool _audio;
        static bool _graphics;
        static bool _window;

        /// <summary>
        /// Ensures that <see cref="System"/> is loaded.
        /// </summary>
        /// <returns>True on success, false on error.</returns>
        public static bool EnsureSystem() => _system || (_system = Load( System ));

        /// <summary>
        /// Ensures that <see cref="System"/> and <see cref="Audio"/> are loaded.
        /// </summary>
        /// <returns>True on success, false on error.</returns>
        public static bool EnsureAudio() => _audio || (_audio = EnsureSystem() | Load( Audio ));

        /// <summary>
        /// Ensures that <see cref="System"/> and <see cref="Window"/> are loaded.
        /// </summary>
        /// <returns>True on success, false on error.</returns>
        public static bool EnsureWindow() => _window || (_window = EnsureSystem() | Load( Window ));

        /// <summary>
        /// Ensures that <see cref="System"/>, <see cref="Window"/> and <see cref="Graphics"/> are loaded.
        /// </summary>
        /// <returns>True on success, false on error.</returns>
        public static bool EnsureGraphics() => _graphics || (_graphics = EnsureWindow() | Load( Graphics ));

        static bool Load( string name )
        {
            var path = GetNativeLibraryPath( name ) ?? throw new FileNotFoundException( "Unable to locate native file.", GetNativeLibraryPath( _executingAssemblyDirectory, name ) );
            var fName = Path.GetFileNameWithoutExtension( path );
            var local = Path.Combine( AppContext.BaseDirectory, fName );
            if( !File.Exists( local ) ) File.Copy( path, local );
            if( RuntimeInformation.IsOSPlatform( OSPlatform.Windows ) )
            {
                return LoadWindowsLibrary( name ) != IntPtr.Zero;
            }
            return LoadUnixLibrary( name, RTLD_NOW ) != IntPtr.Zero;
        }

        /// <summary>
        /// Gets the native library path in runtimes, depending on the <see cref="OperatingSystem"/>.
        /// </summary>
        /// <param name="name">Name of the component (no extension).</param>
        /// <returns>The full file path or null if not found.</returns>
        static string GetNativeLibraryPath( string name )
        {
            string p = _executingAssemblyDirectory;
            while( p.Length > 3 )
            {
                string file = GetNativeLibraryPath( p, name );
                if( File.Exists( file ) ) return file;
                p = Path.GetDirectoryName( p );
            }
            return null;
        }

        /// <summary>
        /// Gets the native library path in runtimes, depending on the <see cref="OperatingSystem"/>.
        /// </summary>
        /// <param name="path">Starting path.</param>
        /// <param name="name">Name of the component (no extension).</param>
        /// <returns>The full file path.</returns>
        static string GetNativeLibraryPath( string path, string name )
        {
            switch( Platform.OperatingSystem )
            {
                case OperatingSystemType.Windows:
                    return $"{path}/runtimes/win-x64/native/{name}.dll";

                case OperatingSystemType.MacOSX:
                    return $"{path}/runtimes/os-x64/native/{name}.dylib";

                case OperatingSystemType.Unix:
                    return $"{path}/runtimes/linux-x64/native/{name}.so";
            }
            throw new PlatformNotSupportedException();
        }

        static string GetExecutingAssemblyDirectory()
        {
            // Assembly.CodeBase is not actually a correctly formatted
            // URI.  It's merely prefixed with `file:///` and has its
            // backslashes flipped.  This is superior to EscapedCodeBase,
            // which does not correctly escape things, and ambiguates a
            // space (%20) with a literal `%20` in the path.  Sigh.
            var managedPath = Assembly.GetExecutingAssembly().CodeBase;
            if( managedPath == null )
            {
                managedPath = Assembly.GetExecutingAssembly().Location;
            }
            else if( managedPath.StartsWith( "file:///" ) )
            {
                managedPath = managedPath.Substring( 8 ).Replace( '/', '\\' );
            }
            else if( managedPath.StartsWith( "file://" ) )
            {
                managedPath = @"\\" + managedPath.Substring( 7 ).Replace( '/', '\\' );
            }

            managedPath = Path.GetDirectoryName( managedPath );
            return managedPath;
        }

        public const int RTLD_NOW = 0x002;

        [DllImport( "libdl", EntryPoint = "dlopen" )]
        private static extern IntPtr LoadUnixLibrary( string path, int flags );

        [DllImport( "kernel32", EntryPoint = "LoadLibrary" )]
        private static extern IntPtr LoadWindowsLibrary( string path );
    }
}
