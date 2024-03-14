void GetClosestSoundObject_float(UnityTexture2D _SoundObjectPositions, const UnitySamplerState _Sampler, const float3 _ReferencePoint, out float _ClosestNormalisedRange)
{
    float closestNormalisedRange = 0;
    float closestRange = 0;

    const uint w = 2;

    for (uint x = 0; x < w; x++)
    {
        float u = (x + 0.5) / w;
        float v = 1;

        float4 col = SAMPLE_TEXTURE2D_LOD(_SoundObjectPositions, _Sampler, float2(u, v), 0);
        const float range = col.w;

        //if (range <= 0) break;

        const float distance = length(col.xyz - _ReferencePoint);

        const float normalisedRange = 1 - saturate(distance / range);

        closestNormalisedRange = max(closestNormalisedRange, normalisedRange);
    }

    _ClosestNormalisedRange = closestNormalisedRange;
}
