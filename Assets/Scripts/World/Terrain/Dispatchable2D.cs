using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 14/09/2022.

public class Dispatchable2D
{
    protected readonly int width;
    protected readonly int breadth;

    protected readonly ComputeShader shader;
    protected readonly int kernelId;
    protected readonly uint threadGroupSizeX;
    protected readonly uint threadGroupSizeY;

    public Dispatchable2D(int width, int breadth, ComputeShader shader, string kernelname)
    {
        this.width = width;
        this.breadth = breadth;
        this.shader = shader;

        kernelId = shader.FindKernel(kernelname);
        shader.GetKernelThreadGroupSizes(kernelId, out threadGroupSizeX, out threadGroupSizeY, out _);
    }

    protected virtual void Dispatch()
    {
        int dimensionX = width / (int)threadGroupSizeX;
        int dimensionY = breadth / (int)threadGroupSizeY;

        shader.Dispatch(kernelId, dimensionX, dimensionY, 1);
    }
}
