void Quantize_float(float3 _In, float levels, out float3 _Out)
{
    const float greyscale = max(_In.r, max(_In.g, _In.b));

    const float lower     = floor(greyscale * levels) / levels;
    const float lowerDiff = abs(greyscale - lower);

    const float upper     = ceil(greyscale * levels) / levels;
    const float upperDiff = abs(upper - greyscale);

    const float level      = lowerDiff <= upperDiff ? lower : upper;
    const float adjustment = level / greyscale;

    _Out.rgb = _In.rgb * adjustment;
}
