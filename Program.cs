using System;
using System.Threading;
using Serilog;
using Telegram.Bot;

namespace TelegramBotSigner
{
    class Program
    {

        static ManualResetEvent resetEvent;

        static void Main(string[] args)
        {
            var logerFormat =  "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] {Message:lj}{NewLine}{Exception}";
            //настройка логгера
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: logerFormat)
                .WriteTo.File("./logs/log.log", outputTemplate: logerFormat, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //поток
            resetEvent = new System.Threading.ManualResetEvent(false);

            Console.CancelKeyPress += Console_CancelKeyPress;

            //инициализация клиента телеграм бота
            var token = Environment.GetEnvironmentVariable("BOTTOKEN");

            if (token == null)
            {
                Log.Error("Bot token is empty. Please check your Environment Variable 'BOTTOKEN'");
                return;
            }
           
            var client = new TelegramBotClient(token);
            var bot = new SignerBot(client, Log.Logger);
            bot.Run(resetEvent);
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Log.Warning("Program exit requested.");
            resetEvent.Set();
            e.Cancel = true;
        }
    }
}
