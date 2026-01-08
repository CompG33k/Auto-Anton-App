using AutoAnto;
using System.Diagnostics;
using System.Security.Principal;

namespace Auto_Anton_App;

internal static class Program
{
    private static Mutex? _mutex;

    [STAThread]
    static void Main()
    {
        _mutex = new Mutex(true, "AutoAnton.Singleton", out bool created);

        if (!created)
        {
            MessageBox.Show(
                "Auto-Anton is already running.",
                "Auto-Anton",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            return;
        }

        if (!IsAdministrator())
        {
            RelaunchAsAdmin();
            return;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }

    static bool IsAdministrator()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    static void RelaunchAsAdmin()
    {
        var exe = Process.GetCurrentProcess().MainModule!.FileName!;
        Process.Start(new ProcessStartInfo(exe)
        {
            UseShellExecute = true,
            Verb = "runas"
        });
    }
}
