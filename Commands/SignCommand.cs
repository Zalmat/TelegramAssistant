using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotSigner.Services;

namespace TelegramBotSigner.Commands
{
    public class SignCommand : ICommand
    {
        TelegramBotClient Bot;
        ILogger Logger;
        Dictionary<int, SignState> signStateFlag;

        public string Name => "/sign";

        public SignCommand(TelegramBotClient bot, ILogger logger)
        {
            Bot = bot;
            Logger = logger;
            signStateFlag = new Dictionary<int, SignState>();
        }


        /*
         Сценарий обработки:             
             клиент -> /sign
             бот <- "введите текст для подписи"  [флаг = ожидаю текст]
             клиент -> {текст}
             бот <- "защифрованное сообщение"    [флаг = текст принял]

         
        */
        public async Task ExecuteAsync(CommandArgs args)
        {
            //обработка флагов состояний
            if (args.CommandText == string.Empty)
            {
                signStateFlag[args.FromId] = SignState.SendCommand;
                Logger.Information($"Received command /sign from user {args.FromName}");
            }
            else if (signStateFlag.TryGetValue(args.FromId, out var state))
            {
                if (state == SignState.WaitText)
                {
                    signStateFlag[args.FromId] = SignState.SendText;
                    Logger.Information($"Received text for sign from user {args.FromName}");
                }
            }
            else
            {
                await Bot.SendTextMessageAsync(args.ChatId, $"Неизвестная команда.{Environment.NewLine}Для справки используй /help");
                return;
            }

            
            switch(signStateFlag[args.FromId])
            {
                //прислали /sign
                case SignState.SendCommand:
                    await Bot.SendTextMessageAsync(args.ChatId, $"Введи строку для генерации подписи");
                    Logger.Information($"Send to {args.FromName} message to WaitText");
                    signStateFlag[args.FromId] = SignState.WaitText;
                    break;
                //прислали строку для подписи
                case SignState.SendText:
                    //await Bot.SendTextMessageAsync(args.ChatId, args.CommandText.GetHashCode().ToString());
                    await Bot.SendTextMessageAsync(args.ChatId, SignHash.CkassaMD5(args.CommandText.ToString()));
                    
                    Logger.Information($"Send to {args.FromName} sign text");
                    signStateFlag.Remove(args.FromId);
                    break;
            }
            
        }
    }

    public enum SignState
    {
        All = 0,
        SendCommand = 1,
        WaitText = 2,
        SendText = 3
    }
}
