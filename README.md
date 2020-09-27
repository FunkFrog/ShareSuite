[//]: # ( Header imgur album: https://imgur.com/a/2uaeCao )
![Header Image](https://i.imgur.com/4HTi92E.png "ShareSuite Header Image")

[![Discord](https://img.shields.io/discord/614480101647843330?color=%237289DA&label=Discord&style=flat-square)](https://discord.gg/c7QnQeb)[![GitHub](https://img.shields.io/badge/GitHub-visit-c7c7c7?style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems)

Have you ever had someone swoop in and steal that item you just bought? Ever accidentally touched and picked up an item that you were saving for your friend? Aggravating, right? This mod has been developed in response to frustration caused the way items are distributed in Risk of Rain 2. With ShareSuite, we aim to fix that!

Multiplayer RoR2 games should be fast-paced wacky fun. Often times, though, players run into problems with loot being stolen or one player dominating the game. Obviously, the best way to solve this issue is to remove the incentive to hoard the loot in the first place!

|    Most Recent Update - 2.4.0    |
|:--------------------------------:|
| Introduced a new Sacrifice system to combat poor drop rates |

**If you'd like more info on this update, check the changelog at the bottom of the page!**

### ![Features](https://i.imgur.com/6jCWYtn.png "ShareSuite Features")

**Want a more detailed look at any of our features? Click the [*Show me more*] button next to the bullet!**

- Item Sharing — The main goal of this mod is to split items across all players evenly. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#item-sharing)
    - Any items that are picked up are given to all living members of your party.
    - By default, lunar items and items that provide bonuses for all members of the party are not shared.
    - You can also enable the option to give each member of your party a random item of the same tier you received, if that's more your style!
    
- Compatible with 3D Printers and Cauldrons — You get to customize your build with them. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#3d-printercauldron-compatibility)
    - Any player using a printer or cauldron only changes THEIR items, leaving others to build as they please.
    
- A robust money sharing/spending system. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#money-sharing)
    - When anyone gets money, it gets added to the group's money pool.
    - When anyone spends money, it gets taken away from the group's money pool.
    - Now includes a gained money scalar — Want more money? Turn it up!

- A shared equipment system — flutter like a group of butterflies or rain lightning from the sky, together. (*`RECENTLY UPDATED`*) [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#shared-equipment)
    - When you pick up equipment, everyone gets it.
    - When someone picks up equipment, they drop the one everyone currently has.
    - When someone buys an equipment drone, everyone loses their equipment.
    - Handle blacklisted equipment in two ways: drop their item, or don't change it at all.
    
- Worried about the game becoming unbalanced? We've got you. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#balance)
    - Tuned to be balanced by default.
    - Built to feel like vanilla singleplayer; The mod scales to feel right with any amount of players, whether you're playing with 2 or 20.
    - Easily customizable — Want more boss loot? Easy! Want more chests? Righty-o, turn that scalar up.

- Want red items to be unique? Hate the fact that everyone gets hooves? No worries, we've got a solution. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#customization)
    - Config options for enabling/disabling sharing specific item types (*white, green, red, lunar, boss*). 
    - Item and equipment blacklists also exist for disabling specific items you don't want shared.

- Want to easy reference who got what item? Whether or not something's shared? Who got what when pickups are randomized? Custom chat messages are here for you! 
    - Rich Chat Messages let you know who got an item (or who didn't!)
    - Custom message for items that are set to not share, to remove ambiguity.
    - Randomized pickups now display who got what - no more confusion about who got what.

- Worried about an early game power spike associated with everyone having items? We've got something in the works for that!
    - Experimental Scaling mode is on its way to becoming the new default scaling mode for interactables
    - Starts players off with much fewer interactables on the first handful of rounds.
    - As players progress through the game, the penalty becomes less and less harsh. 
    - Works best with Money Sharing on!

- The config file allows you to customize the mod down to the slightest detail. [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#config)
    - See the **Configuration** section for more information!

- Native config GUI integration with [SharedModLibrary](https://thunderstore.io/package/ToyDragon/SharedModLibrary/) v2.0.4+ (*[Preview of the GUI](https://i.imgur.com/muzxIsW.png)*) [[*Show me more*]](https://github.com/FunkFrog/RoR2SharedItems/blob/master/DetailedFeatures.md#gui)
    - Integration with the BepInEx config UI coming soon!
    - CURRENTLY DEFUNCT -- WORKING ON INTEGRATION WITH NEW UI NOW

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
| Setting                    | Default Value |                            Command |
| :------------------------- | :-----------: | ---------------------------------: |
| Mod Enabled                |          True |                         ss_Enabled |
| Money is Shared            |         False |                   ss_MoneyIsShared |
| White Items are Shared     |          True |                ss_WhiteItemsShared |
| Green Items are Shared     |          True |                ss_GreenItemsShared |
| Red Items are Shared       |          True |                  ss_RedItemsShared |
| Equipment is Shared        |         False |                 ss_EquipmentShared |
| Lunar Items are Shared     |         False |                ss_LunarItemsShared |
| Boss Items are Shared      |          True |                 ss_BossItemsShared |
| Rich Messages Enabled      |          True |             ss_RichMessagesEnabled |
| Drop BL Equip Mode         |          True | ss_DropBlacklistedEquipmentOnShare |
| Randomized Item Sharing    |         False |          ss_RandomizeSharedPickups |
| Dupe Fix                   |          True |              ss_PrinterCauldronFix |
| Sacrifice Fix              |          True |             ss_SacrificeFixEnabled |
| Dead Players Get Items     |         False |             ss_DeadPlayersGetItems |
| Override Player Scaling    |          True |           ss_OverridePlayerScaling |
| Experimental Mode          |         False |                ss_ExperimentalMode |
| Interactables Credit       |           1.0 |             ss_InteractablesCredit |
| Override Boss Loot Scaling |          True |         ss_OverrideBossLootScaling |
| Boss Loot Credit           |             1 |                  ss_BossLootCredit |
| Override Void Field Scaling|          True |           ss_OverrideVoidFieldLoot |
| Void Field Loot Credit     |             1 |             ss_VoidFieldLootCredit |
| Money Scalar Enabled       |         False |              ss_MoneyScalarEnabled |
| Emulate Singleplayer Money |          True |        ss_EmulateSingleplayerMoney |
| Money Scalar               |           1.0 |                     ss_MoneyScalar |
| Item Blacklist             |   53,60,82,86 |                                N/A |
| Equipment Blacklist        |         Empty |                                N/A |

**Tip: Want to reset your config?**

- Stop the game if it's currently running
- Navigate to `\Risk of Rain 2\BepInEx\config\`
- Delete `com.funkfrog_sipondo.sharesuite.cfg`
- Start the game again. Upon boot, a fresh new config will be generated!
    
### ![FAQ](https://i.imgur.com/dL9L88y.png "ShareSuite FAQ")

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
    
```New GUI-based configuration manager coming soon!```

#### Can I use this mod in quick play?
```We DO NOT condone use of this mod in any quick play or prismatic trial games. We will refuse any support for the use of this mod in Quick Play.```

### ![To-Do](https://i.imgur.com/hOqXT4A.png "ShareSuite To-Do")

#### Known Bugs

**None at the moment! :)**

#### Features in Development

- Experimental Mode - **Currently on development hiatus**
    - Tune Experimental Mode to feel better
    - Add customizability to the difficulty of experimental mode
    - Rename the mode - send us suggestions in the Discord!
- Integrate with [new config manager](https://thunderstore.io/package/JackPendarvesRead/BepinexConfigurationManager/)
- Merge with AutoItemPickup to add new ways to distribute items

### ![Bug Reports & Suggestions](https://i.imgur.com/ssmfn9f.png "ShareSuite Bug Reports + Suggestions")

[![Bug Reports](https://img.shields.io/github/issues/FunkFrog/RoR2SharedItems/bug?label=Bug%20Reports&style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems/issues)[![Feature Requests](https://img.shields.io/github/issues/FunkFrog/RoR2SharedItems/enhancement?label=Feature%20Requests&style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems/issues)

**We have multiple channels of contact! Feel free to use any of the following.**

- Join our [Discord Server](https://discord.gg/c7QnQeb).
- DM us on Discord at `FunkFrog#0864` or `Sipondo#5150`.
- Make a new issue on our [GitHub Repo](https://github.com/FunkFrog/RoR2SharedItems).

### ![Tested Compatibility](https://i.imgur.com/kvxSeNw.png "ShareSuite Mod Compatibility")

**Mod Developers: If you've tested your mod with ShareSuite and there are no foul interactions, DM me on Discord with the mod link + version tested and I'll add it to this list!**

- [Multitudes](https://thunderstore.io/package/wildbook/Multitudes/) `1.4.0`
    - *Please change the Override Player Scaling setting and Override Boss Loot Scaling to false in the config file. Multitudes will then take priority in modifying the scaling settings.*
    
### ![Incompatible Mods](https://i.imgur.com/jDXbLYR.png "ShareSuite Incompatible Mods")

**Creators of these mods: If you are open to collaborate with us, we'd love to work with you to resolve the conflict!**

- *There are no incompatible updated mods we're currently aware of :)*

### ![Changelog](https://i.imgur.com/5qt4b0r.png "ShareSuite Changelog")

[![Build](https://img.shields.io/travis/com/FunkFrog/RoR2SharedItems?label=Build&style=flat-square)](https://travis-ci.com/FunkFrog/RoR2SharedItems)[![Latest commit to Master](https://img.shields.io/github/last-commit/FunkFrog/RoR2SharedItems/master?label=Latest%20Commit%20%28master%29&style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems)[![Latest commit to Dev](https://img.shields.io/github/last-commit/FunkFrog/RoR2SharedItems/dev?label=Latest%20Commit%20%28dev%29&style=flat-square)](https://github.com/FunkFrog/RoR2SharedItems/tree/dev)

### `2.4.0 (CURRENT)`
- Introduces a new Sacrifice fix to fix poor drop rates with larger groups of people
    - Thanks to raeon for the PR!

### `2.3.0`
- Fixed a bug where scrapper wouldn't output the correct amount of scrap

### `2.2.0`
- Updated the mod to work with the latest patch
- Adjusted the Sacrifice Fix to not drop extremely low amounts of items with more then 4 players in a lobby

### `2.1.0`
- Fixed an issue regarding scrappers sharing to everyone 
- Fixed item picking messages printing twice when rich messages are disabled 
- Updated the game for the 1.0 RoR2 update! 

### `2.0.1`
- Fixed an issue regarding Scavengers dropping less items than intended.
- Fixed an issue where other mods were unable to access shared money while inside the bazaar.

### `2.0.0`
- The mod turns 1 year old in a handful of days! Happy birthday, ShareSuite! Thank you, everyone, for your continued support of the mod <3
- **MAJOR** Full completion of the Shared Equipment system
    - Equipment Drones no longer de-sync Shared Equipment
        - When purchased, everybody sharing the equipment with the player activating the drone has their equipment removed from their inventory
        - If a blacklisted equipment is spent, only that equipment is used
        - Blacklisted equipment are left alone if a shared equipment is used to purchase
    - Adds a new config option to control how shared equipment handles blacklisted equips
    - Blacklisted equipment are now handled in two toggleable modes:
        - Drop Item Mode: If you have a blacklisted equipment, drop it next to you and get the new item
        - Leave Alone Mode: If you have a blacklisted equipment, don't do anything to it
- **MAJOR** Addition of brand new shiny rich text messages 
    - Rich messages tell you who exactly got what
    - The messages will state if an item isn't shared
    - If randomized loot sharing is on, the message will tell who got what items
- ShareSuite gets an updated logo!
    - To celebrate one year of ShareSuite, we've now got an updated logo with new characters and items!
- Finally fix the bug with the crowdfunder desyncing shared money
    - Definitely didn't leave this uncorrected for a year.
    - And we definitely didn't only get a bug report about 2 months ago. Nope!
- Scavenger now drops an appropriate amount of items for the runs
- Add another entrypoint for other mods to check the status of shared money
- Fixes a bug concerning Ghor's Tome not adding gold
- Fixes an issue with Sacrifice creating WAY too many dropped items
- Blacklist more maps for interactables scaling to prevent chests from going where they shouldn't be
- Fixes an issue where picking up items would display [0] after the item
- Fixes an issue where randomized shared loot would sometimes crash the game

### `1.15.1 + 1.15.0`
- **1.15.1 ADDRESSES A CRITICAL BUG THAT BREAKS ITEM/MONEY SHARING**
- Hello everyone! It's been a while, I hope you're all doing well and staying healthy during this rough time.
- Make sure you grab the newest test build of R2API.dll and MMHOOK_Assembly-CSharp.dll from #r2api and #hook, respectively, [in the main modding discord!](https://discord.gg/5MbXZvd)
- Fixed a few bugs concerning the ~~new~~ (3 month old) randomized shared loot
    - Randomised drops are now only pulled from the currently available items, so you won't be able to get anything you haven't unlocked
    - Randomized drops will no longer include items on the sharing blacklist
- Fixed a bug concerning deaths by the Grovetender 
- [Added an entrypoint for other mods to add money while Shared Money is on](https://github.com/FunkFrog/RoR2SharedItems/issues/67#issuecomment-606823289)
    - Finally, I got around to adding a method to reflect into to add/remove money from the shared money pool. Click the link above to read more about how to use this. Thanks a ton, Harb!
- Boolean commands are now more user friendly!
    - "Yes", "True", and "1" now all work for affirmatives
    - "No, "False", "0", and "-1" now all work for negatives
    - **Case doesn't matter!**
- **MAJOR** Introduction of Void Field Loot Scaling!
    - Now you don't have to worry about the Void Fields netting an excessive amount of loot!
    - Added two new commands
        - `ss_OverrideVoidFieldLoot <boolean>` enables or disables the scaling override
        - `ss_VoidFieldLootCredit <integer>` changes the amount of items each cell drops
        - And, as always, both of these commands are live-update (no restarts needed!)

**Looking for the changelogs for versions older then 5 prior? [Click here!](https://github.com/FunkFrog/RoR2SharedItems/blob/master/PreviousVersions.md)**

