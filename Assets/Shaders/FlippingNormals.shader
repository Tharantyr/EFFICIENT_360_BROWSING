Shader "Custom/FlippingNormals" {
	Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Pass {
            Cull Off
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
            sampler2D _MainTex;



            float4 frag(v2f_img input) : COLOR {
                 float4 result;
                 result.rgb = tex2D(_MainTex, float2(1.0 - input.uv.x, input.uv.y));
                 return result;
            }

            ENDCG
        }
    }
}
