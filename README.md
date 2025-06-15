# <span style="color:red">S</span>tage <span style="color:red">T</span>racking <span style="color:red">A</span>nalysis & <span style="color:red">G</span>overnance (STAG) 🦌

**STAG** is a Rhino plugin built for AEC professionals to bring structured stage management and geometry governance directly into the modeling environment.

Track the lifecycle of elements — from **Design ➜ Production ➜ Fabrication** — using Rhino's built-in User Attributes without disrupting your workflow. STAG makes your geometry **smart, trackable, and protected** through customizable stages and event listeners that control:

- Movement  
- Transformation  
- Object Attributes  
- User Object Attributes

---

## 🚀 Features

- ✅ Assign customizable stages (e.g. Design, Fabrication, Issued)
- 🔒 Lock geometry movement and transformation based on stage permissions
- 📦 Store stage metadata invisibly in Rhino User Attributes
- 🔁 Listen to object modification events to allow or deny changes (movement, transform, or attribute edits)

---

## 🛠️ Installation Guide

1. **Download the latest STAG plugin (.rhp)** from [Releases](https://github.com/your-org/STAG/releases)
2. Open Rhino (v7 or v8)
3. Drag & drop the `.rhp` file into Rhino, or use the `PluginManager`:
   - Run `_PluginManager`
   - Click `Install`
   - Select the `.rhp` file
4. Run the command `STAG` in Rhino to open the plugin panel

> ℹ️ No admin rights needed. All stage data is saved directly in the Rhino model.

---

## 🧪 Quick Start Tutorial

### 1. Open STAG Panel  
The panel loads automatically. You can also access it via the Rhino `Panels` toolbar.

### 2. Create Stages  
Click `Add New Stage` — define stages like **Design**, **Production**, or **Fabrication**.  
Each stage includes constraints such as:
- Allow move
- Allow transform
- Allow attribute edits
- Allow user object attribute edits

### 3. Assign Stage to Geometry  
Select one or more objects in Rhino and assign a stage from the panel. All selected elements will receive the same stage metadata.

### 4. Enforce Restrictions  
Try moving, transforming, or editing object/user attributes — if the current stage disallows it, STAG will **block the action and roll it back** in Rhino.  
> Yup, we actually implemented rollbacks! 🎯

### 5. Change Stage  
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
