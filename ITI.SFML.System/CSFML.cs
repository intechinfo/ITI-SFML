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


        /// <summary>
        /// Attempts to load native library path in runtimes, depending on the <see cref="OperatingSystem"/>
        /// relative to the given assembly location.
        /// </summary>
        /// <param name="a">Assembly from which runtimes will be searched.</param>
        /// <param name="name">Name of the component (no extension).</param>
        public static void LoadNative( Assembly a, string name )
        {
            string baseDirectory = GetExecutingAssemblyDirectory( a );
            string path = FindNativeLibraryPath( baseDirectory, name );
            if( path == null ) throw new FileNotFoundException( $"Unable to find native file {name} from {baseDirectory}." );
            var fName = Path.GetFileNameWithoutExtension( path );
            var local = Path.Combine( AppContext.BaseDirectory, fName );
            if( !File.Exists( local ) ) File.Copy( path, local );
            if( RuntimeInformation.IsOSPlatform( OSPlatform.Windows ) )
            {
                if( LoadWindowsLibrary( name ) == IntPtr.Zero )
                {
                    throw new FileNotFoundException( $"Unable to load '{name}' (Windows)." );
                }
            }
            if( LoadUnixLibrary( name, RTLD_NOW ) == IntPtr.Zero )
            {
                throw new FileNotFoundException( $"Unable to load '{name}' (Unix)." );
            }
        }

        /// <summary>
        /// Finds the native library path in runtimes, depending on the <see cref="OperatingSystem"/>
        /// relative to the starting point.
        /// </summary>
        /// <param name="startingPoint">Starting directory from which runtimes will be searched.</param>
        /// <param name="name">Name of the component (no extension).</param>
        /// <returns>The full file path or null if not found.</returns>
        static string FindNativeLibraryPath( string startingPoint, string name )
        {
            while( startingPoint.Length > 3 )
            {
                string file = GetNativeFilePath( startingPoint, name );
                if( File.Exists( file ) ) return file;
                startingPoint = Path.GetDirectoryName( startingPoint );
            }
            return null;
        }

        /// <summary>
        /// Gets the native library path in runtimes, depending on the <see cref="OperatingSystem"/>.
        /// </summary>
        /// <param name="path">Starting path.</param>
        /// <param name="name">Name of the component (no extension).</param>
        /// <returns>The full file path.</returns>
        static string GetNativeFilePath( string path, string name )
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

        static string GetExecutingAssemblyDirectory( Assembly a )
        {
            // Assembly.CodeBase is not actually a correctly formatted
            // URI.  It's merely prefixed with `file:///` and has its
            // backslashes flipped.  This is superior to EscapedCodeBase,
            // which does not correctly escape things, and ambiguates a
            // space (%20) with a literal `%20` in the path.  Sigh.
            var managedPath = a.CodeBase;
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
