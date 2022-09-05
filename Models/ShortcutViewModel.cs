using System.Collections.Generic;
using ShortcutsBotHost.ShortcutBot;

namespace ShortcutsBotHost.Models
{
    public class ShortcutViewModel 
    {
        public string Token { get; set; }
        public List<Shortcut> Shortcuts { get; set; }
    }
}