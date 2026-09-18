Shader "Ja2/ButonShaded"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
	}
	SubShader
	{
		Tags
		{
			"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True"
		}
		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		// Multiply
		Blend DstColor Zero
		ColorMask [_ColorMask]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			fixed4 _Color;

			v2f vert(appdata_t v)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(v.vertex);
				OUT.texcoord = v.texcoord;
				OUT.color = v.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 col = tex2D(_MainTex
					, IN.texcoord
				) * IN.color;

				return col;
			}
			ENDCG
		}
	}
}