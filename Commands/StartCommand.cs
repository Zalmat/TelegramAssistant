using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBotSigner.Commands
{
    public class StartCommand : ICommand
    {
        TelegramBotClient Bot;

        public string Name => "/start";

        public StartCommand(TelegramBotClient bot)
        {
            Bot = bot;
        }

        public async Task ExecuteAsync(CommandArgs args)
        {
            await Bot.SendTextMessageAsync(args.ChatId, $"Приветствую в боте для подписи.{Environment.NewLine}Для информации используй /help");
        }
    }
}
