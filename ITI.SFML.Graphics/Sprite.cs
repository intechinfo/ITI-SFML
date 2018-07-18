using System;
using System.Drawing;
using System.Security;
using System.Runtime.InteropServices;
using SFML.System;

namespace SFML.Graphics
{
    /// <summary>
    /// This class defines a sprite : texture, transformations,
    /// color, and draw on screen.
    /// </summary>
    /// <remarks>
    /// See also the note on coordinates and undistorted rendering in SFML.Graphics.Transformable.
    /// </remarks>
    public class Sprite : Transformable, IDrawable
    {
        Texture _texture;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Sprite()
            : base( sfSprite_create() )
        {
        }

        /// <summary>
        /// Constructs the sprite from a source texture.
        /// </summary>
        /// <param name="texture">Source texture to assign to the sprite.</param>
        public Sprite( Texture texture )
            : base( sfSprite_create() )
        {
            Texture = texture;
        }

        /// <summary>
        /// Constructs the sprite from a source texture.
        /// </summary>
        /// <param name="texture">Source texture to assign to the sprite.</param>
        /// <param name="rectangle">Sub-rectangle of the texture to assign to the sprite.</param>
        public Sprite( Texture texture, Rectangle rectangle )
            : base( sfSprite_create() )
        {
            Texture = texture;
            TextureRect = rectangle;
        }

        /// <summary>
        /// Constructs the sprite from another sprite.
        /// </summary>
        /// <param name="copy">Sprite to copy</param>
        public Sprite( Sprite copy )
            : base( sfSprite_copy( copy.CPointer ) )
        {
            Origin = copy.Origin;
            Position = copy.Position;
            Rotation = copy.Rotation;
            Scale = copy.Scale;
            Texture = copy.Texture;
        }

        /// <summary>
        /// Gets or sets the global color of the sprite.
        /// </summary>
        public Color Color
        {
            get { return sfSprite_getColor( CPointer ); }
            set { sfSprite_setColor( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the source texture displayed by the sprite.
        /// </summary>
        public Texture Texture
        {
            get { return _texture; }
            set { _texture = value; sfSprite_setTexture( CPointer, value != null ? value.CPointer : IntPtr.Zero, false ); }
        }

        /// <summary>
        /// Gets or sets the sub-rectangle of the source image displayed by the sprite.
        /// </summary>
        public Rectangle TextureRect
        {
            get { return sfSprite_getTextureRect( CPointer ); }
            set { sfSprite_setTextureRect( CPointer, value ); }
        }

        /// <summary>
        /// Gets the local bounding rectangle of the entity.
        /// <para>
        /// The returned rectangle is in local coordinates, which means
        /// that it ignores the transformations (translation, rotation,
        /// scale, ...) that are applied to the entity.
        /// In other words, this function returns the bounds of the
        /// entity in the entity's coordinate system.
        /// </para>
        /// </summary>
        /// <returns>Local bounding rectangle of the entity.</returns>
        public RectangleF GetLocalBounds()
        {
            return sfSprite_getLocalBounds( CPointer );
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
        /// <returns>Global bounding rectangle of the entity.</returns>
        public RectangleF GetGlobalBounds()
        {
            // we don't use the native getGlobalBounds function,
            // because we override the object's transform
            return Transform.TransformRect( GetLocalBounds() );
        }

        /// <summary>
        /// Provides a string describing the object
        /// </summary>
        /// <returns>String description of the object</returns>
        public override string ToString()
        {
            return "[Sprite]" +
                   " Color(" + Color + ")" +
                   " Texture(" + Texture + ")" +
                   " TextureRect(" + TextureRect + ")";
        }

        /// <summary>
        /// Draws the sprite to a render target.
        /// </summary>
        /// <param name="target">Render target to draw to</param>
        /// <param name="states">The render states to use.</param>
        public void Draw( IRenderTarget target, in RenderStates states )
        {
            RenderStates.MarshalData marshaled = states.WithAppliedTransform( Transform ).Marshal();
            if( target is RenderWindow )
            {
                sfRenderWindow_drawSprite( ((RenderWindow)target).CPointer, CPointer, ref marshaled );
            }
            else if( target is RenderTexture )
            {
                sfRenderTexture_drawSprite( ((RenderTexture)target).CPointer, CPointer, ref marshaled );
            }
        }

        /// <summary>
        /// Handles the destruction of the object.
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void Destroy( bool disposing )
        {
            sfSprite_destroy( CPointer );
        }

        #region Imports

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfSprite_create();

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfSprite_copy( IntPtr Sprite );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSprite_destroy( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSprite_setColor( IntPtr CPointer, Color Color );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Color sfSprite_getColor( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfRenderWindow_drawSprite( IntPtr CPointer, IntPtr Sprite, ref RenderStates.MarshalData states );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfRenderTexture_drawSprite( IntPtr CPointer, IntPtr Sprite, ref RenderStates.MarshalData states );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSprite_setTexture( IntPtr CPointer, IntPtr Texture, bool AdjustToNewSize );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfSprite_setTextureRect( IntPtr CPointer, Rectangle Rect );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Rectangle sfSprite_getTextureRect( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern RectangleF sfSprite_getLocalBounds( IntPtr CPointer );
        #endregion
    }
}
