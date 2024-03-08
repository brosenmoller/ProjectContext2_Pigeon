#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED
#define PI 3.141592f

void Rotate_float(float Green, float2 UV, out float2 Out) {
    float radians = Green * 2 * PI;
    float2 pivot = float2(0.5f, 0.5f);
    float cosAngle = cos(radians);
    float sinAngle = sin(radians);
    float2x2 rot = float2x2(cosAngle, -sinAngle, sinAngle, cosAngle);

    float2 uv = UV - pivot;
    float2 rotUV = mul(rot, uv);
    Out = rotUV + pivot;
}
#endif //MYHLSLINCLUDE_INCLUDED