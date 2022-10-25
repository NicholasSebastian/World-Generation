using UnityEngine;
using UnityEditor;

// Written by Nicholas Sebastian Hendrata on 05/09/2022.

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var worldGenerator = target as WorldGenerator;

        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Chunk"))
            worldGenerator.CreateChunk();

        if (GUILayout.Button("Load Variables"))
            worldGenerator.Initialize();
    }
}
