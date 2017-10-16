

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
    }
}