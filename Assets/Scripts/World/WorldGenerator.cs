using System;
using UnityEngine;

// Written by Nicholas Sebastian Hendrata on 07/09/2022.
// * World Generator v4 *

// ============================
//        CONSIDERATIONS
// ============================

// Given that the nature task of generating the world is multifarious,
// as the world is made up of multiple chunks, which uses a height map,
// which in turn is made up of multiple noise maps, which in turn is
// made up of multiple noise values, which in turn is made up of multiple
// octave values, this process can get quite performance intensive.

// Running it as-is in the single main thread is rather impractical.
// We can distribute these thousands of small calculations to get more
// performance out of this process, in which I now see only 2 ways to do:

// --- CPU MULTITHREADING ---
// Using Unity's build-in Jobs System, the world generation task can be
// outsourced to multiple other CPU threads to execute in parallel,
// independent from the main thread. This approach can also take advantage
// of Unity's Burst Compiler, which just-in-time compiles the C# bytecode
// into native code, which can run super fast and efficiently.

// --- GPU COMPUTE KERNELS ---
// Using Compute Shaders, the world generation task can be outsourced to
// the GPU's thousands of CUDA cores to execute in parallel, independent
// from the CPU processes. This approach also allows the data to be used
// directly in the GPU to render the mesh and texture efficiently.
// The downside to this is that passing data between the CPU and GPU is slow.

// TODO: Marching Cubes Mesh Building
// TODO: Entity Spawning
// TODO: Chunk Load and Unload
// TODO: Lakes and Rivers
// TODO: Caves
// TODO: Terrain Textures
// TODO: Grass Geometry Shader

public class WorldGenerator : MonoBehaviour
{
    public WorldMode mode;
    public WorldSettings worldSettings;

    [Header("Seed Settings")]
    public bool randomize = true;
    [Min(1)] public uint seed;
    private Vector2 seedOffset;

    [Header("Compute Shaders")]
    [SerializeField] private ComputeShader densityCompute;
    [SerializeField] private ComputeShader terrainCompute;
    [SerializeField] private Shader marchingCubes;
    [SerializeField] private ComputeShader flatMapTextureCompute;

    private DensityGenerator densityGenerator;
    private MeshGenerator meshGenerator;
    private TerrainGenerator terrainGenerator;
    private MaterialManager materialManager;
    private TextureGenerator textureGenerator;

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        TryRandomizeSeed();

        // Initialize the helper classes instances.
        densityGenerator = new DensityGenerator(worldSettings, densityCompute, seedOffset);
        terrainGenerator = new TerrainGenerator(worldSettings, terrainCompute, seedOffset);
        meshGenerator = new MeshGenerator(marchingCubes);
        materialManager = new MaterialManager(worldSettings);
        textureGenerator = new TextureGenerator(worldSettings, flatMapTextureCompute);
    }

    public void CreateChunk()
    {
        var offset = Vector2.zero;
        // TODO

        CreateChunk(offset);
    }

    private void TryRandomizeSeed()
    {
        if (randomize)
            seed = (uint) UnityEngine.Random.Range(0, int.MaxValue);

        // Prepare a random offset for the map based on the given seed.
        var random = new Unity.Mathematics.Random(seed);
        seedOffset = new Vector2()
        {
            x = random.NextFloat(-100000, 100000),
            y = random.NextFloat(-100000, 100000)
        };
    }

    private void CreateChunk(Vector2 position)
    {
        terrainGenerator.Generate(position, out var heightMap);

        if (mode == WorldMode.FlatMap)
        {
            var material = textureGenerator.Generate(heightMap);

            CreateFlatChunkObject(position, material);

            heightMap.Dispose();
        }
        else
        {
            densityGenerator.Generate(position, heightMap, out var density);
            meshGenerator.Create(density, position);
            // var material = materialManager.GetMaterial(mode);

            heightMap.Dispose();
            density.Dispose();
        }
    }

    //private void CreateChunkObject(Vector2 position, Mesh mesh, Material material)
    //{
    //    var chunk = new GameObject("Chunk " + position);
    //    chunk.transform.parent = transform;
    //    chunk.transform.position = new Vector3(position.x, 0, position.y);

    //    chunk.AddComponent<MeshFilter>().sharedMesh = mesh;
    //    chunk.AddComponent<MeshRenderer>().sharedMaterial = material;
    //    chunk.AddComponent<MeshCollider>().sharedMesh = mesh;
    //}

    private void CreateFlatChunkObject(Vector2 position, Material material)
    {
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.name = "Chunk " + position;
        plane.transform.parent = transform;
        plane.transform.position = new Vector3(position.x, 0, position.y);

        plane.GetComponent<MeshRenderer>().sharedMaterial = material;
    }
}

[Serializable]
public enum WorldMode
{
    ColourMap,
    HeightMap,
    FlatMap
}
