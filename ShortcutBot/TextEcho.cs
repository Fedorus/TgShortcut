using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TgBotFramework;

namespace ShortcutsBotHost.ShortcutBot;

public class TextEcho : IUpdateHandler<ShortcutBotContext>
{
    public async Task HandleAsync(ShortcutBotContext context, UpdateDelegate<ShortcutBotContext> next, CancellationToken cancellationToken)
    {
        if (context.Update.Type == UpdateType.Message)
            await context.Bot.Client.SendTextMessageAsync(context.Update.Message.Chat, context.Update.Message.Text, cancellationToken: cancellationToken);
    }
}