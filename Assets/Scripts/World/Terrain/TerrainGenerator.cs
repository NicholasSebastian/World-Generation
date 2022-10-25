using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 11/09/2022.

public class TerrainGenerator
{
    private readonly int width;
    private readonly int breadth;
    private readonly Vector2 seedOffset;

    private readonly MapsGenerator mapsGenerator;
    private readonly Normalizer normalizer;
    private readonly MapsMerger mapsMerger;

    public TerrainGenerator(WorldSettings settings, ComputeShader shader, Vector2 seedOffset)
    {
        width = settings.width;
        breadth = settings.breadth;
        this.seedOffset = seedOffset;

        var octaveSettings = settings.OctaveSettings;
        var splineSettings = settings.SplineSettings;

        // Helper classes.
        mapsGenerator = new MapsGenerator(shader, width, breadth, octaveSettings);
        normalizer = new Normalizer(shader, width, breadth, octaveSettings);
        mapsMerger = new MapsMerger(shader, width, breadth, splineSettings);

        // Set the constant uniform compute shader variables.
        shader.SetInt("width", settings.width);
        shader.SetInt("breadth", settings.breadth);
        shader.SetInt("mapCount", settings.noiseMapSettings.Length);
    }

    public void Generate(Vector2 offset, out ComputeBuffer heightMap)
    {
        mapsGenerator.Generate(offset + seedOffset, out var maps);
        normalizer.Process(ref maps);
        mapsMerger.Process(maps, out heightMap);

        maps.Dispose();
    }
}
