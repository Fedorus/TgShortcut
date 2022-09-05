using System.Collections.Generic;
using ShortcutsBotHost.Models;
using ShortcutsBotHost.MongoModels;

namespace ShortcutsBotHost.ShortcutBot
{
    public class ShortcutUser : MongoItem
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public List<Shortcut> Shortcuts { get; set; } 
    }
}