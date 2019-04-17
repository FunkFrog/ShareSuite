# ShareSuite
#### By FunkFrog and Sipondo

This mod has been developed out of frustration of the item distribution in Risk of Rain 2.

Multiplayer RoR2 games should be quick wacky fun, but are often plagued by loot and chest sniping. This mod aims to fix that!

---

## Features

- Any item pickups (non-gland and non-lunar by default) are given to all living members of your party.

- Compatible with 3D Printers and Cauldrons - each player may consume their own items to earn an item from the cauldron. These items are given directly to the purchaser so that it does not affect other players' items.

- Toggleable scaling of boss loot and interactables spawning for balance.

- Configuration options for enabling/disabling sharing specific item types (white, green, red, lunar, boss).

- Config file (RiskOfRain2/BepInEx/config/com.funkfrog_sipondo.sharesuite.cfg) allows you to personalise the mod.
---

### TO-DO

- Shared Equipment
- In-Game commands for changing config options, as well as a config option to completely disable the mod
- Config option for disabling specific items from being shared

---

### Installation Guide

- Copy the SharedSuite.dll file to your BepInEx plugins folder.
- Enjoy your items / money

---

### Configuration
1. Make sure you run the game with the mod installed to generate the config file
2. Navigate to "\Risk of Rain 2\BepInEx\config"
3. Open "com.funkfrog_sipondo.sharesuite.cfg" in any text editor
4. Edit the 'true' or 'false' values for settings as you see fit!

#### Default Settings
- Mod Enabled : True
- Dupe Fix : True
- Disable Player Scaling : True
- Interactables Credit : 1
- Disable Boss Loot Scaling : True
- Boss Loot Credit : 1
- Money Shared : False
- White Items are Shared : True
- Green Items are Shared : True
- Red Items are Shared : True
- Lunar Items are Shared : False
- Boss Items are Shared : True
- Queens Glands are Shared : False
- Blacklist should be empty.

---
### FAQ
---
*How do 3d printers and cauldrons work?*

3d printers and cauldrons directly add the item to your item pool.
You will get the item directly instead of via an orb.

---
*Does this make the game easier?*

Technically, the game should be ever so slightly harder than vanilla this way. Either way it should be extremely close to the original game's balance.

For example, the amount of interactables (chests etc.) is similar to that in a single player game.

---
*Why do I only get 1 item (plus shrine of the mountain drops) from the boss?*

These items are shared as well, so you'll all get a copy.

This can be configured with the Boss Loot Credit config setting.

---
*How do blood shrines work when share money is on?*

The user who uses the shrine loses the normal health percentage, but the calculations for how much gold they recieve is done based on the max health of the member with the highest max health on your team.

---
*I want to play this with my friends. Do they also need to install this mod?*

This is always a good idea for stability, but is not required. This mod should still be fully functional if your friends only have BepInEx installed without the mod.

---
*I want to play this mod with more than 4 players!*

Please combine with  [TooManyFriends](https://thunderstore.io/package/wildbook/TooManyFriends/). If you'd like to change the amount of boss drops or amount of chests, you can configure that in the config file.

---
*How can I make [Multitudes](https://thunderstore.io/package/wildbook/Multitudes/) spawn more interactables when combined with this mod?*

Please change the Disable Player Scaling setting and Disable Boss Loot Scaling to false in the config file. Multitudes will then take priority in modifying the scaling settings.

---
*How do I configure the mod while the game is running?*

Open up the console window. All commands starts with **ss** and will autocomplete.

---

**Please feel free to contact us for suggestions/feedback/questions on discord (FunkFrog#0864 or Sipondo#5150).**

---
### Tested compatibility

- [RolltheRain](https://thunderstore.io/package/Sipondo/RolltheRain/)
- [Multitudes](https://thunderstore.io/package/wildbook/Multitudes/)
- [TooManyFriends](https://thunderstore.io/package/wildbook/TooManyFriends/)
- [DropInMultiplayer](https://thunderstore.io/package/Morris1927/DropinMultiplayer/)

---
### Changelog

1.4.0: Introduced item blacklisting, made item sharing behave like Loot4All again.

1.3.0: Introduced ingame config commands

1.2.0: Shared money alpha.

1.1.1: Fix of logic concerning 3D printers and Cauldrons.

1.1.0: Addition of config option for sharing/not sharing Queen's Glands.

1.0.1: Update of the README.md with a new to-do entry and a new FAQ, as well as a slight modification of the manifest.

1.0.0: Initial release, merged Loot4All and FunkFrog Shared Items.
