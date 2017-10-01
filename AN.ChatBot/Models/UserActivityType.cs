using AN.ChatBot.Common;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Threading.Tasks;

namespace AN.ChatBot.Models
{
    public enum ActivityType
    {
        BUY,
        SELL
    }

    [Serializable]
    public class UserActivityType
    {
        public static IForm<UserActivityType> BuildForm()
        {
            return new FormBuilder<UserActivityType>()
                    .Field(nameof(PrefferedActivity))
                    .Field(nameof(ZipCode))
                    .OnCompletion(ActivityTypeSubmitted)
                    .Message(BotConstants.MESSAGE_ACTIVITY_TYPE)
                    .Build();
        }

        private static Task ActivityTypeSubmitted(IDialogContext context, UserActivityType state)
        {
            throw new NotImplementedException();
        }

        [Template(TemplateUsage.EnumSelectOne, BotConstants.DIALOG_GREET_HELP)]
        public ActivityType? PrefferedActivity;

        [Prompt(BotConstants.PROMPT_ZIP_CODE)]
        [Pattern(BotConstants.REGX_ZIP_CODE)]
        public string ZipCode;
    }
}