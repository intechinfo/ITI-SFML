using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security;
using SFML.System;

namespace SFML.Graphics
{
    /// <summary>
    /// This class defines a view (position, size, etc.). You can consider it as a 2D camera.
    /// </summary>
    /// <remarks>
    /// See also the note on coordinates and undistorted rendering in <see cref="Transformable"/>.
    /// </remarks>
    public class View : ObjectBase
    {
        readonly bool _external;

        /// <summary>
        /// Creates a default view (1000x1000).
        /// </summary>
        public View()
            : base( sfView_create() )
        {
        }

        /// <summary>
        /// Constructs the view from a rectangle.
        /// </summary>
        /// <param name="viewRect">Rectangle defining the position and size of the view.</param>
        public View( FloatRect viewRect )
            : base( sfView_createFromRect( viewRect ) )
        {
        }

        /// <summary>
        /// Constructs the view from its center and size.
        /// </summary>
        /// <param name="center">Center of the view.</param>
        /// <param name="size">Size of the view.</param>
        public View( Vector2 center, Vector2 size )
            : base( sfView_create() )
        {
            Center = center;
            Size = size;
        }

        /// <summary>
        /// Constructs the view from another view.
        /// </summary>
        /// <param name="copy">View to copy.</param>
        public View( View copy )
            : base( sfView_copy( copy.CPointer ) )
        {
        }

        /// <summary>
        /// Gets or sets the center of the view.
        /// </summary>
        public Vector2 Center
        {
            get { return sfView_getCenter( CPointer ); }
            set { sfView_setCenter( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the half-size of the view.
        /// </summary>
        public Vector2 Size
        {
            get { return sfView_getSize( CPointer ); }
            set { sfView_setSize( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the rotation of the view, in degrees.
        /// </summary>
        public float Rotation
        {
            get { return sfView_getRotation( CPointer ); }
            set { sfView_setRotation( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the target viewport of the view, defined as a factor of the
        /// size of the target to which the view is applied.
        /// </summary>
        public FloatRect Viewport
        {
            get { return sfView_getViewport( CPointer ); }
            set { sfView_setViewport( CPointer, value ); }
        }

        /// <summary>
        /// Rebuilds the view from a rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle defining the position and size of the view.</param>
        public void Reset( FloatRect rectangle )
        {
            sfView_reset( CPointer, rectangle );
        }

        /// <summary>
        /// Moves this view.
        /// </summary>
        /// <param name="offset">Offset to move the view.</param>
        public void Move( Vector2 offset )
        {
            sfView_move( CPointer, offset );
        }

        /// <summary>
        /// Rotates this view.
        /// </summary>
        /// <param name="angle">Angle of rotation, in degrees.</param>
        public void Rotate( float angle )
        {
            sfView_rotate( CPointer, angle );
        }

        /// <summary>
        /// Resizes the view rectangle to simulate a zoom / unzoom effect.
        /// </summary>
        /// <param name="factor">Zoom factor to apply, relative to the current zoom.</param>
        public void Zoom( float factor )
        {
            sfView_zoom( CPointer, factor );
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[View]" +
                   " Center(" + Center + ")" +
                   " Size(" + Size + ")" +
                   " Rotation(" + Rotation + ")" +
                   " Viewport(" + Viewport + ")";
        }

        /// <summary>
        /// Internal constructor for other classes which need to manipulate raw views.
        /// </summary>
        /// <param name="cPointer">Direct pointer to the view object in the C library</param>
        internal View( IntPtr cPointer )
            : base( cPointer )
        {
            _external = true;
        }

        /// <summary>
        /// Handles the destruction of the object.
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void Destroy( bool disposing )
        {
            if( !_external ) sfView_destroy( CPointer );
        }

        #region Imports
        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfView_create();

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfView_createFromRect( FloatRect Rect );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfView_copy( IntPtr View );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfView_destroy( IntPtr View );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfView_setCenter( IntPtr View, Vector2 center );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfView_setSize( IntPtr View, Vector2 size );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfView_setRotation( IntPtr View, float Angle );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfView_setViewport( IntPtr View, FloatRect Viewport );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfView_reset( IntPtr View, FloatRect Rectangle );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Vector2 sfView_getCenter( IntPtr View );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Vector2 sfView_getSize( IntPtr View );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern float sfView_getRotation( IntPtr View );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern FloatRect sfView_getViewport( IntPtr View );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfView_move( IntPtr View, Vector2 offset );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfView_rotate( IntPtr View, float Angle );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfView_zoom( IntPtr View, float Factor );
        #endregion
    }
}
