using System;

namespace ShortcutsBotHost.ShortcutBot
{
    public class Shortcut
    {
        public Guid Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
    }
}