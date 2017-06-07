// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/briSatConShader" {
	Properties {
		
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Brightness("Brightness",Float) = 1
		_Saturation("Saturation",Float) = 1
		_Contrast("Contrast",Float) = 1
		_Red("Red",Float) = 0
		_Green("Green",Float) = 0
		_Blue("Blue",Float) = 0
			//可以省略 写出来只是为了显示在面板上
	}
		SubShader{
			//Tags{"Queue"="Background"}
			Pass{
				//ZTest Always Cull Off ZWrite Off//no semicolon//去掉这一句可以用于一般object
				//屏幕后处理实际上在场景中绘制了与屏幕同宽同高的四边形面片,onrenderimage执行在所有不透明pass执行后/透明pass执行前,所以要关闭深度写入防止遮挡//永远用于屏幕后处理
				CGPROGRAM
				#pragma vertex vert  
				#pragma fragment frag  

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				half _Brightness;
				half _Saturation;
				half _Contrast;
				half _Red;
				half _Green;
				half _Blue;
				

				struct v2f {
					float4 pos : SV_POSITION;
					half2 uv:TEXCOORD0;//纹理坐标
				};//end with semicolon

				v2f vert(appdata_img v) {//appdata_img结构体包含了顶点坐标和纹理坐标变量(in UnityCG.cginc)
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord;
					return o;
				}

				fixed4 frag(v2f i) :SV_Target{
					fixed4 renderTex = tex2D(_MainTex,i.uv);//采样取得片元对应的uv texture
				
					//apply brightness
					fixed3 finalColor = renderTex.rgb*_Brightness;//原颜色*亮度系数（brightness is arithmetic mean of rgb）

					//apply saturation
					fixed luminance = 0.2125*renderTex.r + 0.7154*renderTex.g + 0.0721*renderTex.b;
					fixed3 luminanceColor = fixed3(luminance, luminance, luminance); //灰色(饱和度为0) 饱和度=(max(rgb)-min(rgb))/max
					finalColor = lerp(luminanceColor, finalColor, _Saturation);
					//lerp(a,b,t):interpolates a towards b by t(clamped between 0and1)

					//apply contrast
					fixed3 avgColor = fixed3(0.5, 0.5, 0.5);//对比度为0
					finalColor = lerp(avgColor, finalColor, _Contrast);

					finalColor.r = finalColor.r + _Red;
					finalColor.g = finalColor.g + _Green;
					finalColor.b = finalColor.b + _Blue;

					return fixed4(finalColor, renderTex.a);//插值是近似处理
				}
					ENDCG
			}
		}

		FallBack off	
}
