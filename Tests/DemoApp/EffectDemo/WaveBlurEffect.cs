using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoApp.EffectDemo
{
    /// <summary>
    /// "Wave" vertex shader + "blur" fragment shader.
    /// </summary>
    class WaveBlurEffect : Effect
    {
        readonly Text _text;
        readonly Shader _shader;

        public WaveBlurEffect( Font font )
            : base( "wave + blur", font )
        {
            // Create the text
            _text = new Text();
            _text.DisplayedString = "Praesent suscipit augue in velit pulvinar hendrerit varius purus aliquam.\n" +
                                     "Mauris mi odio, bibendum quis fringilla a, laoreet vel orci. Proin vitae vulputate tortor.\n" +
                                     "Praesent cursus ultrices justo, ut feugiat ante vehicula quis.\n" +
                                     "Donec fringilla scelerisque mauris et viverra.\n" +
                                     "Maecenas adipiscing ornare scelerisque. Nullam at libero elit.\n" +
                                     "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.\n" +
                                     "Nullam leo urna, tincidunt id semper eget, ultricies sed mi.\n" +
                                     "Morbi mauris massa, commodo id dignissim vel, lobortis et elit.\n" +
                                     "Fusce vel libero sed neque scelerisque venenatis.\n" +
                                     "Integer mattis tincidunt quam vitae iaculis.\n" +
                                     "Vivamus fringilla sem non velit venenatis fermentum.\n" +
                                     "Vivamus varius tincidunt nisi id vehicula.\n" +
                                     "Integer ullamcorper, enim vitae euismod rutrum, massa nisl semper ipsum,\n" +
                                     "vestibulum sodales sem ante in massa.\n" +
                                     "Vestibulum in augue non felis convallis viverra.\n" +
                                     "Mauris ultricies dolor sed massa convallis sed aliquet augue fringilla.\n" +
                                     "Duis erat eros, porta in accumsan in, blandit quis sem.\n" +
                                     "In hac habitasse platea dictumst. Etiam fringilla est id odio dapibus sit amet semper dui laoreet.\n";
            _text.Font = Font;
            _text.CharacterSize = 22;
            _text.Position = new Vector2f( 30, 20 );
            // Load the shader
            _shader = new Shader( "resources/wave.vert", null, "resources/blur.frag" );
        }

        protected override void OnUpdate( float time, float x, float y )
        {
            _shader.SetUniform( "wave_phase", time );
            _shader.SetUniform( "wave_amplitude", new Vector2f( x * 40, y * 40 ) );
            _shader.SetUniform( "blur_radius", (x + y) * 0.008F );
        }

        protected override void OnDraw( IRenderTarget target, in RenderStates states )
        {
            target.Draw( _text, states.WithShader( _shader ) );
        }

        public override void Dispose()
        {
            _text.Dispose();
            _shader.Dispose();
        }

    }

}
