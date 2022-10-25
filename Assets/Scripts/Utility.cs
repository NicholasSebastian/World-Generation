using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

// Written by Nicholas Sebastian Hendrata on 08/09/2022.

public static class Utility
{
    /// <summary>
    /// Creates and returns a NativeArray from the given Array super-efficiently.
    /// </summary>
    public static unsafe void ToNativeArray<T>(T[] array, out NativeArray<T> nativeArray)
        where T : unmanaged
    {
        nativeArray = new NativeArray<T>(array.Length,
            Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

        // Fixed so that we can use the array without it moving around in memory.
        fixed (void* pointer = array)
        {
            UnsafeUtility.MemCpy(
                NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(nativeArray),
                pointer,
                array.Length * (long)UnsafeUtility.SizeOf<T>());
        }
    }

    /// <summary>
    /// Creates and returns a regular C# Array from the given NativeArray super-efficiently.
    /// </summary>
    public static unsafe void ToManagedArray<T>(NativeArray<T> nativeArray, out T[] array)
        where T : unmanaged
    {
        array = new T[nativeArray.Length];

        // Fixed so that we can use the array without it moving around in memory.
        fixed (void* pointer = array)
        {
            UnsafeUtility.MemCpy(
                pointer,
                NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks(nativeArray),
                array.Length * (long)UnsafeUtility.SizeOf<T>());
        }
    }
}
