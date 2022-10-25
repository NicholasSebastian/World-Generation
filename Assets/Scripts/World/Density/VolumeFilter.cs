using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 14/09/2022.

public class VolumeFilter : Dispatchable3D
{
    public VolumeFilter(ComputeShader shader, int width, int breadth, int height) :
        base(width, breadth, height, shader, "ModifyTerrain") { }

    public void Process(ref ComputeBuffer density, ComputeBuffer terrain)
    {
        shader.SetInt("heightScale", 0);
        shader.SetInt("terrainOffset", 0);
        shader.SetFloat("squashingFactor", 0);
        shader.SetBuffer(kernelId, "heightMap", terrain);
        shader.SetBuffer(kernelId, "density", density);

        Dispatch();
    }
}
