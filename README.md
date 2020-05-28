<div align="center">
  <img alt="Unity Scene Tools v0.1.0" src="https://imgur.com/Mfg6Dii.png">
</div>

---

## Overview
A collection of useful Unity scene tools to enhance productivity, packaged as a custom Editor window. Tested on Unity 2019.3.5f1.

## Features
[**v0.1.0**] As of the latest release, the following features are included. Note that some buttons accept modifier keys (hold the modifier key and press the button) for alternate actions. The layout can be oriented horizontally or vertically for your preferred docking style.


### New GameObject
![New GameObject Icon](https://imgur.com/1JFg1X2.png) 
Adds a new empty GameObject into the scene at world space zero.
* **(SHIFT)**  Inserts a new empty GameObject as a child of **each** of the selected object(s) in the hierarchy window.
* **(ALT)**  Inserts a new empty GameObject as a parent of **each** of the selected object(s) in the hierarchy window.
* **(CTRL)**  Inserts a new empty GameObject as a parent of **all** of the selected object(s) in the hierarchy window.

### Reset PSR
![New GameObject Icon](https://imgur.com/yMHrLR8.png) 
Resets the Transform component (local PSR) of the selected GameObject(s) to 0.
* **(SHIFT)**  Resets the local (p)osition of the selected GameObject(s) to 0.
* **(ALT)**  Resets the local (s)cale of the selected GameObject(s) to 0.
* **(CTRL)**  Resets the local (r)otation of the selected GameObject(s) to 0.

### Unparent
![New GameObject Icon](https://imgur.com/mDGIi5U.png) 
Removes the selected GameObject(s) from their nested hierarchy and into the scene root.

### Drop To Ground
![New GameObject Icon](https://imgur.com/V0YafEY.png) 
Places the selected 3D GameObject(s) onto the default ground plane (Y = 0). Takes into account the bounding box of the object's mesh and respects orientation + scale.

### Solo Objects
![New GameObject Icon](https://imgur.com/PcNGLk8.png) 
Hides and isolates the selected GameObject(s) from the scene and hierarchy windows.

### Unsolo All
![New GameObject Icon](https://imgur.com/TROLNyX.png) 
Brings back all hidden and isolated objects.

## Install
1. Download the latest version from the [releases](https://github.com/robertobrambila/unityscenetools/releases) page.
2. Unzip and copy the Assets folder into the root of your existing Unity project.

## Run
After installation, you can launch the dockable window via:

* Window > FutureSupervillain > Scene Tools

## Layout Orientation
After running, you can switch between horizontal and vertical layout orientations by:

* Right-clicking the "Scene Tools" window title and selecting "Toggle Layout Direction".
