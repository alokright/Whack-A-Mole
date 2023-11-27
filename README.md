# Whack-A-Mole Unity Game Design Document

## Overview

Welcome to the Whack-A-Mole Unity Game! This document outlines the design and architecture of the game, providing insights into its main components, flow, and additional features.

## Game Flow

The game follows a structured flow:

- MainMenu => ShowLevels => User Selects a Level => GameManager Loads Levels using LevelManager
- GamePlayManager Updates UI and LevelManager Starts Game Timer
- LevelManager Shows Moles at Random Time & tracks Lives
- Level Finishes or GameOver
- User gets a game end popup; in case of level failure, they can watch an ad and continue with an extra life.
- Player Comes back to MainMenu

## Additional Game Features

1. Players start with 3 Max lives.
2. Players can increase Max Lives by watching Ads.
3. Players can Watch Ads and resume a level.
4. Game gets paused if put in the background. If users close the game, the current level ID is serialized, and the player will start from that level.
5. One life gets replenished every 2 minutes.

## Main Components

### 1. GameManager (`Assets/Scripts/Managers/GameManager.cs`)

- **Design:** Simple MonoBehaviour ensuring loose coupling.
- **Purpose:** Manages MainMenu visibility, saved games, and game states (e.g., GameOver, Level Complete) by listening to events.
- **Extension:** Currently no specific extension. Designed for easy additions without disrupting existing functionality.

### 2. GameEventManager (`Assets/Scripts/Managers/GameEventManager.cs`)

- **Design:** Singleton Observer pattern.
- **Purpose:** Subject-agnostic game events provider, allowing subjects to trigger events.
- **Extension:** Encourages easy addition of new events and methods for subjects and observers through interfaces.

### 3. LevelManager (`Assets/Scripts/Managers/LevelManager.cs`)

- **Design:** Singleton for centralized level management.
- **Purpose:** Provides level data and helper methods for streamlined level-loading processes.
- **Extension:** Singleton design facilitates straightforward integration of additional level-related features.

### 4. LevelData, LevelDataProvider, ILevelDataProvider (`Assets/Scripts/Levels/`)

- **Design:** LevelData as ScriptableObjects encapsulating details.
- **Purpose:** Offers a structured way to provide level-specific data.
- **Extension:** Supports various providers (local, networked, etc.) through interfaces for flexibility.

### 5. PlayerDataManager, LocalPlayerDataProvider, IPlayerDataProvider, NetworkPlayerDataProvider (`Assets/Scripts/PlayerData/`)

- **Design:** Singleton managing player profile and progress data.
- **Purpose:** Centralized access to player data.
- **Extension:** Dynamic polymorphism via IPlayerDataProvider for seamless integration of multiple data sources. Supports compile-time switching between sources.

### 6. Mole (`Assets/Scripts/`)

- **Design:** Simple MonoBehaviour with potential for refactoring into components like IMoveable, IKillable, MoleVisuals.
- **Purpose:** Implements basic Mole movement and interaction.
- **Extension:** Modular design for easy composition of various Mole types. Enables extension and customization of Mole behavior.

### 7. AdManager, AbstractAdProvider, IAdListener, IronsourceAdProvider (`Assets/Scripts/Ads/`)

- **Design:** AdManager follows a similar architecture as PlayerDataManager, utilizing an abstract AdProvider and common listener interfaces.
- **Purpose:** Central hub for managing advertisements, ensuring a cohesive and easily extensible structure.
- **Extension:** Modular design facilitates the addition of new ad providers and listeners through interfaces. AbstractAdProvider acts as a blueprint for specific providers.

### 8. AudioManager (`Assets/Scripts/Managers/`)

- **Design:** Event-driven AudioManager.
- **Purpose:** Plays audio based on game events.

### 9. LiveManager (`Assets/Scripts/Managers/`)

- **Design:** Simple MonoBehaviour initialized by GameManager.
- **Purpose:** Triggers events and tracks full life state and timer UI.
- **Extension:** Adaptable for managing various local timers and associated UIs in the future.

### 10. GameSaveHandler and ISaveGameState (`Assets/Scripts/GameSave/`)

- **Design:** Simple MonoBehaviour.
- **Purpose:** Tracks GameState data from objects implementing ISaveGameState, providing helper methods for game restart.
- **Extension:** Supports dynamic binding and different data sources like local, networked, etc.

### 11. UI components

#### - GameEndUI

- **Design:** Event-driven game end popup.

#### - LevelUI

- **Purpose:** Main menu level details UI.



**Happy Whacking!**
