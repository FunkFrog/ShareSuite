Versions older then 5 prior are no longer listed on the mod's README, and will be here after each update.

`2.2.0`
- Updated the mod to work with the latest patch
- Adjusted the Sacrifice Fix to not drop extremely low amounts of items with more then 4 players in a lobby

`2.1.0`
- Fixed an issue regarding scrappers sharing to everyone
- Fixed item picking messages printing twice when rich messages are disabled
- Updated the game for the 1.0 RoR2 update!

`2.0.1`
- Fixed an issue regarding Scavengers dropping less items than intended.
- Fixed an issue where other mods were unable to access shared money while inside the bazaar.

`2.0.0`
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

`1.15.1 + 1.15.0`
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

`1.14.0`
- Complete internal refactor of the codebase for increased maintainability - a HUGE thanks to Harb for this PR
    - If you'd like more information on what exactly this entails, [Visit here](https://github.com/FunkFrog/RoR2SharedItems/pull/66).
- Resolves issue where lunar equipment cannot be purchased
- Incompatibility with BiggerBazaar resolved
- Resolves issues with commands that use decimal numbers
- **MAJOR** New Feature: Randomized Item Sharing
    - When enabled, each player (except the player who picked up the item) will get a randomized item of the same rarity.
    - Thank you to Splamy for PRing this feature!
- Apologies for the rather late update, I've been caught up with family stuff for the holidays. I hope everyone had a great new years!

`1.13.4`
- Updated dependencies
- Fixed issues with Cleansing Pool and Overgrown Chests
- Fixed issues preventing the mod from working with the newest game version
- Thanks to Harb for helping me get this out while I'm low on time! <3

`1.13.3`
- Recompiled with latest versions of R2API and BepInEx
    - Should solve issue with chests not dropping items? (I haven't had any further issues)
- Updated dependencies
- Removed Frogtown Shared Mod Manager

`1.13.2`
- Revision 2 of Experimental Scaling mode
    - Greatly reduces the amount of interactables for the first x rounds - should help to combat the early-game spike in power that comes with everyone having the items.
        - x = Players on team * 2
    - Reduces less and less as you get closer to x rounds.
    - Should feel better to play with as a whole - will no longer be branded as a 'hard mode' but rather the default scaling to balance the mod further.
        - Will include a config option to disable it, but it will default to on
- Support for decimal numbers in the config
    - InteractablesCredit and MoneyScalar are the only scalars effected by this - please do not try to set BossLootCredit to anything other then an integer (whole) number!
- Addition of EmulateSingleplayerMoney
    - The new Shared Money mechanic is much more stable, however, we've noticed that it (~~actually works~~) doesn't have the same effect achieved with the old version of shared money
    - EmulateSingleplayerMoney will scale the amount of money you receive back, adjusted for current prices, so that it feels the same as singleplayer would
        - E.G. - A chest costs $25 in singleplayer. The same chest costs $34 with 2 players.
            - Old money system would give the 2 players $25 - the same as a single player would receive - for a loss at no fault of the players.
            - New money system gives the 2 players $50 - when a single player would receive $25 - resulting in an excess of money that feels imbalanced.
            - EmulateSingleplayerMoney will give the 2 players $34 - when a single player would receive $25 - resulting in the chests "costing the same". 

`1.13.1`
- Changed the way the interactables scaling works to vary with maps - bigger maps will now feel appropriately filled out
- Added a new Experimental Mode! (Future name may be Hardcore/Hard mode, we're not sure yet)
    - Higher numbers of players reduces the amount of interactable spawns.
    - Makes the game more difficult but feels fairer to play to some.
    - Try it out and let us know how you feel on the [Discord](https://discord.gg/c7QnQeb)!
- Fixed a critical bug with new shared money - you can now enter portals without soft-locking your run.

`1.12.0`
- Fixed critical bug with new shared money logic - you should be able to gain money from killing enemies and opening capsules now.
- Fixed critical bug with 3D printer/cauldron logic - they should now function properly with the dupe fix disabled.
    - Thank you to Commie for helping me test and fix these bugs!
- Made improvements to the README's writing to make it sound less like a student who wanted to meet a sentence requirement on an essay.

`1.11.0`
- Complete README.md refactor. It should be a lot nicer to look at and easier to navigate now!
- Resolve issue where items would spawn on maps they weren't supposed to.
- Re-worked the gold sharing system to resolve state issues that have been experienced.
- Add Lepton Daisy and Halcyon Seed to the default unshared list as they provide buffs for everyone in the lobby and granting them to everyone makes them stronger. 
    - If you'd like to update your config to match these changes, please add `,82,86` to the end of the Item Blacklist in your config!

`1.10.4` 
- Fixed a similar bug as 1.10.3; Money will no longer stay above the highest amount of money a dead player has. (Thanks to Elysium for finding this!) 
- Updated R2Api dependency to 2.1.16.

`1.10.3` 
- Moved Discord link below Features list. 
- Fixed incorrectly formatted horizontal rule. 
- Fixed bug with Shared money keeping everyone at the same balance if a teammate died. 
- What should I work on next? [Vote here!](https://www.strawpoll.me/18656636)

`1.10.2` 
- Added Discord link to the README. Mod file is unchanged.

`1.10.1` 
- Update to R2API-2.1.15
- Fixes bug involving boss drops not dropping the correct amount of loot.

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
