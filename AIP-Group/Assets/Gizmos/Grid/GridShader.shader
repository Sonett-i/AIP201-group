Shader "Custom/GridShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _GridColor("Grid Color", Color) = (0,0,0,1)
        _GridSpacing("Grid Spacing", Range(0.1, 10)) = 1.0
    }
        SubShader
        {
            Tags {
                "RenderType" = "Transparent"
                "Queue" = "Transparent"
            }

            ZWrite Off
            Blend One One

            CGPROGRAM
            #pragma surface surf Lambert

            struct Input
            {
                float2 uv_MainTex;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _GridColor;
            float _GridSpacing;

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 texColor = _Color;
                float2 gridUV = IN.uv_MainTex / _GridSpacing;
                float2 gridUVPixel = frac(gridUV * _GridSpacing) * _GridSpacing;
                float4 gridLines = step(0.01, fmod(gridUVPixel.x, 1)) + step(0.01, fmod(gridUVPixel.y, 1));
                fixed4 finalColor = texColor * (1 - gridLines * _GridColor);
                o.Albedo = finalColor.rgb;
                o.Alpha = 1;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
