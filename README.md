![](logo.png)
# KindredCommands for V Rising
KindredCommands is a server modification for V Rising that adds chat commands
This is a successor in the line of ChatCommands by Nopey, RPGMods by Kaltharos, CommunityCommands by deca, and AdminCommands by willis. Credits to all of them for their work and inspiration. Also, thanks to the V Rising modding community for ideas!


## Staff Commands
- `.bloodpotion (Bloodtype) (Quality) (Amount)`
  - will give Merlot of specified type and quality. You can also specify an amount
  - Example: *.bp creature 100 1*
  - Shortcut: *.bp*
- `.buff (Buff GUID) (Player)`
  - Adds a buff to a player named, or the user if no one is named.	Be careful, as some buffs can break things. Always test on a test server first.
  - Example: *.buff 476186894 Bob*
- `.debuff (Buff GUID) (Player)`
  - Removes a buff from a player named, or the user if no one is named. Will work on offline players.
  - Example: *.debuff 476186894 Bob*
- `.listbuffs (Player)`
  - will show all buffs on a player																								
  - Example: *.listbuffs Bob*
- `.resetcooldown (Player)`
  - Resets all ability and skill cooldowns for the player		
  - Example: *.resetcooldown Bob*
  - Shortcut: *.cd*
- `.god (Player)`
  - will toggle godmode on a player named, or the user if no one is named. Super speed, spells, damage, etc.																								
  - Example: *.god Bob*
- `.mortal (player)`
  - will toggle godmode off a player named, or the user if no one is named.																								
  - Example: *.mortal Bob*
- `.give (Item prefab name) (amount)`			
  - Example: *.give Headgear_arcmageCrown 1*
  - Shortcut: *.g*
- `.search item (name) (page #)`
  - Will respond with the item prefab name needed.
  - Example: *.search item arcmage*
  - Shortcut: *.search i*
- `.search npc (name) (page #)`
  - Will respond with the item prefab name needed.
  - Example: *.search item vblood*
  - Shortcut: *.search n*
- `.kill (Player)`
  - instantly kills the player named, or the user if no one is named.
  - Example: *.kill Bob*
- `.rename (Old Name) (New Name)`
  - Renames a player. Original name will still show on map to clanmates.
  - Example: *.rename Bob Joe*
- `.revive (Player)`
  - will pick you up from being downed. If fully dead, sends player to coffin.
  - Example: *.revive Joe*
- `.gear [repair/break] (Player)`
  - will repair or break gear on a player (or self if no player specified)
  - Example: *.gear repair Joe* or *.gear break Joe*
  - Shortcut: *.gear r* or *.gear b*
- `.claim (Player) `
  - will change a castle heart owner to whomever is named. This will only work if you're on top of a heart, making it very apparent which heart you'll be changing.
- `.toggleadmin (player)`
  - will add or remove a player from the admin list, authing and deauthing
- `.reloadadmin `
  - will reload the admin list
- `.spawnnpc (guid) (amount)`
  - Spawns an npc specified at your location in the amount specified
  - Example: *.spawnnpc CHAR_ChurchOfLight_Lightweaver 1*
  - Shortcut: *.spwn*
- `.customspawn (Prefab ID) (BloodType) (BloodQuality) (Consumable: true/false) (duration) (level)`
  - Spawns an npc with specific blood type, quality, whether or not you can 'eat' it, how long it will be up, and at what level.
  - Example: *.customspawn CHAR_ChurchOfLight_Lightweaver scholar 100 true -1 100*
  - Shortcut: *.cspwn*
- `.spawnhorse (speed) (acceleration) (rotation) (Spectral: true/false) (amount)`
  - Spawns a horse at your location with the specified stats and amount..
  - Example: *.spawnhorse 10 10 10 false 1*
  - Shortcut: *.sh*
- `banishhorse`
  - will teleport all ghost horses to the eastern side of the incomplete zone. Useful for cleaning up ghost horses without killing them (despawn will kill them, making them unsummonable/dead)
  - Shortcut: *.bh*
- `.despawnnpc (guid) (range)`
  - will kill any entity matching the ID specified. Use sparingly as this is an expensive call, and could cause minor lag depending. Just for the cases where you can't kill something by hand. 
  - Example: *.despawnnpc CHAR_ChurchOfLight_Lightweaver 10*
  - Shortcut: *.dspwn*
- `.spawnban (Prefab GUID name) (reason)`
  - saves a GUID to the banned list, preventing customspawn or spawnnpc from creating it. Helps prevent server crashes and corruption. To remove from the ban list, delete the line from the nospawn.json in the Config folder.
  - Example: *.spawnban CHAR_ChurchOfLight_Lightweaver "This NPC is too cute"*
- `.boss modify (bossname) (level)`
  - will change the level of the boss to the level specified. Upon respawn, they will be their original level. You can still modify the level of the boss while its in its 'blood walk', and it will spawn with that level. Must be near boss.
  - Example: *.boss modify solarus 100*
  - Shortcut: *.boss m*
- `.boss teleportto (name) (WhichOne)`
  - will teleport you to the boss specified. Must be near boss. If multiple bosses are up, you can specify which one to teleport to. Bosses must have been spawned in at least once on the map to be teleported to.
  - Example: *.boss teleportto TheNameOfTheBoss 1*
  - Shortcut: *.boss tt*
- `.reloadstaff`
  - reloads the staff list config file
- `.setstaff (Player) (Rank)`
  - Adds a player to the staff list, with whatever rank for listing in the online staff command return. This isn't the same as admin, as it allows for non-admin staff to be listed as well.
  - Example: *.setstaff Joe King*
- `.removestaff (Player)`
  - Removes a player from the staff list, so they will no longer show in the staff command return when online.
  - Example: *.removestaff Joe*
- `.clan add (Player) “Clan Name”`
  - adds the player named to the clan named. Can exceed clan limitations through this method. (Kick WIP)
  - Example: *.clan add Joe “The Best Clan”*
  - Shortcut: *.c a*
- `.clan changerole (Player) (Role)`
  - Changes the role of a player in their clan. Roles: member, officer, leader
  - Example: *.clan changerole Joe 1*
  - Shortcut: *.c cr*
- `.whereami`
  - will tell you your current location in the world
  - Shortcut: *.wai*



## Player Commands:
- `.afk`
  - will put Zzzzz above character and lock wasd movement. It's a toggle.
- `.staff`
  - will list staff who are online.
- `.ping`
  - tells you your latency
- `.clan list (page #)`
  - Shows a list of populated clans (and their message). Newest clans are at the start of the list.
  - Example: *.clan list 1*
  - Shortcut: *.c l*
- `.clan members “Clan Name”`
  - Shows a list and ranking of players within a named clan. Use quotes around a clan name with any spaces.
  - Example: *.clan members “The Best Clan”*
  - Shortcut: *.c m*
