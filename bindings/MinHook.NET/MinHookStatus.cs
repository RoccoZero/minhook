namespace MinHookNET;

/// <summary>Identifies the result of a MinHook operation.</summary>
public enum MinHookStatus
{
    /// <summary>The status value is unknown.</summary>
    Unknown = -1,

    /// <summary>The operation completed successfully.</summary>
    Ok = 0,

    /// <summary>The MinHook library is already initialized.</summary>
    ErrorAlreadyInitialized,

    /// <summary>The MinHook library is not initialized.</summary>
    ErrorNotInitialized,

    /// <summary>A hook is already created for the specified target.</summary>
    ErrorAlreadyCreated,

    /// <summary>No hook is created for the specified target.</summary>
    ErrorNotCreated,

    /// <summary>The specified hook is already enabled.</summary>
    ErrorEnabled,

    /// <summary>The specified hook is already disabled.</summary>
    ErrorDisabled,

    /// <summary>The specified target or detour address is not executable.</summary>
    ErrorNotExecutable,

    /// <summary>The target function cannot be hooked because it is unsupported.</summary>
    ErrorUnsupportedFunction,

    /// <summary>Native memory allocation failed.</summary>
    ErrorMemoryAllocation,

    /// <summary>Changing native memory protection failed.</summary>
    ErrorMemoryProtection,

    /// <summary>The specified module was not found.</summary>
    ErrorModuleNotFound,

    /// <summary>The specified exported function was not found.</summary>
    ErrorFunctionNotFound
}
