using ShortcutsBotHost.ShortcutBot;
using TgBotFramework;

namespace ShortcutsBotHost;

public class ShortcutBotContext : UpdateContext
{
    public ShortcutUser User { get; set; }
}