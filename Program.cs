﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Telegram.Bot;
using cl_sstut;
using NLog;


namespace bot_test_zalmat
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Константа токена бота.
        const string TOKEN = "###";
        static void Main(string[] args)
        {
            //Вызываем метод авторизации.
            GetMessages().Wait();
/*
            logger.Trace("trace message");
            logger.Debug("debug message");
            logger.Info("info message");
            logger.Warn("warn message");
            logger.Error("error message");
            logger.Fatal("fatal message");
*/          
        }
        static async Task GetMessages()
        {
            TelegramBotClient bot = new TelegramBotClient(TOKEN);
            int offset = 0;
            int timeout = 0;            
            DateTime now = DateTime.Now;            
            try
            {
                //Отключение вебхука
                await bot.SetWebhookAsync("");
                bool a = false;
                string sign;
                bool udiNafig = false;

                while (!udiNafig)
                {
                    //Получаем обнолвения и вытаскиваем из них сообщения.
                    var updates = await bot.GetUpdatesAsync(offset, timeout);
                    foreach (var update in updates)
                    {
                        var message = update.Message;
                        if (message.Text == "Ты кто?")
                        {
                            //Этим можно логировать, что писали.
                            //Console.WriteLine("Получено сообщение: " + message.Text);
                            await bot.SendTextMessageAsync(message.Chat.Id, "Я твой бот");
                            logger.Trace("{0:G}","Пользователь: \"" +  message.Chat.Username + "\" спрашивал кто я О_о ");
                        }

                        else if (message.Text == "Подпись")
                        {
                            await bot.SendTextMessageAsync(message.Chat.Id, "Введи строку из которой будем вытаскивать подпись");
                            a = true;
                        }
                        else if (a == true)
                        {
                            sign = HashText.CkassaMD5(message.Text);
                            await bot.SendTextMessageAsync(message.Chat.Id, sign);
                            //Console.WriteLine("{0:G}", now + " Читал подпись для " + message.Chat.Username);
                            logger.Trace("{0:G}","Посчитана подпись для пользователя: " + message.Chat.Username);
                            a = false;
                            sign = "";
                        }
                        else
                        {
                            await bot.SendTextMessageAsync(message.Chat.Id, "Уважаемый, " + message.Chat.Username + @" я знаю команды: " + Environment.NewLine + "\t\tТы кто?" + Environment.NewLine + "\t\tПодпись");
                            //Console.WriteLine("{0:G}", now + " Что-то писал " + message.Chat.Username);
                            //logger.Debug("{0:G}", "Тест дебаг вывода" + now + " Что-то писал " + message.Chat.Username);
                            logger.Trace("{0:G}", "Общается пользователь: " + message.Chat.Username);
                        }
                            offset = update.Id + 1;
                        
                    }
                    
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Взаимодействие с API ТГ приостановлено. Ошибка: " + ex);
                //Console.WriteLine("Взаимодействие с API ТГ приостановлено. Ошибка: " + ex);
            }
        }
    }
}