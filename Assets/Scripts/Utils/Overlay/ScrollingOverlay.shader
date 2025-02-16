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
                // Define a base resolution (Full HD: 1920x1080) that your current offset factors were tuned for.
                float2 baseRes = float2(1920.0, 1080.0);
                // _ScreenParams.xy contains the current screen width and height in pixels.
                float2 currentRes = _ScreenParams.xy;
                // Compute a scaling factor to adjust the offset for any resolution.
                float2 resFactor = currentRes / baseRes;

                // Multiply the camera's world position by your tuned constants,
                // then scale by the resolution factor.
                float2 cameraOffset = _WorldSpaceCameraPos.xy * float2(0.029, 0.05) * resFactor;

                // Use the standard UV from the canvas, add time-based scrolling and the resolution-adapted camera offset.
                float2 scrolledUV = frac(IN.uv + 0.2 * _Time.x + cameraOffset);

                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, scrolledUV) * 0.1;
                return texColor * IN.color;
            }
            ENDHLSL
        }
    }
}