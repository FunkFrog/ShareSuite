Versions older then 5 prior are no longer listed on the mod's README, and will be here after each update.

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
