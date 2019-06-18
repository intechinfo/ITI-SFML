uniform float Xorigin;
uniform float Yorigin;
uniform float range;
uniform float lighting;
uniform sampler2D texture;

void main()
{
	vec2 target = gl_FragCoord.xy;
	vec2 origin = vec2(Xorigin,Yorigin);
	float distance = sqrt((target.x - origin.x)*(target.x - origin.x)+
  (target.y - origin.y)*(target.y - origin.y));

	float distanceInRangeOnPercent = min(1,distance/range);
	if(distanceInRangeOnPercent > 1.0)
		distanceInRangeOnPercent = 0.0;

	distanceInRangeOnPercent = 1 - distanceInRangeOnPercent;

	vec4 color = texture2D(texture,gl_TexCoord[0].xy);
	if (color.a != 0.0)
		color.a = distanceInRangeOnPercent * lighting;
	gl_FragColor = color; 
}
