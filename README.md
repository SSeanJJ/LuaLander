
# Lua Lander

Lua Lander is a 2D Game where the player controls a spaceship and has to navigate an alien world.
The goal is to pick up coins and make sure the ship has enough fuel to land properly to ensure its score multiplier!
Depending on the angle and how fast the ship comes in to land will affect the multiplier and scoring rewards.


## Built With
- **Unity (2D,URP)**
- **C#**

## Project Background
The foundation Project was developed following a Unity Tutorial in Unity 6.5.
I followed the project carefully to get a playabale base and learn Unity Tools. 
I then treated the project as a codebase to then extend and debug rather than keeping it as a finished product!

## What I Added
**Fixed thruster audio bug.** The thruster is a looping ``AudioSource`` that stopped each frame by the landers ``OnBeforeforce`` Event.
Pause would set ``Time.timeScale = 0f ``, the lander would freeze and that event would stop firing. This would cause the loop to be played indefinitely over the pause menu.
Fixed by subscribing ``LanderAudio`` to ``GameManager.onGamePaused`` and pausing the source directly. No unpause handler was needed. 
The existing logic restored the correct state on the first frame after resume.

**Fixed levels loading frozen after retry.** ``Time.timeScale = 0f`` is global and survives scene loads, 
so retrying from pause menu would load the next scene with time still staying at ``0f``.
This would show a fully frozen level with no visible cause in the error list. Moved the global reset into ``SceneLoader.LoadScene()`` rather than the calling button, so every scene load unfreezes time regardless of entry point.

**Added retry button to pause menu.** created an additional game object (retryButton) to the existing ``PausedUI`` script creating a retry button wried to ``GameManager.RetryLevel()``

**Added additional retry button to sucessful landing UI.** Previously a single button swapped its label and action using textmesh from "CONTINUE" and "RETRY" depending on the outcome of the level.
Added a dedicated Retry button to show only when landing is sucessful, to allow the player to achieve a higher score for the level if they wanted to do so. 

## Architecture Notes
This project is event driven. Managers announce state changes rather than calling depdents directly:

```csharp
// GameManager announces, without knowing who listens
OnGamePaused?.Invoke(this, EventArgs.Empty);
 
// LanderAudio reacts, without GameManager knowing it exists
GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
```

Every fix was made without modifying the ``GameManager`` Script.
<br><br>
Managers Consist of (`GameManager`, `Soundmanager`, `MusicManager`).

## Running it 
1. Clone Repository.
2. Open the project folder in Unity Hub.
3. Open `MainMenuScene` and Press Play. 

## Controls Key Board 
| Input | Action |
| --- | --- |
| W / Up | Main thruster | 
| A / Left | Rotate left |
| D / Right | Rotate right |
| Esc | Pause |

## Controls Controller
| Input | Action |
| --- | --- |
| A / Up | Main thruster | 
| Left Thumbstick / Left | Rotate left |
| Left ThumbStick / Right | Rotate right |
| Start Button | Pause |


## Controls Joystick
| Input | Action |
| --- | --- |
| Left Thumbstick / Up | Main thruster | 
| Left Thumbstick / Left | Rotate left |
| Left ThumbStick / Right | Rotate right |
| Start Button | Pause |

## RoadMap
- Level Select Screen
- Additional Levels
- Online Leaderboard Backed by an ASP.NET core API
- Additional Polishing on Levels and Enviroment
<img width="1557" height="846" alt="140" src="https://github.com/user-attachments/assets/cad00f2e-3fc3-4958-8527-fa931e1d3635" />
