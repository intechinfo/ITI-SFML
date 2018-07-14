using System.Numerics;
using System.Runtime.InteropServices;
using System.Security;
using SFML.System;

namespace SFML.Graphics
{
    /// <summary>
    /// Defines a 3x3 transform matrix.
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    public struct MutableTransform
    {
        float m00, m01, m02;
        float m10, m11, m12;
        float m20, m21, m22;

        /// <summary>
        /// Constructs a transform from a 3x3 matrix.
        /// </summary>
        /// <param name="a00">Element (0, 0) of the matrix.</param>
        /// <param name="a01">Element (0, 1) of the matrix.</param>
        /// <param name="a02">Element (0, 2) of the matrix.</param>
        /// <param name="a10">Element (1, 0) of the matrix.</param>
        /// <param name="a11">Element (1, 1) of the matrix.</param>
        /// <param name="a12">Element (1, 2) of the matrix.</param>
        /// <param name="a20">Element (2, 0) of the matrix.</param>
        /// <param name="a21">Element (2, 1) of the matrix.</param>
        /// <param name="a22">Element (2, 2) of the matrix.</param>
        public MutableTransform( float a00, float a01, float a02,
                                 float a10, float a11, float a12,
                                 float a20, float a21, float a22 )
        {
            m00 = a00; m01 = a01; m02 = a02;
            m10 = a10; m11 = a11; m12 = a12;
            m20 = a20; m21 = a21; m22 = a22;
        }

        /// <summary>
        /// Returns an immutable <see cref="Transform"/> from this mutable one.
        /// </summary>
        /// <returns>The transform.</returns>
        public unsafe Transform ToTransform()
        {
            fixed ( MutableTransform* f = &this )
            {
                return *((Transform*)(void*)f);
            }
        }

        /// <summary>
        /// Combines the this transform with an immutable <see cref="Transform"/>.
        /// <para>
        /// The result is a transform that is equivalent to applying
        /// this followed by transform. Mathematically, it is
        /// equivalent to a matrix multiplication.
        /// </para>
        /// </summary>
        /// <param name="transform">Transform to combine to this transform.</param>
        public void Combine( in Transform transform )
        {
            sfTransform_combine( ref this, in transform );
        }

        /// <summary>
        /// Combines this transform with another one.
        /// <para>
        /// The result is a transform that is equivalent to applying
        /// this followed by transform. Mathematically, it is
        /// equivalent to a matrix multiplication.
        /// </para>
        /// </summary>
        /// <param name="transform">Transform to combine to this transform.</param>
        public void Combine( ref MutableTransform transform )
        {
            sfTransform_combine( ref this, ref transform );
        }

        /// <summary>
        /// Combines the current transform with a translation.
        /// </summary>
        /// <param name="x">Offset to apply on X axis.</param>
        /// <param name="y">Offset to apply on Y axis.</param>
        public void Translate( float x, float y ) => sfTransform_translate( ref this, x, y );

        /// <summary>
        /// Combine the current transform with a translation.
        /// </summary>
        /// <param name="offset">Translation offset to apply.</param>
        public void Translate( Vector2 offset ) => Translate( offset.X, offset.Y );

        /// <summary>
        /// Combines the current transform with a rotation.
        /// </summary>
        /// <param name="angle">Rotation angle, in degrees.</param>
        public void Rotate( float angle ) => sfTransform_rotate( ref this, angle );

        /// <summary>
        /// Combines the current transform with a rotation.
        /// <para>
        /// The center of rotation is provided for convenience as a second
        /// argument, so that you can build rotations around arbitrary points
        /// more easily (and efficiently) than the usual
        /// Translate(-center); Rotate(angle); Translate(center).
        /// </para>
        /// </summary>
        /// <param name="angle">Rotation angle, in degrees.</param>
        /// <param name="centerX">X coordinate of the center of rotation.</param>
        /// <param name="centerY">Y coordinate of the center of rotation.</param>
        public void Rotate( float angle, float centerX, float centerY )
        {
            sfTransform_rotateWithCenter( ref this, angle, centerX, centerY );
        }

        /// <summary>
        /// Combines the current transform with a rotation.
        /// <para>
        /// The center of rotation is provided for convenience as a second
        /// argument, so that you can build rotations around arbitrary points
        /// more easily (and efficiently) than the usual
        /// Translate(-center); Rotate(angle); Translate(center).
        /// </para>
        /// </summary>
        /// <param name="angle">Rotation angle, in degrees.</param>
        /// <param name="center">Center of rotation.</param>
        public void Rotate( float angle, Vector2 center ) => Rotate( angle, center.X, center.Y );

        /// <summary>
        /// Combines the current transform with a scaling.
        /// </summary>
        /// <param name="scaleX">Scaling factor on the X axis.</param>
        /// <param name="scaleY">Scaling factor on the Y axis.</param>
        public void Scale( float scaleX, float scaleY ) => sfTransform_scale( ref this, scaleX, scaleY );

        /// <summary>
        /// Combines the current transform with a scaling.
        /// <para>
        /// The center of scaling is provided for convenience as a second
        /// argument, so that you can build scaling around arbitrary points
        /// more easily (and efficiently) than the usual
        /// Translate(-center); Scale(factors); Translate(center).
        /// </para>
        /// </summary>
        /// <param name="scaleX">Scaling factor on X axis</param>
        /// <param name="scaleY">Scaling factor on Y axis</param>
        /// <param name="centerX">X coordinate of the center of scaling</param>
        /// <param name="centerY">Y coordinate of the center of scaling</param>
        public void Scale( float scaleX, float scaleY, float centerX, float centerY )
        {
            sfTransform_scaleWithCenter( ref this, scaleX, scaleY, centerX, centerY );
        }

        /// <summary>
        /// Combines the current transform with a scaling.
        /// </summary>
        /// <param name="factors">Scaling factors.</param>
        public void Scale( Vector2 factors ) => Scale( factors.X, factors.Y );

        /// <summary>
        /// Combines the current transform with a scaling.
        /// <para>
        /// The center of scaling is provided for convenience as a second
        /// argument, so that you can build scaling around arbitrary points
        /// more easily (and efficiently) than the usual
        /// Translate(-center); Scale(factors); Translate(center).
        /// </para>
        /// </summary>
        /// <param name="factors">Scaling factors.</param>
        /// <param name="center">Center of scaling.</param>
        public void Scale( Vector2 factors, Vector2 center ) => Scale( factors.X, factors.Y, center.X, center.Y );

        /// <summary>
        /// The identity transform (does nothing).
        /// </summary>
        public static MutableTransform Identity
        {
            get
            {
                return new MutableTransform( 1, 0, 0,
                                             0, 1, 0,
                                             0, 0, 1 );
            }
        }

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return string.Format( "[MutableTransform]" +
                   " Matrix(" +
                   "{0}, {1}, {2}," +
                   "{3}, {4}, {5}," +
                   "{6}, {7}, {8}, )",
                   m00, m01, m02,
                   m10, m11, m12,
                   m20, m21, m22 );
        }

        #region Imports

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_combine( ref MutableTransform transform, in Transform other );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_combine( ref MutableTransform transform, ref MutableTransform other );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_translate( ref MutableTransform transform, float x, float y );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_rotate( ref MutableTransform transform, float angle );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_rotateWithCenter( ref MutableTransform transform, float angle, float centerX, float centerY );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_scale( ref MutableTransform transform, float scaleX, float scaleY );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_scaleWithCenter( ref MutableTransform transform, float scaleX, float scaleY, float centerX, float centerY );
        #endregion
    }
}
