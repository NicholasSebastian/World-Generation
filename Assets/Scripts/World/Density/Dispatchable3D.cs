using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 14/09/2022.

public abstract class Dispatchable3D : Dispatchable2D
{
    protected readonly int height;
    protected readonly uint threadGroupSizeZ;

    public Dispatchable3D(int width, int breadth, int height, ComputeShader shader, string kernelname) :
        base(width, breadth, shader, kernelname)
    {
        this.height = height;
        shader.GetKernelThreadGroupSizes(kernelId, out _, out _, out threadGroupSizeZ);
    }

    protected override void Dispatch()
    {
        int dimensionX = width / (int)threadGroupSizeX;
        int dimensionY = breadth / (int)threadGroupSizeY;
        int dimensionZ = height / (int)threadGroupSizeZ;

        shader.Dispatch(kernelId, dimensionX, dimensionY, dimensionZ);
    }
}
