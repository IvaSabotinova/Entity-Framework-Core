using Footballers.Data.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Footballers.DataProcessor.ExportDto
{
    public class TeamWithFootballersOutputModel
    {
        public string Name { get; set; }

        public FootballerOfTeamOutputModel[] Footballers { get; set; }
    }

    public class FootballerOfTeamOutputModel
    {
        public string FootballerName { get; set; }

        [JsonProperty("ContractStartDate")]
        public string ContractStartDate { get; set; }

        [JsonProperty("ContractEndDate")]

        public string ContractEndDate { get; set; }

        [JsonProperty("BestSkillType")]
        public string BestSkillType { get; set; }

        [JsonProperty("PositionType")]
        public string PositionType { get; set; }
        
    }
}
