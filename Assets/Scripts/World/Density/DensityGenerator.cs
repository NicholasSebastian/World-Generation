using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 08/09/2022.

public class DensityGenerator
{
    private readonly Vector2 seedOffset;
    private readonly VolumeGenerator volumeGenerator;
    private readonly VolumeFilter volumeFilter;
    private readonly VolumeFormatter volumeFormatter;

    public DensityGenerator(WorldSettings settings, ComputeShader shader, Vector2 seedOffset)
    {
        var width = settings.width;
        var breadth = settings.breadth;
        var height = settings.height;
        var scale = settings.noiseScale;
        this.seedOffset = seedOffset;

        volumeGenerator = new VolumeGenerator(shader, width, breadth, height);
        volumeFilter = new VolumeFilter(shader, width, breadth, height);
        volumeFormatter = new VolumeFormatter(shader, width, breadth, height);

        shader.SetInt("width", width);
        shader.SetInt("breadth", breadth);
        shader.SetFloat("scale", scale);
    }

    public void Generate(Vector2 offset, ComputeBuffer heightMap, out ComputeBuffer result)
    {
        volumeGenerator.Generate(offset + seedOffset, out var volume);
        volumeFilter.Process(ref volume, heightMap);
        volumeFormatter.Process(volume, out result);
    }
}
