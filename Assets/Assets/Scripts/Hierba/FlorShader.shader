Shader "Custom/ShaderFlor"
{
    Properties
    {
        _MainTex ("Textura Albedo (RGBA)", 2D) = "white" {}
        _BaseColor ("Color Base (Tinte)", Color) = (1, 1, 1, 1)
        
        [Header(Configuracion del Viento)]
        _WindSpeed ("Velocidad del Viento", Range(0, 10)) = 2.0
        _WindStrength ("Fuerza del Viento", Range(0, 2)) = 0.3
        _WindFrequency ("Frecuencia de Ola", Range(0, 1)) = 0.2
        
        [Header(Configuracion de Interaccion VR)]
        _InteractionRadius ("Radio de Mano/Pie", Range(0.002, 2)) = 0.009
        _InteractionStrength ("Fuerza de Empuje", Range(0, 5)) = 1.5

        [Header(Variacion de Color por Ruido)]
        _NoiseScale ("Escala del Ruido (Tamano Manchas)", Range(0.001, 0.5)) = 0.02
        _DarkenFactor ("Factor Oscurecer", Range(0, 1)) = 0.25
        _LightenFactor ("Factor Aclarar", Range(0, 1)) = 0.15
    }

    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline" 
            "Queue"="Geometry"
            "CastShadows"="False"
        }
        Cull Off 
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR; // <-- LEER COLOR DE VÉRTICE DESDE EL MESH
                UNITY_VERTEX_INPUT_INSTANCE_ID 
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR; 
                float fogFactor     : TEXCOORD1;
                half3 noiseColorTint : TEXCOORD4; 
                UNITY_VERTEX_OUTPUT_STEREO     
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _BaseColor;
                float _WindSpeed;
                float _WindStrength;
                float _WindFrequency;
                float _InteractionRadius;
                float _InteractionStrength;
                float _NoiseScale;
                float _DarkenFactor;
                float _LightenFactor;
            CBUFFER_END

            Texture2D _MainTex;
            SamplerState sampler_MainTex;

            float4 _VRInteractorPositions[4]; 
            int _VRInteractorCount;

            float RealWorldNoise(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453123);
            }

            float ValueNoise2D(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float2 u = f * f * (3.0 - 2.0 * f);

                float a = RealWorldNoise(i + float2(0.0, 0.0));
                float b = RealWorldNoise(i + float2(1.0, 0.0));
                float c = RealWorldNoise(i + float2(0.0, 1.0));
                float d = RealWorldNoise(i + float2(1.0, 1.0));

                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);

                // --- CALCULO DE COLOR POR RUEDO ---
                float noiseVal = ValueNoise2D(worldPos.xz * _NoiseScale); 
                float tintMap = (noiseVal * 2.0) - 1.0;
                
                half3 finalTint = half3(1.0, 1.0, 1.0);
                if (tintMap < 0.0)
                {
                    finalTint -= _DarkenFactor * abs(tintMap);
                }
                else
                {
                    finalTint += _LightenFactor * tintMap;
                }
                output.noiseColorTint = finalTint;

                // --- MÁSCARA POR VERTEX COLOR ---
                // Usamos el canal Rojo del vertex color. 
                // Asegúrate de que en el mesh, la base sea negra (0) y el capullo sea blanco (1).
                float mask = saturate(input.color.r); 

                // --- VIENTO ---
                float time = _Time.y * _WindSpeed;
                float windWave = sin(time + (worldPos.x + worldPos.z) * _WindFrequency);
                windWave += cos(time * 1.5 + worldPos.y) * 0.2;

                worldPos.x += windWave * _WindStrength * mask;
                worldPos.z += windWave * (_WindStrength * 0.5) * mask;

                // --- INTERACCIÓN VR ---
                float3 totalPush = float3(0, 0, 0);

                if (_VRInteractorCount > 0 && _VRInteractorCount <= 4)
                {
                    for (int i = 0; i < _VRInteractorCount; i++)
                    {
                        float3 interactorPos = _VRInteractorPositions[i].xyz;
                        float isActive = _VRInteractorPositions[i].w;

                        if (isActive > 0.5)
                        {
                            float3 dirToGrass = worldPos - interactorPos;
                            float distanceToHand = length(dirToGrass);
                            
                           if (distanceToHand > 0.001 && distanceToHand < _InteractionRadius)
                           {
                                float attenuation = saturate(1.0 - (distanceToHand / _InteractionRadius));
                                attenuation = smoothstep(0, 1, attenuation);

                                float3 pushDir = dirToGrass / distanceToHand; 
                                totalPush.xz += pushDir.xz * attenuation * _InteractionStrength;
                                totalPush.y += pushDir.y * attenuation * (_InteractionStrength * 0.4);
                           }
                        }
                    }
                }

                worldPos += totalPush * mask;

                output.positionCS = TransformWorldToHClip(worldPos);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color;
                output.fogFactor = ComputeFogFactor(output.positionCS.z);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                half4 texColor = _MainTex.Sample(sampler_MainTex, input.uv);
                half4 finalColor = texColor * _BaseColor;
                
                finalColor.rgb *= input.noiseColorTint;
                
                half3 lightDir = _MainLightPosition.xyz;
                half3 lightColor = _MainLightColor.rgb;
                half NdotL = saturate(dot(half3(0, 1, 0), lightDir)); 
                
                finalColor.rgb *= (NdotL * 0.4 + 0.6) * lightColor;
                finalColor.rgb = MixFog(finalColor.rgb, input.fogFactor);

                return finalColor;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}