<!-- README.md -->

# Dingo Level Based Input System

A level-based (layered) input system for Unity (C#) that helps manage input priority and activation depending on the current game state (gameplay, UI, menus, pause, cutscenes, etc.).

## Features

- Split input into levels (layers/levels) with priorities.
- Enable and disable input levels when switching states (for example, Gameplay → UI).
- Easy to extend for different devices and control schemes.
- Example usage in `Sample`.

## Project layout

- `InputControllerModels/`  Controller and scheme models/descriptions.
- `Inputs/`  Definitions of specific inputs (actions/axes/buttons).
- `InputsHandle/`  Input processing and routing.
- `Sample/`  Integration example.

## Installation (Unity)

### Option A. Copy into your project
1. Download the repository.
2. Copy `InputControllerModels`, `Inputs`, `InputsHandle`, `Sample` into `Assets/` (or a subfolder such as `Assets/Plugins/DingoLevelBasedInputSystem/`).

### Option B. Git submodule
```bash
cd <YourUnityProject>/Assets
git submodule add https://github.com/DingoBite/DingoLevelBasedInputSystem.git ThirdParty/DingoLevelBasedInputSystem
