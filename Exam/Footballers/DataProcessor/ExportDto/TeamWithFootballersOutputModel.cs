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
                
        public string ContractStartDate { get; set; }
        
        public string ContractEndDate { get; set; }
             
        public string BestSkillType { get; set; }

        public string PositionType { get; set; }
        
    }
}
