using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace MinHook.NET;

/// <summary>Provides managed access to the MinHook API for creating and controlling native function hooks.</summary>
public static unsafe partial class MinHook
{
    private const string LibraryName = "MinHook";

    /// <summary>Gets the special target value used to apply an operation to all created hooks.</summary>
    public static nint AllHooks => 0;

    /// <summary>Initializes the MinHook library. Call this once before using other MinHook operations.</summary>
    /// <returns>A status value describing the result.</returns>
    [LibraryImport(LibraryName, EntryPoint = "MH_Initialize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    public static partial MinHookStatus Initialize();

    /// <summary>Uninitializes the MinHook library and removes all created hooks.</summary>
    /// <returns>A status value describing the result.</returns>
    [LibraryImport(LibraryName, EntryPoint = "MH_Uninitialize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    public static partial MinHookStatus Uninitialize();

    /// <summary>Creates a hook for the specified target function without enabling it.</summary>
    /// <param name="target">Address of the target function.</param>
    /// <param name="detour">Address of the detour function.</param>
    /// <param name="original">Receives the trampoline address used to call the original function.</param>
    /// <returns>A status value describing the result.</returns>
    [LibraryImport(LibraryName, EntryPoint = "MH_CreateHook")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    public static partial MinHookStatus CreateHook(nint target, nint detour, out nint original);

    /// <summary>Creates a hook for an exported function without enabling it.</summary>
    /// <param name="moduleName">Name of the loaded module containing the exported function.</param>
    /// <param name="procedureName">Name of the exported function.</param>
    /// <param name="detour">Address of the detour function.</param>
    /// <param name="original">Receives the trampoline address used to call the original function.</param>
    /// <returns>A status value describing the result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="moduleName"/> or <paramref name="procedureName"/> is <see langword="null"/>.</exception>
    public static MinHookStatus CreateHookApi(string moduleName, string procedureName, nint detour, out nint original)
    {
        ArgumentNullException.ThrowIfNull(moduleName);
        ArgumentNullException.ThrowIfNull(procedureName);

        var byteCount = Encoding.UTF8.GetByteCount(procedureName);
        Span<byte> buffer = byteCount < 256 ? stackalloc byte[byteCount + 1] : new byte[byteCount + 1];
        Encoding.UTF8.GetBytes(procedureName, buffer);

        fixed (char* moduleNamePointer = moduleName)
        fixed (byte* procedureNamePointer = buffer)
        {
            return CreateHookApi(moduleNamePointer, procedureNamePointer, detour, out original);
        }
    }

    /// <summary>Creates a hook for an exported function and returns both its target and trampoline addresses.</summary>
    /// <param name="moduleName">Name of the loaded module containing the exported function.</param>
    /// <param name="procedureName">Name of the exported function.</param>
    /// <param name="detour">Address of the detour function.</param>
    /// <param name="original">Receives the trampoline address used to call the original function.</param>
    /// <param name="target">Receives the target function address.</param>
    /// <returns>A status value describing the result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="moduleName"/> or <paramref name="procedureName"/> is <see langword="null"/>.</exception>
    public static MinHookStatus CreateHookApiEx(string moduleName, string procedureName, nint detour, out nint original, out nint target)
    {
        ArgumentNullException.ThrowIfNull(moduleName);
        ArgumentNullException.ThrowIfNull(procedureName);

        var byteCount = Encoding.UTF8.GetByteCount(procedureName);
        Span<byte> buffer = byteCount < 256 ? stackalloc byte[byteCount + 1] : new byte[byteCount + 1];
        Encoding.UTF8.GetBytes(procedureName, buffer);

        fixed (char* moduleNamePointer = moduleName)
        fixed (byte* procedureNamePointer = buffer)
        {
            return CreateHookApiEx(moduleNamePointer, procedureNamePointer, detour, out original, out target);
        }
    }

    /// <summary>Creates a hook for an exported function using unmanaged name pointers.</summary>
    /// <param name="moduleName">Pointer to a null-terminated UTF-16 module name.</param>
    /// <param name="procedureName">Pointer to a null-terminated ANSI procedure name.</param>
    /// <param name="detour">Address of the detour function.</param>
    /// <param name="original">Receives the trampoline address used to call the original function.</param>
    /// <returns>A status value describing the result.</returns>
    [LibraryImport(LibraryName, EntryPoint = "MH_CreateHookApi")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    public static partial MinHookStatus CreateHookApi(char* moduleName, byte* procedureName, nint detour, out nint original);

    /// <summary>Creates a hook for an exported function and returns its resolved address using unmanaged name pointers.</summary>
    /// <param name="moduleName">Pointer to a null-terminated UTF-16 module name.</param>
    /// <param name="procedureName">Pointer to a null-terminated ANSI procedure name.</param>
    /// <param name="detour">Address of the detour function.</param>
    /// <param name="original">Receives the trampoline address used to call the original function.</param>
    /// <param name="target">Receives the target function address.</param>
    /// <returns>A status value describing the result.</returns>
    [LibraryImport(LibraryName, EntryPoint = "MH_CreateHookApiEx")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    public static partial MinHookStatus CreateHookApiEx(char* moduleName, byte* procedureName, nint detour, out nint original, out nint target);

    /// <summary>Removes a previously created hook.</summary>
    /// <param name="target">Target function address, or <see cref="AllHooks"/> to remove all hooks.</param>
    /// <returns>A status value describing the result.</returns>
    [LibraryImport(LibraryName, EntryPoint = "MH_RemoveHook")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    public static partial MinHookStatus RemoveHook(nint target);

    /// <summary>Enables a previously created hook.</summary>
    /// <param name="target">Target function address, or <see cref="AllHooks"/> to enable all hooks.</param>
    /// <returns>A status value describing the result.</returns>
    [LibraryImport(LibraryName, EntryPoint = "MH_EnableHook")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    public static partial MinHookStatus EnableHook(nint target);

    /// <summary>Disables a previously created hook.</summary>
    /// <param name="target">Target function address, or <see cref="AllHooks"/> to disable all hooks.</param>
    /// <returns>A status value describing the result.</returns>
    [LibraryImport(LibraryName, EntryPoint = "MH_DisableHook")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    public static partial MinHookStatus DisableHook(nint target);

    /// <summary>Queues a request to enable a hook without applying it immediately.</summary>
    /// <param name="target">Target function address, or <see cref="AllHooks"/> to queue all hooks.</param>
    /// <returns>A status value describing the result.</returns>
    [LibraryImport(LibraryName, EntryPoint = "MH_QueueEnableHook")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    public static partial MinHookStatus QueueEnableHook(nint target);

    /// <summary>Queues a request to disable a hook without applying it immediately.</summary>
    /// <param name="target">Target function address, or <see cref="AllHooks"/> to queue all hooks.</param>
    /// <returns>A status value describing the result.</returns>
    [LibraryImport(LibraryName, EntryPoint = "MH_QueueDisableHook")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    public static partial MinHookStatus QueueDisableHook(nint target);

    /// <summary>Applies all queued hook enable and disable operations in one transaction.</summary>
    /// <returns>A status value describing the result.</returns>
    [LibraryImport(LibraryName, EntryPoint = "MH_ApplyQueued")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    public static partial MinHookStatus ApplyQueued();

    /// <summary>Returns the native MinHook description for a status value.</summary>
    /// <param name="status">Status value to describe.</param>
    /// <returns>The native description, or the enum member name when no description is available.</returns>
    public static string StatusToString(MinHookStatus status) => Marshal.PtrToStringUTF8(StatusToStringPointer(status)) ?? status.ToString();

    [LibraryImport(LibraryName, EntryPoint = "MH_StatusToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    private static partial nint StatusToStringPointer(MinHookStatus status);
}
