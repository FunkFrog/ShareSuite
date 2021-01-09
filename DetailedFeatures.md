# [Item Sharing](#item-sharing)
- Any items anyone pick up are given to all living members of your party.
    - By default, lunar items and items that provide bonuses for all members of the party are not shared.
- Items are given directly to everyone but the one who picked the item up, but we're working on changing that. Pick-up banners for everyone are on their way.
- Dead members don't recieve the item, but you can easily change this with the `DeadPlayersGetItems` setting.

# [3D Printer/Cauldron Compatibility](#3d-printer-cauldron-compatibility)
- Each player may consume their own items to earn an item from the cauldron. These items are given directly to the purchaser so that it does not affect other players' items.
- This can be disabled, though it WILL create a duplication glitch. 
- Toggle it with the `PrinterCauldronFix` setting.

# [Money Sharing](#money-sharing)
- Easily share money to balance the mod more towards the singleplayer loot playstyle.
    - When anyone gets money, it gets added to the group's money pool.
    - When anyone spends money, it gets taken away from the group's money pool.
- Adds a gained money scalar — Want more money? Turn it up!
    - Toggle `MoneyScalarEnabled` to then change `MoneyScalar` to change the multiplier of money gained.

# [Shared Equipment](#shared-equipment)
- When you pick up equipment, everyone gets it.
- **(TO-DO)** When someone picks up equipment, they drop the one everyone currently has.
    - At the moment, the equipment just changes directly
- **(TO-DO)** When someone buys an equipment drone, everyone loses their equipment.
    - This currently completely de-sync's shared equipment

# [Balance](#balance)
- Technically, the game should be ever so slightly harder than vanilla this way. Either way, it should be extremely close to the original game's balance.
    - We only spawn the amount of interactables that would be spawned for 1 player in a lobby of any size. 
    - Boss and Lunar items are not shared, along with items that provide buffs for everyone in the party. 
    - There are no ways to abuse this mod to dupe items. 
    - We cut player XP gained from money at the end of rounds to combat leveling faster. 
    - Teleporters only drop one item per boss killed. 
    - This mod has been tediously balanced and we do everything we can to keep the experience as close to vanilla as possible. 
    - If you have any ideas of ways to improve this, please let us know!
    
# [Customization](#customization)
- Toggleable scaling of boss drops and interactables (chests, capsules, etc) for balance
    - Tuned for highly balanced by default.
- Easily customizable - Scalars and multipliers for everything you'd want to change

# [Config](#config)
- Configuration options for enabling/disabling sharing specific item types (*white, green, red, lunar, boss*).
- Item and equipment blacklists also exist for disabling specific items.
- Config file (*RiskOfRain2/BepInEx/config/com.funkfrog_sipondo.sharesuite.cfg*) allows you to customize the mod down to the slightest detail.
