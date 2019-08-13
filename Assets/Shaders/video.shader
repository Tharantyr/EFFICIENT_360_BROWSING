 Shader "Custom/VideoShader" {
     Properties {
        _MainTex("Main Texture", 2D) = "white" {}
	_Alpha("Alpha", float) = 0.5
     }
     SubShader {
        Tags { "Queue"="Transparent" "RenderType" = "Opaque" }
  
  
        CGPROGRAM
        #pragma surface surf Lambert alpha

        uniform float _Alpha;
  
        struct Input {
          float4 color : COLOR;
          float2 uv_MainTex;
          float3 viewDir;
        };
  
        sampler2D _MainTex;

        void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * IN.color;
           o.Alpha = _Alpha; // For example. Could also be the alpha channel on the interpolated vertex color (IN.color.a), or the one from the texture.
        }
        ENDCG
     } 
     FallBack "Diffuse"
 }