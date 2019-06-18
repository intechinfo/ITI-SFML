uniform sampler2D texture;
uniform float u_resolutionX;
uniform float u_resolutionY;
uniform float u_time;
uniform float change;

float random(in float x)
  { return fract(sin(x)*43758.5453); }
float random(in vec2 st)
  { return fract(sin(dot(st.xy ,vec2(12.9898,78.233))) * 43758.5453); }

void main(){
	vec2 mamene = vec2(u_resolutionX,u_resolutionY);
    vec2 st = gl_FragCoord.st/mamene;
    st.x *= u_resolutionX/u_resolutionY;

	vec4 currentColor = texture2D(texture,gl_TexCoord[0].xy);
    vec3 color = currentColor.xyz;

    // Digits
    vec2 blocks_st = floor(st*6.);
    float t = u_time*.8+random(blocks_st);
    float time_i = floor(t);
    float time_f = fract(t);
    color.rgb += step(change,random(blocks_st+time_i))*(1.0-time_f);

    gl_FragColor = vec4( color , 1.0);
}
