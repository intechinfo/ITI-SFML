using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;

namespace DemoApp.EffectDemo
{
    /// <summary>
    /// "Opacity" fragment shader.
    /// </summary>

    class Opacity : Effect
    {
        Sprite _sprite;
        Sprite _background;
        Shader _shader;

        public Opacity(Font font)
            : base( "opacity", "Gentleman ghost with changing Opacity \n and Size", "resources/opacity.frag", font )
        {
            // Load texture
            _sprite = new Sprite( new Texture( "resources/ghost1.png" ) );
            _background = new Sprite( new Texture( "resources/forest.jpg" ) );

            //Load Shader
            _shader = new Shader( null, null, "resources/opacity.frag" );

            //Initialize sprite position and size
            _sprite.Origin = new Vector2f( _sprite.Texture.Size.X / 2, _sprite.Texture.Size.Y / 2 );
            _sprite.Position = new Vector2f( 400, 300 );
            _sprite.Scale = new Vector2f( 1.6f, 1.6f );

        }

        protected override void OnUpdate( float time, float x, float y )
        {
            //Update opacity + size
            _shader.SetUniform( "Opacity", (x / 2.0f)*2 );
            _sprite.Scale = new Vector2f( Math.Abs( y )*2, Math.Abs( y )*2 );
            _shader.SetUniform( "texture", Shader.CurrentTexture );
        }

        public override void Dispose()
        {
            _sprite.Dispose();
            _shader.Dispose();
        }

        protected override void OnDraw( IRenderTarget target, in RenderStates states )
        {
            target.Draw( (_background) );
            target.Draw( (_sprite), new RenderStates( _shader ) );
        }

    }
}
