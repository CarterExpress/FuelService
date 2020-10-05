using System.Collections.Generic;

namespace FuelService.Models
{

        public class FuelArray
        {
            public bool IsLoveStore { get; set; }
            public bool IsRestaurant { get; set; }
            public bool IsTireCare { get; set; }
            public bool IsStorage { get; set; }
            public bool IsHotel { get; set; }
            public bool IsCNG { get; set; }
            public bool IsTrillium { get; set; }
            public bool IsFoodTruck { get; set; }
            public bool IsSpeedCo { get; set; }
            public bool IsTravelStopWithSpeedCo { get; set; }
            public bool IsPrivate { get; set; }
            public List<object> BusinessHours { get; set; }
            public List<AcceptedPaymentType> AcceptedPaymentTypes { get; set; }

            public List<FuelPrice> FuelPrices { get; set; }

            public StoreSearchData StoreSearchData { get; set; }
            public List<StationCode> StationCodes { get; set; }
            public List<object> FoodConcepts { get; set; }

            public int FacilityId { get; set; }
            public int FacilitySubTypeId { get; set; }
            public int DisplayFacilitySubTypeId { get; set; }
            public int FacilityTypeId { get; set; }
            public int SiteId { get; set; }
            public string SiteName { get; set; }
            public string PreferredName { get; set; }
            public int Number { get; set; }
            public string FacilitySubtypeName { get; set; }
            public string Address { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
            public string Highway { get; set; }
            public string ExitNumber { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public int UTCOffset { get; set; }
            public bool UsesDaylightSavings { get; set; }
            public string MainPhone { get; set; }
            public string MainEmail { get; set; }
            public string Fax { get; set; }
            public object GeoLocation { get; set; }
            public Site Site { get; set; }
        }

}
