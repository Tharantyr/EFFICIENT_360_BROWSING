Shader "Postprocess/Blur" {
	Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _HorDist ("Horizontal Distance", Float) = 0.1
        _VertDist ("Vertical Distance", Float) = 0.1
    }
    SubShader {
        Pass {
            Cull Off
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
            uniform sampler2D _MainTex;
            uniform float _HorDist;
            uniform float _VertDist;
            float coef_sum = 0;

            float4 sample_point(float offset, v2f_img input) {
                float sqr_dist = offset * offset;
                float coef = exp(-offset * offset * 2);
                coef_sum += coef;
                return tex2D(_MainTex, input.uv + float2(offset * _HorDist, offset * _VertDist)) * coef;
            }

            float4 frag(v2f_img input) : COLOR {
                 float4 result = float4(0, 0, 0, 1);
                 coef_sum = 0;

                 result.rgb += sample_point(-1.0, input);
                 result.rgb += sample_point(-0.9, input);
                 result.rgb += sample_point(-0.8, input);
                 result.rgb += sample_point(-0.7, input);
                 result.rgb += sample_point(-0.6, input);
                 result.rgb += sample_point(-0.5, input);
                 result.rgb += sample_point(-0.4, input);
                 result.rgb += sample_point(-0.3, input);
                 result.rgb += sample_point(-0.2, input);
                 result.rgb += sample_point(-0.1, input);
                 result.rgb += sample_point(1.0, input);
                 result.rgb += sample_point(0.9, input);
                 result.rgb += sample_point(0.8, input);
                 result.rgb += sample_point(0.7, input);
                 result.rgb += sample_point(0.6, input);
                 result.rgb += sample_point(0.5, input);
                 result.rgb += sample_point(0.4, input);
                 result.rgb += sample_point(0.3, input);
                 result.rgb += sample_point(0.2, input);
                 result.rgb += sample_point(0.1, input);

                 result.rgb /= coef_sum;
                 return result;
            }

            ENDCG
        }
    }
}
