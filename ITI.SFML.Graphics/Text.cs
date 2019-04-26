using System;
using System.Security;
using System.Runtime.InteropServices;
using System.Text;
using SFML.System;

namespace SFML.Graphics
{
    /// <summary>
    /// This class defines a graphical 2D text, that can be drawn on screen.
    /// </summary>
    /// <remarks>
    /// See also the note on coordinates and undistorted rendering in SFML.Graphics.Transformable.
    /// </remarks>
    public class Text : Transformable, IDrawable
    {
        Font _font;

        /// <summary>
        /// Enumerate the string drawing styles
        /// </summary>
        [Flags]
        public enum Styles
        {
            /// <summary>
            /// Regular characters, no style.
            /// </summary>
            Regular = 0,

            /// <summary>
            /// Bold characters.
            /// </summary>
            Bold = 1 << 0,

            /// <summary>
            /// Italic characters.
            /// </summary>
            Italic = 1 << 1,

            /// <summary>
            /// Underlined characters.
            /// </summary>
            Underlined = 1 << 2,

            /// <summary>Strike through characters</summary>
            StrikeThrough = 1 << 3
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Text()
            : this( "", null )
        {
        }

        /// <summary>
        /// Constructs the text from a string, font and size.
        /// </summary>
        /// <param name="str">String to display.</param>
        /// <param name="font">Font to use.</param>
        /// <param name="characterSize">Base characters size.</param>
        public Text( string str, Font font, uint characterSize = 30 )
            : base( sfText_create() )
        {
            DisplayedString = str;
            Font = font;
            CharacterSize = characterSize;
        }


        /// <summary>
        /// Constructs the text from another text.
        /// </summary>
        /// <param name="copy">Text to copy.</param>
        public Text( Text copy )
            : base( sfText_copy( copy.CPointer ) )
        {
            Origin = copy.Origin;
            Position = copy.Position;
            Rotation = copy.Rotation;
            Scale = copy.Scale;

            Font = copy.Font;
        }

        /// <summary>
        /// Gets or sets the fill color of the object.
        /// </summary>
        /// <remarks>
        /// By default, the text's fill color is opaque white.
        /// Setting the fill color to a transparent color with an outline
        /// will cause the outline to be displayed in the fill area of the text.
        /// </remarks>
        public Color FillColor
        {
            get { return sfText_getFillColor( CPointer ); }
            set { sfText_setFillColor( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the outline color of the object.
        /// </summary>
        /// 
        /// <remarks>
        /// By default, the text's outline color is opaque black.
        /// </remarks>
        public Color OutlineColor
        {
            get { return sfText_getOutlineColor( CPointer ); }
            set { sfText_setOutlineColor( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the thickness of the object's outline.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>By default, the outline thickness is 0.</para>
        /// <para>Be aware that using a negative value for the outline
        /// thickness will cause distorted rendering.</para>
        /// </remarks>
        public float OutlineThickness
        {
            get { return sfText_getOutlineThickness( CPointer ); }
            set { sfText_setOutlineThickness( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the string which is displayed.
        /// </summary>
        public string DisplayedString
        {
            get
            {
                // Get a pointer to the source string (UTF-32)
                IntPtr source = sfText_getUnicodeString( CPointer );
                // Find its length (find the terminating 0)
                uint length = 0;
                unsafe
                {
                    for( uint* ptr = (uint*)source.ToPointer(); *ptr != 0; ++ptr )
                        length++;
                }
                // Copy it to a byte array
                byte[] sourceBytes = new byte[length * 4];
                Marshal.Copy( source, sourceBytes, 0, sourceBytes.Length );
                // Convert it to a C# string
                return Encoding.UTF32.GetString( sourceBytes );
            }

            set
            {
                // Copy the string to a null-terminated UTF-32 byte array
                byte[] utf32 = Encoding.UTF32.GetBytes( value + '\0' );
                // Pass it to the C API
                unsafe
                {
                    fixed ( byte* ptr = utf32 )
                    {
                        sfText_setUnicodeString( CPointer, (IntPtr)ptr );
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the font used to display the text.
        /// </summary>
        public Font Font
        {
            get { return _font; }
            set { _font = value; sfText_setFont( CPointer, value?.CPointer ?? IntPtr.Zero ); }
        }

        /// <summary>
        /// Gets or sets the base size of characters.
        /// </summary>
        public uint CharacterSize
        {
            get { return sfText_getCharacterSize( CPointer ); }
            set { sfText_setCharacterSize( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the style of the text (see <see cref="Styles"/> enum).
        /// </summary>
        public Styles Style
        {
            get { return sfText_getStyle( CPointer ); }
            set { sfText_setStyle( CPointer, value ); }
        }

        /// <summary>
        /// Returns the visual position of the Index-th character of the text,
        /// in coordinates relative to the text.
        /// (note : translation, origin, rotation and scale are not applied)
        /// </summary>
        /// <param name="index">Index of the character.</param>
        /// <returns>Position of the Index-th character (end of text if Index is out of range).</returns>
        public Vector2f FindCharacterPos( uint index )
        {
            return sfText_findCharacterPos( CPointer, index );
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
        public FloatRect GetLocalBounds()
        {
            return sfText_getLocalBounds( CPointer );
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
        public FloatRect GetGlobalBounds()
        {
            // we don't use the native getGlobalBounds function,
            // because we override the object's transform
            return Transform.TransformRect( GetLocalBounds() );
        }

        /// <summary>
        /// Gets or sets the size of the letter spacing factor
        /// </summary>
        public float LetterSpacing
        {
            get { return sfText_getLetterSpacing( CPointer ); }
            set { sfText_setLetterSpacing( CPointer, value ); }
        }

        /// <summary>
        /// Gets or sets the size of the line spacing factor
        /// </summary>
        public float LineSpacing
        {
            get { return sfText_getLineSpacing( CPointer ); }
            set { sfText_setLineSpacing( CPointer, value ); }
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return "[Text]" +
                   " FillColor(" + FillColor + ")" +
                   " OutlineColor(" + OutlineColor + ")" +
                   " String(" + DisplayedString + ")" +
                   " Font(" + Font + ")" +
                   " CharacterSize(" + CharacterSize + ")" +
                   " OutlineThickness(" + OutlineThickness + ")" +
                   " Style(" + Style + ")";
        }

        /// <summary>
        /// Draws the text to a render target.
        /// </summary>
        /// <param name="target">Render target to draw to.</param>
        /// <param name="states">Current render states.</param>
        public void Draw( IRenderTarget target, in RenderStates states )
        {
            RenderStates.MarshalData marshaled = states.WithAppliedTransform( Transform ).Marshal();
            switch (target)
            {
                case RenderWindow window:
                    sfRenderWindow_drawText( window.CPointer, CPointer, ref marshaled );
                    break;
                case RenderTexture texture:
                    sfRenderTexture_drawText( texture.CPointer, CPointer, ref marshaled );
                    break;
            }
        }

        /// <summary>
        /// Handles the destruction of the object.
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void OnDispose( bool disposing )
        {
            sfText_destroy( CPointer );
        }

        #region Imports
        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfText_create();

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfText_copy( IntPtr Text );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfText_destroy( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity, Obsolete]
        static extern void sfText_setColor( IntPtr CPointer, Color Color );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfText_setFillColor( IntPtr CPointer, Color Color );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfText_setOutlineColor( IntPtr CPointer, Color Color );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfText_setOutlineThickness( IntPtr CPointer, float thickness );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity, Obsolete]
        static extern Color sfText_getColor( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Color sfText_getFillColor( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Color sfText_getOutlineColor( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern float sfText_getOutlineThickness( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfRenderWindow_drawText( IntPtr CPointer, IntPtr Text, ref RenderStates.MarshalData states );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfRenderTexture_drawText( IntPtr CPointer, IntPtr Text, ref RenderStates.MarshalData states );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfText_setUnicodeString( IntPtr CPointer, IntPtr Text );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfText_setFont( IntPtr CPointer, IntPtr Font );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfText_setCharacterSize( IntPtr CPointer, uint Size );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfText_setLineSpacing( IntPtr CPointer, float spacingFactor );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfText_setLetterSpacing( IntPtr CPointer, float spacingFactor );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfText_setStyle( IntPtr CPointer, Styles Style );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfText_getString( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfText_getUnicodeString( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern uint sfText_getCharacterSize( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern float sfText_getLetterSpacing( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern float sfText_getLineSpacing( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Styles sfText_getStyle( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern FloatRect sfText_getRect( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Vector2f sfText_findCharacterPos( IntPtr CPointer, uint Index );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern FloatRect sfText_getLocalBounds( IntPtr CPointer );
        #endregion
    }
}
