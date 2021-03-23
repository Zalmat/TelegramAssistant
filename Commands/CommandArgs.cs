using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBotSigner.Commands
{
    public class CommandArgs
    {
        public long ChatId { get; set; }
        public long MessageId { get; set; }
        public int FromId { get; set; }
        public string FromName { get; set; }
        public string CommandText { get; set; }
    }
}
