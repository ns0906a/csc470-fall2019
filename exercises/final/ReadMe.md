# Design Document – Cubus



## The Game Overview:

*Cubus*, which is just a Latin word for cube (very clever), is meant to be a game that is inspired by the Super Mario 3D World games and therefore will be a mix of platforming and puzzle elements. In the game, the player will take control of 3-4 blocks, known as “Cubi”, per game level that they will need to use to get to various areas, activate switches, collect coins, and ultimately progress. Each Cubi the players controls can move, jump, stack ontop of other Cubi, and have unique abilities and disabilities that the player must figure out how to  use together to achieve the level goals. The game will consist of several levels the player must progress in order, and the game ends once they have done so.
Game Mechanics:

### Input:
The game will be able to be played with Keyboard or Xbox Controller, but using the Xbox Controller is recommended. If multiplayer levels are implemented, each player will need to use an Xbox Controller each. Below are the control schemes for Keyboard and Xbox Controller respectfully.

#### Xbox Controller:

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/final/ReadmePictures/Xbox%20Controls%20.png "Xbox Controller Controls")

#### Keyboard:

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/final/ReadmePictures/Keyboard%20Controls.png "Keyboard Controls")

### Players:
The Core mechanic of the game revolves around the blocks, or “Cubi,” that the player can control. By pressing the corresponding button on their controller, the player can switch between which Cubi is currently selected. While a Cubi is selected, it’s color becomes slightly darker (to signify to the player it’s being controlled) and the player can now make it walk around using the Joystick, jump using the jump button, and do it’s special action using the special action button (if it has one and that feature has been implemented). If the player makes a Cubi jump onto another other Cubi, it will act as a platform. Using this, the player can use Cubi to create steps or otherwise help other Cubi reach ledges and objectives they couldn’t reach on their own. 

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/final/ReadmePictures/Hopping.png "Player can get a Cubi to a higher ledge by jumping it off of another Cubi")

If the player were to select the Cubi currently being stood on (or acting as a platform), they can move it as a stack of all the Cubi currently standing on it. These simple features will allow for creative problem-solving levels to be made, where the player will need to have Cubi jump off each other, or stack ontop of each other to collect or achieve goals. 

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/final/ReadmePictures/Move.png "Two or more Cubi stacked ontop of each other can move as a stack")

In addition, each Cubi will have unique abilities and disabilities that the player will use to their advantage and must account for. These will be shown to the player based on the color the Cubi. Some examples include:

#### Red Cubi (default): 
Normally a Cubi’s speed decreases for each Cubi currently stacked on top of it (the more Cubi stacked, the slower the bottom Cubi walks). The Red Cubi, however, walks normal speed regardless of how many Cubi stacked ontop of it. This makes it very good for transporting the other Cubi and for puzzles that require the Cubi be stacked and moving. 

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/final/ReadmePictures/Red.png "Red Cubi move at normal speed regardless of Cubi stacked")

#### Blue Cubi: 
Normally a Cubi sinks in water. The Blue Cubi, however, can swim. This is good for achieving goals on the surface of water, and for transporting other Cubi across water.

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/final/ReadmePictures/Blue.png "Blue Cubi swim in water. Other Cubi sink")

#### Yellow Cubi: 
The Yellow Cubi jumps twice as high as the other Cubi. This makes it good for reaching areas other Cubi can’t, and for getting all Cubi’s onto the same ledge.

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/final/ReadmePictures/Yellow.png "Yellow Cubi jump twice as high as other Cubi")
	
#### Miscellaneous Player Information: 
-	Cubi walk slower depending on the amount of other Cubi stacked on top of it
-	If a Cubi has other Cubi on top of it, it can’t jump
-	Cubi sink in water (but luckily they don’t need oxygen)

### Level Design:
		
The plan for *Cubus* is for it to consist of various separate levels. Each level with have a goal for the player to use their Cubi to complete. This goal will vary between activating switches to move platforms, open doors, etc, to reaching specific locations or collecting coins scattered around the level. Once the goal has been completed, the level ends and the player will be prompted to continue to the next level.

Each level will also be planned around the philosophy of Super Mario 3D World Co-Designer Koichi Hayashida. In essence, each level will have a specific theme or challenge. The challenge will be introduced early in the level in a safe way to allow the player to understand what the challenge is and then the challenge will return at the end of the level in a more dangerous way to see if the player has fully mastered it. Any challenges or themes that appeared in previous levels can also return in later levels if it complements that level in an interesting way.

As an example, the first level would consist of two Red Cubi and introduce the player to the Cubi’s movement capabilities in addition to their stacking capabilities. It can do this by having a coin or switch that can’t be accessed without stacking the Cubi ontop of each other. The second level could then introduce the Yellow Cubi but still have elements that require the Cubi to stack thereby making it important that the player mastered the challenge presented in the first level.

The final level would then combine all challenges faced in levels prior as one final check to see if the player has mastered the mechanics of the game.

## Style:

The general plan for the Style of *Cubus* is keep it sweet and cartoony. That means vibrant colors, well- lit areas, rounded edges, and upbeat music for the most part. 

### Visual: 

#### Characters:

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/final/ReadmePictures/Cubi%20-%20Model.PNG "Model for Red Cubi")

The characters that the player controls in the game are blocks with faces and legs known as Cubi. The Cubi, as aforementioned, are rounded cubes with legs sticking out of the bottom and screens at the front with their faces (which normally consist of just two blinking eyes). Each Cubi will have a color that corresponds to their special abilities in the game. While this has been mentioned before, some examples include Red Cubi’s that don’t slow down when carrying other Cubi, and Blue Cubi that show they can swim in Water.
Each Cubi will also have animations for the actions they can do. This includes, bouncing up and down and blinking while waiting, walking when moving, swimming when in the water, and animations for jumping and landing. In addition, it would 

#### Environment:

The game's enviroment will also be cartoony. Once again this means vibrant colors and rounded edges. In addition, 

####


### Audio:
#### Sound Effects:
#### Background Music:

### Interface:
Being the nature of *Cubus* being that it is not a RTS or Strategy game, having a constant interface will loads of information is not so important. Instead, the Interface should simply allow the player to navigate to the option t


## Story/Theme:

## Targets:
### “Low bar”:
The “low bar” for this game is essentially the bare bones minimum stuff that it needs to run. For me this means a few things need to be there:

- The Cubi’s have to stack
- The Cubi’s have to have unique abilities
- There needs to be a single working level
	- That makes use of stacking and unique abilities
	- Has a goal that’s achievable
- The game can be completed
- A menu to start and end the game
### What I expect to get done:
Once the “low bar” has been reached, I can add more to the game to make it more interesting and fun to play. What I expect to have done for the final version of the game includes:

- Everything in the "low bar" list
- 4-5 unique levels
	- Beginning with a starting level that introduces the players to the main game mechanics
	- Ending with a final level that puts all the mechanics to the test
	- Each level explores a unique mechanics
- A Menu that allows the player to select a level
- Music for each level
- Sound Effects for all interactions
- Working (and easy to shift) Keyboard and Controller Support
### “High Bar”:
If I can manage to implement I expect to get done, there are a few others things I'd love to include

- Options Menus:
	- For Sound Volume
	- For Key/Controller Configurations
- Cinematics at the Beginning and end of the game
- Multiplayer
	- With 3-4 Multiplayer Levels

## TimeLine:

11/14 - Submit Design Document

11/18 -

11/21 -

**11/25 - Core Mechanic Playtesting**

11/28 -

12/2 -

12/6 - 

12/9 - 

**12/12 - Final Submission**

