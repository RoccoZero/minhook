namespace MinHookNET;

/// <summary>Identifies the result of a MinHook operation.</summary>
public enum MinHookStatus
{
    /// <summary>An unknown error occurred. MinHook should not return this value.</summary>
    Unknown = -1,

    /// <summary>The operation completed successfully.</summary>
    Ok = 0,

    /// <summary>The MinHook library is already initialized.</summary>
    ErrorAlreadyInitialized,

    /// <summary>The MinHook library is not initialized yet or has already been uninitialized.</summary>
    ErrorNotInitialized,

    /// <summary>A hook is already created for the specified target.</summary>
    ErrorAlreadyCreated,

    /// <summary>A hook has not been created yet for the specified target.</summary>
    ErrorNotCreated,

    /// <summary>The specified hook is already enabled.</summary>
    ErrorEnabled,

    /// <summary>The specified hook is not enabled yet or is already disabled.</summary>
    ErrorDisabled,

    /// <summary>The specified pointer is invalid because it refers to an unallocated or non-executable memory region.</summary>
    ErrorNotExecutable,

    /// <summary>The target function cannot be hooked because it is unsupported.</summary>
    ErrorUnsupportedFunction,

    /// <summary>Native memory allocation failed.</summary>
    ErrorMemoryAllocation,

    /// <summary>Changing native memory protection failed.</summary>
    ErrorMemoryProtection,

    /// <summary>The specified module is not loaded.</summary>
    ErrorModuleNotFound,

    /// <summary>The specified function was not found.</summary>
    ErrorFunctionNotFound
}
