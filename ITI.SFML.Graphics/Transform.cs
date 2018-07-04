using System.Runtime.InteropServices;
using System.Security;
using SFML.System;

namespace SFML.Graphics
{
    /// <summary>
    /// Defines a 3x3 transform matrix.
    /// </summary>
    [StructLayout( LayoutKind.Sequential )]
    public readonly struct Transform
    {
        internal readonly float m00, m01, m02;
        internal readonly float m10, m11, m12;
        internal readonly float m20, m21, m22;

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
        public Transform( float a00, float a01, float a02,
                         float a10, float a11, float a12,
                         float a20, float a21, float a22 )
        {
            m00 = a00; m01 = a01; m02 = a02;
            m10 = a10; m11 = a11; m12 = a12;
            m20 = a20; m21 = a21; m22 = a22;
        }

        /// <summary>
        /// Returns the inverse of the transform.
        /// If the inverse cannot be computed, an identity transform
        /// is returned.
        /// </summary>
        /// <returns>A new transform which is the inverse of this.</returns>
        public Transform GetInverse() => sfTransform_getInverse( in this );

        /// <summary>
        /// Transforms a 2D point.
        /// </summary>
        /// <param name="x">X coordinate of the point to transform.</param>
        /// <param name="y">Y coordinate of the point to transform.</param>
        /// <returns>Transformed point.</returns>
        public Vector2f TransformPoint( float x, float y ) => TransformPoint( new Vector2f( x, y ) );

        /// <summary>
        /// Transforms a 2D point.
        /// </summary>
        /// <param name="point">Point to transform.</param>
        /// <returns>Transformed point.</returns>
        public Vector2f TransformPoint( Vector2f point ) => sfTransform_transformPoint( in this, point );

        /// <summary>
        /// Transforms a rectangle.
        /// <para>
        /// Since SFML doesn't provide support for oriented rectangles,
        /// the result of this function is always an axis-aligned
        /// rectangle. Which means that if the transform contains a
        /// rotation, the bounding rectangle of the transformed rectangle
        /// is returned.
        /// </para>
        /// </summary>
        /// <param name="rectangle">Rectangle to transform.</param>
        /// <returns>Transformed rectangle.</returns>
        public FloatRect TransformRect( FloatRect rectangle ) => sfTransform_transformRect( in this, rectangle );

        /// <summary>
        /// Returns a <see cref="MutableTransform"/> from this <see cref="Transform"/>.
        /// </summary>
        /// <returns>The mutable transform.</returns>
        public unsafe MutableTransform ToMutable()
        {
            fixed ( Transform* f = &this )
            {
                return *((MutableTransform*)(void*)f);
            }
        }

        /// <summary>
        /// Combines the current transform with another one.
        /// <para>
        /// The result is a transform that is equivalent to applying
        /// this followed by transform. Mathematically, it is
        /// equivalent to a matrix multiplication.
        /// </para>
        /// </summary>
        /// <param name="transform">Transform to combine to this transform.</param>
        public Transform Combine( Transform transform )
        {
            MutableTransform m = ToMutable();
            m.Combine( in transform );
            return m.ToTransform();
        }

        /// <summary>
        /// Combines the current transform with a translation.
        /// </summary>
        /// <param name="x">Offset to apply on X axis.</param>
        /// <param name="y">Offset to apply on Y axis.</param>
        public Transform Translate( float x, float y )
        {
            MutableTransform m = ToMutable();
            m.Translate( x, y );
            return m.ToTransform();
        }

        /// <summary>
        /// Combine the current transform with a translation.
        /// </summary>
        /// <param name="offset">Translation offset to apply.</param>
        public Transform Translate( Vector2f offset ) => Translate( offset.X, offset.Y );

        /// <summary>
        /// Combines the current transform with a rotation.
        /// </summary>
        /// <param name="angle">Rotation angle, in degrees.</param>
        public Transform Rotate( float angle )
        {
            MutableTransform m = ToMutable();
            m.Rotate( angle );
            return m.ToTransform();
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
        /// <param name="centerX">X coordinate of the center of rotation.</param>
        /// <param name="centerY">Y coordinate of the center of rotation.</param>
        public Transform Rotate( float angle, float centerX, float centerY )
        {
            MutableTransform m = ToMutable();
            m.Rotate( angle, centerX, centerY );
            return m.ToTransform();
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
        public Transform Rotate( float angle, Vector2f center ) => Rotate( angle, center.X, center.Y );

        /// <summary>
        /// Combines the current transform with a scaling.
        /// </summary>
        /// <param name="scaleX">Scaling factor on the X axis.</param>
        /// <param name="scaleY">Scaling factor on the Y axis.</param>
        public Transform Scale( float scaleX, float scaleY )
        {
            MutableTransform m = ToMutable();
            m.Scale( scaleX, scaleY );
            return m.ToTransform();
        }

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
        public Transform Scale( float scaleX, float scaleY, float centerX, float centerY )
        {
            MutableTransform m = ToMutable();
            m.Scale( scaleX, scaleY, centerX, centerY );
            return m.ToTransform();
        }

        /// <summary>
        /// Combines the current transform with a scaling.
        /// </summary>
        /// <param name="factors">Scaling factors.</param>
        public Transform Scale( Vector2f factors ) => Scale( factors.X, factors.Y );

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
        public Transform Scale( Vector2f factors, Vector2f center ) => Scale( factors.X, factors.Y, center.X, center.Y );

        /// <summary>
        /// Overload of binary operator * to combine two transforms.
        /// This call is equivalent to calling left.Combine(right).
        /// </summary>
        /// <param name="left">Left operand (the first transform).</param>
        /// <param name="right">Right operand (the second transform).</param>
        /// <returns>New combined transform</returns>
        public static Transform operator *( Transform left, Transform right ) => left.Combine( right );

        /// <summary>
        /// Overload of binary operator * to transform a point.
        /// This call is equivalent to calling left.TransformPoint(right).
        /// </summary>
        /// <param name="left">Left operand (the transform)</param>
        /// <param name="right">Right operand (the point to transform)</param>
        /// <returns>New transformed point</returns>
        public static Vector2f operator *( Transform left, Vector2f right )
        {
            return left.TransformPoint( right );
        }

        /// <summary>
        /// The identity transform (does nothing).
        /// </summary>
        public static readonly Transform Identity = new Transform( 1, 0, 0,
                                                                   0, 1, 0,
                                                                   0, 0, 1 );

        /// <summary>
        /// Provides a string describing the object.
        /// </summary>
        /// <returns>String description of the object.</returns>
        public override string ToString()
        {
            return string.Format( "[Transform]" +
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
        static extern Transform sfTransform_getInverse( in Transform transform );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern Vector2f sfTransform_transformPoint( in Transform transform, Vector2f point );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern FloatRect sfTransform_transformRect( in Transform transform, FloatRect rectangle );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_combine( ref Transform transform, ref Transform other );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_translate( ref Transform transform, float x, float y );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_rotate( ref Transform transform, float angle );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_rotateWithCenter( ref Transform transform, float angle, float centerX, float centerY );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_scale( ref Transform transform, float scaleX, float scaleY );

        [DllImport( CSFML.Graphics, CallingConvention = CallingConvention.Cdecl ), SuppressUnmanagedCodeSecurity]
        static extern void sfTransform_scaleWithCenter( ref Transform transform, float scaleX, float scaleY, float centerX, float centerY );
        #endregion
    }
}

