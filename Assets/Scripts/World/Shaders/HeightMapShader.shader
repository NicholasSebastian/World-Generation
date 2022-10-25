Shader "Custom/HeightMapShader"
{
    // Written by Nicholas Sebastian Hendrata on 02/09/2022.

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        uniform float minHeight;
        uniform float maxHeight;

        float inverseLerp(float a, float b, float value)
        {
            float ratio = (value - a) / (b - a);
            return saturate(ratio);
        }

        void surf (Input input, inout SurfaceOutputStandard output)
        {
            float height = inverseLerp(minHeight, maxHeight, input.worldPos.y);
            output.Albedo = height;
        }
        ENDCG
    }
}
