using System;
using System.Runtime.InteropServices;

namespace SFML.Graphics
{
    /// <summary>
    /// Defines the states used for drawing to a RenderTarget.
    /// </summary>
    public readonly struct RenderStates
    {
        /// <summary>
        /// Special value holding the default render states.
        /// </summary>
        public static readonly RenderStates Default = new RenderStates( BlendMode.Alpha, Transform.Identity, null, null );

        /// <summary>
        /// Blending mode.
        /// </summary>
        public readonly BlendMode BlendMode;

        /// <summary>
        /// Transform.
        /// </summary>
        public readonly Transform Transform;

        /// <summary>
        /// Texture.
        /// </summary>
        public readonly Texture Texture;

        /// <summary>
        /// Shader.
        /// </summary>
        public readonly Shader Shader;

        /// <summary>
        /// Constructs a default set of render states with a custom blend mode.
        /// </summary>
        /// <param name="blendMode">Blend mode to use.</param>
        public RenderStates( BlendMode blendMode )
            : this( blendMode, Transform.Identity, null, null )
        {
        }

        /// <summary>
        /// Constructs a default set of render states with a custom transform.
        /// </summary>
        /// <param name="transform">Transform to use.</param>
        public RenderStates( Transform transform )
            : this( BlendMode.Alpha, transform, null, null )
        {
        }

        /// <summary>
        /// Constructs a default set of render states with a custom texture.
        /// </summary>
        /// <param name="texture">Texture to use.</param>
        public RenderStates( Texture texture )
            : this( BlendMode.Alpha, Transform.Identity, texture, null )
        {
        }

        /// <summary>
        /// Constructs a default set of render states with a custom shader.
        /// </summary>
        /// <param name="shader">Shader to use.</param>
        public RenderStates( Shader shader )
            : this( BlendMode.Alpha, Transform.Identity, null, shader )
        {
        }

        /// <summary>
        /// Constructs a set of render states with all its attributes.
        /// </summary>
        /// <param name="blendMode">Blend mode to use.</param>
        /// <param name="transform">Transform to use.</param>
        /// <param name="texture">Texture to use.</param>
        /// <param name="shader">Shader to use.</param>
        public RenderStates( BlendMode blendMode, Transform transform, Texture texture, Shader shader )
        {
            BlendMode = blendMode;
            Transform = transform;
            Texture = texture;
            Shader = shader;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="copy">States to copy.</param>
        public RenderStates( RenderStates copy )
        {
            BlendMode = copy.BlendMode;
            Transform = copy.Transform;
            Texture = copy.Texture;
            Shader = copy.Shader;
        }

        /// <summary>
        /// Returns a new render states based on this one but with a new <see cref="Shader"/>.
        /// </summary>
        /// <param name="s">The new Shader.</param>
        /// <returns>A render state.</returns>
        public RenderStates WithShader( Shader s ) => new RenderStates( BlendMode, Transform, Texture, s );

        /// <summary>
        /// Returns a new render states based on this one but with a new <see cref="Transform"/>.
        /// </summary>
        /// <param name="t">The new Transform.</param>
        /// <returns>A render state.</returns>
        public RenderStates WithNewTransform( Transform t ) => new RenderStates( BlendMode, t, Texture, Shader );

        /// <summary>
        /// Returns a new render states based on this one but with a new <see cref="Transform"/>.
        /// </summary>
        /// <param name="t">The new Transform.</param>
        /// <returns>A render state.</returns>
        public RenderStates WithAppliedTransform( Transform t ) => new RenderStates( BlendMode, Transform.Combine( t ), Texture, Shader );


        /// <summary>
        /// Returns a marshalled version of the instance, that can directly be passed to the C API.
        /// </summary>
        /// <returns>The data.</returns>
        internal MarshalData Marshal()
        {
            MarshalData data = new MarshalData
            {
                blendMode = BlendMode,
                transform = Transform,
                texture = Texture?.CPointer ?? IntPtr.Zero,
                shader = Shader?.CPointer ?? IntPtr.Zero
            };
            return data;
        }

        [StructLayout( LayoutKind.Sequential )]
        internal struct MarshalData
        {
            public BlendMode blendMode;
            public Transform transform;
            public IntPtr texture;
            public IntPtr shader;
        }
    }
}
