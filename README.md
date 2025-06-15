# STAG — Stage Tracking Analysis & Governance 🦌

**STAG** is a Rhino plugin for AEC professionals that brings structured stage management and geometry control directly into your modeling environment.

Easily track the lifecycle of elements — from **Design ➜ Production ➜ Fabrication** — using Rhino's built-in User Attributes without disrupting your workflow. STAG makes your geometry **smart, trackable, and protected** through customizable stages and event listeners that control movement, transformation , object attributes and User object attributes.

---

## 🚀 Features

- ✅ Assign customizable stages (e.g. Design, Fabrication, Issued)
- 🔒 Lock geometry movement & Transformation based on stage permissions
- 📦 Store stage metadata invisibly in Rhino User Attributes
- 🔁 Listen to elements modification events to allow or deny changes


---

## 🛠️ Installation Guide

1. **Download the latest STAG plugin (.rhp)** from [Releases](https://github.com/your-org/STAG/releases)
2. Open Rhino (version 7 or 8)
3. Drag & drop the `.rhp` file into Rhino, or use the `PluginManager`:
   - Run `_PluginManager`
   - Click `Install`
   - Select the `.rhp` file
4. Type `STAG` in the command line to open the panel

> ℹ️ No admin rights required. All user data is saved directly in the Rhino model.

---

## 🧪 Quick Start Tutorial

### 1. Open STAG Panel
The panel will be automatically loaded you will eb able to access it therou the panel tool bar.

### 2. Create Stages
Click `Add Stage` (e.g., "Design", "Production", "Fabrication")  
Each stage can have constraints: **allow move**, **allow transform**, **allow modifying attributes** & **allow modifying user object attributes**.

### 3. Assign Stage to Geometry
Select one or more objects in Rhino, then add the stage deatils which will be assinged to all those selected elements.

### 4. Movement and attributes Restriction
Try moving , transforming or even modifiying the object atirubtes or user object attirbures the object — if the stage restricts it, STAG will block the action resulting into a roll Back in Rhino !! Yup we did that :).

### 5. Change Stage
Need to progress the model? Just reassign a new stage via the panel either upgrade or downgrade th stage.

---

## 👨‍💻 For Developers

- Plugin built with **.NET (C#)** and RhinoCommon
- Listens to geometry and document events for deep model control
- Designed to be **modular and open-source-friendly**

Want to contribute? ontact me through [ahmedsalahelsonpaty@gmail.com]

---

## 🏆 AEC + Tech Barcelona 2025

This project was ceated in the AEC + Tech  barcelona 2025 Hackathon((https://www.aectech.us/aectech-barcelona)) 
We believe in open tools for a better, more connected AEC industry.

If you like this idea — **star the repo ⭐ and share feedback!**

---

## 📬 Contact

Developed by:

Ahmed El-Sonpaty (https://github.com/ahmedsonpaty)
Sylvain Usai (https://github.com/usai-sylvain))
Ammar Salama (https://github.com/amabedsalama)
Eesha Jain (https://github.com/eesha-on-jupiter)
Sevan Mohammadpour (https://github.com/sewanmp)
Razi S (https://github.com/RaziS15)
Ziad Fayed (https://github.com/ZiadFayed)
---

## 📄 License

MIT License — free for commercial and personal use.
