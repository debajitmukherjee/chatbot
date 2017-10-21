using AN.ChatBot.Common;
using AN.SC.BLL.Models.Inventory;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Linq;
using System;
using AutoNation.DSF.Datacontracts.Vehicle;
using Microsoft.Bot.Builder.Dialogs;
using AutoNation.DSF.Datacontracts.Store;
using AN.SC.Commons.Settings;

namespace AN.ChatBot.Helper
{
    public static class SearchHelper
    {

        public static int MaxMileage { get; set; }

        public static int PageSize { get; set; }

        public static int MaxPrice { get; set; }

        public static string SortBy { get; set; }

        public static string DefaultZipCode { get; set; }

        public static SimpleSearchFilter GetSearchFilter(LuisResult result, string postalCode)
        {

            //Check if value exists
            if (PageSize <= 0)
            {
                PageSize = AppSettingsUtility.GetInt("RecordsPerPage");
                MaxMileage = AppSettingsUtility.GetInt("MaxMileage");
                MaxPrice = AppSettingsUtility.GetInt("MaxPrice");
                SortBy = AppSettingsUtility.GetString("SortBy");
                DefaultZipCode = AppSettingsUtility.GetString("DefaultZipCode");
            }


            var filter = new SimpleSearchFilter
            {
                MinMileage = 0,
                MaxMileage = MaxMileage,
                PageNo = 1,
                PageSize = PageSize,
                MinPrice = 0,
                MaxPrice = MaxPrice,
                Radius = -1,
                SortBy = SortBy,
                SortDirection = 0,
                PostalCode = DefaultZipCode

            };

            if (!string.IsNullOrEmpty(postalCode))
            {
                filter.PostalCode = postalCode;
            }

            if (result != null && result.Entities != null && result.Entities.Count > 0)
            {
                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_CONDITION))
                {
                    filter.StockType = result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_CONDITION).Entity;
                }

                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_MAKE))
                {
                    filter.Make = result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_MAKE).Entity;
                }

                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_MODEL))
                {
                    filter.Model = result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_MODEL).Entity;
                }

                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_COLOR))
                {
                    filter.ExteriorColor = result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_COLOR).Entity;
                }

                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_YEAR))
                {
                    filter.Year = result.Entities.FirstOrDefault(e => e.Type == BotConstants.LUIS_ENTITY_YEAR).Entity;
                }

                var priceOptions = GetPriceOptions(result);

                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_PRICE_UNDER) && priceOptions.Count > 0)
                {
                    filter.MaxPrice = priceOptions.OrderByDescending(p => p).FirstOrDefault();
                }

                if (result.Entities.Any(e => e.Type == BotConstants.LUIS_ENTITY_PRICE_ABOVE) && priceOptions.Count > 0)
                {
                    filter.MinPrice = priceOptions.OrderBy(p => p).FirstOrDefault();
                }
            }

            return filter;
        }

        public static IList<Attachment> GetCardsAttachments(SimpleSearchFilter filter, bool isKnownUser)
        {
            var response = GetSimpleSearchResult(filter);
            var cards = new List<Attachment>();
            string carDetailsUrl = ConfigurationManager.AppSettings["CarDetailsURL"];
            var stores = response.Stores;

            foreach (var car in response.SearchVehicles)
            {
                string price = GetCarPrice(car, isKnownUser);
                string vin = BotConstants.LABEL_VIN + BotConstants.LABEL_SPACE + car.Vehicle.Vin;
                string condition = BotConstants.LABEL_CONDITION + BotConstants.LABEL_SPACE + car.Vehicle.StockType;
                string inventoryStatus = GetCarInventoryStatus(car);
                var cardAction = GetCardAction(car, isKnownUser);
                var distance = GetDistance(car, stores);

                cards.Add(CardHelper.GetHeroCard(
                       title: car.Name,
                       subtitle: vin + BotConstants.LABEL_NEW_LINE + condition + BotConstants.LABEL_NEW_LINE + inventoryStatus + BotConstants.LABEL_NEW_LINE + distance,
                       text: price,
                       cardImages: new List<CardImage> { new CardImage(url: BotConstants.IMAGE_PROTOCOL + car.ImageURL) },
                       cardActions: new List<CardAction> {
                           cardAction,
                           new CardAction(ActionTypes.OpenUrl,BotConstants.BUTTON_VIEW_CAR, value: carDetailsUrl + car.Vehicle.Vin)
                       }
                    ));
            }


            return cards;
        }

        public static bool IsKnownUser(IDialogContext context)
        {
            string firstName = string.Empty;
            context.UserData.TryGetValue(BotConstants.USER_DATA_FIRST_NAME, out firstName);

            return !string.IsNullOrEmpty(firstName);
        }

        private static SimpleSearchResponse GetSimpleSearchResult(SimpleSearchFilter filter)
        {
            var serializer = new JavaScriptSerializer();
            var url = ConfigurationManager.AppSettings["SearchURL"] + serializer.Serialize(filter);
            var webRequestHelper = new HttpRequestHelper(url);
            var responseponse = webRequestHelper.GetResponse();

            return serializer.Deserialize<SimpleSearchResponse>(responseponse);
        }

        private static string GetCarPrice(SimpleSearchVehicle car, bool isKnownUser)
        {
            string price = string.Empty;
            if (car.Vehicle.PricingStack != null && car.Vehicle.PricingStack.Items.Count > 0)
            {
                PaymentStackItemType pricingItem;
                if (isKnownUser)
                {
                    pricingItem = car.Vehicle.PricingStack.Items.FirstOrDefault(pi => pi.ItemDisplayMode == BotConstants.ITEM_DISPLAY_MODE_UNLOCKED || pi.ItemDisplayMode == BotConstants.ITEM_DISPLAY_MODE_BOTH);
                }
                else
                {
                    pricingItem = car.Vehicle.PricingStack.Items.FirstOrDefault(pi => pi.ItemDisplayMode == BotConstants.ITEM_DISPLAY_MODE_LOCKED || pi.ItemDisplayMode == BotConstants.ITEM_DISPLAY_MODE_BOTH);
                }

                price = PopulatePrice(pricingItem);
            }

            return price;
        }

        private static string PopulatePrice(PaymentStackItemType pricingItem)
        {
            string price = string.Empty;
            int priceValue = 0;

            if (AnConstants.PricingStackItems.Any(p => p.Key == pricingItem.Name.ToLowerInvariant()))
            {
                var pricingStackItem = AnConstants.PricingStackItems.FirstOrDefault(p => p.Key == pricingItem.Name.ToLowerInvariant());
                price = pricingStackItem.Value;
                var pricingItemValue = pricingItem.Value;
                priceValue = (int)Convert.ToDouble(pricingItemValue);
            }
            else
            {
                price = pricingItem.Name;
            }

            if (priceValue > 0)
            {
                price += BotConstants.LABEL_SPACE + BotConstants.LABEL_DOLLAR + string.Format(BotConstants.CURRENCY_FORMAT, priceValue);
            }


            return price;
        }

        private static string GetCarInventoryStatus(SimpleSearchVehicle car)
        {
            string inventoryStatus = string.Empty;

            if (AnConstants.InventoryStatusTypes.Any(i => i.Key == car.Vehicle.InventoryStatus))
            {
                var anInventoryStatus = AnConstants.InventoryStatusTypes.FirstOrDefault(i => i.Key == car.Vehicle.InventoryStatus);
                inventoryStatus = BotConstants.LABEL_INVENTORY_STATUS + BotConstants.LABEL_SPACE + anInventoryStatus.Value;
            }

            return inventoryStatus;
        }

        private static CardAction GetCardAction(SimpleSearchVehicle car, bool isKnownUser)
        {
            if (!isKnownUser && car.UnKnownUserView.ShowGetLowerPrice)
            {
                return new CardAction(ActionTypes.ImBack, BotConstants.BUTTON_GET_LOWER_PRICE, value: BotConstants.BUTTON_GET_LOWER_PRICE);
            }
            else
            {
                return new CardAction(ActionTypes.ImBack, BotConstants.BUTTON_CONTACT_STORE, value: BotConstants.BUTTON_CONTACT_STORE);
            }

        }

        private static string GetDistance(SimpleSearchVehicle car, List<StoreDistanceType> stores)
        {
            string distance = string.Empty;
            var store = stores.FirstOrDefault(s => s.HyperionId == car.Vehicle.HyperionId);
            if (store != null)
            {
                string milesFormat = string.Format(BotConstants.MILES_FORMAT, store.Distance);
                distance = milesFormat + BotConstants.LABEL_SPACE + BotConstants.LABEL_MILES_AWAY;
            }

            return distance;
        }

        private static List<int> GetPriceOptions(LuisResult result)
        {
            List<int> numbers = new List<int>();
            List<EntityRecommendation> luisNumbers = new List<EntityRecommendation>();
            if (result != null)
            {
                luisNumbers = result.Entities.Where(e => e.Type == BotConstants.LUIS_ENTITY_NUMBER).ToList();
            }

            if (luisNumbers.Count > 0)
            {
                foreach (var luisNumber in luisNumbers)
                {
                    int value = Convert.ToInt32(luisNumber.Entity.Replace(",", "").Replace(".", ""));
                    if (value >= 4000)
                    {
                        numbers.Add(value);
                    }
                }

            }

            return numbers;

        }


    }
}