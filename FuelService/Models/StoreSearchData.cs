using System.Collections.Generic;

namespace FuelService.Models
{

        public class StoreSearchData
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public List<string> MapIconId { get; set; }
            public int MapIconZIndex { get; set; }
            public string SAMMappedValue { get; set; }
            public List<string> SearchResultTileIcon { get; set; }
            public string ODataFilter { get; set; }
            public bool ShowViewStoreDetailsButton { get; set; }
            public int DisplayFacilitySubTypeId { get; set; }
            public string CustomFields { get; set; }
            public int Version { get; set; }
        }


}
