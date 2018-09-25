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

namespace Telegram
{
    [DisplayName("Send Message")]
    [Description("Sends message from Telegram Bot to user or group")]
    public sealed class SendMessage : CodeActivity
    {

        // Define an activity input argument of type string
        [Category("Input")]
        [DisplayName("Chat ID")]
        [RequiredArgument]
        [Description("Enter Chat_ID to which bot sends message")]
        public InArgument<Int64> Chat_ID { get; set; }

        // Define an activity input argument of type string
        [Category("Input")]
        [DisplayName("Text Message ")]
        [RequiredArgument]
        [Description("Enter message text to be delivered by bot")]
        public InArgument<string> Message_Text { get; set; }

        public SendMessage()
        {
            this.Constraints.Add(ActivityConstraints.HasParentType<SendMessage, TelegramAppScope>(string.Format("Activity is valid only inside {0}", (object)typeof(TelegramAppScope).Name)));
        }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.

        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            //string text = context.GetValue(this.Text);

            TelegramProp telegramDetails = (TelegramProp)context.DataContext.GetProperties()["telegramDetails"].GetValue(context.DataContext);

            var botToken = telegramDetails.authToken;

            var messageText = Message_Text.Get(context);

            if(messageText == null)
                throw new ArgumentException("Message text input is missing");

            var chatID = Chat_ID.Get(context);

            var chatID_str = Convert.ToString(chatID);

            if (chatID_str == null)
                throw new ArgumentException("Chat-ID input is missing");

          
            var botClient = new TelegramBotClient(botToken);

            try
            {
                Message message = botClient.SendTextMessageAsync(chatID, messageText).GetAwaiter().GetResult();
            }

            catch (Telegram.Bot.Exceptions.ChatNotFoundException)
            {
                throw new Exception("Input Chat_ID "+chatID_str+" does not exist");
            }
            catch (Telegram.Bot.Exceptions.ChatNotInitiatedException)
            {
                throw new Exception("Input Chat_ID "+chatID_str+ " has not yet initiated a chat with bot yet");
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException)
            {
                throw new Exception("Input Bot Token - Represents an API error");
            }
            catch (System.Exception ex)
            {
                throw new Exception("Telegram Send Message Failed, Exception:"+ex.Message);
            }
            
        }

    }
}
