Shader "Custom/FlameTrailShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Texture de la flamme
        _Color ("Color", Color) = (1, 0.5, 0, 1) // Couleur de la flamme (orange par défaut)
        _Distortion ("Distortion Strength", Range(0, 1)) = 0.1
        _Fade ("Fade Amount", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha // Transparence

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            float _Fade;
            float _Distortion;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Ajout d'une légère distorsion
                float2 distortedUV = i.uv + sin(_Time.y * 5 + i.uv.y * 10) * _Distortion;

                // Récupère la texture avec le décalage UV
                fixed4 texColor = tex2D(_MainTex, distortedUV);

                // Applique la couleur et la transparence
                texColor *= _Color;
                texColor.a *= 1.0 - i.uv.y * _Fade; // Dissipation progressive

                return texColor;
            }
            ENDCG
        }
    }
}
