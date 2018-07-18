using System.Drawing;
using System.Runtime.InteropServices;

namespace SFML.Graphics
{
    /// <summary>
    /// Structure describing a glyph (a visual character).
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    public readonly struct Glyph
    {
        /// <summary>
        /// Offset to move horizontally to the next character.
        /// </summary>
        public readonly float Advance;

        /// <summary>
        /// Bounding rectangle of the glyph, in coordinates relative to the baseline.
        /// </summary>
        public readonly RectangleF Bounds;

        /// <summary>
        /// Texture coordinates of the glyph inside the font's texture.
        /// </summary>
        public readonly Rectangle TextureRect;
    }
}
