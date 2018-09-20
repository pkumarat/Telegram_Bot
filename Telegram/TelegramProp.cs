using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram
{
    public class TelegramProp
    {

        public TelegramProp(string token)
        {
            this.authToken = token;
        }

        public String authToken { get; set; }

    }
}
