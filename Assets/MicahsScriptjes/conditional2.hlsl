#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

void textureDetail_float(float Noise, float Min, float Max, Texture2D TexA, Texture2D TexB, float2 UVcoord, SamplerState sState, float4 Col, out float4 Out) {
	Out = Col;
}
#endif //MYHLSLINCLUDE_INCLUDED