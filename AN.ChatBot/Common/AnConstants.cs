

using System.Collections.Generic;

namespace AN.ChatBot.Common
{
    public static class AnConstants
    {
        public static List<Models.KeyValuePair> InventoryStatusTypes
        {
            get
            {
                return new List<Models.KeyValuePair>
                        {
                            new Models.KeyValuePair { Key = "S", Value = "IN STOCK" },
                            new Models.KeyValuePair { Key = "T", Value = "IN TRANSIT" },
                            new Models.KeyValuePair { Key  = "R", Value = "RESERVED" },
                            new Models.KeyValuePair { Key = "G", Value = "SOLD" }
                        };
            }

        }


        public static List<Models.KeyValuePair> PricingStackItems
        {
            get
            {
                return new List<Models.KeyValuePair>
                        {
                            new Models.KeyValuePair { Key = "msrp", Value = BotConstants.LABEL_MSRP },
                            new Models.KeyValuePair { Key = "msrpwithaccessories", Value = BotConstants.LABEL_MSRP_WITH_ACCESSORIES },
                            new Models.KeyValuePair { Key  = "autonationprice", Value = BotConstants.LABEL_AUTONATION_PRICE },
                            new Models.KeyValuePair { Key = "callforprice", Value = BotConstants.LABEL_CALL_FOR_PRICE }
                        };
            }

        }

        public static List<string> Jokes
        {
            get
            {
                return new List<string>
                {
                    "Going to church doesn't make you a Christian any more than standing in a garage makes you a car.",
                    "Subway is definitely the healthiest fast food available because they make you get out of the car.",
                    "Two fish are sitting in a tank. One looks over at the other and says: \"Hey, do you know how to drive this thing ?\" ",
                    "Why do chicken coupes have 2 doors? \r\r  Because if they had 4 doors they’d be a chicken sedan",
                    "What's the difference between BMWs and Porcupines? \r\r Porcupines carry their pricks on the outside.",
                    "Isn't it weird how when a cop drives by you feel paranoid instead of protected.",
                    "Children in the back seats of cars cause accidents, but accidents in the back seats of cars cause children.",
                    "Get a new car for your spouse - it'll be a great trade!",
                    "What do women and police cars have in common? \r\rThey both make a lot of noise to let you know they are coming.",
                    "What's the difference between a tire and 365 used rubbers? \r\rOne is a Goodyear and the other is a great year.",
                    "Why are women like parking spaces? \r\r Because all the best ones are taken and the rest are handicapped.",
                    "Confucius say, man who runs behind car will get exhausted, but man who runs in front of car will get tired.",
                    "The Best way to get back on your feet is to miss a couple of car payments!",
                    "Before marriage, men would wander parking lots aimlessly because they had no one to point out the open spots.",
                    "Scratches and dents on the doors of your car are the side effects of bad driving.",
                    "To avoid a collision I ran into the other car.",
                    "Any car is a self-driving car if you don't give a shit.",
                    "Don't drink and drive because you might spill the drink."
                };
            }
        }
    }
}