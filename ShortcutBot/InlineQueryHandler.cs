using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using TgBotFramework;

namespace ShortcutsBotHost.ShortcutBot;

public class InlineQueryHandler : IUpdateHandler<ShortcutBotContext>
{
    public async Task HandleAsync(ShortcutBotContext context, UpdateDelegate<ShortcutBotContext> next, CancellationToken cancellationToken)
    {
        var user = context.User;
        if (user == null)
            throw new Exception("UserNotFound");

        var inlineQuery = context.Update.InlineQuery;
            
            
        if (user.Shortcuts.Count>0)
        {
            var list = user.Shortcuts.Select(pair => new InlineQueryResultArticle(pair.Header, pair.Header, new InputTextMessageContent(pair.Content){ ParseMode = ParseMode.Html})).Cast<InlineQueryResult>().ToList();

            try
            {
                await context.Bot.Client.AnswerInlineQueryAsync(inlineQuery.Id, list, cacheTime: 1, isPersonal: true, cancellationToken: cancellationToken);
            }
            catch (ApiRequestException e)
            {
                await context.Bot.Client.SendTextMessageAsync(inlineQuery.From.Id, "There was error with one of your inline responses:\n"+e.Message, cancellationToken: cancellationToken);
            }
        }

    }
}