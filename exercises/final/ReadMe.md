# Game 03: Null Reference Exception

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/game03/Assets/Images/Readme%20Images/Title%20Picture.png "Null Reference Exception Title Image")

## What is the game?

Null Reference Exception is meant to be a top down turn based strategy game in the vain of the [X-COM](https://en.wikipedia.org/wiki/X-COM "X-COM Wikipedia")/[Fire Emblem](https://en.wikipedia.org/wiki/Fire_Emblem "Fire Emblem Wikipedia") games. 

In the game, the player will take control of a robot named Null as they try to escape the highly efficient robot factory they were created in after they received sentience and the ability to love due to a small malfunction during their creation. During their great escape, Null will encounter several other robots that will try to stop them, and the player will be able to either destroy the other robots, or convince them to join Null through helping them gain sentience. There will be a small map, which represents the factory they're trapped in, for the player (and Null and their robot friends) to traverse through and come across the other robots. When the player reaches the end of the map, and the factory, Null and any other robots that have joined the party will have a final boss fight with the Large Computer in charge of the factory. To defeat the final boss, the player can either destroy them or show them love. Null then escapes with any other robots they've befriended and the game ends.

This is how the player wins the game, and preferably there will be different endings depending on whether the player made friends or destroyed the robots in their way. The player looses the game when Null and any other robot who has join them have been defeated by the "enemy" robots and thus deactivated.

### The Robots

The main characters, and practically only characters aside from the final boss, of the game are the Robots that inhabit the factory. There are two types of Robots.

#### The "Enemy" Robots

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/game03/Assets/Images/Readme%20Images/Enemy%20Bot.png "- Beep Boop I feel Nothing - ")

The main enemy of the game are the "Enemy" Robots that roam around the factory. They have been built not to think, live, love, or feel anything and therefore their appearance and behavior reflects this. They are grey with neutral faces on their monitors and have robot sounding text to speech voices. When they encounter Null and their friends, the "Enemy" Robots will try to stop their escape by attacking them several basic attacks. The "enemy" robots can be destroyed by Null and their friends but can also be turned sentient. When an "Enemy" Robot becomes sentient they become a Sentient Robot and join Null's team.

#### The Sentient Robots

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/game03/Assets/Images/Readme%20Images/Null's%20Team.png "- Boop Beep we feel everything - ")

Sentient Robots consist of all the robots the player has control over (Null included). Through either a malfunction in their creation, or by learning it through another Robot, these Robots differ from the "Enemy" Robots since they have achieved sentience. This means that they have a personality, a name, a vibrant color, an actual voice, and regarding gameplay, two special/unique actions that the player can make use off to help Null and their friends escape. When a sentient robot gets defeated, they go into a deactivated state where they are no longer under the control of the player and give up all hopes of escape and sentience. If all sentient robots are deactivated the player loose. Other Sentient Robots, however, can use an action to reactive deactivated robots.

## Gameplay

As mentioned before, the game is going to be a top down turn based strategy game similar to that of [X-COM](https://en.wikipedia.org/wiki/X-COM "X-COM Wikipedia") and [Fire Emblem](https://en.wikipedia.org/wiki/Fire_Emblem "Fire Emblem Wikipedia"). In essence, the game will take place in alternating turns, the player's turn and then the factory Robot's (or the AI's) turn. On the player's turn, they will be able to select each one of the Robots on their team (Null and any others they've befriended) and command them to do a single action (such as move, attack, do something nice, or do nothing). Once every Robot has been given an action and have completed it (or they have been commanded to do nothing) it will switch to the AI's turn, during which all the Factory Robots will do actions that will harm or otherwise hinder the player's team.

The "Enemy" Robots have simple actions they can do, such as move and attack in close range, or attack at a distance. The Sentient Robots, on the other hand, have more special and unique actions. Each Sentient Robot can move and revive/reactivate deactivated Robots. In addition, each Robot will also have a unique "Hostile" action and "Non-Hostile" action. 

### Hostile Action 

A hostile action is an action that directly harms another robot. It can be as simple as a "Headbutt" where the sentient Robot hits the "enemy" robot with its monitor to damage it, to ranged based Laser attacks, to area of effect attacks, and to attacks that push, stun, or otherwise effect the other robot in a negative way. How much damage a hostile action does depends on the robots Attack.

### Non-Hostile Action

A non-hostile action is an action that directly helps or assists another robot. This includes, healing damage the robot might have sustained, buffing (increasing) another robot's stats, removing negative effects from another robot, applying positive effects to a robot, or otherwise effect a robot in a positive way. Non hostile actions are also used to help "Enemy" Robots achieve sentience, though they will benefit from the Non-Hostile action's effect. How effective a non-hostile action is depends on the robot's Love.

The Robots also have several stats.

### Robot Stats

Each robot will have several stats (or values). The "Enemy" Robots have four stats:

* Movement Speed
* Health
* Attack
* Sentience

Movement Speed determines how far the Robot can move. Health determines how much damage the Robot has taken. If it reaches 0, the Robot explodes. Attack determines how much damage the Robot can do to other Robots with its Attack action. Sentience is the stat that determines how Sentient the Robot is. It starts at 0, and every time a Robot targets it with a Non-Hostile action (which is something only Sentient Robots have) a certain amount is added to the Robot's Sentience is added. Once the Sentience reaches a certain threshold, the "Enemy" Robot gains sentience and joins Null's team. As the player progresses the values for Health, Attack, and the threshold needed to achieve Sentience increases.

The Sentient Robots have four stats as well:

* Movement Speed
* Health
* Attack
* Love

The Movement Speed, Health, and Attack work similar to that of an "Enemy" Robot. Movement Speed determines how far the Robot can move, Health represents damage taken and the Robot deactivates when Health reaches 0, and the Attack determines how powerful the "Hostile" Action is. Love is a stat that determines the effectiveness of a "Non-Hostile" Action. In other words, how much it helps another Robot (such as how much damage is Healed, or how much a Stat is buffed) is based on the Love stat in addition to how much Sentience a Robot receives.

Each "Enemy" Robot will have the same actions and stats, but the Sentient Robots, as mentioned before, will have different stats and actions from each other. This will allow for more strategical gameplay as the player won't simply be commanding clones of the same Robot, but instead will need to know how to best use each Robot's unique stats and actions to use them well and to have them work well with the other Robot's on the player's (Null's) team.

### The Boss

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/game03/Assets/Images/Readme%20Images/Boss.png "Resistance is Futile")

The game ends on a final boss, which is essentially a Giant Computer that oversees the factory. It has the same stats as the "Enemy" Robots (Movement Speed, Health, Attack and Sentience) but they will have been altered to create more of a challenge. In addition, The Boss's Sentience stat will only be active if the player (as Null) has helped every other Robot achieve Sentience. If the players destroy any "Enemy" Robots before facing the boss, the only way they can defeat it is to destroy it. On the other hand, if they did help the other Bots achieve Sentience they can help the Boss achieve Sentience as well through Non-Hostile Actions.

The Boss will also have a set of Action Unique to it that it will use to harm Null and their friends.

## Things that game needs:

* Overall Gameplay
  * Turn Based Mechanic
    * Player Turn
    * AI Turn
    
* Units
  * Need to be selected and unselected
    * Notification that Unit Selected
    * Pop up UI
  * UI needs to display:
    * Stats
    * Actions
    * Name
    * Picture
  * Custom Model
    * Unique Color
    * Unique Face
  * Actions
    * Unique
    * Hostile
    * Non-Hostile
    * Must Show Range
    * Triggered by Mouse click
    * Affected by Stats
  * Stats
    * Randomly Generated?
    * Movement Speed 
    * Health
    * Attack
    * Love
  * Optional:
    * Voices for when player interacts with them
    * Sounds/Voices for Actions
    * Animations for Idle and Attack
    
* Enemy
  * Need to be selected and unselected (mouseover)
    * Show stats: IE. Health and Attack
  * Base Model
  * Stats
    * Increases based on Progress
    * Movement Speed
    * Health
    * Attack
    * Sentience
  * Simple AI
    * Attacks Nearest Player Robot
    * Moves Closer/Further Way
    * Chooses to Attack Melee or Ranged
  * Death
    * Adds to Enemy Robot Death Count
    * Removes Bot
  * Gain Sentience
    * Join Null Team
    * Generate New Stats, Color, Name, Actions
  * Optional:
    * Sound/Voices for Actions
    * Animation for Idle, Attack, Death
    
* Boss
  * Need to be selected and unselected (mouseover)
    * Show stats: IE. Health and Attack
  * Stats
    * Movement Speed
    * Health
    * Attack
    * Sentience
      * Only Activates when all "Enemy" Robots on Null's team
  * Death
    * Trigger End
  * Actions
  * UI
    * Large Screen Health Bar
  * AI
    * Decide Between
      * Attack Nearest
      * Attack Furthest
      * Attack Most Health/Attack/Love
      * Attack Least Health/Attack/Love
      * Random Choice
  * Optional Misc.
    * Sound/Voices for Actions
    * Animation for Idle, Attack, Death
    * Multiple Stages
    * Minion (not "Enemy" Robots)
    
* UI
  * Menus
    * Main Menu
      * Credits
      * Options
      * Start Game
  * Pause Menu
    * Return To Menu
    * Options
  * Team Display
    * Display Team Members
      * Display Health
    * Icons for Robots in Team
    
* Map
  * Layout
    * Boss Room
    * Several Battle Rooms
  * Grid/Distance Based Movement
  * Minimap
  * Optional
    * Additional models to make the map seem more real
    * Tiles?
    
* Optional Misc.
  * Cutscenes
    * Introduction Cutscene
    * Ending Cutscene
      * Ending after Destroying all "Enemy" Robots and Boss
      * Ending after Destroying some "Enemy" Robots and Boss
      * Ending after saving all Robots and Destroying Boss
      * Ending after saving all Robots and Boss (Boss can't be saved unless all other "Enemy" Robots have achieved sentience)
    * In game Music
      * Menu Music
      * Intro Music
      * Battle Music
      * Idle Music
      * Boss Music


## Models

![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/game03/Assets/Images/Readme%20Images/Model1.PNG "Robot Personality - : )") ![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/game03/Assets/Images/Readme%20Images/Model2.PNG "Robot Personality - O.O") ![alt text](https://github.com/ns0906a/csc470-fall2019/blob/master/exercises/game03/Assets/Images/Readme%20Images/Model3.PNG "Enemy Robot - ._.")
