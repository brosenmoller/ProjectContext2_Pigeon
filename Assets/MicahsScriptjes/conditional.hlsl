#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

void ChooseTex_float(float Red, Texture2D Tex1, Texture2D Tex2, Texture2D Tex3, Texture2D Tex4, Texture2D Tex5, Texture2D Tex6, Texture2D Tex7, Texture2D Tex8, SamplerState SS, float2 UV, float TexNumber, out float4 Out) {
    if (Red < 1 / TexNumber)
    {
        Out = SAMPLE_TEXTURE2D(Tex1, SS, UV);
    }
    if (Red >= 1 / TexNumber && Red < 2 / TexNumber)
    {
        Out = SAMPLE_TEXTURE2D(Tex2, SS, UV);
    }
    if (Red >= 2 / TexNumber && Red < 3 / TexNumber)
    {
        Out = SAMPLE_TEXTURE2D(Tex3, SS, UV);
    }
    if (Red >= 3 / TexNumber && Red < 4 / TexNumber)
    {
        Out = SAMPLE_TEXTURE2D(Tex4, SS, UV);
    }
    if (Red >= 4 / TexNumber && Red < 5 / TexNumber)
    {
        Out = SAMPLE_TEXTURE2D(Tex5, SS, UV);
    }
    if (Red >= 5 / TexNumber && Red < 6 / TexNumber)
    {
        Out = SAMPLE_TEXTURE2D(Tex6, SS, UV);
    }
    if (Red >= 6 / TexNumber && Red < 7 / TexNumber)
    {
        Out = SAMPLE_TEXTURE2D(Tex7, SS, UV);
    }
    if (Red >= 7 / TexNumber)
    {
        Out = SAMPLE_TEXTURE2D(Tex8, SS, UV);
    }
}
#endif //MYHLSLINCLUDE_INCLUDED