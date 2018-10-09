Shader "Hidden/SnowflakesShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
			
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;				
				float4 vertex : SV_POSITION;
			};
			
			
			//randomize function using float variables.
			float rand(float3 co)
			{
				//dot= create a dot product between input factor and magic numbers, dot comes from when program is creating shadows on an object
				//sin = sinus of the dot product, executed after the dotproduct
				// frac = frac product of the sinus result executed last, 
				//it return only the decimal values of the result meaning that the frac value will always be lower than 1
				return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
			}

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half _SnowflakeAmount,_FlakeOpacity;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
			// function creates random value based on the uv coordinates of the terrain object-1- the amounts of snowflakes we want to fall on the ground.
			float rValue = ceil(rand(float3(i.uv.x,i.uv.y,0)*_Time.x)-(1-_SnowflakeAmount));// Time.x is the internal clock of the gpu,ceil makes the value to always fall between 0&1

				return saturate(col-(rValue*_FlakeOpacity));
			}
			ENDCG
		}
	}
}
