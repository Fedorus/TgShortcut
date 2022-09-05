using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShortcutsBotHost.Models;
using ShortcutsBotHost.MongoModels;
using ShortcutsBotHost.ShortcutBot;

namespace ShortcutsBotHost.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly MongoCrud<ShortcutUser> _users;

        public ItemsController(ILogger<ItemsController> logger, MongoCrud<ShortcutUser> users)
        {
            _logger = logger;
            _users = users;
        }
        
        // GET
        [Route("Items/{token}/")]
        public async Task<IActionResult> Index(string token)
        {
            var result = await GetShortcutViewModelAsync(token);
            if (result == null)
            {
                return base.NotFound();
            }
            return View("Index", result);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [Route("Items/{token}/Create")]
        public async Task<IActionResult> Create(string token)
        {
            var user = await _users.GetAsync(x => x.Token == token);

            return View(new SubmitNewShortcutViewModel(){ Token = user.Token, Shortcut = new Shortcut()});
        }
        
        [HttpPost]
        [Route("Items/Submit")]
        public async Task<IActionResult> Submit(SubmitNewShortcutViewModel item)
        {
            var user = await _users.GetAsync(x => x.Token == item.Token);

            if (!string.IsNullOrWhiteSpace(item.Shortcut.Header) && !string.IsNullOrWhiteSpace(item.Shortcut.Content))
            {
                if (item.Shortcut.Id == Guid.Empty)
                {
                    user.Shortcuts.Add(new Shortcut()
                        {Id = Guid.NewGuid(), Header = item.Shortcut.Header, Content = item.Shortcut.Content});
                }
                else
                {
                    var shortcut = user.Shortcuts.First(x => x.Id == item.Shortcut.Id);
                    shortcut.Content = item.Shortcut.Content;
                    shortcut.Header = item.Shortcut.Header;
                }
                await _users.ReplaceOneAsync(user);
            }

            return Redirect("/Items/" + item.Token);
        }
        
        [Route("Items/{token}/Delete")]
        public async Task<IActionResult> Delete(string token, Guid item)
        {
            var user = await _users.GetAsync(x => x.Token == token);
            
            if (user.Shortcuts.Any(x=>x.Id == item))
            {
                user.Shortcuts.Remove(user.Shortcuts.First(x => x.Id == item));
                await _users.ReplaceOneAsync(user);
            }

            var result = await GetShortcutViewModelAsync(token);
            if (result == null)
            {
                return base.NotFound();
            }
            return View("Index", result);
        }
        [Route("Items/{token}/Edit")]
        public async Task<IActionResult> Edit(string token, Guid item)
        {
            var user = await _users.GetAsync(x => x.Token == token);
            var shortcut = user.Shortcuts.First(x => x.Id == item);
            return View("Create", new SubmitNewShortcutViewModel(){ Token = user.Token, Shortcut =shortcut});
        }

        private async Task<ShortcutViewModel> GetShortcutViewModelAsync(string token)
        {
            //await _users.InsertAsync(new ShortcutUser() {Token = token, Shortcuts = new List<Shortcut>()});
            var user = await _users.GetAsync(x => x.Token == token);
            if (user == null)
                return null;
            return new ShortcutViewModel() {Token = user.Token, Shortcuts = user.Shortcuts ?? new List<Shortcut>()};

        }
    }
}