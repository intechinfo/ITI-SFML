using SFML.System;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SFML.Graphics
{
    public class VertexBuffer : ObjectBase, IDrawable
    {
        /// <summary>
        /// Usage specifiers.
        /// <para>
        /// If data is going to be updated once or more every frame, set the usage to Stream.
        /// If data is going to be set once and used for a long time without being
        /// modified, set the usage to Static.
        /// For everything else Dynamic should be a good compromise.
        /// </para>
        /// </summary>
        public enum UsageSpecifier
        {
            /// <summary>
            /// Data is going to be updated once or more every frame.
            /// </summary>
            Stream,

            /// <summary>
            /// Data is updated but not as much as in <see cref="Stream"/> and less than
            /// the <see cref="Static"/> usage.
            /// </summary>
            Dynamic,

            /// <summary>
            /// Data is going to be set once and used for a long time without being modified.
            /// </summary>
            Static
        }

        /// <summary>
        /// Gets whether or not the system supports vertex buffers
        /// <para>
        /// This function should always be called before using
        /// the vertex buffer features. If it returns false, then
        /// any attempt to use sf::VertexBuffer will fail.
        /// </para>
        /// </summary>
        public static bool Available => sfVertexBuffer_isAvailable();

        /// <summary>
        /// Creates a new vertex buffer with a specific PrimitiveType and usage specifier.
        /// <para>
        /// Creates the vertex buffer, allocating enough graphics memory to hold
        /// <paramref name="vertexCount"/> vertices, and sets its primitive type to
        /// <paramref name="primitiveType"/> and usage to <paramref name="usageType"/>.
        /// </para>
        /// </summary>
        /// <param name="vertexCount">Amount of vertices.</param>
        /// <param name="primitiveType">Type of primitives.</param>
        /// <param name="usageType">Usage specifier.</param>
        public VertexBuffer( uint vertexCount, PrimitiveType primitiveType, UsageSpecifier usageType )
            : base(sfVertexBuffer_create(vertexCount, primitiveType, usageType))
        {
        }

        /// <summary>
        /// Constructs the vertex buffer from another vertex array
        /// </summary>
        /// <param name="copy">VertexBuffer to copy</param>
        public VertexBuffer( VertexBuffer copy )
            : base(sfVertexBuffer_copy(copy.CPointer))
        {
        }

        /// <summary>
        /// Gets the total vertex count.
        /// </summary>
        public uint VertexCount => sfVertexBuffer_getVertexCount( CPointer );

        /// <summary>
        /// OpenGL handle of the vertex buffer or 0 if not yet created
        /// <para>
        /// You shouldn't need to use this property, unless you have
        /// very specific stuff to implement that SFML doesn't support,
        /// or implement a temporary workaround until a bug is fixed.
        /// </para>
        /// </summary>
        public uint NativeHandle => sfVertexBuffer_getNativeHandle( CPointer); 

        /// <summary>
        /// Gets or sets the type of primitives drawn by the vertex buffer
        /// </summary>
        public PrimitiveType PrimitiveType
        {
            get { return sfVertexBuffer_getPrimitiveType( CPointer); }
            set { sfVertexBuffer_setPrimitiveType( CPointer, value); }
        }

        /// <summary>
        /// Gets or sets the usage specifier for the vertex buffer
        /// </summary>
        public UsageSpecifier Usage
        {
            get { return sfVertexBuffer_getUsage( CPointer); }
            set { sfVertexBuffer_setUsage( CPointer, value); }
        }

        /// <summary>
        /// Updates a part of the buffer from an array of vertices
        /// offset is specified as the number of vertices to skip
        /// from the beginning of the buffer.
        ///<para>
        /// If offset is 0 and vertexCount is equal to the size of
        /// the currently created buffer, its whole contents are replaced.
        ///</para>
        /// <para>
        /// If offset is 0 and vertexCount is greater than the
        /// size of the currently created buffer, a new buffer is created
        /// containing the vertex data.
        /// </para>
        /// <para>
        /// If offset is 0 and vertexCount is less than the size of
        /// the currently created buffer, only the corresponding region
        /// is updated.
        /// </para>
        /// <para>
        /// If offset is not 0 and offset + vertexCount is greater
        /// than the size of the currently created buffer, the update fails.
        /// </para>
        /// <para>
        /// No additional check is performed on the size of the vertex
        /// array, passing invalid arguments will lead to undefined
        /// behavior.
        /// </para>
        /// </summary>
        /// <param name="vertices">Array of vertices to copy to the buffer.</param>
        /// <param name="vertexCount">Number of vertices to copy.</param>
        /// <param name="offset">Offset in the buffer to copy to.</param>
        public bool Update( Vertex[] vertices, uint vertexCount, uint offset )
        {
            unsafe
            {
                fixed (Vertex* verts = vertices)
                {
                    return sfVertexBuffer_update( CPointer, verts, vertexCount, offset );
                }
            }
        }

        /// <summary>
        /// Updates a part of the buffer from an array of vertices
        /// assuming an offset of 0 and a vertex count the length of the passed array.
        /// <para>
        /// If offset is 0 and vertexCount is equal to the size of
        /// the currently created buffer, its whole contents are replaced.
        /// </para>
        /// <para>
        /// If offset is 0 and vertexCount is greater than the
        /// size of the currently created buffer, a new buffer is created
        /// containing the vertex data.
        /// </para>
        /// <para>
        /// If offset is 0 and vertexCount is less than the size of
        /// the currently created buffer, only the corresponding region
        /// is updated.
        /// </para>
        /// <para>
        /// If offset is not 0 and offset + vertexCount is greater
        /// than the size of the currently created buffer, the update fails.
        /// </para>
        /// <para>
        /// No additional check is performed on the size of the vertex
        /// array, passing invalid arguments will lead to undefined
        /// behavior.
        /// </para>
        /// </summary>
        /// <param name="vertices">Array of vertices to copy to the buffer.</param>
        public bool Update( Vertex[] vertices )
        {
            return this.Update(vertices, (uint) vertices.Length, 0);
        }

        /// <summary>
        /// Updates a part of the buffer from an array of vertices
        /// assuming a vertex count the length of the passed array.
        /// <para>
        /// If offset is 0 and vertexCount is equal to the size of
        /// the currently created buffer, its whole contents are replaced.
        /// </para>
        /// <para>
        /// If offset is 0 and vertexCount is greater than the
        /// size of the currently created buffer, a new buffer is created
        /// containing the vertex data.
        /// </para>
        /// <para>
        /// If offset is 0 and vertexCount is less than the size of
        /// the currently created buffer, only the corresponding region
        /// is updated.
        /// </para>
        /// <para>
        /// If offset is not 0 and offset + vertexCount is greater
        /// than the size of the currently created buffer, the update fails.
        /// </para>
        /// <para>
        /// No additional check is performed on the size of the vertex
        /// array, passing invalid arguments will lead to undefined
        /// behavior.
        /// </para>
        /// </summary>
        /// <param name="vertices">Array of vertices to copy to the buffer.</param>
        /// <param name="offset">Offset in the buffer to copy to</param>
        public bool Update( Vertex[] vertices, uint offset )
        {
            return this.Update(vertices, (uint) vertices.Length, offset);
        }

        /// <summary>
        /// Copies the contents of another buffer into this buffer.
        /// </summary>
        /// <param name="other">Vertex buffer whose contents to copy into first vertex buffer.</param>
        public bool Update( VertexBuffer other )
        {
            return sfVertexBuffer_updateFromVertexBuffer( CPointer, other.CPointer);
        }

        /// <summary>
        /// Swaps the contents of another buffer into this buffer.
        /// </summary>
        /// <param name="other">Vertex buffer whose contents to swap with.</param>
        public void Swap( VertexBuffer other )
        {
            sfVertexBuffer_swap( CPointer, other.CPointer);
        }

        /// <summary>
        /// Handles the destruction of the object.
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void OnDispose( bool disposing )
        {
            sfVertexBuffer_destroy( CPointer);
        }

        /// <summary>
        /// Draws the vertex buffer to a render target.
        /// </summary>
        /// <param name="target">Render target to draw to.</param>
        /// <param name="states">Current render states.</param>
        public void Draw( IRenderTarget target, in RenderStates states )
        {
            RenderStates.MarshalData marshaledStates = states.Marshal();
            if( target is RenderWindow )
            {
                sfRenderWindow_drawVertexBuffer( ((RenderWindow)target).CPointer, CPointer, ref marshaledStates );
            }
            else if( target is RenderTexture )
            {
                sfRenderTexture_drawVertexBuffer( ((RenderTexture)target).CPointer, CPointer, ref marshaledStates );
            }
        }

        #region Imports
        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfVertexBuffer_create( uint vertexCount, PrimitiveType type, UsageSpecifier usage );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern IntPtr sfVertexBuffer_copy( IntPtr copy );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfVertexBuffer_destroy( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern uint sfVertexBuffer_getVertexCount( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern unsafe bool sfVertexBuffer_update( IntPtr CPointer, Vertex* vertices, uint vertexCount, uint offset );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern bool sfVertexBuffer_updateFromVertexBuffer( IntPtr CPointer, IntPtr other );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfVertexBuffer_swap( IntPtr CPointer, IntPtr other );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern uint sfVertexBuffer_getNativeHandle( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfVertexBuffer_setPrimitiveType( IntPtr CPointer, PrimitiveType primitiveType );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern PrimitiveType sfVertexBuffer_getPrimitiveType( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfVertexBuffer_setUsage( IntPtr CPointer, UsageSpecifier usageType );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern UsageSpecifier sfVertexBuffer_getUsage( IntPtr CPointer );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern bool sfVertexBuffer_isAvailable();

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfRenderWindow_drawVertexBuffer( IntPtr CPointer, IntPtr VertexArray, ref RenderStates.MarshalData states );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfRenderTexture_drawVertexBuffer( IntPtr CPointer, IntPtr VertexBuffer, ref RenderStates.MarshalData states );
        #endregion
    }
}
