// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#include "UnityCG.cginc"
uniform sampler2D _MainTex;uniform float4 _MainTex_ST;uniform float4 _GrabTex_ST;struct v2fB {float4 pos : SV_POSITION;float2 uv : TEXCOORD0;float2 uvScreen : TEXCOORD1;fixed4 color : COLOR0;};v2fB vertB (appdata_full v){v2fB o;o.uv.xy = TRANSFORM_TEX(v.texcoord.xy, _MainTex);o.pos = UnityObjectToClipPos (v.vertex);o.uvScreen = ComputeGrabScreenPos (o.pos);o.uvScreen *= _GrabTex_ST.xy;o.uvScreen += _GrabTex_ST.zw;o.color = v.color;return o;}