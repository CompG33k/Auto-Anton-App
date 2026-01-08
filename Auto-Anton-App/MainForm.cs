using Auto_Anton_App;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoAnto
{

    public class MainForm : Form
    {
        AppConfig config;
        string ConfigFile => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AutoAnton",
            "config.json"
        );

        string OllamaExe => config.OllamaPath;
        const string OllamaApi = "http://127.0.0.1:11434/api/version";
        const string UiUrl = "http://127.0.0.1:5000";

        Button btnStart;
        TextBox txtLog;

        Panel glass;
        Label glassLabel;

        public MainForm()
        {
            config = AppConfig.Load();
            // 🔹 REQUIRED so the window actually shows
            Text = "Auto-Anton Launcher";
            Width = 600;
            Height = 400;
            StartPosition = FormStartPosition.CenterScreen;

            // 🔹 Start button
            btnStart = new Button
            {
                Text = "▶ Start Auto-Anton",
                Height = 45,
                Dock = DockStyle.Top
            };
            btnStart.Click += btnStart_Click;
            this.Controls.Add(btnStart);

            // 🔹 Log output
            txtLog = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(txtLog);

            CreateGlassOverlay();
        }

        async void btnStart_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Start Auto-Anton?\n\nThis will stop any existing Auto-Anton instances.",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            btnStart.Enabled = false;
            ShowGlass(true);

            try
            {
                Log("🧹 Cleaning old processes...");
                KillExistingAnton();

                Log("🔍 Checking Ollama...");

                if (!EnsureOllamaPath())
                    return;

                if (!await OllamaRunning())
                {
                    Log("🚀 Starting Ollama...");
                    StartOllama();
                    await Task.Delay(5000);
                }
                else
                {
                    Log("✅ Ollama already running");
                }

                Log("🌐 Starting Web UI...");
                StartWebUi();

                await Task.Delay(2000);
                Log("🌍 Opening browser...");
                //Process.Start(new ProcessStartInfo(UiUrl) { UseShellExecute = true });
                OpenOrFocusBrowser();

                Log("✅ Auto-Anton running");
            }
            catch (Exception ex)
            {
                Log("❌ ERROR: " + ex.Message);
            }
            finally
            {
                ShowGlass(false);
                btnStart.Enabled = true;
            }
        }

        async Task<bool> OllamaRunning()
        {
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
                await client.GetAsync(OllamaApi);
                return true;
            }
            catch { return false; }
        }

        bool EnsureOllamaPath()
        {
            if (!string.IsNullOrWhiteSpace(config.OllamaPath) &&
                File.Exists(config.OllamaPath))
                return true;

            MessageBox.Show(
                "Ollama was not found.\nPlease locate ollama.exe",
                "Ollama Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );

            using var dlg = new OpenFileDialog
            {
                Title = "Locate ollama.exe",
                Filter = "ollama.exe|ollama.exe"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                config.OllamaPath = dlg.FileName;
                config.Save();
                Log("✅ Ollama path saved");
                return true;
            }

            Log("❌ Ollama path not selected");
            return false;
        }

        void StartOllama()
{
    if (string.IsNullOrWhiteSpace(OllamaExe))
    {
        Log("❌ Ollama path is empty");
        return;
    }

    if (!File.Exists(OllamaExe))
    {
        Log($"❌ Ollama not found at: {OllamaExe}");
        return;
    }

    try
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = OllamaExe,
            Arguments = "serve",
            CreateNoWindow = true,
            UseShellExecute = false
        });
    }
    catch (Exception ex)
    {
        Log($"❌ Failed to start Ollama: {ex.Message}");
    }
}

        void StartWebUi()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string ps1 = System.IO.Path.Combine(dir, "build_and_run_autonanton.ps1");

            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy Bypass -File \"{ps1}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            });
        }

        bool PromptForOllamaPath()
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Locate Ollama executable",
                Filter = "ollama.exe|ollama.exe"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                config.OllamaPath = dlg.FileName;
                config.Save();
                Log($"✅ Ollama path saved: {dlg.FileName}");
                return true;
            }

            return false;
        }

        void KillExistingAnton()
        {
            string[] targets = { "AutoAnton", "python", "ollama" };

            foreach (var p in Process.GetProcesses())
            {
                try
                {
                    if (targets.Any(t =>
                        p.ProcessName.Contains(t, StringComparison.OrdinalIgnoreCase)) &&
                        p.Id != Process.GetCurrentProcess().Id)
                    {
                        p.Kill(true);
                    }
                }
                catch { }
            }
        }

        void Log(string msg)
        {
            if (InvokeRequired)
                Invoke(() => txtLog.AppendText(msg + Environment.NewLine));
            else
                txtLog.AppendText(msg + Environment.NewLine);
        }

        // ======================
        // Glass overlay
        // ======================
        void CreateGlassOverlay()
        {
            glass = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.FromArgb(120, 0, 0, 0),
                Visible = false
            };

            glassLabel = new Label
            {
                Text = "Starting Auto-Anton...",
                ForeColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 14),
                AutoSize = true
            };

            glass.Controls.Add(glassLabel);
            Controls.Add(glass);
            glass.BringToFront();

            Resize += (_, _) =>
            {
                glassLabel.Left = (Width - glassLabel.Width) / 2;
                glassLabel.Top = (Height - glassLabel.Height) / 2;
            };
        }

        void ShowGlass(bool show)
        {
            if (InvokeRequired)
                Invoke(() => glass.Visible = show);
            else
                glass.Visible = show;
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // MainForm
            // 
            ClientSize = new Size(282, 253);
            Name = "MainForm";
            ResumeLayout(false);

        }
        void OpenOrFocusBrowser()
        {
         //   const string url = "http://127.0.0.1:5000";

            // Try focus existing Chrome tab
            foreach (var p in Process.GetProcessesByName("chrome"))
            {
                try
                {
                    if (p.MainWindowHandle != IntPtr.Zero)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = UiUrl,
                            UseShellExecute = true
                        });
                        return;
                    }
                }
                catch { }
            }

            // Fallback
            Process.Start(new ProcessStartInfo(UiUrl)
            {
                UseShellExecute = true
            });
        }

    }
}
