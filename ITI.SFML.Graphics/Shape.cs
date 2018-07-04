using System;
using System.Runtime.InteropServices;
using System.Security;
using SFML.System;

namespace SFML.Graphics
{
    /// <summary>
    /// Base class for textured shapes with outline.
    /// </summary>
    public abstract class Shape : Transformable, IDrawable
    {
        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        delegate uint GetPointCountCallbackType( IntPtr UserData );

        [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
        delegate Vector2f GetPointCallbackType( uint index, IntPtr UserData );

        GetPointCountCallbackType _getPointCountCallback;
        GetPointCallbackType _getPointCallback;
        Texture _texture;

        /// <summary>
        /// Source texture of the shape.
        /// </summary>
        public Texture Texture
        {
            get { return _texture; }
            set { _texture = value; sfShape_setTexture( CPointer, value != null ? value.CPointer : IntPtr.Zero, false ); }
        }

        /// <summary>
        /// Sub-rectangle of the texture that the shape will display.
        /// </summary>
        public IntRect TextureRect
        {
            get { return sfShape_getTextureRect( CPointer ); }
            set { sfShape_setTextureRect( CPointer, value ); }
        }

        /// <summary>
        /// Fill color of the shape
        /// </summary>
        public Color FillColor
        {
            get { return sfShape_getFillColor( CPointer ); }
            set { sfShape_setFillColor( CPointer, value ); }
        }

        /// <summary>
        /// Outline color of the shape
        /// </summary>
        public Color OutlineColor
        {
            get { return sfShape_getOutlineColor( CPointer ); }
            set { sfShape_setOutlineColor( CPointer, value ); }
        }

        /// <summary>
        /// Thickness of the shape's outline
        /// </summary>
        public float OutlineThickness
        {
            get { return sfShape_getOutlineThickness( CPointer ); }
            set { sfShape_setOutlineThickness( CPointer, value ); }
        }

        /// <summary>
        /// Gets the total number of points of the shape
        /// </summary>
        /// <returns>The total point count</returns>
        public abstract uint GetPointCount();

        /// <summary>
        /// Gets the position of a point.
        /// <para>
        /// The returned point is in local coordinates, that is,
        /// the shape's transforms (position, rotation, scale) are
        /// not taken into account.
        /// The result is undefined if index is out of the valid range.
        /// </para>
        /// </summary>
        /// <param name="index">Index of the point to get, in range [0 .. PointCount - 1].</param>
        /// <returns>index-th point of the shape.</returns>
        public abstract Vector2f GetPoint( uint index );

        /// <summary>
        /// Gets the local bounding rectangle of the entity.
        ///<para>
        /// The returned rectangle is in local coordinates, which means
        /// that it ignores the transformations (translation, rotation,
        /// scale, ...) that are applied to the entity.
        /// In other words, this function returns the bounds of the
        /// entity in the entity's coordinate system.
        ///</para>
        /// </summary>
        /// <returns>Local bounding rectangle of the entity.</returns>
        public FloatRect GetLocalBounds()
        {
            return sfShape_getLocalBounds( CPointer );
        }

        /// <summary>
        /// Gets the global bounding rectangle of the entity.
        /// <para>
        /// The returned rectangle is in global coordinates, which means
        /// that it takes in account the transformations (translation,
        /// rotation, scale, ...) that are applied to the entity.
        /// In other words, this function returns the bounds of the
        /// sprite in the global 2D world's coordinate system.
        /// </para>
        /// </summary>
        /// <returns>Global bounding rectangle of the entity</returns>
        public FloatRect GetGlobalBounds()
        {
            // we don't use the native getGlobalBounds function,
            // because we override the object's transform
            return Transform.TransformRect( GetLocalBounds() );
        }

        /// <summary>
        /// Draw the shape to a render target
        /// </summary>
        /// <param name="target">Render target to draw to</param>
        /// <param name="states">The render states to use.</param>
        public void Draw( IRenderTarget target, in RenderStates states )
        {
            RenderStates.MarshalData marshaled = states.WithAppliedTransform( Transform ).Marshal();
            if( target is RenderWindow )
            {
                sfRenderWindow_drawShape( ((RenderWindow)target).CPointer, CPointer, ref marshaled );
            }
            else if( target is RenderTexture )
            {
                sfRenderTexture_drawShape( ((RenderTexture)target).CPointer, CPointer, ref marshaled );
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected Shape()
            : base( IntPtr.Zero )
        {
            _getPointCountCallback = new GetPointCountCallbackType( InternalGetPointCount );
            _getPointCallback = new GetPointCallbackType( InternalGetPoint );
            CPointer = sfShape_create( _getPointCountCallback, _getPointCallback, IntPtr.Zero );
        }

        /// <summary>
        /// Constructs the shape from another shape.
        /// </summary>
        /// <param name="copy">Shape to copy.</param>
        public Shape( Shape copy )
            : base( IntPtr.Zero )
        {
            _getPointCountCallback = new GetPointCountCallbackType( InternalGetPointCount );
            _getPointCallback = new GetPointCallbackType( InternalGetPoint );
            CPointer = sfShape_create( _getPointCountCallback, _getPointCallback, IntPtr.Zero );

            Origin = copy.Origin;
            Position = copy.Position;
            Rotation = copy.Rotation;
            Scale = copy.Scale;

            Texture = copy.Texture;
            TextureRect = copy.TextureRect;
            FillColor = copy.FillColor;
            OutlineColor = copy.OutlineColor;
            OutlineThickness = copy.OutlineThickness;
        }

        /// <summary>
        /// Recomputes the internal geometry of the shape.
        ///<para>
        /// This function must be called by the derived class everytime
        /// the shape's points change (ie. the result of either
        /// PointCount or GetPoint is different).
        ///</para>
        /// </summary>
        protected void Update() => sfShape_update( CPointer );

        /// <summary>
        /// Handles the destruction of the object.
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void Destroy( bool disposing )
        {
            sfShape_destroy( CPointer );
        }

        /// <summary>
        /// Callback passed to the C API.
        /// </summary>
        uint InternalGetPointCount( IntPtr userData ) => GetPointCount();

        /// <summary>
        /// Callback passed to the C API.
        /// </summary>
        Vector2f InternalGetPoint( uint index, IntPtr userData ) => GetPoint( index );


        #region Imports
        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfShape_create( GetPointCountCallbackType getPointCount, GetPointCallbackType getPoint, IntPtr userData );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfShape_copy( IntPtr Shape );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfShape_destroy( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfShape_setTexture( IntPtr CPointer, IntPtr Texture, bool AdjustToNewSize );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfShape_setTextureRect( IntPtr CPointer, IntRect Rect );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntRect sfShape_getTextureRect( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfShape_setFillColor( IntPtr CPointer, Color Color );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Color sfShape_getFillColor( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfShape_setOutlineColor( IntPtr CPointer, Color Color );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Color sfShape_getOutlineColor( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfShape_setOutlineThickness( IntPtr CPointer, float Thickness );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern float sfShape_getOutlineThickness( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern FloatRect sfShape_getLocalBounds( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfShape_update( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfRenderWindow_drawShape( IntPtr CPointer, IntPtr Shape, ref RenderStates.MarshalData states );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfRenderTexture_drawShape( IntPtr CPointer, IntPtr Shape, ref RenderStates.MarshalData states );
        #endregion
    }
}
