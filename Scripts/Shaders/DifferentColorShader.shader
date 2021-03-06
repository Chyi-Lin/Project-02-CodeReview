Shader "Custom/DifferentColorShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_SecondaryColor ("SecondaryColor", Color) = (1,1,1,1)
		_LifePercent("Life Percent", Float) = 1
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "DisableBatching" = "true"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard finalcolor:mycolor vertex:myvert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float4 pos : SV_POSITION;
			//float3 worldPos;
			float4 hpos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		fixed4 _SecondaryColor;
		float _LifePercent;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
		
		void myvert(inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);
			//data.worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);
			//data.worldPos = mul(_Object2World, v.vertex);
			data.hpos = v.vertex;
		}

		void mycolor(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
			if (IN.hpos.y + .5f > _LifePercent)
				color.rgb *= _Color;
			else
				color.rgb *= _SecondaryColor;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
