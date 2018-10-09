Shader "Unlit/Trackshader"
//this shader is used to draw red pixels on a black texture the general size of the terrain 
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Coordinate("Coordinate",vector) = (0,0,0,0)			
		_Color("Draw Color",Color) = (1,0,0,0)
		_Size("Size",Range(1,500)) = 1
		_Strength("Strength",Range(0,1)) = 1
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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Coordinate, _Color;
			half _Size, _Strength;
			
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
			// the function below is used to create the red pixels on the splatmap earlier defined in this shader
			// it uses the input coordinate from the drawtracks script and transform them into locations on the uv map which
			// shall be red.
			float draw = pow(saturate(1 - distance(i.uv, _Coordinate.xy)),500/_Size);//pow = strength/ size of brush, saturate ensures value stays between 1 and 0. Pow stands for size of brush
			fixed4 drawCol = _Color*(draw * _Strength);//our drawcolor is property _Color *draw multiplied with a variable for strenght.
			return saturate(col + drawCol);
			}
			ENDCG
		}
	}
}
