Auto-Anton 🚀

Local Ollama Launcher + Web UI (Windows / .NET 8)

Auto-Anton is a Windows desktop launcher (.exe) that safely starts Ollama, launches a local web UI, prevents duplicate instances, and opens or focuses the browser automatically.

It is designed to make local LLM usage easy, reliable, and repeatable.

✨ Features

✅ One-click Start Auto-Anton

✅ Automatically starts Ollama if not running

✅ Detects existing Ollama + UI instances

✅ Prevents multiple background processes

✅ Web UI opens or focuses existing browser tab

✅ User-selectable ollama.exe path (saved permanently)

✅ Glass overlay (prevents double-clicks)

✅ Process cleanup (kills stale Anton / Python / Ollama)

✅ .NET 8 LTS compliant

✅ Works as a standalone .exe

🧱 Architecture (High Level)
[ Auto-Anton.exe ]
        |
        ├── Ensures Ollama is running (ollama serve)
        ├── Starts local Web UI (Flask / Python)
        ├── Opens or focuses browser (127.0.0.1:5000)
        └── Manages process lifecycle & cleanup

📦 Requirements
System

Windows 10 / 11 (64-bit)

Administrator access (recommended)

Software

Ollama (installed locally)
👉 https://ollama.com

Python 3.10+ (if running from source)

.NET 8 Desktop Runtime (for .exe)
👉 https://dotnet.microsoft.com/en-us/download/dotnet/8.0

🚀 Installation (Recommended – EXE)

Download the latest release from GitHub Releases

Extract the folder

Run:

Auto-Anton.exe


🔔 On first run, you may be prompted to locate ollama.exe

That’s it.

🛠 First-Time Setup (Important)
Ollama Path Selection

If Ollama is not found automatically:

A file picker will appear

Select:

ollama.exe


Example paths:

C:\Program Files\Ollama\ollama.exe
C:\Users\<you>\AppData\Local\Programs\Ollama\ollama.exe


The path is saved permanently in:

%LOCALAPPDATA%\AutoAnton\config.json

▶ How To Use

Launch Auto-Anton

Click ▶ Start Auto-Anton

Confirm startup

Browser opens to:

http://127.0.0.1:5000

What Happens Automatically

Existing Anton / Ollama processes are cleaned

Ollama starts if needed

Web UI starts

Browser opens or focuses existing tab

🌐 Web UI Features

Chat interface (ChatGPT-style)

Copy-to-clipboard buttons

File upload support

Error handling surfaced in UI

No external API calls

Fully local

🧹 Cleanup & Safety

Auto-Anton automatically:

Prevents multiple instances

Kills stale processes

Blocks repeated clicks while starting

Uses a glass overlay during startup

You can safely close the app at any time.

🐛 Troubleshooting
❌ Ollama won’t start

Ensure Ollama is installed

Verify the selected ollama.exe path

Run Auto-Anton as Administrator

❌ Port 11434 blocked

Allow Ollama through firewall:

New-NetFirewallRule -DisplayName "Ollama" -Direction Inbound -Protocol TCP -LocalPort 11434 -Action Allow

❌ Web UI doesn’t open

Check that build_and_run_autonanton.ps1 exists

Ensure PowerShell execution policy allows scripts:

Set-ExecutionPolicy Bypass -Scope CurrentUser

🧑‍💻 Development (Optional)
Run from Source
git clone <repo>
cd auto-open-claude
dotnet build

Python UI
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

No cloud calls

No API keys

📜 License

MIT License
Use, modify, distribute freely.

🙌 Credits

Built with:

.NET 8 (WinForms)

Python + Flask

Ollama (local LLM runtime)

🗺 Roadmap (Optional)

⏳ Tray icon mode

⏳ Auto-start on Windows boot

⏳ Model selector UI

⏳ Status indicators (Ollama / UI / Browser)

⏳ MSI Installer
