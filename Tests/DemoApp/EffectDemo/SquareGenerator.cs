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
    /// "Disco" fragment shader.
    /// </summary>

    class SquareGenerator : Effect
    {
        Sprite _sprite;
        Shader _shader;

        public SquareGenerator(Font font)
            : base( "DISCO", "Square generator", "resources/disco.frag" , font )
        {
            //Load sprite
            _sprite = new Sprite( new Texture( "resources/forest.jpg" ) );

            //Load shader
            _shader = new Shader( null, null, "resources/disco.frag" );
            _shader.SetUniform( "change", 0.5f );

        }

        protected override void OnUpdate( float time, float x, float y )
        {
            //Update shader
            _shader.SetUniform( "u_resolutionX", 1280.0f );
            _shader.SetUniform( "u_resolutionY", 640.0f );
            _shader.SetUniform( "u_time", time );
            _shader.SetUniform( "texture", Shader.CurrentTexture );
            _shader.SetUniform( "change", (float) 1.5*x+y );
        }

        public override void Dispose()
        {
            _sprite.Dispose();
            _shader.Dispose();
        }

        protected override void OnDraw( IRenderTarget target, in RenderStates states )
        {
            target.Draw( (_sprite), new RenderStates( _shader ) );
        }
    }
}
