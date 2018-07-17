using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Runtime.ConstrainedExecution;
using SFML.System;

namespace SFML.Window
{
    /// <summary>
    /// This class defines a .NET interface to an SFML OpenGL Context
    /// </summary>
    public class Context : CriticalFinalizerObject
    {
        IntPtr _this = IntPtr.Zero;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Context()
        {
            _this = sfContext_create();
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~Context()
        {
            sfContext_destroy( _this );
        }

        /// <summary>
        /// Activate or deactivate the context
        /// </summary>
        /// <param name="active">True to activate, false to deactivate</param>
        /// <returns>true on success, false on failure</returns>
        public bool SetActive( bool active )
        {
            return sfContext_setActive( _this, active );
        }

        /// <summary>
        /// Gets the settings of the context.
        /// </summary>
        public ContextSettings Settings
        {
            get { return sfContext_getSettings( _this ); }
        }

        /// <summary>
        /// Gets the global helper context.
        /// </summary>
        public static Context Global
        {
            get
            {
                if( ourGlobalContext == null )
                    ourGlobalContext = new Context();

                return ourGlobalContext;
            }
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object</returns>
        public override string ToString()
        {
            return "[Context]";
        }

        static Context ourGlobalContext = null;

        #region Imports
        [DllImport( CSFML.Window, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfContext_create();

        [DllImport( CSFML.Window, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfContext_destroy( IntPtr View );

        [DllImport( CSFML.Window, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern bool sfContext_setActive( IntPtr View, bool Active );

        [DllImport( CSFML.Window, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern ContextSettings sfContext_getSettings( IntPtr View );
        #endregion
    }
}
