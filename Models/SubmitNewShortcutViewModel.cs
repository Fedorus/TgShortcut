using ShortcutsBotHost.ShortcutBot;

namespace ShortcutsBotHost.Models
{
    public class SubmitNewShortcutViewModel
    {
        public string Token { get; set; }
        public Shortcut Shortcut { get; set; }
    }
}