using AN.ChatBot.Common;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Threading.Tasks;

namespace AN.ChatBot.Models
{
    public enum ActivityType
    {
        SHOP,
        SELL,
        SERVICE
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
            //Save user data
            context.UserData.SetValue(BotConstants.USER_DATA_ACTIVITY_TYPE, state.PrefferedActivity.ToString());
            context.UserData.SetValue(BotConstants.USER_DATA_POST_CODE, state.ZipCode);

            return null;
        }

        [Template(TemplateUsage.EnumSelectOne, BotConstants.DIALOG_GREET_HELP)]
        public ActivityType? PrefferedActivity;

        [Prompt(BotConstants.PROMPT_ZIP_CODE)]
        [Pattern(BotConstants.REGX_ZIP_CODE)]
        public string ZipCode;
    }
}