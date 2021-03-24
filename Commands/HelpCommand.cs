using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBotSigner.Commands
{
    public class HelpCommand : ICommand
    {
        TelegramBotClient Bot;

        public string Name => "/help";     

        public HelpCommand(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public async Task ExecuteAsync(CommandArgs args)
        {
            await Bot.SendTextMessageAsync(args.ChatId, $"Справка:{Environment.NewLine}/sign {{текст}} - генерирует подпись из отправленого текста");
        }
    }
}
