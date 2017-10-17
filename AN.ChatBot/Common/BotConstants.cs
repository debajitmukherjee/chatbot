
namespace AN.ChatBot.Common
{
    public static class BotConstants
    {
        public const string INTENT_NONE = "";

        public const string INTENT_GREET_WELCOME = "Greet.Welcome";

        public const string INTENT_GREET_FAREWELL = "Greet.Farewell";

        public const string INTENT_SEARCH_CAR = "Search.Car";

        public const string INTENT_GET_LOWER_PRICE = "Get.LowerPrice";

        public const string LUIS_APP_ID = "ab95bd01-4b89-4b22-a975-40951655ee90";

        public const string LUIS_APP_KEY = "aa4b670e0a9d48b1ab4d286afcc3f254";

        public const string DIALOG_NONE = "Not sure I understand what you are saying, but I am learning more every day.";

        public const string DIALOG_GREET_WELCOME = "Hello! This is Auton, the AutoNation's assistant bot for buying and selling cars.";

        public const string DIALOG_GREET_HELP = "How can I help? {||}";

        public const string DIALOG_GREET_FAREWELL = "Ok. See you next time! Goodbye!";

        public const string BUTTON_BUY = "BUY";

        public const string BUTTON_SELL = "SELL";

        public const string BUTTON_GET_LOWER_PRICE = "GET LOWER PRICE";

        public const string BUTTON_CONTACT_STORE = "CONTACT STORE";

        public const string BUTTON_VIEW_CAR = "VIEW CAR DETAILS";

        public const string MESSAGE_LEAD_FORM = "Please provide your personal details.";

        public const string MESSAGE_ACTIVITY_TYPE = "Thank you, let me find some cars near you.\r\r You can search cars by make, model, year, color, vehicle condition and price";

        public const string MESSAGE_LEAD_SUCCESS = "Thank you for submitting the information. You have unlocked the AutoNation price for all vehicles. Our representative will contact you soon.";

        public const string PROMPT_FIRST_NAME = "What is your first name?";

        public const string PROMPT_LAST_NAME = "What is your last name?";

        public const string PROMPT_EMAIL = "What is your email?";

        public const string PROMPT_PHONE_NO = "What is your phone no?";

        public const string PROMPT_CONTACT_OPTION_1 = "What is the best way to get in touch with you? {||}";

        public const string PROMPT_CONTACT_OPTION_2 = "How would you like us to contact you? {||}";

        public const string PROMPT_ZIP_CODE = "What is your zip code?";

        public const string CONFIRM_LEAD_FORM = "Is this information correct?\n{*}";

        public const string REGX_EMAIL = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public const string REGX_ZIP_CODE = @"^\d{5}(?:[-\s]\d{4})?$";

        public const string LABEL_VIN = "VIN:";

        public const string LABEL_MSRP = "MSRP:";

        public const string LABEL_MSRP_WITH_ACCESSORIES = "MSRP WITH ACCESSORIES:";

        public const string LABEL_AUTONATION_PRICE = "AUTONATION PRICE:";

        public const string LABEL_CALL_FOR_PRICE = "CALL FOR PRICE";

        public const string LABEL_CONDITION = "CONDITION:";

        public const string LABEL_INVENTORY_STATUS = "INVENTORY STATUS:";

        public const string LABEL_MILES_AWAY = "Miles Away";

        public const string LABEL_SPACE = " ";

        public const string LABEL_DOLLAR = "$";

        public const string LABEL_NEW_LINE = "\r\r";

        public const string IMAGE_PROTOCOL = "https:";

        public const string USER_DATA_POST_CODE = "PostCode";

        public const string USER_DATA_MAKE = "Make";

        public const string USER_DATA_MODEL = "Model";

        public const string USER_DATA_COLOR = "Color";

        public const string USER_DATA_ACTIVITY_TYPE = "ActivityType";

        public const string USER_DATA_FIRST_NAME = "FirstName";

        public const string USER_DATA_LAST_NAME = "LastName";

        public const string USER_DATA_EMAIL = "Email";

        public const string USER_DATA_PHONE = "Phone";

        public const string USER_DATA_PREFERED_CONTACT_METHOD = "ContactMethod";

        public const string LUIS_ENTITY_MAKE = "Car.Make";

        public const string LUIS_ENTITY_MODEL = "Car.Model";

        public const string LUIS_ENTITY_COLOR = "Car.Color";

        public const string LUIS_ENTITY_YEAR = "Car.Year";

        public const string LUIS_ENTITY_CONDITION = "Car.Condition";

        public const string LUIS_ENTITY_PRICE_ABOVE = "Car.Price.Above";

        public const string LUIS_ENTITY_PRICE_UNDER = "Car.Price.Under";

        public const string LUIS_ENTITY_NUMBER = "builtin.number";

        public const string ITEM_DISPLAY_MODE_LOCKED = "Locked";

        public const string ITEM_DISPLAY_MODE_UNLOCKED = "Unlocked";

        public const string ITEM_DISPLAY_MODE_BOTH = "Both";

        public const string CURRENCY_FORMAT = "{0:n0}";

        public const string MILES_FORMAT = "{0:0.##}";





    }
}