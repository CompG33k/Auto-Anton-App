# Auto-Anton 🚀  
**Local Ollama Launcher + Web UI (Windows / .NET 8)**

Auto-Anton is a **Windows desktop launcher (.exe)** that safely starts **Ollama**, launches a **local web UI**, prevents duplicate instances, and opens or focuses the browser automatically.

It is designed to make **local LLM usage easy, reliable, and repeatable**.

---

## ✨ Features

- ✅ One-click **Start Auto-Anton**
- ✅ Automatically starts **Ollama** if not running
- ✅ Detects existing Ollama + UI instances
- ✅ Prevents multiple background processes
- ✅ Web UI opens or focuses existing browser tab
- ✅ User-selectable `ollama.exe` path (saved permanently)
- ✅ Glass overlay (prevents double-clicks)
- ✅ Process cleanup (kills stale Anton / Python / Ollama)
- ✅ .NET 8 LTS compliant
- ✅ Works as a standalone `.exe`

---

## 🧱 Architecture (High Level)
[ Auto-Anton.exe ]
|
├── Ensures Ollama is running (ollama serve)
├── Starts local Web UI (Flask / Python)
├── Opens or focuses browser (127.0.0.1:5000)
└── Manages process lifecycle & cleanup


---

## 📦 Requirements

### System
- Windows 10 / 11 (64-bit)
- Administrator access (recommended)

### Software
- **Ollama** (installed locally)  
  👉 https://ollama.com
- **Python 3.10+** (if running from source)
- **.NET 8 Desktop Runtime**  
  👉 https://dotnet.microsoft.com/download/dotnet/8.0

---

## 🚀 Installation (Recommended – EXE)

1. Download the latest release from **GitHub Releases**
2. Extract the folder
3. Run:



Auto-Anton.exe


> 🔔 On first run, you may be prompted to locate `ollama.exe`

---

## 🛠 First-Time Setup (Important)

### Ollama Path Selection
If Ollama is not found automatically:
- A file picker will appear
- Select `ollama.exe`

Common paths:


C:\Program Files\Ollama\ollama.exe
C:\Users<you>\AppData\Local\Programs\Ollama\ollama.exe


The path is saved permanently to:


%LOCALAPPDATA%\AutoAnton\config.json


---

## ▶ How To Use

1. Launch **Auto-Anton**
2. Click **▶ Start Auto-Anton**
3. Confirm startup
4. Browser opens to:



http://127.0.0.1:5000


---

## 🌐 Web UI Features

- ChatGPT-style interface
- Copy-to-clipboard buttons
- File upload support
- Error handling shown in UI
- Fully local (no cloud calls)

---

## 🧹 Cleanup & Safety

Auto-Anton automatically:
- Prevents multiple instances
- Kills stale processes
- Blocks repeated clicks while starting
- Uses a glass overlay during startup

Safe to close at any time.

---

## 🐛 Troubleshooting

### ❌ Ollama won’t start
- Ensure Ollama is installed
- Verify selected `ollama.exe` path
- Run Auto-Anton as **Administrator**

---

### ❌ Port 11434 blocked
Allow Ollama through firewall:

```powershell
New-NetFirewallRule -DisplayName "Ollama" -Direction Inbound -Protocol TCP -LocalPort 11434 -Action Allow

❌ Web UI doesn’t open

Ensure build_and_run_autonanton.ps1 exists

Allow PowerShell scripts:

Set-ExecutionPolicy Bypass -Scope CurrentUser

🧑‍💻 Development (Optional)
Build from Source
git clone <repo>
cd auto-open-claude
dotnet build

Run Web UI Only
python web_ui.py

📁 Project Structure
Auto-Anton/
│
├── Auto-Anton.exe
├── MainForm.cs
├── AppConfig.cs
├── web_ui.py
├── build_and_run_autonanton.ps1
├── README.md
└── /dist

🔒 Privacy

100% local

No telemetry

No cloud APIs

No API keys

📜 License

MIT License
Free to use, modify, and distribute.

🙌 Credits

Built with:

.NET 8 (WinForms)

Python + Flask

Ollama (local LLM runtime)
