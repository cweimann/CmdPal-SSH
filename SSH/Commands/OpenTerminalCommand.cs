using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using SSH.Properties;
using SSH.Terminal;

namespace SSH.Commands;

internal sealed partial class OpenTerminalCommand(
	string host,
	string title,
	WindowMode mode,
	TerminalType type,
	bool suppressTitleChange) : InvokableCommand
{
	public override string Name => Resources.open_in_terminal;

	public override ICommandResult Invoke()
	{
		// Pass on the foreground rights Command Palette just granted us (via
		// CoAllowSetForegroundWindow) immediately, while they are still valid, so the
		// terminal we launch can take foreground/keyboard focus.
		_ = NativeMethods.AllowSetForegroundWindow(NativeMethods.ASFW_ANY);
		_ = TerminalHelper.OpenTerminal(host, title, mode, type, suppressTitleChange);
		// KeepOpen: let the palette hide on its own when the terminal takes focus, avoiding
		// the Dismiss foreground hand-back racing with the new terminal window.
		return CommandResult.KeepOpen();
	}
}
