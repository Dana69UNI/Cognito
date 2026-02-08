Shader "Custom/HoverURP"
{
    Properties
    {
        _BaseColor("Color de Fondo", Color) = (0.5, 0.5, 0.5, 1)
        _MainTex("Textura (Opcional)", 2D) = "white" {}
        
        [Header(Configuracion de Puntos)]
        _DotColor("Color de los Puntos", Color) = (1, 0, 0, 1)
        _DotSize("Tamaño Puntos", Range(0.01, 0.5)) = 0.1
        _Density("Densidad", Range(1, 100)) = 20
        
        [Header(Efecto Pulso)]
        _PulseSpeed("Velocidad Latido", Range(0, 10)) = 3.0
        _MaxEmission("Brillo Máximo", Range(0, 5)) = 0.7
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
                float2 uv           : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
                float4 shadowCoord  : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _DotColor;
                float _DotSize;
                float _Density;
                float _PulseSpeed;
                float _MaxEmission;
                float4 _MainTex_ST;
            CBUFFER_END

            sampler2D _MainTex;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = positionInputs.positionCS;
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

             
                float2 st = frac(IN.uv * _Density);
                float dist = distance(st, float2(0.5, 0.5));
                float dotMask = 1.0 - smoothstep(_DotSize, _DotSize + 0.01, dist);

              
                float pulse = sin(_Time.y * _PulseSpeed) * 0.5 + 0.5;
               
                float effectFactor = dotMask * pulse;

               
                half3 emissiveDot = _DotColor.rgb * _MaxEmission;
                
             
                half3 finalColor = lerp(finalSurfaceColor, emissiveDot, effectFactor);

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}