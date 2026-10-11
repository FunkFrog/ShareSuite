# ShareSuite — fixes fork

This fork is the original [ShareSuite](https://github.com/FunkFrog/ShareSuite) with a few general bug fixes and compatibility fixes for newer Risk of Rain 2 and DLC updates. The main focus is keeping item sharing working with the pickup changes in RoR2 1.5.0.

ShareSuite lets a co-op group share items, money and, optionally, equipment. This fork keeps the original settings, sharing rules and balance options, with a small set of fixes on top.

## What this fork fixes

- **Item pickups on RoR2 1.5.0.** Updates the pickup-message hook and fixes an initialization error that could stop players from collecting items.
- **Rental pickups from DLC3.** Items and equipment marked as rentals use the game's normal pickup handling, so sharing does not turn them into permanent items for teammates. This covers the rental system introduced with [Alloyed Collective](https://store.steampowered.com/app/2781620/Risk_of_Rain_2_Alloyed_Collective/).
- **Normal items with equipment sharing enabled.** The equipment-sharing handler passes ordinary items through to the normal pickup code.
- **Interactions that cost a percentage of health.** Blood-shrine money calculations only run for blood shrines. Other health-cost interactions use the game's normal handling, and players without an active body are skipped in the shrine calculation.

The pickup compatibility work builds on [tyxkk2's upstream PR #254](https://github.com/FunkFrog/ShareSuite/pull/254). See the [changelog](CHANGELOG.md) for the fork changes and the preserved upstream history.

## Original ShareSuite features

- Share eligible item pickups with living teammates, or enable sharing with dead players too.
- Pool money across the group and adjust money gain through configuration.
- Enable shared equipment, with options for handling equipment on the blacklist.
- Keep printers and cauldrons tied to the player using them.
- Adjust boss drops, interactable scaling, item blacklists and randomized sharing.
- Show chat messages about who received each pickup.

The [feature guide](DetailedFeatures.md) explains these settings in more detail.

## Installing the fork

The local ZIP is meant to replace the DLL in an existing ShareSuite installation:

[Download the fork ZIP](https://github.com/Luiz-AVcruz/ShareSuite/raw/refs/heads/local/ror2-1.5-pickups/dist/ShareSuite-2.10.1-local-RoR2-1.5.0.zip).

1. Install and enable ShareSuite and its dependencies in your r2modman profile.
2. Close the game and r2modman before replacing files.
3. Extract the fork ZIP. Replace `BepInEx/plugins/FunkFrog-and-Sipondo-ShareSuite/ShareSuite/ShareSuite.dll` in the profile with `ShareSuite/ShareSuite.dll` from the ZIP.
4. Start the game through **Start modded**.

Keep one active copy of `ShareSuite.dll` in the profile. The fork uses the original plugin ID and configuration file, so your existing settings carry over. The package and BepInEx version remain **2.10.1**; the local DLL's informational version is `2.10.1+local.ror2-1.5.pickup-fix.1`.

The fork writes this line to `BepInEx/LogOutput.log` when it starts:

```text
Local pickup compatibility patch for RoR2 1.5.0 (based on upstream PR #254).
```

Updating or reinstalling ShareSuite through the mod manager can replace the fork DLL. For multiplayer testing, use the same fork build on the host and clients.

## Configuration

The configuration file is `BepInEx/config/com.funkfrog_sipondo.sharesuite.cfg` inside your mod profile. It is created when ShareSuite first starts. Edit it with the game closed, or use the existing `ss_` console commands in-game.

The fork retains the upstream defaults:

| Setting                          | Default Value |                            Command |
| :--------------------------------| :-----------: | ---------------------------------: |
| Mod Enabled                      |          True |                         ss_Enabled |
| Money is Shared                  |          True |                   ss_MoneyIsShared |
| White Items are Shared           |          True |                ss_WhiteItemsShared |
| Green Items are Shared           |          True |                ss_GreenItemsShared |
| Red Items are Shared             |          True |                  ss_RedItemsShared |
| Equipment is Shared              |         False |                 ss_EquipmentShared |
| Lunar Items are Shared           |         False |                ss_LunarItemsShared |
| Boss Items are Shared            |          True |                 ss_BossItemsShared |
| Void Items are Shared            |         False |                 ss_VoidItemsShared |
| Rich Messages Enabled            |          True |             ss_RichMessagesEnabled |
| Drop BL Equip Mode               |          True | ss_DropBlacklistedEquipmentOnShare |
| Randomized Item Sharing          |         False |          ss_RandomizeSharedPickups |
| Lunar Item Randomizing           |         False |                                N/A |
| Boss Item Randomizing            |         False |                                N/A |
| Void Item Randomizing            |         False |                                N/A |
| Dupe Fix                         |          True |              ss_PrinterCauldronFix |
| Sacrifice Fix                    |          True |             ss_SacrificeFixEnabled |
| Dead Players Get Items           |         False |             ss_DeadPlayersGetItems |
| Override Player Scaling          |          True |           ss_OverridePlayerScaling |
| Experimental Mode                |         False |                ss_ExperimentalMode |
| Interactables Credit             |           1.0 |             ss_InteractablesCredit |
| Interactables Offset             |           0.0 |             ss_InteractablesOffset |
| Override Boss Loot Scaling       |          True |         ss_OverrideBossLootScaling |
| Override Simulacrum Loot Scaling |          True |          ss_OverrideSimulacrumLoot |
| Boss Loot Credit                 |             1 |                  ss_BossLootCredit |
| Override Void Field Scaling      |          True |           ss_OverrideVoidFieldLoot |
| Void Field Loot Credit           |             1 |             ss_VoidFieldLootCredit |
| Simulacrum Loot Credit           |             1 |            ss_SimulacrumLootCredit |
| Money Scalar Enabled             |         False |              ss_MoneyScalarEnabled |
| Emulate Singleplayer Money       |          True |        ss_EmulateSingleplayerMoney |
| Money Scalar                     |           1.0 |                     ss_MoneyScalar |

## Compatibility and testing

This build targets **RoR2 1.5.0** and is based on **ShareSuite 2.10.1**. Compilation and assembly checks passed against the installed RoR2 1.5.0#1210 dependencies. The regression check reproduces the original pickup-message error and confirms that the patched initializer selects the correct method.

In-game multiplayer checks are still pending for normal pickups, equipment sharing, rental expiry and blood-shrine purchases. The fixes listed above describe the code changes; they are not a claim that every DLC feature or combination of mods has been tested.

## Building from source

Use .NET SDK 8 and an existing mod profile containing BepInEx, R2API Core, R2API CommandHelper and the generated MMHOOK for your game version. From the repository root:

```sh
dotnet build local/ShareSuite.Local.csproj -c Release \
  -p:GameManaged="/path/to/Risk of Rain 2_Data/Managed" \
  -p:R2Profile="/path/to/r2modman/profile"
```

The DLL is written to `local/bin/Release/netstandard2.1/ShareSuite.dll`. If you are building from the ZIP, run the command from its `source/` directory. The archive includes the matching source and license; game and dependency DLLs are supplied by your own installation. See [dependencies](dependencies.md) for the assemblies used by the build.

## Issues and credits

Report fork issues in [this repository](https://github.com/Luiz-AVcruz/ShareSuite/issues), with your game version, mod list and the relevant section of `BepInEx/LogOutput.log`.

ShareSuite was created by **FunkFrog and Sipondo**. This fork builds on their work and the pickup fixes contributed by **tyxkk2**. The original project is available at [FunkFrog/ShareSuite](https://github.com/FunkFrog/ShareSuite).

The code and this fork are licensed under [GPL-3.0](LICENSE).
