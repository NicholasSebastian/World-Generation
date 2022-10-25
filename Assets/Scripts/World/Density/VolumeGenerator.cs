using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 14/09/2022.

public class VolumeGenerator : Dispatchable3D
{
    public VolumeGenerator(ComputeShader shader, int width, int breadth, int height) :
        base(width, breadth, height, shader, "GenerateDensity") { }

    public void Generate(Vector2 offset, out ComputeBuffer result)
    {
        result = new ComputeBuffer(width * breadth * height, sizeof(float));

        shader.SetVector("offset", offset);
        shader.SetBuffer(kernelId, "density", result);

        Dispatch();
    }
}
