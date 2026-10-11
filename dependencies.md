# Build dependencies

The fork uses the same core dependencies as ShareSuite. The package requirements are listed in `manifest.json`; the local build reads their assemblies from an existing r2modman profile.

The build needs:

- **BepInEx 5**, including `BepInEx.dll`, `Mono.Cecil.dll` and `MonoMod.Utils.dll` from `BepInEx/core`.
- **R2API Core** and **R2API CommandHelper** from the profile's plugins directory.
- **`MMHOOK_RoR2.dll`**, generated for the installed game version by HookGenPatcher.
- **RoR2 and Unity assemblies**, including `RoR2.dll`, `HGCSharpUtils.dll`, `HGUnityUtils.dll`, `com.unity.multiplayer-hlapi.Runtime.dll` and `UnityEngine*.dll` from `Risk of Rain 2_Data/Managed`.
- **.NET SDK 8** to compile the `netstandard2.1` local project.

Supply the game and profile paths through the `GameManaged` and `R2Profile` MSBuild properties, as shown in the [README](README.md#building-from-source).

Game and dependency DLLs are read from your installation and are not bundled in the fork ZIP.
