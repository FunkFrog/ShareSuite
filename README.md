[//]: # ( Header imgur album: https://imgur.com/a/2uaeCao )
<p align="center"> 
<img src="https://i.imgur.com/4HTi92E.png">
</p>

[![Discord](https://img.shields.io/discord/614480101647843330?color=%237289DA&label=Discord&style=flat-square)](https://discord.gg/c7QnQeb)[![GitHub](https://img.shields.io/badge/GitHub-visit-c7c7c7?style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems)

Have you ever had someone swoop in and steal that item you just bought? Ever accidentally touched and picked up an item that you were saving for your friend? Frustrating, right? This mod has been developed in response to frustration caused the way items are distributed in Risk of Rain 2. With ShareSuite, we aim to fix that!

Multiplayer RoR2 games should be quick, wacky fun, but often have loot being stolen and power imbalances between players. Obviously, the best way to solve this issue is to remove the incentive to steal the loot in the first place!

|   Most Recent Update - 1.13.0    |
|:--------------------------------|
| **MAJOR** Correction of default scaling to vary with maps |
| **MAJOR** Addition of Experimental Mode — Check the changelog! |
| **MAJOR** Fix critical bug with entering portals |

**If you'd like more info on this update, check the changelog at the bottom of the page!**

<p align="center"> 
<img src="https://i.imgur.com/6jCWYtn.png">
</p>

**Want a more detailed look at any of our features? Click the [*Show me more*] button next to the bullet!**

<img align="right" src="https://media.giphy.com/media/jpzc9YGSe1aDxjQRaN/giphy.gif" width="200">

- Item Sharing — The main goal of this mod is to split items across all players evenly. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#item-sharing)
    - Any items that are picked up are given to all living members of your party.
    - By default, lunar items and items that provide bonuses for all members of the party are not shared.

- Compatible with 3D Printers and Cauldrons — You get to customize your build with them. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#3d-printercauldron-compatibility)
    - Any player using a printer or cauldron only changes THEIR items, leaving others to build as they please.
    
- A robust money sharing/spending system. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#money-sharing)
    - When anyone gets money, it gets added to the group's money pool.
    - When anyone spends money, it gets taken away from the group's money pool.
    - Now includes a gained money scalar — Want more money? Turn it up!

- A shared equipment system — flutter like a group of butterflies or rain lightning from the sky, together. (*`NEW`*) [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#shared-equipment)
    - When you pick up equipment, everyone gets it.
    - ***(TO-DO)*** When someone picks up equipment, they drop the one everyone currently has.
    - ***(TO-DO)*** When someone buys an equipment drone, everyone loses their equipment.
    
- Worried about the game becoming unbalanced? We've got you. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#balance)
    - Tuned to be balanced by default.
    - Built to feel like vanilla singleplayer; The mod scales to feel right with any amount of players, whether you're playing with 2 or 20.
    - Easily customizable — Want more boss loot? Easy! Want more chests? Righty-o, turn that scalar up.

- Want red items to be unique? Hate the fact that everyone gets hooves? No worries, we've got a solution. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#customization)
    - Config options for enabling/disabling sharing specific item types (*white, green, red, lunar, boss*). 
    - Item and equipment blacklists also exist for disabling specific items you don't want shared.

- Want the game to be harder when you're playing with friends? Turn on the Experimental Mode! (*`NEW`*)
    - Scales the interactable spawns to be fewer when you have more players with you.
    - Makes the game harder while still maintaining the core feel of ShareSuite.
    - Works best with Money Sharing on!

- The config file allows you to customize the mod down to the slightest detail. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#config)
    - See the **Configuration** section for more information!

- Native config GUI integration with [SharedModLibrary](https://thunderstore.io/package/ToyDragon/SharedModLibrary/) v2.0.4+ (*[Preview of the GUI](https://i.imgur.com/muzxIsW.png)*) [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#gui)
    - Integration with the BepInEx config UI coming soon!

<p align="center"> 
<img src="https://i.imgur.com/6oHhqsV.png">
</p>

- Install the latest version of **[R2API](https://thunderstore.io/package/tristanmcpherson/R2API/)** if you haven't already. 
- Download and unzip the files with the download button above.
- Place `ShareSuite.dll` in your BepInEx/plugins/ folder
- Run the game and have a great time!

<p align="center"> 
<img src="https://i.imgur.com/XmCL1MI.png">
</p>

1. **Make sure you run the game with the mod installed to generate the config file!**
2. Navigate to `\Risk of Rain 2\BepInEx\config\`
3. Open `com.funkfrog_sipondo.sharesuite.cfg` in any text editor (*we recommend Notepad++ if you have it installed!*)
4. Edit the values for settings as you see fit!

**You can also set settings in-game with the commands listed below.**

1. Open console with `~ + ctrl + alt`
    - Note: you can easily open the console after you've opened it the first time by just pressing `~`!
2. Type in the Command, followed by 
    - A `True/False` value for toggles
    - An integer number for scalars and credits
3. Press enter and you're done!

### Default Config Settings
| Setting                    | Default Value |                    Command |
| :------------------------- | :-----------: | -------------------------: |
| Mod Enabled                |          True |                 ss_Enabled |
| Money is Shared            |         False |           ss_MoneyIsShared |
| White Items are Shared     |          True |        ss_WhiteItemsShared |
| Green Items are Shared     |          True |        ss_GreenItemsShared |
| Red Items are Shared       |          True |          ss_RedItemsShared |
| Equipment is Shared        |         False |         ss_EquipmentShared |
| Lunar Items are Shared     |         False |        ss_LunarItemsShared |
| Boss Items are Shared      |          True |         ss_BossItemsShared |
| Dupe Fix                   |          True |      ss_PrinterCauldronFix |
| Dead Players Get Items     |         False |     ss_DeadPlayersGetItems |
| Override Player Scaling    |          True |   ss_OverridePlayerScaling |
| Experimental Mode          |         False |        ss_ExperimentalMode |
| Interactables Credit       |             1 |     ss_InteractablesCredit |
| Override Boss Loot Scaling |          True | ss_OverrideBossLootScaling |
| Boss Loot Credit           |             1 |          ss_BossLootCredit |
| Money Scalar Enabled       |         False |      ss_MoneyScalarEnabled |
| Money Scalar               |             1 |             ss_MoneyScalar |
| Item Blacklist             |   53,60,82,86 |                        N/A |
| Equipment Blacklist        |         Empty |                        N/A |

**Tip: Want to reset your config?**

- Stop the game if it's currently running
- Navigate to `\Risk of Rain 2\BepInEx\config\`
- Delete `com.funkfrog_sipondo.sharesuite.cfg`
- Start the game again. Upon boot, a fresh new config will be generated!

<p align="center"> 
<img src="https://i.imgur.com/dL9L88y.png">
</p>

#### How do 3d printers and cauldrons work with this mod installed?
```3d printers and cauldrons add the item directly to your item pool. No item orb will drop, it will just appear in your inventory.```

#### Does this make the game easier? How do you balance it?
```Technically, the game should be ever so slightly harder than vanilla this way. Either way, it should be extremely close to the original game's balance.```

```We only spawn the amount of interactables that would be spawned for 1 player in a lobby of any size. Boss and Lunar items are not shared, along with items that provide buffs for everyone in the party. There are no ways to abuse this mod to dupe items. We cut player XP gained from money at the end of rounds to combat leveling faster. Teleporters only drop one item per boss killed. This mod has been tediously balanced and we do everything we can to keep the experience as close to vanilla as possible. If you have any ideas of ways to improve this, please let us know!```

#### Why do I only get 1 item (plus Shrine of the Mountain extras) from the boss?
```These items are shared, so they've been set to drop only 1 by default for balance. You can change this in the config with the Boss Loot Credit config option.```

#### How do blood shrines work when share money is on?
```The user who uses the shrine loses health, but the calculations for how much gold everyone receives is done based on the highest max health player in your party.```

#### I want to play this with my friends. Do they also need to install this mod?
```Everyone having the same mods installed is always a good idea for stability, but is not required. This mod should still be fully functional if your friends only have BepInEx/R2API installed.```

#### How do I play with my friends who don't have mods installed?
```Easy! Install a Build ID changing mod and play on!```
**[Here's an easy link to get to the current Build ID mods!](https://thunderstore.io/?q=Build)**

#### I want to play this mod with more than 4 players!
```Please combine with TooManyFriends. If you'd like to change the amount of boss drops or amount of chests, you can configure that in the config file.```

#### How do I configure the mod while the game is running?
```Open up the console window (ctrl + alt + ~ ). All commands starts with 'ss_' and will autocomplete.```
    
```Or, if you prefer a GUI, you can install SharedModLibrary (link in compatible mods section), press ctrl + f10 while in a game, and navigate to the Settings tab.```

#### Can I use this mod in quick play?
```We DO NOT condone use of this mod in any quick play or prismatic trial games. We will refuse any support for the use of this mod in Quick Play.```

<p align="center"> 
<img src="https://i.imgur.com/hOqXT4A.png">
</p>

#### Known Bugs

- Shared money's state is often inconsistent
    - Crowdfunder de-syncs shared money
- Equipment drones de-sync shared equipment

#### Features in Development

- Experimental Mode (*`NEW`*)
    - Tune Experimental Mode to feel better
    - Add customizability to the difficulty of experimental mode
    - Rename the mode - send us suggestions in the Discord!
- Custom Chat Messages
- Item pickup cards for every player
- Shared Equipment
    - Shared Equipment feature completion
        - Drop one equipment upon a character picking up a new one
        - Remove all equipment upon purchase of equipment drone
    - Remove invalid items from the FrogTown equipment blacklist slider

<p align="center"> 
<img src="https://i.imgur.com/ssmfn9f.png">
</p>

[![Bug Reports](https://img.shields.io/github/issues/FunkFrog/RoR2SharedItems/bug?label=Bug%20Reports&style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems/issues)[![Feature Requests](https://img.shields.io/github/issues/FunkFrog/RoR2SharedItems/enhancement?label=Feature%20Requests&style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems/issues)

**We have multiple channels of contact! Feel free to use any of the following.**

- Join our [Discord Server](https://discord.gg/c7QnQeb).
- DM us on Discord at `FunkFrog#0864` or `Sipondo#5150`.
- Make a new issue on our [GitHub Repo](https://github.com/FunkFrog/RoR2SharedItems).

<p align="center"> 
<img src="https://i.imgur.com/kvxSeNw.png">
</p>

**Mod Developers: If you've tested your mod with ShareSuite and there are no foul interactions, DM me on Discord with the mod link + version tested and I'll add it to this list!**

- [Multitudes](https://thunderstore.io/package/wildbook/Multitudes/) `1.3.0`
    - *Please change the Override Player Scaling setting and Override Boss Loot Scaling to false in the config file. Multitudes will then take priority in modifying the scaling settings.*
- [SharedModLibrary](https://thunderstore.io/package/ToyDragon/SharedModLibrary/) `2.1.1`

<p align="center"> 
<img src="https://i.imgur.com/5qt4b0r.png">
</p>

[![Build](https://img.shields.io/travis/com/FunkFrog/RoR2SharedItems?label=Build&style=flat-square)](https://travis-ci.com/FunkFrog/RoR2SharedItems)[![Latest commit to Master](https://img.shields.io/github/last-commit/FunkFrog/RoR2SharedItems/master?label=Latest%20Commit%20%28master%29&style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems)[![Latest commit to Dev](https://img.shields.io/github/last-commit/FunkFrog/RoR2SharedItems/dev?label=Latest%20Commit%20%28dev%29&style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems/tree/dev)

### `1.13.0 (CURRENT)`
- Changed the way the interactables scaling works to vary with maps - bigger maps will now feel appropriately filled out
- Added a new Experimental Mode! (Future name may be Hardcore/Hard mode, we're not sure yet)
    - Higher numbers of players reduces the amount of interactable spawns.
    - Makes the game more difficult but feels fairer to play to some.
    - Try it out and let us know how you feel on the [Discord](https://discord.gg/c7QnQeb)!
- Fixed a critical bug with new shared money - you can now enter portals without soft-locking your run.

### `1.12.0`
- Fixed critical bug with new shared money logic - you should be able to gain money from killing enemies and opening capsules now.
- Fixed critical bug with 3D printer/cauldron logic - they should now function properly with the dupe fix disabled.
    - Thank you to Commie for helping me test and fix these bugs!
- Made improvements to the README's writing to make it sound less like a student who wanted to meet a sentence requirement on an essay.
    
### `1.11.0`
- Complete README.md refactor. It should be a lot nicer to look at and easier to navigate now!
- Resolve issue where items would spawn on maps they weren't supposed to.
- Re-worked the gold sharing system to resolve state issues that have been experienced.
- Add Lepton Daisy and Halcyon Seed to the default unshared list as they provide buffs for everyone in the lobby and granting them to everyone makes them stronger. 
    - If you'd like to update your config to match these changes, please add `,82,86` to the end of the Item Blacklist in your config!

### `1.10.4` 
- Fixed a similar bug as 1.10.3; Money will no longer stay above the highest amount of money a dead player has. (Thanks to Elysium for finding this!) 
- Updated R2Api dependency to 2.1.16.

### `1.10.3` 
- Moved Discord link below Features list. 
- Fixed incorrectly formatted horizontal rule. 
- Fixed bug with Shared money keeping everyone at the same balance if a teammate died. 
- What should I work on next? [Vote here!](https://www.strawpoll.me/18656636)

**Looking for the changelogs for versions older then 5 prior? [Click here!](https://github.com/FunkFrog/RoR2SharedItems/blob/master/PreviousVersions.md)**
