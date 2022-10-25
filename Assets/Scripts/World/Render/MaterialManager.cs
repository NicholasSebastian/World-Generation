using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct TerrainType
{
    public string name;
    [Min(0)] public float height;
    public Color colour;
    [Min(0.00001f)] public float blend;
}

public class MaterialManager
{
    private readonly Material heightShaderMaterial;
    private readonly Material colourShaderMaterial;

    public MaterialManager(WorldSettings settings)
    {
        var sortedTerrain = settings.colourSettings.OrderBy(terrain => terrain.height);
        var heights = sortedTerrain.Select(terrain => terrain.height).ToArray();
        var colours = sortedTerrain.Select(terrain => terrain.colour).ToArray();
        var blends = sortedTerrain.Select(terrain => terrain.blend).ToArray();

        // Shader for Height Map mode.
        var heightShader = Shader.Find("Custom/HeightMapShader");
        heightShaderMaterial = new Material(heightShader);
        heightShaderMaterial.SetFloat("minHeight", 0);
        heightShaderMaterial.SetFloat("maxHeight", settings.height);

        // Shader for Colour Map mode.
        var colourShader = Shader.Find("Custom/ColourMapShader");
        colourShaderMaterial = new Material(colourShader);
        colourShaderMaterial.SetInteger("colourCount", settings.colourSettings.Length);
        colourShaderMaterial.SetFloatArray("startHeights", heights);
        colourShaderMaterial.SetColorArray("colours", colours);
        colourShaderMaterial.SetFloatArray("blends", blends);
    }

    public Material GetMaterial(WorldMode mode)
    {
        switch (mode)
        {
            case WorldMode.HeightMap:
                return heightShaderMaterial;

            default:
                return colourShaderMaterial;
        }
    }
}
