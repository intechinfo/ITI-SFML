using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoApp.EffectDemo
{
    /// <summary>
    /// "Pixelate" fragment shader.
    /// </summary>
    class PixelateEffect : Effect
    {
        readonly Texture _texture;
        readonly Sprite _sprite;
        readonly Shader _shader;

        public PixelateEffect( Font font )
            : base( "pixelate", font )
        {
            // Load the texture and initialize the sprite
            _texture = new Texture( "resources/background.jpg" );
            _sprite = new Sprite( _texture );
            // Load the shader
            _shader = new Shader( null, null, "resources/pixelate.frag" );
            _shader.SetUniform( "texture", Shader.CurrentTexture );
        }

        protected override void OnUpdate( float time, float x, float y )
        {
            _shader.SetUniform( "pixel_threshold", (x + y) / 30 );
        }

        protected override void OnDraw( IRenderTarget target, in RenderStates states )
        {
            target.Draw( _sprite, states.WithShader( _shader ) );
        }

        public override void Dispose()
        {
            _texture.Dispose();
            _sprite.Dispose();
            _shader.Dispose();
        }
    }

}
