Shader "Custom/EstaticaShader"
{
    Properties
    {
        _Color1 ("Color Estática A", Color) = (0, 0, 0, 1)
        _Color2 ("Color Estática B", Color) = (0.5, 0.5, 0.5, 1)
        _Speed ("Velocidad", Float) = 240.0
        _GrainSize ("Tamaño Grano", Float) = 1000000.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+500" }
        LOD 100
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 viewDir : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            fixed4 _Color1;
            fixed4 _Color2;
            float _Speed;
            float _GrainSize;

            float hash(float3 p) {
                p = frac(p * 0.3183099 + 0.1);
                p *= 17.0;
                return frac(p.x * p.y * p.z * (p.x + p.y + p.z));
            }

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.viewDir = v.uv;
                
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float3 dir = normalize(i.viewDir);
                float noiseSeed = _Time.y * _Speed;
                float3 samplePos = floor(dir * _GrainSize + noiseSeed);
                
                float n = hash(samplePos);
                fixed4 finalColor = lerp(_Color1, _Color2, n);

                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                return finalColor;
            }
            ENDCG
        }
    }
}