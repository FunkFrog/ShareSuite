[//]: # ( Header imgur album: https://imgur.com/a/2uaeCao )
![Header Image](https://i.imgur.com/4HTi92E.png "ShareSuite Header Image")

[![Discord](https://img.shields.io/discord/614480101647843330?color=%237289DA&label=Discord&style=flat-square)](https://discord.gg/c7QnQeb)[![GitHub](https://img.shields.io/badge/GitHub-visit-c7c7c7?style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems)

[![FunkFrog Donate](https://i.imgur.com/L9TfFGx.png)](https://ko-fi.com/funkfrog)[![Sipondo Donate](https://i.imgur.com/B7JuCNB.png)](https://ko-fi.com/sipondo)

Have you ever had someone swoop in and steal that item you just bought? Ever accidentally touched and picked up an item that you were saving for your friend? Aggravating, right? This mod has been developed in response to frustration caused the way items are distributed in Risk of Rain 2. With ShareSuite, we aim to fix that!

Multiplayer RoR2 games should be fast-paced wacky fun. Often times, though, players run into problems with loot being stolen or one player dominating the game. Obviously, the best way to resolve this issue is to remove the incentive to hoard loot in the first place!

ShareSuite is a mod with a multitude of systems designed around sharing the things you gather throughout a run, be it items, money, experience, equipment, or otherwise. Check out the "Features" section for more information!

|                       Most Recent Update - 2.10.0                        |
|:------------------------------------------------------------------------:|
|                          Update to support DLC3                          |
|                 Add specific DLC3 items to the blacklist                 |
| There are **KNOWN ISSUES** with this release! Check below for more info. |

*If you'd like more info about this update, check the changelog at the bottom of the page!*

### ![Features](https://i.imgur.com/6jCWYtn.png "ShareSuite Features")

**Want a more detailed look at any of our features? Click the [*Show me more*] button next to the bullet!**

---

**On-By-Default Systems**

- Item Sharing — The main goal of this mod is to split items across all players evenly. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#item-sharing)
  - Any items that are picked up are given to all living members of your party.
  - By default, lunar items and items that provide bonuses for all members of the party are not shared.
  - You can also enable the option to give each member of your party a random item of the same tier you received, if that's more your style!

- Worried about the game becoming unbalanced? We've got you. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#balance)
  - Tuned to be balanced by default.
  - Built to feel like vanilla singleplayer; The mod scales to feel right with any amount of players, whether you're playing with 2 or 20.
  - Easily customizable — Want more boss loot? Easy! Want more chests? Righty-o, turn that scalar up.

- Compatible with 3D Printers and Cauldrons — You get to customize your build with them. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#3d-printercauldron-compatibility)
  - Any player using a printer or cauldron only changes THEIR items, leaving others to build as they please.

- Want to easily reference who got what item? Whether or not something's shared? Who got what when pickups are randomized? Custom chat messages are here for you! [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#chat-messages)
  - Rich Chat Messages let you know who got an item (or who didn't!)
  - Custom message for items that are set to not share, to remove ambiguity.
  - Randomized pickups now display who got what - no more confusion over randomized drops.

---

**Additional Systems**

- A robust money sharing/spending system. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#money-sharing)
  - When anyone gets money, it gets added to the group's money pool.
  - When anyone spends money, it gets taken away from the group's money pool.
  - Now includes a gained money scalar — Want more money? Turn it up!

- A shared equipment system — flutter like a kaleidoscope of butterflies or rain lightning from the sky, together. (*`RECENTLY UPDATED`*) [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#shared-equipment)
  - When you pick up equipment, everyone gets it.
  - When someone picks up equipment, they drop the one everyone currently has.
  - When someone buys an equipment drone, everyone loses their equipment.
  - Handle blacklisted equipment in two ways: drop their item, or don't change it at all.

- Want red items to be unique? Hate the fact that everyone gets hooves? No worries, we've got a solution. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#customization)
  - Config options for enabling/disabling sharing specific item types (*white, green, red, lunar, boss*).
  - Item and equipment blacklists also exist for disabling specific items you don't want shared.

- The config file allows you to customize the mod down to the slightest detail. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#config)
  - See the **Configuration** section for more information!


### ![Installation Guide](https://i.imgur.com/6oHhqsV.png "ShareSuite Installation Guide")

- Install the latest version of **[R2API](https://thunderstore.io/package/tristanmcpherson/R2API/)** if you haven't already.
- Download and unzip the files with the download button above.
- Place `ShareSuite.dll` in your BepInEx/plugins/ folder
- Run the game and have a great time!

### ![Configuration](https://i.imgur.com/XmCL1MI.png "ShareSuite Configuration")

1. **Make sure you run the game with the mod installed to generate the config file!**
2. Navigate to `\Risk of Rain 2\BepInEx\config\`
3. Open `com.funkfrog_sipondo.sharesuite.cfg` in any text editor (*we recommend Notepad++ if you have it installed!*)
4. Edit the values for settings as you see fit!

**You can also set settings in-game with the commands listed below.**

1. Open console with `~ + ctrl + alt`
  - Note: you can easily open the console after you've opened it the first time by just pressing `~`!
2. Type in the Command, followed by
  - A `True/False` value for toggles
  - An integer number Boss Loot
  - A decimal number for Money and Interactable scaling.
3. Press enter and you're done!

### Default Config Settings
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

**Tip: Want to reset your config?**

- Stop the game if it's currently running
- Navigate to `\Risk of Rain 2\BepInEx\config\`
- Delete `com.funkfrog_sipondo.sharesuite.cfg`
- Start the game again. Upon boot, a fresh new config will be generated!

### ![FAQ](https://i.imgur.com/dL9L88y.png "ShareSuite FAQ")

#### How do 3d printers and cauldrons work with this mod installed?
```3d printers and cauldrons add the item directly to your item pool. No item orb will drop, it will just appear in your inventory.```

#### Does this make the game easier? How do you balance it?
```Technically, the game should be ever so slightly harder than vanilla this way. Either way, it should be extremely close to the original game's difficulty.```

```We only spawn the amount of interactables that would be spawned for 1 player in a lobby of any size. Boss and Lunar items are not shared, along with items that provide buffs for everyone in the party. There are no ways to abuse this mod to dupe items. We cut player XP gained from money at the end of rounds to combat leveling faster. Teleporters only drop one item per boss killed. This mod has been tediously balanced and we do everything we can to keep the experience as close to vanilla as possible. If you have any ideas of ways to improve this, please let us know!```

#### Why do I only get 1 item (plus Shrine of the Mountain extras) from the boss?
```These items are shared, so they've been set to drop only 1 by default for balance. You can change this in the config with the Boss Loot Credit config option.```

#### How do blood shrines work when share money is on?
```The user who uses the shrine loses health, but the calculations for how much gold everyone receives is done based on the highest max health player in your party. This is to prevent calling the highest health player over to use the shrine for the maximum coin value.```

#### I want to play this with my friends. Do they also need to install this mod?
```Everyone having the same mods installed is always a good idea for stability, but is not required. This mod should still be fully functional if your friends only have BepInEx/R2API installed, or you have a Build ID changing mod to allow you to play with unmodded clients.```

#### How do I play with my friends who don't have mods installed?
```Easy! Install a Build ID changing mod and play on!```
**[Here's an easy link to get to the current Build ID changing mod!](https://thunderstore.io/package/frostycpu/UnmoddedVersion/)**

#### I want to play this mod with more than 4 players!
```Please combine with TooManyFriends. If you'd like to change the amount of boss drops or amount of chests, you can configure that in the config file.```

#### How do I configure the mod while the game is running?
```Open up the console window (ctrl + alt + ~ ). All commands starts with 'ss_' and will autocomplete.```

```New GUI-based configuration manager coming soon!```

#### Can I use this mod in quick play?
```We DO NOT condone use of this (or any) mod in any quick play or prismatic trial games. We will refuse any support for the use of this mod in Quick Play. Please do not ruin the game experience for others.```

### ![To-Do](https://i.imgur.com/hOqXT4A.png "ShareSuite To-Do")

#### Known Bugs

**None at the moment! :)**

#### Features in Development

- Everything in our 3.0.0 milestone! https://github.com/FunkFrog/RoR2SharedItems/milestone/2

### ![Bug Reports & Suggestions](https://i.imgur.com/ssmfn9f.png "ShareSuite Bug Reports + Suggestions")

[![Bug Reports](https://img.shields.io/github/issues/FunkFrog/RoR2SharedItems/bug?label=Bug%20Reports&style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems/issues)[![Feature Requests](https://img.shields.io/github/issues/FunkFrog/RoR2SharedItems/enhancement?label=Feature%20Requests&style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems/issues)

**We have multiple channels of contact! Feel free to use any of the following.**

- Join our [Discord Server](https://discord.gg/c7QnQeb).
- DM us on Discord at `FunkFrog#0864` or `Sipondo#5150`.
- Make a new issue on our [GitHub Repo](https://github.com/FunkFrog/RoR2SharedItems).

### ![Tested Compatibility](https://i.imgur.com/kvxSeNw.png "ShareSuite Mod Compatibility")

**Mod Developers: If you've tested your mod with ShareSuite and there are no foul interactions, DM me on Discord with the mod link + version tested (as well as any required settings changes) and I'll add it to this list!**

- [Multitudes](https://thunderstore.io/package/wildbook/Multitudes/) `1.5.2`
  - *Please change the Override Player Scaling setting and Override Boss Loot Scaling to false in the config file. Multitudes will then take priority in modifying the scaling settings.*

### ![Incompatible Mods](https://i.imgur.com/jDXbLYR.png "ShareSuite Incompatible Mods")

**Creators of these mods: If you are open to collaborate with us, we'd love to work with you to resolve the conflict!**

- *There are no incompatible updated mods we're currently aware of :)*

### ![Changelog](https://i.imgur.com/5qt4b0r.png "ShareSuite Changelog")

[![Latest commit to Master](https://img.shields.io/github/last-commit/FunkFrog/RoR2SharedItems/master?label=Latest%20Commit%20%28master%29&style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems)

### `2.10.0 (Current Release)`
- Update to support DLC3
- Add the following DLC3 items to the default blacklist
  - Box of Dynamite
  - Orphaned Core
  - Sentry Key
  - Prison Matrix
  - Encrypted Cerebellum
  - Exposed Cerebellum
  - Quick Fix
  - Seared Steak
  - Hearty Stew
  - Ultimate Meal
  - Sauteed Worms
- **KNOWN ISSUES**
  - Singleplayer chat pickup messages are missing
  - Drones encounter errors when being played as post-death with items and money sharing
  - Scrappers currently encounter issues where if a client uses them, they will consume an item and not drop anything. 
    - It is recommended that you only use scrappers as the host for the time being

**Looking for the changelogs for versions older then the current? Check the 'Changelog' tab on our Thunderstore page!**

