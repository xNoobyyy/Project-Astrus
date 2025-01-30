Shader "Custom/WhiteOverlay"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Overlay("Overlay Amount", Range(0,1)) = 0.0
    }
    
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Overlay;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Lerp between original color and pure white
                // _Overlay = 0 -> original sprite color
                // _Overlay = 1 -> pure white
                fixed3 finalRGB = lerp(col.rgb, fixed3(1,1,1), _Overlay);

                // Keep alpha from the original sprite
                return fixed4(finalRGB, col.a);
            }
            ENDCG
        }
    }
}
