// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Lush/Sh_Lush_Galaxy"
{
	Properties
	{
		[Header(Base Background)][Space(13)]_CubemapBaseTexture("Cubemap Base Texture", CUBE) = "white" {}
		_BaseEmMult("Base Em Mult", Float) = 1
		[Space(13)][Header(Stars)][Space(13)]_CubemapStars("Cubemap Stars", 2D) = "white" {}
		_StarsColor("Stars Color", Color) = (0,0.9407032,1,0)
		_StarsSecondaryColor("Stars Secondary Color", Color) = (0.7015796,0,1,0)
		_StarsEmMult("Stars Em Mult", Float) = 333
		_StarsPanSpeed("Stars Pan Speed", Float) = 0
		_CubemapStarsNoise("Cubemap Stars Noise", 2D) = "black" {}
		_StarsNoiseSpeed("Stars Noise Speed", Float) = 0
		_StarsDistortion("Stars Distortion", Float) = 0
		[Space(13)][Header(Fresnel)][Space(13)]_FrColor("Fr Color", Color) = (0,0.6859784,1,0)
		_FrEmMult("Fr Em Mult", Float) = 13
		_FrBias("Fr Bias", Float) = 0
		_FrScale("Fr Scale", Float) = 1
		_FrPower("Fr Power", Float) = 5
		[Space(13)][Header(Other)][Space(13)]_Metallic("Metallic", Float) = 0
		_Smoothness("Smoothness", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
			float2 uv_texcoord;
		};

		uniform float4 _FrColor;
		uniform float _FrBias;
		uniform float _FrScale;
		uniform float _FrPower;
		uniform float _FrEmMult;
		uniform samplerCUBE _CubemapBaseTexture;
		uniform float _BaseEmMult;
		uniform float4 _StarsColor;
		uniform float4 _StarsSecondaryColor;
		uniform sampler2D _CubemapStars;
		uniform float4 _CubemapStars_ST;
		uniform float _StarsPanSpeed;
		uniform float _StarsDistortion;
		uniform sampler2D _CubemapStarsNoise;
		uniform float4 _CubemapStarsNoise_ST;
		uniform float _StarsNoiseSpeed;
		uniform float _StarsEmMult;
		uniform float _Metallic;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV8 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode8 = ( _FrBias + _FrScale * pow( 1.0 - fresnelNdotV8, _FrPower ) );
			float2 uv_CubemapStars = i.uv_texcoord * _CubemapStars_ST.xy + _CubemapStars_ST.zw;
			float mulTime21 = _Time.y * _StarsPanSpeed;
			float2 uv_CubemapStarsNoise = i.uv_texcoord * _CubemapStarsNoise_ST.xy + _CubemapStarsNoise_ST.zw;
			float mulTime36 = _Time.y * _StarsNoiseSpeed;
			float4 tex2DNode32 = tex2D( _CubemapStarsNoise, ( uv_CubemapStarsNoise + mulTime36 ) );
			float4 tex2DNode28 = tex2D( _CubemapStars, ( ( uv_CubemapStars + mulTime21 ) + ( _StarsDistortion * tex2DNode32.r ) ) );
			float4 lerpResult26 = lerp( _StarsColor , _StarsSecondaryColor , saturate( ( ( tex2DNode28 * tex2DNode32.r ) * 30.0 ) ));
			float4 lerpResult37 = lerp( float4( 0,0,0,0 ) , lerpResult26 , tex2DNode28.r);
			o.Emission = ( ( ( _FrColor * saturate( fresnelNode8 ) ) * _FrEmMult ) + ( ( texCUBE( _CubemapBaseTexture, i.viewDir ) * _BaseEmMult ) + ( lerpResult37 * _StarsEmMult ) ) ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
30;7;1185;770;1981.188;763.3618;1.876322;True;False
Node;AmplifyShaderEditor.RangedFloatNode;35;-2048,1152;Float;False;Property;_StarsNoiseSpeed;Stars Noise Speed;8;0;Create;True;0;0;False;0;0;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;34;-2048,1024;Float;False;0;32;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;36;-1792,1152;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-1664,1024;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-2048,512;Float;False;Property;_StarsPanSpeed;Stars Pan Speed;6;0;Create;True;0;0;False;0;0;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;21;-1792,512;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-2048,384;Float;False;0;28;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;32;-1408,1024;Float;True;Property;_CubemapStarsNoise;Cubemap Stars Noise;7;0;Create;True;0;0;False;0;fb17b8d1af74b684c80b454f14a1a1ad;fb17b8d1af74b684c80b454f14a1a1ad;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-2048,640;Float;False;Property;_StarsDistortion;Stars Distortion;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-1664,384;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1792,640;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-1408,384;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;28;-1152,384;Float;True;Property;_CubemapStars;Cubemap Stars;2;0;Create;True;0;0;False;3;Space(13);Header(Stars);Space(13);70a50518651bffb499553a6c481a4b70;70a50518651bffb499553a6c481a4b70;True;0;False;white;LockedToTexture2D;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-896,1024;Float;False;Constant;_Gain;Gain;14;0;Create;True;0;0;False;0;30;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1024,896;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-896,896;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;40;-768,896;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1408,-192;Float;False;Property;_FrScale;Fr Scale;13;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1408,-256;Float;False;Property;_FrBias;Fr Bias;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;27;-1408,640;Float;False;Property;_StarsColor;Stars Color;3;0;Create;True;0;0;False;0;0,0.9407032,1,0;0,0.9407032,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-1408,-128;Float;False;Property;_FrPower;Fr Power;14;0;Create;True;0;0;False;0;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;30;-1408,800;Float;False;Property;_StarsSecondaryColor;Stars Secondary Color;4;0;Create;True;0;0;False;0;0.7015796,0,1,0;0.7015796,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;7;-1408,128;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;26;-896,640;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;8;-1280,-256;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;-1280,-512;Float;False;Property;_FrColor;Fr Color;10;0;Create;True;0;0;False;3;Space(13);Header(Fresnel);Space(13);0,0.6859784,1,0;0,0.6859784,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;37;-640,640;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-768,512;Float;False;Property;_StarsEmMult;Stars Em Mult;5;0;Create;True;0;0;False;0;333;333;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-768,256;Float;False;Property;_BaseEmMult;Base Em Mult;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1152,128;Float;True;Property;_CubemapBaseTexture;Cubemap Base Texture;0;0;Create;True;0;0;False;2;Header(Base Background);Space(13);14e50fcbc1a2ff842ba76151cc0d64d7;14e50fcbc1a2ff842ba76151cc0d64d7;True;0;False;white;LockedToCube;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;17;-896,-256;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-768,384;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-640,-128;Float;False;Property;_FrEmMult;Fr Em Mult;11;0;Create;True;0;0;False;0;13;13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-768,128;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-768,-256;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-640,-256;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-512,256;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-256,208;Float;False;Property;_Smoothness;Smoothness;16;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-256,128;Float;False;Property;_Metallic;Metallic;15;0;Create;True;0;0;False;3;Space(13);Header(Other);Space(13);0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-384,0;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Lush/Sh_Lush_Galaxy;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;46;-384,-384;Float;False;100;100;;0;Enjoy the shader and have fun luv u all <3;0,0,0,1;0;0
WireConnection;36;0;35;0
WireConnection;33;0;34;0
WireConnection;33;1;36;0
WireConnection;21;0;22;0
WireConnection;32;1;33;0
WireConnection;20;0;29;0
WireConnection;20;1;21;0
WireConnection;43;0;42;0
WireConnection;43;1;32;1
WireConnection;41;0;20;0
WireConnection;41;1;43;0
WireConnection;28;1;41;0
WireConnection;31;0;28;0
WireConnection;31;1;32;1
WireConnection;38;0;31;0
WireConnection;38;1;39;0
WireConnection;40;0;38;0
WireConnection;26;0;27;0
WireConnection;26;1;30;0
WireConnection;26;2;40;0
WireConnection;8;1;12;0
WireConnection;8;2;13;0
WireConnection;8;3;14;0
WireConnection;37;1;26;0
WireConnection;37;2;28;1
WireConnection;2;1;7;0
WireConnection;17;0;8;0
WireConnection;24;0;37;0
WireConnection;24;1;25;0
WireConnection;4;0;2;0
WireConnection;4;1;5;0
WireConnection;16;0;15;0
WireConnection;16;1;17;0
WireConnection;10;0;16;0
WireConnection;10;1;11;0
WireConnection;23;0;4;0
WireConnection;23;1;24;0
WireConnection;9;0;10;0
WireConnection;9;1;23;0
WireConnection;0;2;9;0
WireConnection;0;3;44;0
WireConnection;0;4;45;0
ASEEND*/
//CHKSM=66FCBA445C8BC007B5E3987B00ED19B2AC73F4BC