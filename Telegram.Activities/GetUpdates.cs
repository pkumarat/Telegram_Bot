using System;
using System.Activities;
using System.ComponentModel;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using ServiceNow;
using Telegram.Bot.Types.Enums;
using System.Collections.Generic;

namespace Telegram
{
    [DisplayName("Receive Message")]
    [Description("Receives all the messages sent to Telegram Bot")]
    public sealed class GetUpdates : CodeActivity
    {

        [Category("Output")]
        [DisplayName("Message List")]
        [Description("Output message list to hold Telegram messages")]
        public OutArgument<List<string>> Str_arr { get;  set; }

        public GetUpdates()
        {
            this.Constraints.Add(ActivityConstraints.HasParentType<GetUpdates, TelegramAppScope>(string.Format("Activity is valid only inside {0}", (object)typeof(TelegramAppScope).Name)));
        }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.

        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            //string text = context.GetValue(this.Text);

            TelegramProp telegramDetails = (TelegramProp)context.DataContext.GetProperties()["telegramDetails"].GetValue(context.DataContext);

            var botToken = telegramDetails.authToken;
            
            var botClient = new TelegramBotClient(botToken);

            
            Int32 offset = 0;
            Int32 offset_new = 0;
            List<string> Msg_arr = new List<string> ( );
            
            var updates = botClient.GetUpdatesAsync(0).GetAwaiter().GetResult();
 
            foreach (var update in updates)
            {
                if (update.Message.Type == MessageType.Text)
                { 
                    //Add messages from Telegram into Message List
                    Msg_arr.Add(update.Message.Text);
                }
                offset_new = update.Id + 1;
            }

            Str_arr.Set(context, Msg_arr);

            //var update_new = botClient.GetUpdatesAsync(offset = offset_new).GetAwaiter().GetResult();
            try
            {
                var update_new = botClient.GetUpdatesAsync(offset = offset_new).GetAwaiter().GetResult();
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException)
            {
                throw new Exception("Input Bot Token - Represents an API error");
            }
            catch (System.Exception ex)
            {

                throw new Exception("Telegram Send Message Failed, Exception:" + ex.Message);
            }

        }

       
    }
}

