namespace FuelService.Models
{

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class AcceptedPaymentType
        {
            public object IconPath { get; set; }
            public int SortOrder { get; set; }
            public bool IncludeInExcelExport { get; set; }
            public string Section { get; set; }
            public bool HideInSearch { get; set; }
            public object MyLovesRewardsTypeId { get; set; }
            public string SmaFieldName { get; set; }
            public int FacilityId { get; set; }
            public int FieldId { get; set; }
            public string FieldName { get; set; }
            public string FieldValue { get; set; }
            public object DataType { get; set; }
        }


}
