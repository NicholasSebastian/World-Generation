using System.Linq;
using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 14/09/2022.

public class MapsMerger : Dispatchable2D
{
    private const int sampleCount = 100;
    private readonly ComputeBuffer splinePoints;
    private readonly ComputeBuffer splineConditions;

    public MapsMerger(ComputeShader shader, int width, int breadth, SplineSettings[] settings) :
        base(width, breadth, shader, "CombineMaps")
    {
        // Set the splinePoints variable for the compute shader.
        splinePoints = new ComputeBuffer(sampleCount * settings.Length, sizeof(float));
        splinePoints.SetData(settings.SelectMany(setting => SamplePoints(setting.splinePoints)).ToArray());
        shader.SetBuffer(kernelId, "splinePoints", splinePoints);

        // Set the splineConditions variable for the compute shader.
        splineConditions = new ComputeBuffer(settings.Length, sizeof(float) * 2);
        splineConditions.SetData(settings.Select(setting => new SplineConditions(setting)).ToArray());
        shader.SetBuffer(kernelId, "splineConditions", splineConditions);
    }

    ~MapsMerger()
    {
        splinePoints.Dispose();
        splineConditions.Dispose();
    }

    public void Process(ComputeBuffer maps, out ComputeBuffer map)
    {
        map = new ComputeBuffer(width * breadth, sizeof(float));

        shader.SetBuffer(kernelId, "Maps", maps);
        shader.SetBuffer(kernelId, "Map", map);

        Dispatch();
    }

    private static float[] SamplePoints(AnimationCurve curve)
    {
        var splinePoints = new float[sampleCount];
        var step = 1.0f / (sampleCount - 1);

        for (int i = 0; i < sampleCount; i++)
            splinePoints[i] = curve.Evaluate(i * step);

        return splinePoints;
    }

    private struct SplineConditions
    {
        public float from;
        public float to;

        public SplineConditions(SplineSettings settings)
        {
            from = settings.from;
            to = settings.to;
        }
    }
}
