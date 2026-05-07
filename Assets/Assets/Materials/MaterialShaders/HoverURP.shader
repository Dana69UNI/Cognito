Shader "Custom/HoverStaticSubtle_URP"
{
    Properties
    {
        _BaseColor("Color de Fondo", Color) = (0.5, 0.5, 0.5, 1)
        _MainTex("Textura (Opcional)", 2D) = "white" {}
        
        [Header(Estatica Sutil)]
        _StaticColor("Color de la Estática", Color) = (1, 1, 1, 1)
        _Intensity("Intensidad (Sugerido 0.2)", Range(0, 1)) = 0.2
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float3 normalOS     : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionWS   : TEXCOORD3;
                float2 uv           : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
                float4 shadowCoord  : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _StaticColor;
                float _Intensity;
                float4 _MainTex_ST;
            CBUFFER_END

            sampler2D _MainTex;

            float hash(float3 p) {
                p = frac(p * 0.3183099 + 0.1);
                p *= 17.0;
                return frac(p.x * p.y * p.z * (p.x + p.y + p.z));
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = positionInputs.positionCS;
                OUT.positionWS = positionInputs.positionWS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.shadowCoord = GetShadowCoord(positionInputs);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, IN.uv);
                half3 baseAlbedo = texColor.rgb * _BaseColor.rgb;
                
                float3 normalWS = normalize(IN.normalWS);
                Light mainLight = GetMainLight(IN.shadowCoord);
                half shadow = mainLight.shadowAttenuation;
                half NdotL = saturate(dot(normalWS, mainLight.direction));
                
                half3 ambient = SampleSH(normalWS) * baseAlbedo;
                half3 diffuse = baseAlbedo * (mainLight.color * (NdotL * shadow));
                half3 finalSurfaceColor = diffuse + ambient;

                float3 noisePos = floor(IN.positionWS * 100000.0 + (_Time.y * 70.0));
                float noise = hash(noisePos);

                half3 finalColor = lerp(finalSurfaceColor, _StaticColor.rgb, noise * _Intensity);

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}