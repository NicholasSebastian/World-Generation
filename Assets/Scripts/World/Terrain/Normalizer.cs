using System.Linq;
using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 14/09/2022.

public class Normalizer : Dispatchable2D
{
    private readonly ComputeBuffer minMax;

    public Normalizer(ComputeShader shader, int width, int breadth, OctaveSettings[] settings) :
        base(width, breadth, shader, "Normalize")
    {
        // Set the minMaxValues variable for the compute shader.
        minMax = new ComputeBuffer(settings.Length, sizeof(float));
        minMax.SetData(settings.Select(setting => CalculateMinMax(setting.octaves, setting.persistance)).ToArray());
        shader.SetBuffer(kernelId, "minMaxValues", minMax);
    }

    ~Normalizer()
    {
        minMax.Dispose();
    }

    public void Process(ref ComputeBuffer buffer)
    {
        shader.SetBuffer(kernelId, "Maps", buffer);

        Dispatch();
    }

    private static float CalculateMinMax(int octaves, float persistance)
    {
        float minMax = 0;

        for (int ovIndex = 0; ovIndex < octaves; ovIndex++)
            minMax += Mathf.Pow(persistance, ovIndex);

        return minMax;
    }
}
