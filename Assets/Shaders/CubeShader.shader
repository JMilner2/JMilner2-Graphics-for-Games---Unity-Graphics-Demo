Shader"Custom/GlowingFrameCube" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _Scale ("Pulse Scale", Range (0.5, 2)) = 1.0
        _Speed ("Pulse Speed", Range (0.1, 2)) = 1.0
        _FlowSpeed ("Flow Speed", Range (0.1, 5)) = 1.0
        _GlowIntensity ("Glow Intensity", Range (0.0, 1.0)) = 0.1
        _GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

sampler2D _MainTex;
fixed4 _Color;
float _Scale;
float _Speed;
float _FlowSpeed;
float _GlowIntensity;
fixed4 _GlowColor;

struct Input
{
    float2 uv_MainTex;
};

void vert(inout appdata_full v)
{
            // Pulsating effect on vertices
    float pulse = sin(_Time.y * _Speed) * 0.5 + 0.5;
    float scale = _Scale * (1.0 - pulse); // Invert pulse for edge growth

            // Apply minimum scale
    scale = max(scale, 0.1);

            // Scale the vertices
    v.vertex.xyz *= scale;
}

void surf(Input IN, inout SurfaceOutput o)
{
            // Pulsating color based on the texture and pulsating effect
    float2 uv = IN.uv_MainTex;

            // Add scrolling effect based on time
    uv.x += _Time.y * _FlowSpeed;

    fixed4 texColor = tex2D(_MainTex, uv);
    fixed4 proceduralColor = _Color * texColor;

            // Apply the procedural color to the surface
    o.Albedo = proceduralColor.rgb;
    o.Alpha = proceduralColor.a;

            // Adjusted edge factor based on scale (50 in this case)
    float edgeFactor = fwidth(o.Albedo.r) * 1.15;

            // Add emissive glow on the cube's frame
    o.Emission = _GlowColor.rgb * _GlowIntensity * (1.0 - smoothstep(0.0, edgeFactor, fwidth(o.Albedo.r)));
}
        ENDCG
    }
}
