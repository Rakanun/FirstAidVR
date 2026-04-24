Shader "Custom/PositionHighlight"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _HighlightColor ("Highlight Color", Color) = (1,0.5,0.5,1)
        _HighlightCenter ("Highlight Center", Vector) = (0,0,0,0)
        _HighlightRadius ("Influence Radius", Range(0.1, 5)) = 1.0
        _ColorIntensity ("Color Intensity", Range(0, 2)) = 1.0
        _EdgeSmoothness ("Edge Smoothness", Range(0, 1)) = 0.2
        _DeformationDepth ("Deformation Depth", Range(0, 0.3)) = 0.1
        _DeformationHardness ("Deformation Hardness", Range(0.1, 5)) = 2.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard vertex:vert addshadow
        #pragma target 3.5

        sampler2D _MainTex;
        float3 _HighlightCenter;
        float4 _HighlightColor;
        float _HighlightRadius;
        float _ColorIntensity;
        float _EdgeSmoothness;
        float _DeformationDepth;
        float _DeformationHardness;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        // Vertex shader: applies press deformation along vertex normals
        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            // Transform highlight center from world space to object space
            float3 centerOS = mul(unity_WorldToObject, float4(_HighlightCenter, 1)).xyz;

            float dist = length(v.vertex.xyz - centerOS);

            // Exponential falloff curve for smooth deformation boundary
            float attenuation = pow(saturate(1 - dist / _HighlightRadius), _DeformationHardness);

            // Indent vertices along their normals
            float3 deformation = v.normal * _DeformationDepth * attenuation;
            v.vertex.xyz -= deformation;
        }

        // Surface shader: blends base texture with highlight color
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex);

            float distanceToCenter = distance(IN.worldPos, _HighlightCenter);
            float highlightFactor = 1 - smoothstep(
                _HighlightRadius * (1 - _EdgeSmoothness),
                _HighlightRadius,
                distanceToCenter
            );

            float3 blendedColor = lerp(
                baseColor.rgb,
                _HighlightColor.rgb * _ColorIntensity,
                highlightFactor * _HighlightColor.a
            );

            o.Albedo = blendedColor;
            o.Metallic = 0;
            o.Smoothness = 0.5;
            o.Alpha = baseColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
