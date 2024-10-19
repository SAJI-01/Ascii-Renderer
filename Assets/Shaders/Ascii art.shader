Shader "Custom/ASCII_ArtShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ASCIITex ("ASCII Texture", 2D) = "white" {}
        _Resolution ("Resolution", Float) = 8
    }
    
    SubShader
    {
        Tags {"RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
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
            
            float _Resolution;
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }
            
            float4 frag(Varyings IN) : SV_Target
            {
                // Sample the main texture
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                
                // Calculate luminance
                float luminance = dot(col.rgb, float3(0.299, 0.587, 0.114));
                
                // Quantize luminance to 10 steps
                luminance = floor(luminance * 10) / 10;
                
                // Calculate ASCII texture coordinates
                float2 asciiUV;
                asciiUV.x = fmod(IN.uv.x * _ScreenParams.x, _Resolution) / _Resolution + luminance;
                asciiUV.y = fmod(IN.uv.y * _ScreenParams.y, _Resolution) / _Resolution;
                
                // Sample the ASCII texture
                float4 asciiChar = SAMPLE_TEXTURE2D(_ASCIITex, sampler_ASCIITex, asciiUV);
                
                // Combine original color with ASCII character
                return col * asciiChar;
            }
            ENDHLSL
        }
    }
}