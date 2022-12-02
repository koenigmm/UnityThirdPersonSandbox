# UnityThirdPersonSandbox
Payed assets from the Unity Asset Store are not included. This project is partly based on the course [Unity 3rd Person Combat & Traversal from GameDev.TV](https://www.udemy.com/course/unity-3rd-person-combat-traversal/). The state machine and close combat were adopted. The following is custom-made:

-	Saving System
-	AI
-	Different types of enemies
-	Patrol routes
-	Damage caused by falling
-	Ranged fights
-	Ammunition
-	Simple leveling system
-	Stamina
-	Health
-	Healing potions
-	VFX and particle effects
-	Glow and highlight shaders
-	Outdoor level with a dungeon (assets from Synty Studios)
-	Lighting changes depending on the player’s location. When the player is inside a dungeon, the level is only lit by spotlights and point lights. Outside, the sun is the only light source.

## AI
Enemies can walk along waypoints. They leave their patrolling route if the player is close enough. Attacked enemies chase the player and strop patrolling as well. The chase ends if the player is dead or distant enough. Enemies return to their patrol if the player can escape. AI-controlled characters can use ranged and melee weapons to attack the player.


## Video
[YouTube](https://youtu.be/0dlH6EUKWg4)

### Builds (Windows, MacOS, Linux)
https://www.koenigmarius.de/downloads/

## Assets
- Synty - [Polygon Western Pack](https://assetstore.unity.com/packages/3d/environments/dungeons/polygon-dungeons-low-poly-3d-art-by-synty-102677) 
- Synty - [Polygon Dungeon Pack](https://assetstore.unity.com/packages/3d/environments/historic/polygon-western-low-poly-3d-art-by-synty-112212)
- ClayManStudio - [Fantasy GUI Package](https://assetstore.unity.com/packages/2d/gui/fantasy-gui-package-137976)
