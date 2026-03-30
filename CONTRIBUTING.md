# Contributing to FirstAidVR

Thank you for your interest in contributing! This document outlines how to get involved.

---

## Ways to Contribute

- **Bug reports** — Found something broken? Open an issue with steps to reproduce
- **Feature suggestions** — Have an idea for a new training scenario or mechanic?
- **Code improvements** — Performance, code quality, or new Unity scripts
- **Documentation** — Improve setup guides, add comments to scripts

---

## Getting Started

1. **Fork** the repository
2. **Clone** your fork:
   ```bash
   git clone https://github.com/YOUR_USERNAME/FirstAidVR.git
   ```
3. **Create a branch** with a descriptive name:
   ```bash
   git checkout -b feature/add-aed-scenario
   # or
   git checkout -b fix/compression-depth-calculation
   ```
4. Make your changes
5. **Commit** with a clear message (see convention below)
6. **Push** and open a **Pull Request**

---

## Commit Message Convention

Use [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: add AED defibrillator training scenario
fix: correct CPR compression depth threshold
docs: update build instructions for Unity 2023
chore: clean up unused assets in Resources folder
refactor: simplify HandsDepthController logic
```

---

## Code Style (Unity C#)

- Use **PascalCase** for class names and public members
- Use **camelCase** for private fields (prefix with `_` is optional but consistent)
- Add **XML summary comments** to public methods
- Keep scripts **single-responsibility** — one script per concern
- Avoid `FindObjectOfType` in `Update()` — cache references in `Awake()` or `Start()`

---

## Reporting Bugs

When opening an issue, include:

- Unity version
- Oculus Quest firmware version
- Steps to reproduce the bug
- Expected vs actual behavior
- Any relevant `adb logcat` or Unity console errors

---

## Pull Request Checklist

- [ ] Code compiles without errors in Unity 2022.3+
- [ ] No Debug.Log statements left in production code
- [ ] Scripts have summary comments on public methods
- [ ] No large binary files added (use Git LFS or external storage)
- [ ] README or docs updated if behavior changed
