using UnityEngine;

public class VolumeFormatter : Dispatchable3D
{
    public VolumeFormatter(ComputeShader shader, int width, int breadth, int height) :
        base(width, breadth, height, shader, "FormatVertices") { }

    public void Process(ComputeBuffer input, out ComputeBuffer vertices, out ComputeBuffer indices)
    {
        vertices = new ComputeBuffer(0, 0);
        indices = new ComputeBuffer(0, 0);
        // TODO

        shader.SetBuffer(kernelId, "density", input);
        shader.SetBuffer(kernelId, "vertices", vertices);
        shader.SetBuffer(kernelId, "indices", indices);

        Dispatch();
    }

    protected sealed override void Dispatch()
    {
        int dimensionX = (width - 1) / (int)threadGroupSizeX;
        int dimensionY = (breadth - 1) / (int)threadGroupSizeY;
        int dimensionZ = (height - 1) / (int)threadGroupSizeZ;

        shader.Dispatch(kernelId, dimensionX, dimensionY, dimensionZ);
    }
}
