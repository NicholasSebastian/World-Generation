using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 08/09/2022.

public class MeshGenerator
{
    private readonly Shader shader;

    public MeshGenerator(Shader shader)
    {
        this.shader = shader;
    }

    public void Create(ComputeBuffer density, Vector3 position)
    {
        var material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        material.SetBuffer("Values", density);

        // TODO
        var argsBuffer = new ComputeBuffer(4, sizeof(int));
        argsBuffer.SetData(new[] { 0, 0, 0, 0 });

        var bounds = new Bounds(position, Vector3.one * 10);
        Graphics.DrawProceduralIndirect(material, bounds, MeshTopology.Triangles, argsBuffer);
    }
}
