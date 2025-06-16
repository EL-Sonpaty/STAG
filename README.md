# <span style="color:red">S</span>tage <span style="color:red">T</span>racking <span style="color:red">A</span>nalysis & <span style="color:red">G</span>overnance (STAG) 🦌

**STAG** is a Rhino plugin built for AEC professionals to bring structured stage management and geometry governance directly into the modeling environment.

Track the lifecycle of elements — from **Design ➜ Production ➜ Fabrication** — using Rhino's built-in User Attributes without disrupting your workflow. STAG makes your geometry **smart, trackable, and protected** through customizable stages and event listeners that control:

- Movement  
- Transformation  
- Object Attributes  
- User Object Attributes

**DISCLAIMER** : STAG was born and build over two days at the AEC Tech+ Hackathon. As such, it's not final product and is only provided here as a POC. 
Some command might not work entirely as expected and can lead to loss of data or freeze rhino. Save your model before using STAG.

---

## 🚀 Features

- ✅ Assign customizable stages (e.g. Design, Fabrication, Issued)
- 🔒 Lock geometry movement and transformation based on stage permissions
- 📦 Store stage metadata invisibly in Rhino User Attributes
- 🔁 Listen to object modification events to allow or deny changes (movement, transform, or attribute edits)

---

## 🛠️ Installation Guide

True to it's "Hackathon born" origin, STAG is only available as a VS solution for now. 
You can download and build the solution for yourself in 'Debug' mode. And run it with the "Rhino 8 - netcore" Configuration. 

## 🦺 Requirements 

- only support Windows.
- Requires a valid Rhino license (to run Rhino).
- Build on Rhino 8 SR20 (8.20.25157.13001, 2025-06-06). Not tested on previous Rhino versions.
  
---

## 🧪 Quick Start Tutorial

### 1. Open STAG Panel  
The panel loads automatically. You can also access it via the Rhino `Panels` toolbar.

### 2. Create Stages  
Click `Add New Stage` — define stages like **Design**, **Production**, or **Fabrication**.  
Each stage includes constraints such as:
- Allow Geometrical changes (Trim, Boolean Operations, etc).
- Allow Transformations (Move, Rotate, Scale)
- Allow attribute edits (Name, Color) --> In developement. 
- Allow user object attribute edits (User Text) --> In development.

### 3. Assign Stage to Geometry  
* Set default stage to all Rhino object without a stage already.
* Upgrade selected objects (assign the next stage).
* Downgrade selected objects (assign the previous stage).

### 4. Enforce Restrictions  
Try moving, transforming, or editing object/user attributes — if the current stage disallows it, STAG will **block the action and roll it back** in Rhino.  

--> Important note :
STAG is listening to every changes in the model which can sometime leads to long update loops (as in "endless"). The "restriction" feature isn't very mature yet and require to listen constantly to every rhino event. 
(note for self : 
- best workaround would probably be to disable the "Allow Geometrical changes" change for now as this is bound to the ReplaceObject event in Rhino which is basically called everytime : [#3](https://github.com/EL-Sonpaty/STAG/issues/3) 
- Investigate if the Allow attribute edits is triggered by Hide/Show [#4](https://github.com/EL-Sonpaty/STAG/issues/4) ). 
This being said, you can disable the listening with the command STAG_StopListening and enable it with STAG_StartListening.

### 5. Track progess
Need to update the model's progress? Simply assign a new stage — you can upgrade or downgrade at any time.

---

## 👨‍💻 For Developers

- Built using **.NET (C#)** and **RhinoCommon**
- Listens to geometry and document events for deep model control
- Designed to be **modular**, extensible, and open-source-friendly

Want to contribute? Reach out via [ahmedsalahelsonpaty@gmail.com](mailto:ahmedsalahelsonpaty@gmail.com)

---

## 🏆 AEC + Tech Barcelona 2025

This project was created during the [AEC + Tech Barcelona 2025 Hackathon](https://www.aectech.us/aectech-barcelona).  
We believe in building **open tools** for a more connected and collaborative AEC industry.

If you like this project — **star the repo ⭐ and share your feedback!**

---

## 📬 Contributors

- [Ahmed El-Sonpaty](https://github.com/EL-Sonpaty)  
- [Sylvain Usai](https://github.com/usai-sylvain)  
- [Eesha Jain](https://github.com/eesha-on-jupiter)  
- [Ammar Salama](https://github.com/amabedsalama)  
- [Sevan Mohammadpour](https://github.com/sewanmp)  
- [Razi S](https://github.com/RaziS15)  
- [Ziad Fayed](https://github.com/ZiadFayed)  

---

## 📄 License

MIT License — free for commercial and personal use.
