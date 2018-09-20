using System;
using System.Activities;
using System.ComponentModel;
using System.Activities.Statements;
using System.Security;
using System.Net;

namespace Telegram
{
    [Designer(typeof(TelegramAppScopeDesigner))]
    [DisplayName("Telegram Connector Scope")]
    [Description("Drop Telegram related activities inside this scope.")]
    public class TelegramAppScope : NativeActivity
    {

        [Browsable(false)]
        public ActivityAction<TelegramProp> Body { get; set; }

        // Define an activity input argument of type string
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Bot Token")]
        [Description("Enter the Telegram bot Auth token")]
        public InArgument<SecureString> BotToken { get; set; }

        [Browsable(false)]
        public TelegramProp telegramDetails;

        public TelegramAppScope()
        {
            Body = new ActivityAction<TelegramProp>
            {
                Argument = new DelegateInArgument<TelegramProp>("telegramDetails"),
                Handler = new Sequence { DisplayName = "Execute Telegram activities" }
            };
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

        }

        protected override void Execute(NativeActivityContext context)
        {
            if (BotToken.Get(context) == null)
                throw new ArgumentException("BotToken");

            telegramDetails = new TelegramProp(new NetworkCredential(String.Empty, BotToken.Get(context)).Password);

            if (Body != null)
            {
                //scheduling the execution of the child activities
                // and passing the value of the delegate argument
                context.ScheduleAction<TelegramProp>(Body, telegramDetails, OnCompleted, OnFaulted);
            }
        }


        private void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            //TODO
        }

        private void OnCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            //TODO
        }
    }
}
