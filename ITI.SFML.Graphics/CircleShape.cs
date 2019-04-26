using System;
using SFML.System;

namespace SFML.Graphics
{
    /// <summary>
    /// Specialized shape representing a circle
    /// </summary>
    public class CircleShape : Shape
    {
        float _radius;
        uint _pointCount;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CircleShape()
            : this(0)
        {
        }

        /// <summary>
        /// Constructs the shape with an initial radius and point count.
        /// </summary>
        /// <param name="radius">Radius of the shape</param>
        /// <param name="pointCount">Number of points of the shape</param>
        public CircleShape(float radius, uint pointCount = 30 )
        {
            Radius = radius;
            SetPointCount(pointCount);
        }

        /// <summary>
        /// Constructs the shape from another shape.
        /// </summary>
        /// <param name="copy">Shape to copy</param>
        public CircleShape(CircleShape copy)
            : base(copy)
        {
            Radius = copy.Radius;
            SetPointCount(copy.GetPointCount());
        }

        /// <summary>
        /// Gets or sets the radius of the shape.
        /// </summary>
        public float Radius
        {
            get { return _radius; }
            set { _radius = value; Update(); }
        }

        /// <summary>
        /// Gets the total number of points of the circle.
        /// </summary>
        /// <returns>The total point count.</returns>
        public override uint GetPointCount()
        {
            return _pointCount;
        }

        /// <summary>
        /// Sets the number of points of the circle.
        /// The count must be greater than 2 to define a valid shape.
        /// </summary>
        /// <param name="count">New number of points of the circle.</param>
        public void SetPointCount(uint count)
        {
            _pointCount = count;
            Update();
        }

        /// <summary>
        /// Get the position of a point
        /// <para>
        /// The returned point is in local coordinates, that is,
        /// the shape's transforms (position, rotation, scale) are
        /// not taken into account.
        /// The result is undefined if index is out of the valid range.
        /// </para>
        /// </summary>
        /// <param name="index">Index of the point to get, in range [0 .. PointCount - 1].</param>
        /// <returns>index-th point of the shape.</returns>
        public override Vector2f GetPoint(uint index)
        {
            float angle = (float)(index * 2 * Math.PI / _pointCount - Math.PI / 2);
            float x = (float)Math.Cos(angle) * _radius;
            float y = (float)Math.Sin(angle) * _radius;
            return new Vector2f(_radius + x, _radius + y);
        }

    }
}
