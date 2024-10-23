Shader "LensShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _dirtTex ("Dirt texture", 2D) = "white" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            
            uniform int _time;
            uniform float _vignetteRadius, _vignetteFeather, _aberrationStrength, _aspectRatio;
            uniform bool _useNoise;

            float hash(uint n) {
				// integer hash copied from Hugo Elias
				n = (n << 13U) ^ n;
				n = n * (n * n * 15731U + 0x789221U) + 0x1376312589U;
				return float(n & uint(0x7fffffffU)) / float(0x7fffffff);
			}

            float ratioLength(float2 uv)
            {
                uv.x = uv.x * _aspectRatio;

                return length(uv);
            }

            float vignetteFalloff(float2 uv)
            {
                float radius = 0.2;
                float circle = ratioLength(2 * uv);
                float mask = smoothstep(_vignetteRadius, _vignetteRadius + _vignetteFeather, circle);
                return mask;
            }

            float4 frag(v2f_img i) : SV_TARGET
            {
                float4 chromColour, result;
                float vignetteStrength;

                float2 uv_centered = i.uv - 0.5;

                float rOffset = uv_centered * _aberrationStrength;
                float bOffset = -rOffset;

                float r = dot(tex2D(_MainTex, i.uv + rOffset), float3(1.0f, 0.0f, 0.0f));
                float g = dot(tex2D(_MainTex, i.uv), float3(0.0f, 1.0f, 0.0f)); 
                float b = dot(tex2D(_MainTex, i.uv + bOffset), float3(0.0f, 0.0f, 1.0f));
                chromColour.rgb = float3(r, g, b);
                
                vignetteStrength = vignetteFalloff(uv_centered);       
                
                if(_useNoise)
                {
                    uint2 val = 100*i.uv * 2 - 1;
                    uint seed = val.x + 100 * val.y + 100 * _time;
                    float rand = lerp(0, 1, hash(seed));
                    if(rand > 0.999)
                    chromColour.rgb += 0.2 * rand;
                }

                result.rgb = min(1, (1 - vignetteStrength)) * chromColour.rgb;

                return result;
            }
            ENDCG
        }
    }
}