[//]: # ( Hello :D )
# ShareSuite
#### By FunkFrog and Sipondo

This mod has been developed out of frustration of the item distribution in Risk of Rain 2.

Multiplayer RoR2 games should be quick wacky fun, but are often plagued by loot sniping. This mod aims to fix that!

---
 
## Features

- Any item pickups (*non-gland and non-lunar by default*) are given to all living members of your party.

- Native config GUI integration with [SharedModLibrary](https://thunderstore.io/package/ToyDragon/SharedModLibrary/) v2.0.4+ (*[Preview of the GUI](https://i.imgur.com/muzxIsW.png)*)

- Compatible with 3D Printers and Cauldrons - each player may consume their own items to earn an item from the cauldron. These items are given directly to the purchaser so that it does not affect other players' items.

- Money sharing + a gained money scalar.

- Toggleable scaling of boss loot and interactables spawning for balance.

- Configuration options for enabling/disabling sharing specific item types (*white, green, red, lunar, boss*).

- Config file (*RiskOfRain2/BepInEx/config/com.funkfrog_sipondo.sharesuite.cfg*) allows you to personalise the mod.

---

## Discord
We now have a Discord server for the mod! Come join us at https://discord.gg/c7QnQeb

---

### TO-DO

Known Bugs

- Crowdfunder desyncs shared money
- Equipment drones desync shared equipment

Extra Features

- Custom Chat Messages
- Item pickup cards for every player
- Shared Equipment
    - Shared Equipment feature completion
    - Remove invalid items from the FrogTown equipment blacklist slider

---

### Installation Guide

- Copy the `ShareSuite.dll` file to your BepInEx plugins folder.
- Enjoy your items / money

---

### Configuration
1. **Make sure you run the game with the mod installed to generate the config file**
2. Navigate to `\Risk of Rain 2\BepInEx\config\`
3. Open `com.funkfrog_sipondo.sharesuite.cfg` in any text editor
4. Edit the values for settings as you see fit!

You can also set settings in-game with the commands listed below.

### Settings
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
| Interactables Credit       |             1 |     ss_InteractablesCredit |
| Override Boss Loot Scaling |          True | ss_OverrideBossLootScaling |
| Boss Loot Credit           |             1 |          ss_BossLootCredit |
| Money Scalar Enabled       |         False |      ss_MoneyScalarEnabled |
| Money Scalar               |             1 |             ss_MoneyScalar |
| Item Blacklist             |            53 |                        N/A |
| Equipment Blacklist        |         Empty |                        N/A |

---

### FAQ
---

`How do 3d printers and cauldrons work?`

*3d printers and cauldrons add the item to your item pool. You will get the item directly instead of via an orb.*

---

`Does this make the game easier? How do you balance it?`

*Technically, the game should be ever so slightly harder than vanilla this way. Either way, it should be extremely close to the original game's balance.*

*We only spawn the amount of interactables that would be spawned for 1 player in a lobby of any size. Boss and Lunar items are not shared. There are no ways to abuse this mod to dupe items. We cut player XP gained from money at the end of rounds to combat leveling faster. Teleporters only drop one item per boss killed. This mod has been tediously balanced and we do everything we can to keep the experience as un-broken as possible. If you have any ideas of ways to improve this, let us know!*

---

`How do I use Multitudes with this mod?`

*Please change the Override Player Scaling setting and Override Boss Loot Scaling to false in the config file. Multitudes will then take priority in modifying the scaling settings.*

---

`Why do I only get 1 item (plus shrine of the mountain drops) from the boss?`

*These items are shared, so they've been set to drop only 1 by default for balance. You can change this in the config with the Boss Loot Credit config option.*

---

`How do blood shrines work when share money is on?`

*The user who uses the shrine loses health, but the calculations for how much gold they receive is done based on the highest max health player on your team.*

---

`I want to play this with my friends. Do they also need to install this mod?`

*This is always a good idea for stability, but is not required. This mod should still be fully functional if your friends only have BepInEx installed.*

---

`How do I use the in-game configuration GUI?`

*Please install [SharedModLibrary](https://thunderstore.io/package/ToyDragon/SharedModLibrary), press* `ctrl + f10` *while in a game, and navigate to the* `Settings` *tab.*

---

`I want to play this mod with more than 4 players!`

*Please combine with [TooManyFriends](https://thunderstore.io/package/wildbook/TooManyFriends/). If you'd like to change the amount of boss drops or amount of chests, you can configure that in the config file.*

---

`How do I configure the mod while the game is running?`

*Open up the console window (``ctrl + alt + ` ``). All commands starts with `ss_` and will autocomplete.*

**OR**

*See `How do I use the in-game configuration GUI?`*

---

`Can I use this mod in quick play?`

*We DO NOT condone use of this mod in any quick play or prismatic trial games. Please respect our wishes and only use this mod in private lobbies.*

---

### Bug Reports, Suggestions & Feedback

Please feel free to contact us for suggestions/feedback/bug reports on discord (*`FunkFrog#0864` or `Sipondo#5150`*).

---

### Tested Compatibility

- [RolltheRain](https://thunderstore.io/package/Sipondo/RolltheRain/)
- [Multitudes](https://thunderstore.io/package/wildbook/Multitudes/)
	- Please see the *How do I use Multitudes with this mod?* FAQ.
- [TooManyFriends](https://thunderstore.io/package/wildbook/TooManyFriends/)
- [DropInMultiplayer](https://thunderstore.io/package/Morris1927/DropinMultiplayer/)
- [SharedModLibrary](https://thunderstore.io/package/ToyDragon/SharedModLibrary/)
- [Command Artifact](https://thunderstore.io/package/felixire/Command_Artifact/)
- [Chat Commands](https://thunderstore.io/package/vis-eyth/ChatCommands/)

#### Incompatible Mods

Creators of these mods: If you are open to collaborate with us, we'd love to work with you to resolve the conflict!

- [KookehsDropItem](https://thunderstore.io/package/tristanmcpherson/KookehsDropItem_BepInEx/)

---

### Changelog

`1.10.5` - Add Lepton Daisy and Halcyon Seed to the default unshared list as they provide buffs for everyone in the lobby and granting them to everyone makes them stronger. If you'd like to update your config to match these changes, please add `,82,86` to the end of the Item Blacklist in your config!

`1.10.4` - Fixed a similar bug as 1.10.3; Money will no longer stay above the highest amount of money a dead player has. (Thanks to Elysium for finding this!) Also updated R2Api dependency to 2.1.16.

`1.10.3` - Moved Discord link below Features list. Fixed incorrectly formatted horizontal rule. Fixed bug with Shared money keeping everyone at the same balance if a teammate died. What should I work on next? [Vote here!](https://www.strawpoll.me/18656636)

`1.10.2` - Added Discord link to the README. Mod file is unchanged.

`1.10.1` - Update to R2API-2.1.15, resolve bug involving boss drops not dropping the correct amount of loot.

`1.10.0` - Large bugfix patch. Resolved ItemDropAPI erroring, Aurelionite not dropping the Halcyon seed, and a duplication glitch concerning 3D printers.

`1.9.1` - Update R2Api dependency to 2.1.0

`1.9.0` - Fixed typos. Removed the "Queens Gland is Shared" config option, replaced it with putting the item in the shared blacklist by default (item ID 53 if you would like to add it to your current config! If you're updating from a previous version and want this item to remain unshared, you WILL have to add it to your config) Moved the "BossLootCredit" config entry from the "Settings" category to the "Balance" category. If you've modified this from the default, you'll have to change it once more.

`1.8.3` - Fix a bug concerning the two new boss items -- Halcyon Seed and Little Disciple -- not being shared when Share Boss Items is turned on. (Thanks to Eracat for finding this bug!)

`1.8.2` - General README update.

`1.8.1` - README Update to include equipment sharing console command.

`1.8.0` - Shared equipment is here! Updated mod to function with 6/25/19 patch. 

`1.7.4` - Crowdfunder desync fix

`1.7.3` - Massive readme fix

`1.7.2` - Completion of config refactor. It should be easier to read and use now :)

`1.7.1` - README fix. (whoops!)

`1.7.0` - Fix of Boss Loot live update and rare chance for no interactables to spawn bug. Due to overwhelming use of the mod for the money scaling/sharing feature, we have separated the money scalar and the money sharing into two different toggle options. Have fun!

`1.6.6` - Fixed a bug where console commands would spit out an object instead of the current command value when set. **Modified override config names, if you have changed these, you WILL have to set the new ones to the previous values.** Disable Boss Loot Scaling -> Override Boss Loot Scaling and Disable Player Scaling -> Override Player Scaling.

`1.6.5` - Applied hotpatch to fix very rare cases of interactables not spawning at all also causing no teleporter to spawn. Will look into this bug further.

`1.6.4` - Fixed an error on startup.

`1.6.3` - Fixed a README typo.

`1.6.2` - Fixed bug where explosions would catapult you across the map. (Thanks to RookieCookie for reporting this bug!)

`1.6.1` - Brittle Crown no longer de-syncs shared money.

`1.6.0` - [SharedModLibrary](https://thunderstore.io/package/ToyDragon/SharedModLibrary/) v2.0.4+ now integrates directly with ShareSuite to give you an easy config GUI! If you wish to use this feature, download [SharedModLibrary](https://thunderstore.io/package/ToyDragon/SharedModLibrary/)! Activating teleporters when money is shared now splits your money properly between players so you don't get an unintended boost of xp. Clients now sync their money balance with the host on map start for optimal compatibility with other mods.

`1.5.2` - Fixed a conditional that kept boss loot scaling from ever activating. Removed a hard coded testing number that was unintentionally left in the code. (Thanks to jkbbwr for finding these two bugs!)
New & Improved README! Now with a fancy table (that doesn't really work on the site yet but that's okay.)

`1.5.1` - Boss Loot Drop scalar now features live update support!

`1.5.0` - Definitive fix of all known bugs. Capsules now scale their money properly.

`1.4.2` - Fixed reflection.

`1.4.1` - README.md fix.

`1.4.0` - Introduced item blacklisting, made item sharing behave like Loot4All again.

`1.3.0` - Introduced ingame config commands

`1.2.0` - Shared money alpha.

`1.1.1` - Fix of logic concerning 3D printers and Cauldrons.

`1.1.0` - Addition of config option for sharing/not sharing Queen's Glands.

`1.0.1` - Update of the README.md with a new to-do entry and a new FAQ, as well as a slight modification of the manifest.

`1.0.0` - Initial release, merged Loot4All and FunkFrog Shared Items.
