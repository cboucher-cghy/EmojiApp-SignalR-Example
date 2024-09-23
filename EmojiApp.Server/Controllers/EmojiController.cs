using EmojiApp.Server.DataTransferObject;
using EmojiApp.Server.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EmojiApp.Server.Controllers
{
    [ApiController]
    public class EmojiController(IHubContext<EmojiHub> hubContext) : Controller
    {
        // curl -X POST https://localhost:5001/home/create/😊
        [HttpPost("create/{emojiName}")]
        public async Task<IActionResult> Create(string emojiName)
        {
            // Ajouter à la BD...

            // Notifier qu'un nouvel Emoji est disponible par SignalR
            //hubContext.Clients.All.SendAsync("NewEmoji", new Emoji { Name = emojiName });
            return Ok(new { Message = $"Emoji {emojiName} ajouté!" });
        }
    }
}
