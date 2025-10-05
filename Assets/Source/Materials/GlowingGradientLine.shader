Shader "Custom/AdvancedGlowingLine"
{
    Properties
    {
        [Header(Line Settings)]
        _LineEdgeColor ("Edge Color", Color) = (0,0.2,1,1)
        _LineCenterColor ("Center Color", Color) = (0.5,0.8,1,1)
        _TotalLineWidth ("Total Line Width", Range(0.01, 1)) = 0.2
        _CenterPortion ("Center Portion", Range(0, 1)) = 0.5
        
        [Header(Glow Settings)]
        _GlowColor ("Glow Color", Color) = (0.1,0.2,0.8,0.3)
        _GlowWidth ("Glow Width", Range(0.1, 2)) = 0.5
        _GlowFalloff ("Glow Falloff", Range(0.1, 10)) = 2
        
        [Header(Edge Settings)]
        _EdgeBlur ("Edge Blur", Range(0, 0.5)) = 0.1
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
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
            
            float4 _LineEdgeColor;
            float4 _LineCenterColor;
            float _TotalLineWidth;
            float _CenterPortion;
            float4 _GlowColor;
            float _GlowWidth;
            float _GlowFalloff;
            float _EdgeBlur;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float dist = abs(i.uv.y - 0.5) * 2;
                
                // Calculate zone boundaries
                float centerWidth = _TotalLineWidth * _CenterPortion;
                float edgeWidth = _TotalLineWidth - centerWidth;
                float lineEnd = _TotalLineWidth;
                float glowEnd = _TotalLineWidth + _GlowWidth;
                
                // Calculate masks for each zone with soft transitions
                float centerMask = 1.0 - smoothstep(centerWidth - _EdgeBlur, centerWidth + _EdgeBlur, dist);
                float edgeMask = smoothstep(centerWidth - _EdgeBlur, centerWidth + _EdgeBlur, dist) * 
                               (1.0 - smoothstep(lineEnd - _EdgeBlur, lineEnd + _EdgeBlur, dist));
                float glowMask = 1.0 - smoothstep(lineEnd, glowEnd, dist);
                
                // Apply colors with their original alpha values
                float4 centerPart = _LineCenterColor * centerMask;
                float4 edgePart = _LineEdgeColor * edgeMask;
                
                // Apply glow with falloff - use original alpha from _GlowColor
                float glowIntensity = pow(glowMask, _GlowFalloff);
                float4 glowPart = _GlowColor;
                glowPart.a *= glowIntensity;
                
                // Combine all parts - each maintains its own alpha
                float4 result = centerPart + edgePart + glowPart;
                
                // Ensure we don't exceed reasonable alpha values
                result.a = saturate(result.a);
                
                return result;
            }
            ENDCG
        }
    }
}