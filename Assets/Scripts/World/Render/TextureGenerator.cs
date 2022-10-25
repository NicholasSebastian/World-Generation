using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 09/09/2022.

public class TextureGenerator : Dispatchable2D
{
    private readonly Shader unlitTextureShader;

    public TextureGenerator(WorldSettings settings, ComputeShader shader) :
        base(settings.width, settings.breadth, shader, "GenerateTexture")
    {
        unlitTextureShader = Shader.Find("Unlit/Texture");
        shader.SetInt("width", width);
    }

    public Material Generate(ComputeBuffer heightMap)
    {
        var renderTexture = new RenderTexture(width, breadth, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        shader.SetTexture(kernelId, "Result", renderTexture);
        shader.SetBuffer(kernelId, "Values", heightMap);

        Dispatch();

        var material = new Material(unlitTextureShader);
        material.hideFlags = HideFlags.DontSave;
        material.mainTexture = renderTexture;

        return material;
    }
}
