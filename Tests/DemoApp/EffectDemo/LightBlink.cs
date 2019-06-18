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
    /// "Light blink" fragment shader.
    /// </summary>
    /// 
    class LightBlink : Effect
    {
        Clock _clock;
        Sprite _sprite;
        Shader _shader;
        Vector2f _starterPoint;
        float _lightingDegree;
        int _duration;
        Sprite _background;

        public LightBlink(Font font)
            : base( "blink","Fairy on a couch", "resources/DegradeFromPoint.frag", font )

        {
            _duration = 5;

            //Load texture
            _sprite = new Sprite( new Texture( "resources/clochette.png" ) );

            //Initialize background Sprite
            _background = new Sprite( new Texture( "resources/couch.png" ) );

            //Initialize sprite
            _sprite.Origin = new Vector2f( _sprite.Texture.Size.X / 2, _sprite.Texture.Size.Y / 2 );
            _sprite.Position = new Vector2f( 400, 300 );

            _clock = new Clock();
            _starterPoint = new Vector2f( 400, 300 );

            //Load Shader
            _shader = new Shader( null, null, "resources/DegradeFromPoint.frag" );
            
        }

        public void ClockUpdate()
        {
            if( _clock.ElapsedTime.AsSeconds() > _duration )
            {
                _clock.Restart();
            }
        }

        public void LightingUpdate()
        {
            //Update the lighting
            if( _clock.ElapsedTime.AsSeconds() <= _duration / 2 )
            {
                _lightingDegree = (_clock.ElapsedTime.AsMilliseconds() / 1000f) / (_duration / 2);
            }
            else
            {
                _lightingDegree = 1 - (_clock.ElapsedTime.AsMilliseconds() / 1000.0f - (_duration / 2)) / (_duration / 2);
            }
        }

        protected override void OnUpdate( float time, float x, float y )
        {
            //Update duration+clock+lighting
            _duration = Math.Abs((int) ((x * 3) +5));
            ClockUpdate();
            LightingUpdate();

            //Update shader
            _shader.SetUniform("texture", Shader.CurrentTexture);
            _shader.SetUniform( "Xorigin", _starterPoint.X );
            _shader.SetUniform( "Yorigin", _starterPoint.Y );
            _shader.SetUniform( "range",  (100 * y) + 120 );
            _shader.SetUniform( "lighting", _lightingDegree );
            _shader.SetUniform( "texture", Shader.CurrentTexture );
        }

        public override void Dispose()
        {
            _sprite.Dispose();
            _shader.Dispose();
            _background.Dispose();
        }

        protected override void OnDraw( IRenderTarget target, in RenderStates states )
        {
            target.Draw( _background );
            target.Draw( (_sprite), new RenderStates( _shader ) );
        }
    }
}
