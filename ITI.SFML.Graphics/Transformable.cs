using System;
using SFML.System;

namespace SFML.Graphics
{
    /// <summary>
    /// Decomposed transform defined by a position, a rotation and a scale.
    /// </summary>
    /// <remarks>
    /// A note on coordinates and undistorted rendering:
    /// By default, SFML (or more exactly, OpenGL) may interpolate drawable objects
    /// such as sprites or texts when rendering. While this allows transitions
    /// like slow movements or rotations to appear smoothly, it can lead to
    /// unwanted results in some cases, for example blurred or distorted objects.
    /// In order to render a SFML.Graphics.Drawable object pixel-perfectly, make sure
    /// the involved coordinates allow a 1:1 mapping of pixels in the window
    /// to texels (pixels in the texture). More specifically, this means:
    /// * The object's position, origin and scale have no fractional part
    /// * The object's and the view's rotation are a multiple of 90 degrees
    /// * The view's center and size have no fractional part
    /// </remarks>
    public class Transformable : ObjectBase
    {
        Vector2f _origin = new Vector2f(0, 0);
        Vector2f _position = new Vector2f(0, 0);
        float _rotation = 0;
        Vector2f _scale = new Vector2f(1, 1);
        Transform _transform;
        Transform _inverseTransform;
        bool _transformNeedUpdate = true;
        bool _inverseNeedUpdate = true;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Transformable() 
            : base(IntPtr.Zero)
        {
        }

        /// <summary>
        /// Constructs the transformable from another transformable.
        /// </summary>
        /// <param name="transformable">Transformable to copy.</param>
        public Transformable(Transformable transformable) 
            : base(IntPtr.Zero)
        {
            Origin = transformable.Origin;
            Position = transformable.Position;
            Rotation = transformable.Rotation;
            Scale = transformable.Scale;
        }

        /// <summary>
        /// Gets or sets the position of this object.
        /// </summary>
        public Vector2f Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                _transformNeedUpdate = true;
                _inverseNeedUpdate = true;
            }
        }

        /// <summary>
        /// Gets or sets the rotation of this objec in degrees.
        /// </summary>
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                _transformNeedUpdate = true;
                _inverseNeedUpdate = true;
            }
        }

        /// <summary>
        /// Gets or sets the scale of this object.
        /// </summary>
        public Vector2f Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
                _transformNeedUpdate = true;
                _inverseNeedUpdate = true;
            }
        }

        /// <summary>
        /// Gets or sets the origin of this object.
        /// The origin of an object defines the center point for
        /// all transformations (position, scale, rotation).
        /// The coordinates of this point must be relative to the
        /// top-left corner of the object, and ignores all
        /// transformations (position, scale, rotation).
        /// </summary>
        public Vector2f Origin
        {
            get
            {
                return _origin;
            }
            set
            {
                _origin = value;
                _transformNeedUpdate = true;
                _inverseNeedUpdate = true;
            }
        }

        /// <summary>
        /// Gets the combined transform of the object.
        /// </summary>
        public Transform Transform
        {
            get
            {
                if (_transformNeedUpdate)
                {
                    _transformNeedUpdate = false;

                    float angle = -_rotation * 3.141592654F / 180.0F;
                    float cosine = (float)Math.Cos(angle);
                    float sine = (float)Math.Sin(angle);
                    float sxc = _scale.X * cosine;
                    float syc = _scale.Y * cosine;
                    float sxs = _scale.X * sine;
                    float sys = _scale.Y * sine;
                    float tx = -_origin.X * sxc - _origin.Y * sys + _position.X;
                    float ty = _origin.X * sxs - _origin.Y * syc + _position.Y;

                    _transform = new Transform(sxc, sys, tx,
                                                -sxs, syc, ty,
                                                0.0F, 0.0F, 1.0F);
                }
                return _transform;
            }
        }

        /// <summary>
        /// Gets the combined transform of the object.
        /// </summary>
        public Transform InverseTransform
        {
            get
            {
                if (_inverseNeedUpdate)
                {
                    _inverseTransform = Transform.GetInverse();
                    _inverseNeedUpdate = false;
                }
                return _inverseTransform;
            }
        }

        /// <summary>
        /// Constructs the object from its internal C pointer.
        /// </summary>
        /// <param name="cPointer">Pointer to the object in the C library.</param>
        protected Transformable(IntPtr cPointer) 
            : base(cPointer)
        {
        }

        /// <summary>
        /// Handle the destruction of the object
        /// </summary>
        /// <param name="disposing">Is the GC disposing the object, or is it an explicit call ?</param>
        protected override void Destroy(bool disposing)
        {
            // Does nothing, this instance is either pure C# (if created by the user)
            // or not the final object (if used as a base for a drawable class)
        }

    }
}
