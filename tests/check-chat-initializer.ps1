#Requires -Version 7.0

param(
    [Parameter(Mandatory = $true)][string]$DllPath,
    [Parameter(Mandatory = $true)][string]$GameDirectory,
    [Parameter(Mandatory = $true)][string]$ProfileDirectory,
    [switch]$ExpectLegacyFailure
)

# Run in a fresh PowerShell 7 process for each DLL: assemblies cannot be unloaded
# from the default context. This verifies the real static initializer against the
# installed game's assemblies, without starting Unity or registering hooks.
$ErrorActionPreference = 'Stop'
Add-Type -TypeDefinition @'
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

public static class ChatInitializerRegression
{
    public static void Run(string dllPath, string managed, string profile, bool expectFailure)
    {
        string[] directories = {
            Path.GetDirectoryName(dllPath), managed,
            Path.Combine(profile, "BepInEx", "core"),
            Path.Combine(profile, "BepInEx", "plugins", "MMHOOK"),
            Path.Combine(profile, "BepInEx", "plugins", "RiskofThunder-R2API_Core", "R2API.Core"),
            Path.Combine(profile, "BepInEx", "plugins", "RiskofThunder-R2API_CommandHelper", "R2API.CommandHelper")
        };
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
            string name = new AssemblyName(args.Name).Name + ".dll";
            foreach (string directory in directories) {
                string path = Path.Combine(directory, name);
                if (File.Exists(path)) return Assembly.LoadFrom(path);
            }
            return null;
        };
        Type chat = Assembly.LoadFrom(dllPath).GetType("ShareSuite.ChatHandler", true);
        try {
            RuntimeHelpers.RunClassConstructor(chat.TypeHandle);
        } catch (TypeInitializationException error) {
            if (expectFailure && error.InnerException is AmbiguousMatchException) {
                Console.WriteLine("PASS: original DLL reproduces ChatHandler AmbiguousMatchException.");
                return;
            }
            throw;
        }
        if (expectFailure) throw new Exception("Original DLL did not reproduce the expected failure.");
        Delegate callback = (Delegate)chat.GetField("SendPickupMessage").GetValue(null);
        ParameterInfo[] parameters = callback.Method.GetParameters();
        if (callback.Method.Name != "SendPickupMessage" || parameters.Length != 2 ||
            parameters[0].ParameterType.FullName != "RoR2.CharacterMaster" ||
            parameters[1].ParameterType.FullName != "RoR2.UniquePickup")
            throw new Exception("Incorrect SendPickupMessage overload selected.");
        Console.WriteLine("PASS: patched ChatHandler static initializer completed.");
        Console.WriteLine("PASS: exact overload = " + callback.Method);
        Console.WriteLine("Scope: initialization only; Unity hooks and multiplayer gameplay not executed.");
    }
}
'@

[ChatInitializerRegression]::Run(
    (Resolve-Path -LiteralPath $DllPath).Path,
    (Join-Path $GameDirectory 'Risk of Rain 2_Data\Managed'),
    $ProfileDirectory,
    $ExpectLegacyFailure.IsPresent)
