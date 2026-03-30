# FirstAidVR — VR First Aid Training System

![Unity](https://img.shields.io/badge/Unity-2022.3%2B-000000?style=flat&logo=unity&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-Oculus%20Quest%202-1C1C1C?style=flat&logo=oculus&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green?style=flat)
![Status](https://img.shields.io/badge/Status-Active-brightgreen?style=flat)

A **Unity-based Virtual Reality First Aid Training System** designed for immersive CPR practice using Oculus Quest 2. Built as part of a master's thesis on immersive training and interactive learning.

---

## Table of Contents

- [Features](#features)
- [Demo](#demo)
- [Requirements](#requirements)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Build for Oculus Quest 2](#build-for-oculus-quest-2)
- [Scripts Overview](#scripts-overview)
- [Known Issues](#known-issues)
- [Contributing](#contributing)
- [License](#license)

---

## Features

- **Hands-on CPR Practice** — Physically simulate chest compressions with realistic force feedback using Oculus Touch controllers and hand tracking
- **Real-time Feedback & Scoring** — Get immediate performance evaluation on compression depth, rhythm, and hand placement
- **Oculus Quest 2 Compatible** — Fully optimized for standalone VR without a PC, leveraging Meta XR SDK
- **Classroom Training Scenario** — Immersive school classroom environment built with JP School Classroom V2 assets
- **Interactive Guidance System** — Step-by-step voice instructions and visual cues guide the learner through the procedure
- **Integrated Video Tutorials** — In-world video player for demonstration playback before practice begins
- **Softbody Physics** — Realistic chest softbody simulation using Obi physics for authentic tactile response
- **Smooth Animations** — DOTween-powered UI animations for a polished experience

---

## Demo

> Screenshots and demo video coming soon.

---

## Requirements

| Requirement | Version |
|---|---|
| Unity Editor | 2022.3 LTS or newer |
| Meta XR SDK | Included in project |
| Oculus Integration | Included in project |
| Target Device | Oculus Quest 2 |
| Android Build Support | Required (via Unity Hub) |
| Burst Compiler | Installed via Package Manager |

---

## Getting Started

### 1. Download the Full Project

The GitHub repository contains scripts, configurations, and documentation only. Large binary assets (3D models, textures, audio) are stored externally.

> **Download link:** *(Update with your actual MEGA or Google Drive link)*

### 2. Clone This Repository

```bash
git clone https://github.com/Rakanun/FirstAidVR.git
cd FirstAidVR
```

### 3. Open in Unity

1. Open **Unity Hub**
2. Click **Open** → **Add project from disk**
3. Select the `FirstAidVR` folder
4. Make sure Unity **2022.3** or newer is installed
5. Wait for Unity to import all packages (first launch may take several minutes)

### 4. Install Required Packages

Open `Window → Package Manager` and verify these are installed:
- **Meta XR SDK** (via Packages/manifest.json — auto-installed)
- **Burst Compiler** — Install manually if missing
- **XR Plugin Management** — Required for Oculus XR provider

### 5. Configure XR Settings

1. Go to `Edit → Project Settings → XR Plug-in Management`
2. Under **Android** tab, enable **Oculus**
3. Go to `Edit → Project Settings → Player → Android`
4. Set **Minimum API Level** to Android 10.0 (API 29)
5. Set **Target Architecture** to ARM64

---

## Project Structure

```
FirstAidVR/
├── Assets/
│   ├── Scripts/              # Core C# gameplay scripts
│   ├── Scenes/               # Unity scene files
│   ├── Animation/            # Animation clips and controllers
│   ├── Character/            # Patient character models and rigs
│   ├── MetaXR/               # Meta XR SDK assets
│   ├── Oculus/               # Oculus Integration SDK
│   ├── InteractionSDK/       # Hand interaction components
│   ├── DOTween/              # Animation library
│   ├── Obi/                  # Softbody physics (chest simulation)
│   ├── AllSkyFree/           # Skybox assets
│   ├── JP_School classroom_V2/ # Classroom environment
│   ├── HandyHandPack/        # VR hand models
│   ├── Resources/            # Runtime-loaded assets
│   ├── Samples/              # SDK sample scenes
│   ├── StreamingAssets/      # External video/audio assets
│   ├── AudioManager.cs       # Global audio management
│   ├── HandsDepthController.cs # VR hand depth tracking logic
│   └── PositionHighlight.shader # Hand position visual feedback shader
├── CPR_EXE/                  # Pre-built executable outputs
├── Packages/                 # Unity Package Manager manifest
├── ProjectSettings/          # Unity project configuration
├── .gitignore
├── .vsconfig
├── LICENSE
└── README.md
```

---

## Build for Oculus Quest 2

### Prerequisites

- **Android Build Support** module installed in Unity Hub
- **Android SDK & NDK Tools** (installed alongside Android Build Support)
- **Oculus Developer Mode** enabled on your Quest 2

### Build Steps

1. Connect Oculus Quest 2 via USB and allow ADB access on the headset
2. Open Unity → `File → Build Settings`
3. Set platform to **Android** (click **Switch Platform** if needed)
4. Under **Texture Compression**, select **ASTC**
5. Enable **Development Build** if you want logs (disable for production)
6. Click **Build and Run** to deploy directly to the headset

### APK Sideloading (Alternative)

```bash
# Install ADB tools, then:
adb install -r YourBuild.apk
```

Or use [SideQuest](https://sidequestvr.com/) for easier sideloading.

---

## Scripts Overview

| Script | Location | Description |
|---|---|---|
| `AudioManager.cs` | `Assets/` | Singleton audio manager. Handles BGM, SFX, and voice guidance playback |
| `HandsDepthController.cs` | `Assets/` | Controls VR hand depth perception during CPR compressions. Tracks force, position, and feedback |
| `PositionHighlight.shader` | `Assets/` | Custom Unity shader that highlights correct hand placement on the patient's chest |

---

## Known Issues

- Large `.asset` files (`SoftbodyBlueprint.asset` ~7.9MB, `RightHandBlueprint.asset` ~968KB) are tracked in Git — consider using **Git LFS** for future contributors
- Chinese-named audio files in `Assets/` may cause path issues on some operating systems — renaming recommended
- `git-filter-repo-2.47.0/` directory is a leftover cleanup tool and not part of the project

---

## Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you'd like to change.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -m 'feat: add your feature'`)
4. Push to the branch (`git push origin feature/your-feature`)
5. Open a Pull Request

---

## License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

## Acknowledgements

This project is part of a **master's thesis** focused on immersive training systems and interactive learning using Virtual Reality.

**Third-party assets used:**
- [Meta XR SDK](https://developer.oculus.com/) — Oculus VR framework
- [DOTween](http://dotween.demigiant.com/) — Animation library for Unity
- [Obi Physics](https://obi.virtualmethodstudio.com/) — Softbody physics simulation
- AllSkyFree — Skybox environment
- JP School Classroom V2 — Classroom 3D environment
