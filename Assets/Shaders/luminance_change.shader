Shader "Postprocess/LuminanceChange" {
	Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Multiplier ("Multiplier", Range(0, 1)) = 1
    }
    SubShader {
        Pass {
            Cull Off
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
            uniform sampler2D _MainTex;
            uniform float _Multiplier;

            float4 frag(v2f_img input) : COLOR {
                 float4 result;
                 result.rgb = tex2D(_MainTex, input.uv) * _Multiplier;
                 return result;
            }

            ENDCG
        }
    }
}
