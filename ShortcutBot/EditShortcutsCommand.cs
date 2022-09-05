using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TgBotFramework;

namespace ShortcutsBotHost.ShortcutBot;

public class EditShortcutsCommand : CommandBase<ShortcutBotContext>
{
    private readonly IOptions<BotSettings> _settings;

    public EditShortcutsCommand(IOptions<BotSettings> settings)
    {
        _settings = settings;
    }
    public override async Task HandleAsync(ShortcutBotContext context, UpdateDelegate<ShortcutBotContext> next, string[] args, CancellationToken cancellationToken)
    {
        var user = context.User;
        var message = context.Update.Message;

        string GenerateToken(ShortcutUser user) //https://stackoverflow.com/questions/14643735/how-to-generate-a-unique-token-which-expires-after-24-hours
        {
            byte[] _time     = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] _key      = BitConverter.GetBytes(user.UserId);
            byte[] _Id       = UTF8Encoding.UTF8.GetBytes(user.Id);
            byte[] data       = new byte[_time.Length + _key.Length + _Id.Length];

            Buffer.BlockCopy(_time, 0, data, 0, _time.Length);
            Buffer.BlockCopy(_key , 0, data, _time.Length, _key.Length);
            Buffer.BlockCopy(_Id, 0, data, _time.Length + _key.Length, _Id.Length);

            return Convert.ToBase64String(data.ToArray());
        }

        user.Token = GenerateToken(user).Replace("=", "");
        if (_settings.Value.WebhookDomain.Contains("localhost"))
        {    
            await context.Bot.Client.SendTextMessageAsync(user.UserId, $"Link to your page: {_settings.Value.WebhookDomain}/Items/{user.Token}/\nDon't share your link with others\\. Link changes each time bot sends it\\.", ParseMode.MarkdownV2);
        }
        else
            await context.Bot.Client.SendTextMessageAsync(user.UserId, $"[Link to your page]({_settings.Value.WebhookDomain}/Items/{user.Token}/)\nDon't share your link with others\\. Link changes each time bot sends it\\.", ParseMode.MarkdownV2);
    }
}