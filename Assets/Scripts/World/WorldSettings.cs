using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class NoiseMapSettings
{
    public string name;
    public OctaveSettings samplingSettings;
    public SplineSettings splineSettings;
}

[Serializable]
public struct OctaveSettings
{
    public float scale;
    public int octaves;
    [Range(0, 1)] public float persistance;
    [Min(1)] public float lacunarity;
}

[Serializable]
public class SplineSettings
{
    public AnimationCurve splinePoints;

    [Header("Apply Condition")]
    [Range(0, 1)] public float from = 0;
    [Range(0, 1)] public float to = 1;
}

[CreateAssetMenu(fileName = "New World", menuName = "WorldGen Configuration")]
public class WorldSettings : ScriptableObject
{
    [Header("Chunk Settings")]
    [RangeStep(1, 256, 8)] public int width = 256;
    [RangeStep(1, 256, 8)] public int breadth = 256;
    [RangeStep(1, 256, 8)] public int height = 256;

    [Space(10)]
    public NoiseMapSettings[] noiseMapSettings;

    [Header("Terrain Settings")]
    [Min(1)] public float noiseScale = 10;
    [Min(1)] public int heightScale = 30;
    [Range(1, 255)] public int terrainHeight = 128;
    [Range(0, 1)] public float squashingFactor = 0.1f;

    [Space(10)]
    public TerrainType[] colourSettings;

    public OctaveSettings[] OctaveSettings
    {
        get
        {
            return noiseMapSettings
                .Select(noiseMap => noiseMap.samplingSettings)
                .ToArray();
        }
    }

    public SplineSettings[] SplineSettings
    {
        get
        {
            return noiseMapSettings
                .Select(noiseMap => noiseMap.splineSettings)
                .ToArray();
        }
    }
}
