# Pickup compatibility checks

Risk of Rain 2 1.5.0 exposes both two-argument and three-argument
`GenericPickupController.SendPickupMessage` methods. The original ShareSuite
2.10.1 DLL fails to initialize `ChatHandler` because it looks up that method by
name alone.

## .NET 8 runner

The runner in `local/checks` checks the real static initializer, resolves the
DLL's game and plugin member references, and verifies that pickup hook
registration and removal target the same methods.

Build it against Mono.Cecil from your BepInEx profile:

```sh
dotnet build local/checks/Checks.csproj -c Release \
  -p:R2Profile="/path/to/r2modman/profile"
```

Run the patched DLL in a fresh process:

```sh
dotnet local/checks/bin/Release/net8.0/Checks.dll patched \
  "/path/to/patched/ShareSuite.dll" \
  "/path/to/Risk of Rain 2_Data/Managed" \
  "/path/to/r2modman/profile"
```

For the negative control, use `original` instead of `patched` and supply the
official ShareSuite 2.10.1 DLL. That run passes only if it reproduces the
original `AmbiguousMatchException`.

The exploratory `rentals` mode cannot establish rental behavior outside
Unity: the standalone .NET 8 environment used for validation could not load
the real BepInEx runtime. Rental expiry and sharing need an in-game test.

## PowerShell initializer check

`check-chat-initializer.ps1` loads the actual game and mod assemblies, executes
the `ChatHandler` static initializer, and checks the bound delegate's parameter
types. Run it with PowerShell 7 in a **fresh process for each DLL**; the default
assembly load context cannot unload a previously loaded ShareSuite assembly.
Windows PowerShell 5.1 cannot load all of the current game's types.

Use a mod profile with BepInEx, R2API Core, R2API CommandHelper and an
`MMHOOK_RoR2.dll` generated for the installed game version. The script accepts
the standard Thunderstore/r2modman profile layout.

```powershell
pwsh -NoProfile -File tests/check-chat-initializer.ps1 `
  -DllPath 'C:\path\to\patched\ShareSuite.dll' `
  -GameDirectory 'C:\path\to\Risk of Rain 2' `
  -ProfileDirectory 'C:\path\to\profiles\YourProfile'
```

As a negative control, run the same command with the original 2.10.1 DLL and
append `-ExpectLegacyFailure`. That run only passes if the original static
initializer throws `AmbiguousMatchException`.

This test does not start Unity, register hooks or exercise multiplayer. Before
releasing, verify host and client item pickups, equipment sharing enabled and
disabled, rich messages enabled and disabled, rental and temporary pickups,
and disabling/re-enabling the mod without duplicate rewards.
