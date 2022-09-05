using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShortcutsBotHost.MongoModels;
using TgBotFramework;
using TgBotFramework.WrapperExtensions;

namespace ShortcutsBotHost.ShortcutBot;

public class GetOrCreateUser : IUpdateHandler<ShortcutBotContext>
{
    private MongoCrud<ShortcutUser> Users { get; set; }
    public GetOrCreateUser(MongoCrud<ShortcutUser> users)
    {
        Users = users;
    }

    public async Task HandleAsync(ShortcutBotContext context, UpdateDelegate<ShortcutBotContext> next, CancellationToken cancellationToken)
    {
        var user = context.Update.GetSender();

        if (user != null)
        {
            var dbUser = await Users.GetAsync(x => x.UserId == user.Id);
                
            if (dbUser == null )
            {
                dbUser = await Users.InsertAsync(new ShortcutUser {UserId = user.Id, Shortcuts = new List<Shortcut>()});
            }
                
            context.User = dbUser;
            await next(context, cancellationToken);

            await Users.ReplaceOneAsync(dbUser);
        }
    }
}