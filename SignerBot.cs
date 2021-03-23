using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBotSigner.Commands;

namespace TelegramBotSigner
{
    public class SignerBot
    {
        ILogger _logger;
        TelegramBotClient _bot;
        Dictionary<string, ICommand> _commandDict;

        public SignerBot(TelegramBotClient client, ILogger logger)
        {
            this._logger = logger;
            this._bot = client;
            _bot.OnMessage += async (s, e) => await OnMessage(e);
           
            InitCommands();
        }

        private void InitCommands()
        {
            _logger.Information("Registration commands");

            var commands = new ICommand[]
            {
                new StartCommand(_bot),
                new HelpCommand(_bot),
                new SignCommand(_bot, Log.Logger)
            };

            _commandDict = commands.ToDictionary(c => c.Name);            
        }

        private async Task OnMessage(MessageEventArgs e)
        {
            var message = e.Message;
            _logger.Information($"Message {message.MessageId} from {message.Chat.Username} received");

            string commandName = string.Empty, commandText = string.Empty;

            if (message.Text.StartsWith('/'))
            {
                var splitter = message.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                commandName = splitter.Length > 0 ? splitter[0] : string.Empty;
                commandText = splitter.Length > 1 ? string.Join(' ', splitter.Skip(1)) : string.Empty;
                commandName = commandName.Split('@')[0];
            }
            else
            {
                commandText = message.Text;
            }

            var args = new CommandArgs()
            {
                ChatId = message.Chat.Id,
                MessageId = message.MessageId,
                CommandText = commandText,
                FromId = message.From.Id,
                FromName = message.From.Username
            };

            //Обработка комманд с "/команда"
            if (_commandDict.TryGetValue(commandName, out var command))
            {
                _logger.Information($"Command {commandName} found.");
                await command.ExecuteAsync(args);
            }
            else //иначе просто текст
            {
                _logger.Information("Command not found, run sign executor");
                await _commandDict["/sign"].ExecuteAsync(args);
            }
            

           

                      
        }

        public void Run(ManualResetEvent resetEvent)
        {
            _logger.Information("Starting bot ...");
            _bot.StartReceiving();
            resetEvent.WaitOne();
        }
    }
}
