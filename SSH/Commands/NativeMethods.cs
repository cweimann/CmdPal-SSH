using System.Runtime.InteropServices;

namespace SSH.Commands;

internal static partial class NativeMethods
{
	/// <summary>Special value for <see cref="AllowSetForegroundWindow"/>: allow any process.</summary>
	internal const int ASFW_ANY = unchecked((int)0xFFFFFFFF);

	/// <summary>
	/// Lets another process (here: the terminal we are about to launch) take the foreground.
	/// Command Palette grants our COM server foreground rights via CoAllowSetForegroundWindow,
	/// but those rights are not inherited by child processes (wt.exe -> WindowsTerminal.exe),
	/// so we explicitly pass them on before starting the terminal.
	/// </summary>
	[LibraryImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static partial bool AllowSetForegroundWindow(int dwProcessId);
}
