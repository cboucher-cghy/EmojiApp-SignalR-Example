﻿using EmojiApp.Server.DataTransferObject;
using Microsoft.AspNetCore.SignalR;

namespace EmojiApp.Server.Hubs
{
    public class EmojiHub : Hub
    {
        private static readonly Dictionary<string, Emoji> EmojiLikes = new Dictionary<string, Emoji>
        {
            { "😊", new Emoji { Name = "😊", Likes = 0 } },
            { "😂", new Emoji { Name = "😂", Likes = 0 } },
            { "❤️", new Emoji { Name = "❤️", Likes = 0 } },
            { "👍", new Emoji { Name = "👍", Likes = 0 } }
        };

        public override async Task OnConnectedAsync()
        {
            // Send the current likes to the client when they connect
            await Clients.Caller.SendAsync("ReceiveInitialLikes", EmojiLikes.Values);
            await base.OnConnectedAsync();
        }

        // Ajout via SignalR
        public async Task AddNewEmoji(string name)
        {
            var emoji = new Emoji() { Name = name };
            var isAdded = EmojiLikes.TryAdd(name, emoji);
            if (isAdded)
            {
                // Notifies all connected clients of the new emoji
                await Clients.All.SendAsync("NewEmoji", emoji);
            }
        }

        public async Task LikeEmoji(string emojiName)
        {
            // if (EmojiLikes.TryGetValue(emojiName, out Emoji? value))
            if (EmojiLikes.ContainsKey(emojiName))
            {
                var emoji = EmojiLikes[emojiName];
                emoji.Likes++;
                await Clients.All.SendAsync("UpdateEmoji", emoji);
            }
        }
    }

}
