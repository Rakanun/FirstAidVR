# Build Instructions — Oculus Quest 2

This document explains how to build and deploy **FirstAidVR** as an APK on Oculus Quest 2.

---

## Prerequisites

### Unity Setup

1. Install **Unity 2022.3 LTS** or newer via [Unity Hub](https://unity.com/download)
2. During installation, add the following modules:
   - **Android Build Support**
   - **Android SDK & NDK Tools**
   - **OpenJDK**

### Device Setup

1. Enable **Developer Mode** on your Oculus Quest 2:
   - Open the **Meta Quest** mobile app
   - Go to **Menu → Devices → select headset → Developer Mode → ON**
2. Connect your headset via USB-C cable
3. Put on the headset and **Allow USB Debugging** when prompted

---

## Building in Unity

### Step 1: Switch Platform

1. Open `File → Build Settings`
2. Select **Android** from the platform list
3. Click **Switch Platform** (wait for asset reimport)

### Step 2: Configure Player Settings

Go to `Edit → Project Settings → Player` → Android tab:

| Setting | Value |
|---|---|
| Company Name | Your name |
| Product Name | FirstAidVR |
| Package Name | `com.yourname.firstaidvr` |
| Minimum API Level | Android 10.0 (API 29) |
| Target API Level | Automatic |
| Scripting Backend | IL2CPP |
| Target Architecture | ARM64 only |
| Internet Access | Required |

### Step 3: Configure XR

1. Go to `Edit → Project Settings → XR Plug-in Management`
2. Click **Android** tab
3. Check **Oculus** as the XR provider

### Step 4: Texture Compression

In `Build Settings`:
- Set **Texture Compression** to **ASTC** (best quality for Quest 2)

### Step 5: Build

- **Build and Run** → deploys directly to connected Quest 2
- **Build** → saves `.apk` file locally for manual installation

---

## Sideloading via ADB

If you have the `.apk` and want to install manually:

```bash
# Check device is connected
adb devices

# Install the APK
adb install -r FirstAidVR.apk

# Launch the app
adb shell am start -n com.yourname.firstaidvr/com.unity3d.player.UnityPlayerActivity
```

---

## Sideloading via SideQuest

1. Download [SideQuest](https://sidequestvr.com/setup-howto)
2. Connect your Quest 2
3. Drag and drop the `.apk` into SideQuest
4. Find the app in **Unknown Sources** on your headset

---

## Troubleshooting

| Problem | Solution |
|---|---|
| Device not detected | Re-enable USB Debugging on headset, try different USB cable |
| Build fails (NDK error) | Reinstall Android Build Support modules via Unity Hub |
| App crashes on launch | Check `adb logcat` for Unity errors |
| Black screen in VR | Ensure Oculus XR provider is selected in XR Plugin Management |
| Poor performance | Enable **Adaptive Resolution** in Oculus settings, reduce shadow quality |

---

## Performance Tips

- Use **Fixed Foveated Rendering** (available in Oculus SDK) for better GPU performance
- Target **72 Hz** frame rate minimum; **90 Hz** for optimal comfort
- Keep draw calls under 200 for smooth Quest 2 performance
- Use **ASTC** texture compression consistently across all textures
