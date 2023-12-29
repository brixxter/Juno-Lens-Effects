Shader "LensShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _dirtTex ("Dirt texture", 2D) = "white" {}
        _vignetteTex ("Vignette texture", 2D) = "white" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            uniform sampler2D _vignetteTex;

            uniform float4 _LensDistortionOffset_R;
            uniform float4 _LensDistortionOffset_G;
            uniform float4 _LensDistortionOffset_B;

            uniform float _vignetteIntensity;
         
            uniform float  _aberrationStrength;
            uniform float _Size;
            uniform float _Falloff;

            fixed4 frag(v2f_img i) : Color {
                float4 chromColour;
               
                float4 result;

                float2 uv_centered = i.uv - 0.5;
  
                float vignette = 1.0f - smoothstep(_Size, _Size - _Falloff, length(uv_centered));

                float rOffset= uv_centered * vignette * _aberrationStrength;
                float bOffset = -rOffset;

                float r = dot(tex2D(_MainTex, i.uv + rOffset), float3(1.0f, 0.0f, 0.0f));
                float g = dot(tex2D(_MainTex, i.uv), float3(0.0f, 1.0f, 0.0f)); 
                float b = dot(tex2D(_MainTex, i.uv + bOffset), float3(0.0f, 0.0f, 1.0f));
                chromColour.rgb = float3(r, g, b);

                float4 vignetteCol = tex2D(_vignetteTex, i.uv);
                result.rgb = min(1,(1-vignetteCol.a*_vignetteIntensity))*chromColour.rgb;
                return result;
            }
            ENDCG
        }
    }
}