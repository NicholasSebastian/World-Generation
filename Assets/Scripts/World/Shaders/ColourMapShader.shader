Shader "Custom/ColourMapShader"
{
    // Written by Nicholas Sebastian Hendrata on 02/09/2022.

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        const static int maxColourCount = 12;

        uniform int colourCount;
        uniform float3 colours[maxColourCount];
        uniform float startHeights[maxColourCount];
        uniform float blends[maxColourCount];

        float inverseLerp(float a, float b, float value)
        {
            float ratio = (value - a) / (b - a);
            return saturate(ratio);
        }

        void surf (Input input, inout SurfaceOutputStandard output)
        {
            float height = input.worldPos.y;

            for (int i = 0; i < colourCount; i++)
            {
                float difference = height - startHeights[i];
                float drawStrength = inverseLerp(-blends[i], blends[i], difference);

                float3 prevColour = output.Albedo * (1 - drawStrength);
                float3 nextColour = colours[i] * drawStrength;

                output.Albedo = prevColour + nextColour;
            }
        }
        ENDCG
    }

    FallBack "Diffuse"
}
