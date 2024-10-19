Shader "Custom/ASCIIShaderURPCustomizable"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ASCIITex ("ASCII Texture", 2D) = "white" {}
        _CharCount ("Character Count", Float) = 10
        _CharSize ("Character Size", Float) = 8
    }
    
    SubShader
    {
        Tags {"RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        ENDHLSL

        Pass
        {
            Name "ASCII"
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_ASCIITex);
            SAMPLER(sampler_ASCIITex);
            
            float _CharCount;
            float _CharSize;

            static const float3 charValues = float3(0.2126, 0.7152, 0.0722);

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 screenPos = IN.uv * _ScreenParams.xy;
                float2 charPos = floor(screenPos / _CharSize);
                float2 charUV = frac(screenPos / _CharSize);
                
                float2 sampleUV = (charPos + 0.5) * _CharSize / _ScreenParams.xy;
                float3 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, sampleUV).rgb;
                
                float brightness = dot(color, charValues);
                float charIndex = floor(brightness * (_CharCount - 1));
                
                float2 asciiUV = float2(charUV.x, (charIndex + charUV.y) / _CharCount);
                float ascii = SAMPLE_TEXTURE2D(_ASCIITex, sampler_ASCIITex, asciiUV).r;
                
                return half4(ascii, ascii, ascii, 1);
            }
            ENDHLSL
        }
    }
}