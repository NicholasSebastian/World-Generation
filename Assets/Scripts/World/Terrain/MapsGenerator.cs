using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 14/09/2022.

public class MapsGenerator : Dispatchable2D
{
    private readonly int mapCount;
    private readonly ComputeBuffer settings;

    public MapsGenerator(ComputeShader shader, int width, int breadth, OctaveSettings[] settings) :
        base(width, breadth, shader, "GenerateMaps")
    {
        mapCount = settings.Length;

        // Set the octaveSettings variable for the compute shader.
        this.settings = new ComputeBuffer(settings.Length, (sizeof(float) * 3) + sizeof(int));
        this.settings.SetData(settings);
        shader.SetBuffer(kernelId, "octaveSettings", this.settings);
    }

    ~MapsGenerator()
    {
        settings.Dispose();
    }

    public void Generate(Vector2 offset, out ComputeBuffer result)
    {
        result = new ComputeBuffer(width * breadth * mapCount, sizeof(float));

        shader.SetVector("offset", offset);
        shader.SetBuffer(kernelId, "Maps", result);

        Dispatch();
    }
}
