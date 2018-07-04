using System;
using System.Runtime.InteropServices;

namespace SFML.System
{
    /// <summary>
    /// Defines the 3 supported platforms.
    /// </summary>
    internal enum OperatingSystemType
    {
        Windows,
        Unix,
        MacOSX
    }

    /// <summary>
    /// Platform helper.
    /// </summary>
    internal static class Platform
    {
        /// <summary>
        /// Gets the processor architecture ("x64" or "x86").
        /// </summary>
        public static string ProcessorArchitecture => IntPtr.Size == 8 ? "x64" : "x86";

        /// <summary>
        /// Gets the running operating system.
        /// </summary>
        public static OperatingSystemType OperatingSystem
        {
            get
            {
                if( RuntimeInformation.IsOSPlatform( OSPlatform.Windows ) )
                {
                    return OperatingSystemType.Windows;
                }

                if( RuntimeInformation.IsOSPlatform( OSPlatform.Linux ) )
                {
                    return OperatingSystemType.Unix;
                }

                if( RuntimeInformation.IsOSPlatform( OSPlatform.OSX ) )
                {
                    return OperatingSystemType.MacOSX;
                }

                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Returns true if the runtime is Mono.
        /// </summary>
        public static bool IsRunningOnMono()
            => Type.GetType( "Mono.Runtime" ) != null;

        /// <summary>
        /// Returns true if the runtime is .NET Framework.
        /// </summary>
        public static bool IsRunningOnNetFramework()
            => typeof( object ).Assembly.GetName().Name == "mscorlib" && !IsRunningOnMono();

        /// <summary>
        /// Returns true if the runtime is .NET Core.
        /// </summary>
        public static bool IsRunningOnNetCore()
            => typeof( object ).Assembly.GetName().Name != "mscorlib";
    }
}
