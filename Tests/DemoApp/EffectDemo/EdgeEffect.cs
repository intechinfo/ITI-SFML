using SFML.Graphics;
using System;
using System.Numerics;
using Rectangle = System.Drawing.Rectangle;

namespace DemoApp.EffectDemo
{
    /// <summary>"Edge" post-effect fragment shader</summary>
    class EdgeEffect : Effect
    {
        readonly RenderTexture _surface;
        readonly Texture _backgroundTexture;
        readonly Texture _entityTexture;
        readonly Sprite _backgroundSprite;
        readonly Sprite[] _entities;
        readonly Shader _shader;

        public EdgeEffect( Font font )
            : base( "edge post-effect", font )
        {
            // Creates the off-screen surface.
            _surface = new RenderTexture( 800, 600 );
            _surface.Smooth = true;

            // Load the textures
            _backgroundTexture = new Texture( "resources/sfml.png" );
            _backgroundTexture.Smooth = true;
            _entityTexture = new Texture( "resources/devices.png" );
            _entityTexture.Smooth = true;

            // Initialize the background sprite
            _backgroundSprite = new Sprite( _backgroundTexture );
            _backgroundSprite.Position = new Vector2( 135, 100 );

            // Load the moving entities
            _entities = new Sprite[6];
            for( var i = 0; i < _entities.Length; ++i )
            {
                _entities[i] = new Sprite( _entityTexture, new Rectangle( 96 * i, 0, 96, 96 ) );
            }

            // Load the shader
            _shader = new Shader( null, null, "resources/edge.frag" );
            _shader.SetUniform( "texture", Shader.CurrentTexture );
        }

        protected override void OnUpdate( float time, float x, float y )
        {
            _shader.SetUniform( "edge_threshold", 1 - (x + y) / 2 );

            // Update the position of the moving entities
            for( var i = 0; i < _entities.Length; ++i )
            {
                var posX = (float)Math.Cos( 0.25F * (time * i + (_entities.Length - i)) ) * 300 + 350;
                var posY = (float)Math.Sin( 0.25F * (time * (_entities.Length - i) + i) ) * 200 + 250;
                _entities[i].Position = new Vector2( posX, posY );
            }

            // Render the updated scene to the off-screen surface
            _surface.Clear( Color.White );
            _surface.Draw( _backgroundSprite );
            foreach( var entity in _entities )
                _surface.Draw( entity );
            _surface.Display();
        }

        protected override void OnDraw( IRenderTarget target, in RenderStates states )
        {
            target.Draw( new Sprite( _surface.Texture ), states.WithShader( _shader ) );
        }

        public override void Dispose()
        {
            _surface.Dispose();
            _backgroundTexture.Dispose();
            _entityTexture.Dispose();
            _backgroundSprite.Dispose();
            foreach( var e in _entities ) e.Dispose();
            _shader.Dispose();
        }

    }

}
