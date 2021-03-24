using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBotSigner.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// Command name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Command executor
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync(CommandArgs args);
    }
}
