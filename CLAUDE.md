# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

Lua Lander is a 2D lunar-lander game built in **Unity 6000.5.9f1 (Unity 6, 2D URP)** with C#. It started from a Code Monkey tutorial and is being extended/debugged as a learning codebase. Packages of note: Input System, Cinemachine 3, 2D SpriteShape, Test Framework (installed, but no tests exist yet).

## Building and running

There is no CLI build, lint, or test pipeline. The project is opened, played, and built from the Unity Editor (open the repo folder in Unity Hub). Claude cannot run the game; changes to scripts must be verified by the user in Play mode. `Assembly-CSharp.csproj` / `LuaLander.slnx` are Unity-generated — don't edit them.

Scene build order (`ProjectSettings/EditorBuildSettings.asset`): MainMenuScene, GameScene, GameOverScene, LevelSelectScene. A new scene must be added to both Build Settings and the `SceneLoader.Scene` enum (enum names must match scene file names, since `SceneLoader` loads by `scene.ToString()`).

## Architecture

**Event-driven singletons.** `GameManager`, `Lander`, `GameInput`, `SoundManager`, `MusicManager`, and `CinemachineCameraZoom2D` each expose `static Instance`, set in `Awake()`. Other components subscribe to their C# `EventHandler` events in `Start()` (so `Instance` is guaranteed to exist). Publishers don't know their listeners — e.g. `Lander` raises `OnLanded`/`OnCoinPickup`/`OnStateChanged`, and `GameManager`, `SoundManager`, the UI scripts, and `LanderAudio`/`LanderVisuals` react. The author's convention is to add behavior by subscribing to existing events rather than modifying `GameManager`.

**One GameScene, many levels.** Levels are not separate scenes. `GameManager` holds a serialized `List<GameLevel>` (prefabs `Assets/Prefabs/Level_1`, `Level_2`, …) and on `Start()` instantiates the one whose `GameLevel.levelNumber` matches the static `levelNumber`. Each `GameLevel` prefab supplies the lander start position, camera start target, and zoomed-out ortho size. Adding a level = new `Level_N` prefab with a unique `levelNumber`, added to `GameManager.gameLevelList` in GameScene (and a button in `LevelSelectUI` if it should be selectable). `GoToNextLevel()` goes to GameOverScene when no level matches the next number.

**State across scene loads lives in `static` fields**, since every scene reload recreates all MonoBehaviours: `GameManager.levelNumber`/`totalScore` (reset via `GameManager.ResetStaticData()` from the main menu; set via `GameManager.setLevelNumber()` from level select), `SoundManager.soundVolume`, `MusicManager.musicVolume`/`musicTime` (music resumes where it left off).

**Lander flow.** `Lander.State`: `WaitingToStart` (gravity off until first input) → `Normal` (physics in `FixedUpdate`, fuel consumption) → `GameOver` (set on any collision). Landing is judged in `OnCollisionEnter2D`: non-`LandingPad` collider = crash; relative velocity > 4 = too hard; `Vector2.Dot(up, transform.up) < 0.9` = too steep; otherwise score from angle + speed × the pad's multiplier. Pickups are trigger colliders detected via `TryGetComponent<FuelPickup/CoinPickup>`.

**Pause / time scale.** Pause sets `Time.timeScale = 0` and raises `GameManager.OnGamePaused`/`OnGameUnpaused`. Because `timeScale` is global and survives scene loads, `SceneLoader.LoadScene()` always resets it to 1 — route all scene changes through `SceneLoader`, never `SceneManager` directly. Anything driven by `FixedUpdate` events (e.g. the looping thruster `AudioSource` in `LanderAudio`) stops updating while paused, so it must listen to `OnGamePaused` itself.

**Input.** `Assets/InputActions.inputactions` generates `Assets/InputActions.cs` (don't hand-edit the generated file). `GameInput` wraps it, exposing `IsUp/Left/RightActionPressed()`, `GetMovementInputVector2()` (gamepad, 0.2 deadzone applied in `Lander`), and `OnMenuButtonPressed`.

## Repo notes

- `Assets/CodeMonkeyFree/` is a third-party tutorial helper asset; its ScriptableObject changes on its own when the Editor runs.
- Every asset has a `.meta` file; when moving/deleting assets, move/delete the `.meta` with it (or do it in the Editor) to keep GUID references intact.
- `README.md` documents the author's changes ("What I Added") — keep it updated when adding features or fixes.
