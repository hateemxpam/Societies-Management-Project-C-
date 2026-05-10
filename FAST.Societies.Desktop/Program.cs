using FAST.Societies.Desktop.Forms.Authentication;

namespace FAST.Societies.Desktop;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm());
    }
}
