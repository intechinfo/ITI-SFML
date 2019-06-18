uniform float Opacity;
uniform sampler2D texture;

void main()
{
	vec4 color = texture2D(texture,gl_TexCoord[0].xy);
  if(color.a != 0.0)
	  color.a = Opacity;
	gl_FragColor = color;
}
