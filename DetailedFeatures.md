# ShareSuite feature guide

This fork keeps the original ShareSuite features and settings, with the general fixes and DLC pickup compatibility changes described in the [README](README.md). The sections below explain how sharing is configured.

## Item sharing

Eligible item pickups are given to living teammates. Enable `DeadPlayersGetItems` if dead players should receive shared items too.

Sharing can be configured separately for white, green, red, boss, lunar and void items. Lunar and void sharing are disabled by default. Items on the blacklist are excluded from sharing.

`RandomizeSharedPickups` gives each teammate a random eligible item of the same tier instead of the exact item picked up. Lunar, boss and void randomization have their own settings.

Rental pickups use the game's normal handling and are not copied to teammates as permanent items. Their lifetime remains under the game's rental system.

## Printers and cauldrons

The `PrinterCauldronFix` setting keeps printer and cauldron exchanges tied to the player using them. This lets each player change their own build while ordinary pickups remain shared.

The fix is enabled by default. Disabling it allows those exchanges to interact with ordinary item sharing and can create duplicate rewards.

## Money sharing

With `MoneyShared` enabled, money is pooled across the group. Purchases draw from that pool. Enable `MoneyScalarEnabled` and adjust `MoneyScalar` to change money gain.

Blood-shrine rewards are calculated using the highest maximum health among players with an active body. Other interactions that cost a percentage of health use the game's normal handling.

## Equipment sharing

Equipment sharing is disabled by default. Enable `EquipmentShared` to give eligible equipment to teammates when it is picked up.

The equipment blacklist and `DropBlacklistedEquipmentOnShare` setting control how a player's excluded equipment is handled when shared equipment is received. Rental equipment follows the game's normal pickup path.

Normal item pickups still run through the item-sharing handler when equipment sharing is enabled.

## Scaling and balance

ShareSuite includes settings for player scaling, interactable credits, boss drops, Void Fields and Simulacrum rewards. These let a group adjust how much loot is available alongside shared pickups.

The fork retains the original defaults. If another mod also controls these systems, configure which mod should own the affected scaling settings.

## Chat messages

`RichMessagesEnabled` controls ShareSuite's pickup messages. They show who received an item, including results from randomized sharing. Rental pickup messages use the game's normal message handling.

## Configuration

Settings are stored in `BepInEx/config/com.funkfrog_sipondo.sharesuite.cfg` within your mod profile. The [README](README.md#configuration) lists the upstream defaults and console commands.
