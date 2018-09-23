﻿using System;
using System.IO;
using System.Activities;
using System.ComponentModel;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using ServiceNow;
using System.Linq;

namespace Telegram
{
    [DisplayName("Send Image")]
    [Description("Sends Image from Telegram Bot to user or group")]
    public sealed class SendPhoto : CodeActivity
    {
        // Define an activity input argument of type string
        [Category("Input")]
        [DisplayName("Chat ID")]
        [RequiredArgument]
        [Description("Enter Chat_ID to which bot sends the image")]
        public InArgument<Int64> Chat_ID { get; set; }

        // Define an activity input argument of type string
        [Category("Input")]
        [DisplayName("Image Path ")]
        [RequiredArgument]
        [Description("Enter the Path where the Photo resides")]
        public InArgument<string> Image_Path { get; set; }

        // Define an activity input argument of type string
        [Category("Input")]
        [DisplayName("Image Text ")]
        [Description("Enter text to describe the Image")]
        public InArgument<string> Image_Text { get; set; }

        public SendPhoto()
        {
            this.Constraints.Add(ActivityConstraints.HasParentType<SendPhoto, TelegramAppScope>(string.Format("Activity is valid only inside {0}", (object)typeof(TelegramAppScope).Name)));
        }
        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.

        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            //string text = context.GetValue(this.Text);

            TelegramProp telegramDetails = (TelegramProp)context.DataContext.GetProperties()["telegramDetails"].GetValue(context.DataContext);

            var botToken = telegramDetails.authToken;

            var photopath = Image_Path.Get(context);

            if (photopath == null)
                throw new ArgumentException("Photo Path missing");

            var chatID = Chat_ID.Get(context);

            var chatID_str = Convert.ToString(chatID);

            if (chatID_str == null)
                throw new ArgumentException("Chat-ID missing");

            var image_text = Image_Text.Get(context);

            if (image_text == null)
                image_text = "";

            var botClient = new TelegramBotClient(botToken);
            
            string file = photopath;

            var fileName = file.Split(Path.DirectorySeparatorChar).Last();

            //using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            //    Message Photo = botClient.SendPhotoAsync(chatID, fileStream, image_text).GetAwaiter().GetResult();
            //}
            try
            {
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Message Photo = botClient.SendPhotoAsync(chatID, fileStream, image_text).GetAwaiter().GetResult();
                }
            }
            catch (Telegram.Bot.Exceptions.ChatNotFoundException)
            {
                throw new Exception("Input Chat_ID " + chatID_str + "does not exist");
            }
            catch (Telegram.Bot.Exceptions.ChatNotInitiatedException)
            {
                throw new Exception("Input Chat_ID " + chatID_str + " has not yet initiated a chat with bot yet");
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException)
            {
                throw new Exception("Input Bot Token - Represents an API error");
            }
            catch (System.Exception ex)
            {

                throw new Exception("Telegram Send Image Failed, Exception:" + ex.Message);
            }

        }
    }
}
