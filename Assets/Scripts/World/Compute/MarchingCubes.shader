Shader "Custom/MarchingCubes"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off

        CGPROGRAM

        #pragma surface surf Standard vertexvert fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            // TODO
        };

        void vert(inout appdata v)
        {
            // TODO
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // TODO
        }

        ENDCG
    }
    FallBack "Diffuse"
}
