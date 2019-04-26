using System;
using System.Runtime.InteropServices;
using System.Security;
using SFML.System;

namespace SFML.Window
{
    /// <summary>
    /// Gives access to the real-time state of the touches.
    /// </summary>
    public static class Touch
    {
        /// <summary>
        /// Checks if a touch event is currently down.
        /// </summary>
        /// <param name="Finger">Finger index.</param>
        /// <returns>True if the finger is currently touching the screen, false otherwise.</returns>
        public static bool IsDown( uint Finger )
        {
            return sfTouch_isDown( Finger );
        }

        /// <summary>
        /// This function returns the current touch position
        /// relative to the given window
        /// </summary>
        /// <param name="finger">Finger index</param>
        /// <param name="relativeTo">Reference window</param>
        /// <returns>Current position of the finger</returns>
        public static Vector2i GetPosition( uint finger, Window relativeTo = null )
        {
            return relativeTo?.InternalGetTouchPosition( finger ) ?? sfTouch_getPosition( finger, IntPtr.Zero );
        }

        #region Imports
        [DllImport( CSFML.Window, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern bool sfTouch_isDown( uint Finger );

        [DllImport( CSFML.Window, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Vector2i sfTouch_getPosition( uint Finger, IntPtr RelativeTo );
        #endregion
    }
}
