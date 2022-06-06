# Requirements

- `Unity 2020.3 LTS`
- `spine-unity 3.7` *(already included in the repo)*

# Download

Compiled Windows and Android binaries can be downloaded here.

# About

This is a sample project made as a part of the application process for Unity Developer position at Unfrozen Studio.

### Provided assets

  - Miner model *([Spine](http://esotericsoftware.com/) asset)*
  - Background images

### Objective

Implement barebones combat mechanics, using [Iratus: Lord of the Dead](https://store.steampowered.com/app/807120/Iratus_Lord_of_the_Dead/) and [Darkest Dungeon](https://store.steampowered.com/app/262060/Darkest_Dungeon/) as a reference:

  - There are 2 squads facing each other, with 4 characters each.
  - The combat is turn-based. Each unit moves once per round.
  - The turn order is random.
  - There are 2 action buttons: attack and skip the turn.
  - Pressing "attack" should let the player choose the target.
  - During the attack, the attacker and its target move to the foreground.
  - Character animations were made with Spine. The project should utilize Spine Unity package.

# Documentation

## Classes

#### Combat

  - `CombatGameplay` - Bootstraps Combat game mode. Should be attached to a game object somewhere in the scene.
  - `CombatManager` - Controls the flow of the combat.
    - `CombatQueue` - Turn order of the characters.
  - `CombatBattleground` - Root class for managing character placement in the scene.
    - `CombatFormation` - Contains spots for all the characters in the same team.
      - `CombatFormationSpot` - Contains data for a given combat formation spot. Can be assigned a character.
  - `CombatAction` *(SO)* - Scriptable Object that defines a combat action for a given character (Relevant animation, events, etc.)
  - `CombatUI`

#### Character

  - `Character` - Basically, a character entity that controls the assigned character model.
  - `CharacterHUD`

#### Environment

  - `EnvironmentController` - Controlls paralax effects and background darkening (tint).

#### Camera

  - `CameraController` - Performs camera panning.

#### Other

  - `EventManager` - Basic event system to avoid tight coupling.

## Z Layers

  - `>0` - Scene environment
  - `0` - Characters
  - `0.01` - Tint overlay
  - `-1` - Combat action

# License

This project is licensed under [MIT License](http://opensource.org/licenses/MIT), with the exception of 2 directories:

### `./Resources`

All the assets in this folder belong to Unfrozen Studio, and cannot be (re)used outside of this repo.

### `./Spine`

`spine-unity` package belongs to Esoteric Software.