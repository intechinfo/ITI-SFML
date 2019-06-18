using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoApp.EffectDemo
{
    /// <summary>
    /// "Storm" vertex shader + "blink" fragment shader.
    /// </summary>
    class StormBlinkEffect : Effect
    {

        readonly VertexArray _points;
        readonly Shader _shader;

        public StormBlinkEffect( Font font )
            : base( "storm + blink", "Dust of storm arround cursor", "resources/blink.frag", font )
        {
            var random = new Random();
            // Create the points
            _points = new VertexArray( PrimitiveType.Points );
            for( var i = 0; i < 40000; ++i )
            {
                var x = (float)random.Next( 0, 800 );
                var y = (float)random.Next( 0, 600 );
                var r = (byte)random.Next( 0, 255 );
                var g = (byte)random.Next( 0, 255 );
                var b = (byte)random.Next( 0, 255 );
                _points.Append( new Vertex( new Vector2f( x, y ), new Color( r, g, b ) ) );
            }

            // Load the shader
            _shader = new Shader( "resources/storm.vert", null, "resources/blink.frag" );
        }

        protected override void OnUpdate( float time, float x, float y )
        {
            var radius = 200 + (float)Math.Cos( time ) * 150;
            _shader.SetUniform( "storm_position", new Vector2f( x * 1300, y * 600 ) );
            _shader.SetUniform( "storm_inner_radius", radius / 3 );
            _shader.SetUniform( "storm_total_radius", radius );
            _shader.SetUniform( "blink_alpha", 0.5F + (float)Math.Cos( time * 3 ) * 0.25F );
        }

        protected override void OnDraw( IRenderTarget target, in RenderStates states )
        {
            target.Draw( _points, states.WithShader( _shader ) );
        }

        public override void Dispose()
        {
            _points.Dispose();
            _shader.Dispose();
        }
    }

}
