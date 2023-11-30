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

2. [Level Designer & GameConfig]Custom editor to Create a new level from scratch, Create a new Level from the template and Edit a level. GameConfig Scriptable object to handle design values. 
3. [Ads System]Players can Watch Ads and resume a level.
4. [Save System]Game gets paused if put in the background. If users close the game, the current level ID is serialized, and the player will start from that level.
5. [Local Timer System]One life gets replenished every 15 secs.

## Main Components

### Level Designer Editor & GameConfig ScriptableObject

Custom Editor to create and edit levels which can be accessed from Deca/Level Editor
1. Create new level:
2. Create a level using another level as Template:
3. Edit a Level:

#### LevelManager (`Assets/Scripts/Managers/LevelManager.cs`)

- **Design:** Singleton for centralized level management.
- **Purpose:** Provides level data and helper methods for streamlined level-loading processes.
- **Extension:** Singleton design facilitates straightforward integration of additional level-related features.

#### LevelData, LevelDataProvider, ILevelDataProvider (`Assets/Scripts/Levels/`)

- **Design:** LevelData as ScriptableObjects encapsulating details.
- **Purpose:** Offers a structured way to provide level-specific data.
- **Extension:** Supports various providers (local, networked, etc.) through interfaces for flexibility.

### Save System System Architecture
GameSaveHandler and ISaveGameState (`Assets/Scripts/GameSave/`)

- **Design:** Simple MonoBehaviour.
- **Purpose:** Tracks GameState data from objects implementing ISaveGameState, providing helper methods for game restart.
- **Extension:** Supports dynamic binding and different data sources like local, networked, etc.

### Unity Ads Provider System Architecture

#### 1. `AbstractAdProvider.cs`
```csharp
public abstract class AbstractAdProvider
{
    protected IAdListener Listener;

    public AbstractAdProvider(IAdListener listener)
    {
        InitializeProvider(listener);
    }

    public abstract void InitializeProvider(IAdListener listener);
    public abstract void PreloadAds();
    public abstract bool CanShowAds();
    public abstract void ShowAds();
}
```
  - Serves as a base class for all ad providers.
  - `InitializeProvider`: Initializes the ad provider with a listener.
  - `PreloadAds`: Preloads ads in preparation for showing.
  - `CanShowAds`: Checks if ads can be shown.
  - `ShowAds`: Displays the ads.

#### 2. `IAdListener.cs`
```csharp
public interface IAdListener
{
    // Interface methods (not shown in the file content provided)
}
```
- Defines the contract for ad event listeners.
- Contains methods that respond to ad-related events (e.g., ad loaded, ad failed to load).

#### 3. `IronSourceAdsProvider.cs`
```csharp
public class IronSourceAdsProvider : AbstractAdProvider
{
    public IronSourceAdsProvider(IAdListener listener) : base(listener) { }

    public override void InitializeProvider(IAdListener listener)
    {
        // IronSource-specific initialization code
    }

    public override void PreloadAds()
    {
        // Code to preload IronSource ads
    }

    public override bool CanShowAds()
    {
        // Check if IronSource ads can be shown
        // Return true or false accordingly
    }

    public override void ShowAds()
    {
        // Code to display IronSource ads
    }

}
```
- An implementation of `AbstractAdProvider` for IronSource.
- Implements the abstract methods defined in `AbstractAdProvider` for the specific case of IronSource ads.

#### 4. `AdManager.cs`
```csharp
public class AdManager
{
    private AbstractAdProvider currentAdProvider;

    public AdManager(AbstractAdProvider adProvider)
    {
        currentAdProvider = adProvider;
    }

    public void InitializeAds(IAdListener listener)
    {
        currentAdProvider.InitializeProvider(listener);
    }

    public void LoadAds()
    {
        currentAdProvider.PreloadAds();
    }

    public void DisplayAds()
    {
        if (currentAdProvider.CanShowAds())
        {
            currentAdProvider.ShowAds();
        }
    }

}
```
- Manages the overall ad operations.
- Likely contains logic to handle ad providers, manage their lifecycle, and orchestrate ad displaying.


### Adding New Ad Providers

- **Modularity**: Each ad provider is a separate class extending `AbstractAdProvider`.
- **Steps to Add New Provider**:
  1. Create a new class extending `AbstractAdProvider`.
  2. Implement all abstract methods (`InitializeProvider`, `PreloadAds`, `CanShowAds`, `ShowAds`).
  3. Integrate with `AdManager` for managing its lifecycle.

### Benefits of This Architecture

- **Loose Coupling**: Ad providers are loosely coupled with the system, making it easy to add/remove providers.
- **Scalability**: Easy to scale by adding new ad providers as separate classes.
- **Maintainability**: Changes in one provider do not affect others or the core system.

This architecture allows for straightforward integration of new ad providers, while keeping the system scalable and maintainable.

### 4. LiveManager (`Assets/Scripts/Managers/`)

- **Design:** Simple MonoBehaviour initialized by GameManager.
- **Purpose:** Triggers events and tracks full life state and timer UI.
- **Extension:** Can be refactored for managing various local timers and associated UIs in the future.


### 1. GameManager (`Assets/Scripts/Managers/GameManager.cs`)

- **Design:** Simple MonoBehaviour ensuring loose coupling.
- **Purpose:** Manages MainMenu visibility, saved games, and game states (e.g., GameOver, Level Complete) by listening to events.
- **Extension:** Currently no specific extension. Designed for easy additions without disrupting existing functionality.

### 2. GameEventManager (`Assets/Scripts/Managers/GameEventManager.cs`)

- **Design:** Singleton Observer pattern.
- **Purpose:** Subject-agnostic game events provider, allowing subjects to trigger events.
- **Extension:** Encourages easy addition of new events and methods for subjects and observers through interfaces.


### 5. PlayerDataManager, LocalPlayerDataProvider, IPlayerDataProvider, NetworkPlayerDataProvider (`Assets/Scripts/PlayerData/`)

- **Design:** Singleton managing player profile and progress data.
- **Purpose:** Centralized access to player data.
- **Extension:** Dynamic polymorphism via IPlayerDataProvider for seamless integration of multiple data sources. Supports compile-time switching between sources.

### 6. Mole (`Assets/Scripts/`)

- **Design:** Simple MonoBehaviour with potential for refactoring into components like IMoveable, IKillable, MoleVisuals.
- **Purpose:** Implements basic Mole movement and interaction.
- **Extension:** Modular design for easy composition of various Mole types. Enables extension and customization of Mole behavior.

### 8. AudioManager (`Assets/Scripts/Managers/`)

- **Design:** Event-driven AudioManager.
- **Purpose:** Plays audio based on game events.


### 11. UI components

#### - GameEndUI

- **Design:** Event-driven game end popup.

#### - LevelUI

- **Purpose:** Main menu level details UI.



**Happy Whacking!**
