Shader "Custom/ScrollingOverlay"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "LightMode"="Universal2D"
        }
        Pass
        {
            ZWrite Off
            Cull Off
            Blend SrcColor OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float2 _ScrollSpeed;
            float _Scale;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                float2 texcoord0 : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.worldPos = mul(unity_ObjectToWorld, IN.positionOS).xyz;
                OUT.uv = IN.texcoord0;
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Use the standard UV from the canvas, add time-based scrolling and the resolution-adapted camera offset.
                float2 scrolledUV = frac((IN.uv * 75) + 0.2 * _Time.x);

                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, scrolledUV) * 0.1;
                return texColor * IN.color;
            }
            ENDHLSL
        }
    }
}