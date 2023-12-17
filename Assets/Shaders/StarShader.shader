Shader"Custom/StarShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Radius ("Sphere Radius", Range(0,10)) = 5.0
        _StarColor ("Star Color", Color) = (1, 1, 1, 1)
        _Emission ("Emission", Range (0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

sampler2D _MainTex;

struct Input
{
    float2 uv_MainTex;
    float3 worldPos;
};

half _Glossiness;
half _Metallic;
float _Radius;
fixed4 _StarColor;
half _Emission;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

void surf(Input IN, inout SurfaceOutputStandard o)
{
            // Calculate brightness based on the y-coordinate
    float brightness = 1.0;
    brightness = saturate(brightness);

            // Adjust color based on brightness
    fixed3 finalColor = _StarColor.rgb * brightness;

    o.Albedo = finalColor;
    o.Metallic = _Metallic;
    o.Smoothness = _Glossiness;
    o.Alpha = brightness;

            // Emission
    o.Emission = _Emission * finalColor.rgb;
}
        ENDCG
    }
FallBack"Diffuse"
}
