#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED
#define PI 3.141592f

void divideGrid_float(float2 UV, float4 col1, float4 col2, float4 col3, float4 col4, float4 col5, float4 col6, out float4 Out) {
    if (UV.x < 0.5f)
    {
        if (UV.y <= 0.33f)
        {
            Out = col5;
        }
        if (UV.y > 0.33f && UV.y <= 0.66f)
        {
            Out = col3;
        }
        if (UV.y > 0.66f && UV.y)
        {
            Out = col1;
        }
    }
    else
    {
        if (UV.y <= 0.33f)
        {
            Out = col6;
        }
        if (UV.y > 0.33f && UV.y <= 0.66f)
        {
            Out = col4;
        }
        if (UV.y > 0.66f && UV.y)
        {
            Out = col2;
        }
    }
}
#endif //MYHLSLINCLUDE_INCLUDED