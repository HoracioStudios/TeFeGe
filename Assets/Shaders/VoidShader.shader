Shader "Unlit/VoidShader"
{

	Properties
	{
		_MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
		waveSpeed("Wave Speed", Range(-20, 200)) = 20
		waveFrequency("Frequency", Range(0, 100)) = 10
		waveAmplitude("Amplitude", Range(0, 3)) = 0.02
		redScale("Red Scale", Range(0, 3)) = 1
		greenScale("Green Scale", Range(0, 3)) = 1
		blueScale("Blue Scale", Range(0, 3)) = 1



		distortionX("Texture Distortion X", Float) = 4.2
		distortionY("Texture Distortion Y", Float) = 3.9
		sizeX("Texture Size X", Float) = 0.5
		sizeY("Texture Size Y", Float) = 0.1

		speed("Texture Speed", Range(0, 5)) = 0.02

	}
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


			fixed waveFrequency;
			fixed waveSpeed;
			fixed waveAmplitude;
			fixed redScale;
			fixed greenScale;
			fixed blueScale;

			fixed speed;
			fixed distortionX;
			fixed distortionY;
			fixed sizeX;
			fixed sizeY;

			static float2 size = float2(sizeX, sizeY);
			static float2 distortion = float2(distortionX, distortionY);

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = float2(i.uv.x, 1 - i.uv.y);

				float2 transformed = float2(
					uv.x + sin(uv.y / size.x + _Time.y * speed) * distortion.x,
					uv.y + cos(uv.x / size.y + _Time.y * speed) * distortion.y
					);

				fixed2 uvs = i.uv;
				fixed4 col = tex2D(_MainTex, transformed / uv);
				fixed d = sin(uvs.y * waveFrequency + _Time * waveSpeed) * waveAmplitude;
				col.r += d * redScale;
				col.g += d * greenScale;
				col.b += d * blueScale;

				uvs = i.uv;
				//col = tex2D(_MainTex, transformed);
				d = sin(sin(uvs.x * waveFrequency + _Time * waveSpeed)) * waveAmplitude / 3.0;
				col.r += d * redScale;
				col.g += d * greenScale;
				col.b += d * blueScale;

				uvs = i.uv;
				//col = tex2D(_MainTex, transformed);
				d = tan(cos(uvs.x * waveFrequency + _Time * waveSpeed)) * waveAmplitude / 10.0;
				col.r += d * redScale;
				col.g += d * greenScale;
				col.b += d * blueScale;

				
				return col;

				/*return tex2D(_MainTex, transformed) + float4(
					(cos(uv.x + _Time.y * speed2 * 3.0) + 1.0) / 2.0,
					(uv.x + uv.y) / 2.0,
					(sin(uv.y + _Time.y * speed2) + 1.0) / 2.0,
					0
					);
					*/
			}
            ENDCG
        }
    }
}
