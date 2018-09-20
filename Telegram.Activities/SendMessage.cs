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

    public sealed class SendMessage : CodeActivity
    {

        // Define an activity input argument of type string
        [Category("Input")]
        [DisplayName("Chat ID")]
        [Description("Enter Chat_ID to which bot sends message")]
        public InArgument<Int64> ChatID { get; set; }

        // Define an activity input argument of type string
        [Category("Input")]
        [DisplayName("Text Message ")]
        [Description("Enter message description to be delivered by bot")]
        public InArgument<string> MessageText { get; set; }

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

            var messageText = MessageText.Get(context);

            if(messageText == null)
                throw new ArgumentException("MesageText missing");

            var chatID = ChatID.Get(context);

            var chatID_str = Convert.ToString(chatID);

            if (chatID_str == null)
                throw new ArgumentException("Chat-ID missing");

          
            var botClient = new TelegramBotClient(botToken);
            
            Message message = botClient.SendTextMessageAsync(chatID, messageText).GetAwaiter().GetResult();
        }

    }
}
