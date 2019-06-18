using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoApp.EffectDemo
{
    /// <summary>
    /// Base class for effects.
    /// </summary>
    abstract class Effect : IDrawable, IDisposable
    {
        protected Effect( string name, string explain, string code, Font font )
        {
            Font = font;
            Name = name;
            Explain = explain;
            Code = code;
        }

        public string Name { get; }

        public string Explain { get; }

        public string Code { get; }

        protected Font Font { get; }

        public void Update( float time, float x, float y )
        {
            if( Shader.IsAvailable )
                OnUpdate( time, x, y );
        }

        public void Draw( IRenderTarget target, in RenderStates states )
        {
            if( Shader.IsAvailable )
            {
                OnDraw( target, states );
            }
            else
            {
                var error = new Text( "Shader not\nsupported", Font );
                error.Position = new Vector2f( 320, 200 );
                error.CharacterSize = 36;
                target.Draw( error, states );
            }
        }

        protected abstract void OnUpdate( float time, float x, float y );

        protected abstract void OnDraw( IRenderTarget target, in RenderStates states );

        public abstract void Dispose();

    }

}
