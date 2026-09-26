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
	private const int LaunchDelayMs = 250;

	public override string Name => Resources.open_in_terminal;

	public override ICommandResult Invoke()
	{
		// Launch the terminal slightly later on a background task. On Dismiss, Command
		// Palette hides its window and hands foreground back to the previously active
		// window; launching immediately races with that and the terminal loses focus.
		_ = Task.Run(async () =>
		{
			try
			{
				await Task.Delay(LaunchDelayMs).ConfigureAwait(false);
				_ = TerminalHelper.OpenTerminal(host, title, mode, type, suppressTitleChange);
			}
#pragma warning disable CA1031 // Never let an exception escape and crash the COM server.
			catch (Exception ex)
#pragma warning restore CA1031
			{
				ExtensionHost.LogMessage($"SSH: failed to open terminal for {host}: {ex}");
			}
		});
		return CommandResult.Dismiss();
	}
}
