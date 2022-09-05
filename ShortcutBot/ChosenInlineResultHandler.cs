using System;
using System.Threading;
using System.Threading.Tasks;
using TgBotFramework;

namespace ShortcutsBotHost.ShortcutBot
{
    public class ChosenInlineResultHandler : IUpdateHandler<ShortcutBotContext>
    {
        public async Task HandleAsync(ShortcutBotContext context, UpdateDelegate<ShortcutBotContext> next, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[{DateTime.Now}] User {context.Update.ChosenInlineResult.From} chosen {context.Update.ChosenInlineResult.ResultId} {context.Update.ChosenInlineResult.Query}" );
        }
    }
}