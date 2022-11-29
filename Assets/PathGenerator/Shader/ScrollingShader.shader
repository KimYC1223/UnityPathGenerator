//=====================================================================================================================================
/*      ,------.   ,---. ,--------.,--.  ,--.     ,----.   ,------.,--.  ,--.,------.,------.   ,---. ,--------. ,-----. ,------.
        |  .--. ' /  O  \'--.  .--'|  '--'  |    '  .-./   |  .---'|  ,'.|  ||  .---'|  .--. ' /  O  \'--.  .--''  .-.  '|  .--. '
        |  '--' ||  .-.  |  |  |   |  .--.  |    |  | .---.|  `--, |  |' '  ||  `--, |  '--'.'|  .-.  |  |  |   |  | |  ||  '--'.'
        |  | --' |  | |  |  |  |   |  |  |  |    '  '--'  ||  `---.|  | `   ||  `---.|  |\  \ |  | |  |  |  |   '  '-'  '|  |\  \
        `--'     `--' `--'  `--'   `--'  `--'     `------' `------'`--'  `--'`------'`--' '--'`--' `--'  `--'    `-----' `--' '--'   */
//=====================================================================================================================================
//
//  SCROLLING SHADER
//
//  Shaders that visualize paths
//  Referenced ScrollingFill.shader from the following git repo :
//  UnityCommunity / UnityLibrary
// 
//  경로를 시각화하는 셰이더
//  다음 git repo에서 ScrollingFill.shader를 참조함 :
//  UnityCommunity / UnityLibrary
//
//  Original :
//      animated scrolling texture with fill amount
//      https://unitycoder.com/blog/2020/03/13/shader-scrolling-texture-with-fill-amount/
// 
//-------------------------------------------------------------------------------------------------------------------------------------
//  2022.10.20 _ KimYC1223
//=====================================================================================================================================
Shader "PathGenerator/ScrollingShader"
{
    Properties
    {
        _MainTex ( "Main Texture", 2D ) = "white" {}
        [IntRange] _Speed ( "Speed", Range ( -100, 100 ) ) = 30
        _Alpha ("Alpha", Range(0,1)) = 1
        _Fill("Fill Path", Range(0,1)) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            float _Alpha;
            float _Fill;
            float _Speed;

            v2f vert ( appdata v )
            {
                v2f o;
                o.vertex = UnityObjectToClipPos ( v.vertex );
                o.uv = TRANSFORM_TEX ( v.uv, _MainTex );
                return o;
            }

            fixed4 frag ( v2f i ) : SV_Target
            {
                // get scroll value
                float2 scroll = float2(0, (frac ( _Time.x * _Speed )));

                // sample texture
                float4 _AlphaColor = float4 (1, 1, 1, _Alpha);
                fixed4 col = tex2D ( _MainTex, (i.uv - scroll) ) * _AlphaColor;

                //// discard if uv.y is below below cut value
                clip ( step ( i.uv.y, (_Fill - 0.5)* _MainTex_ST.y) - 0.1);

                return col;

                //make un-animated part black
                //return col*step(i.uv.y, _Cut * _MainTex_ST.y);
            }
            ENDCG
        }
    }
}