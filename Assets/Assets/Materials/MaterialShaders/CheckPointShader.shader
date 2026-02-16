Shader "Custom/CheckPointShader"
{
   Properties
    {
        [HDR] _Color ("Color Principal", Color) = (1, 0, 0, 1)
        _MainTex ("Textura de Ruido (Greyscale)", 2D) = "white" {}
        _ScrollSpeed ("Velocidad de Movimiento", Float) = 0.8
        _FresnelPower ("Transparencia Central (Fresnel)", Range(0.1, 10)) = 3.0
        _Opacity ("Opacidad Maestra", Range(0, 1)) = 0.5
        _FlickerSpeed ("Velocidad de Parpadeo", Float) = 15.0
        _FlickerStrength ("Intensidad de Parpadeo", Range(0, 1)) = 0.1
    }

    SubShader
    {
        // Etiquetas para transparencia y renderizado en ambos lados
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
        LOD 100

        // Configuración de mezcla: Aditivo para efecto neón/luz
        ZWrite Off
        Blend One One
        Cull Off 

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                fixed4 color : COLOR; // Recibe Vertex Colors si la malla los tiene
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 normal : NORMAL;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _ScrollSpeed;
            float _FresnelPower;
            float _Opacity;
            float _FlickerSpeed;
            float _FlickerStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                // Dirección de la cámara para el efecto Fresnel
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                o.normal = UnityObjectToWorldNormal(v.normal);
                
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 1. Movimiento de la textura (Scrolling)
                float2 scrollingUV = i.uv;
                scrollingUV.y -= _Time.y * _ScrollSpeed;
                float noise = tex2D(_MainTex, scrollingUV).r;

                // 2. Efecto Fresnel (Hace el cilindro "hueco")
                float3 normal = normalize(i.normal);
                float3 viewDir = normalize(i.viewDir);
                float fresnel = 1.0 - saturate(dot(viewDir, normal));
                fresnel = pow(fresnel, _FresnelPower);

                // 3. Efecto de Parpadeo (Flicker)
                float flicker = 1.0 - (sin(_Time.y * _FlickerSpeed) * _FlickerStrength);

                // 4. Cálculo de Alpha Final
                // Combinamos ruido, fresnel, opacidad maestra y el alpha de los vértices
                float finalAlpha = noise * fresnel * _Opacity * i.color.a * flicker;

                // Resultado final con color HDR
                return _Color * finalAlpha;
            }
            ENDHLSL
        }
    }
    FallBack "Transparent/Cutout/VertexLit"
}