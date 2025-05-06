namespace DGDiemRenLuyen.DTOs.Responses
{
    public class ParentCriterionDto
    {
        public Guid Id { get; set; }
        public string? CriteriaName { get; set; }
        public int? MaxScore { get; set; }
        public string? Note { get; set; }
        public List<ChildCriterionDto> ChildCriteria { get; set; }
    }

    public class ChildCriterionDto
    {
        public Guid Id { get; set; }
        public string? CriteriaName { get; set; }
        public int? MaxScore { get; set; }
        public string? Note { get; set; }
        public List<CriteriaDetailDto> CriteriaDetails { get; set; }
    }

    public class CriteriaDetailDto
    {
        public Guid Id { get; set; }
        public int? StudentScore { get; set; }
        public int? MoniterScore { get; set; }
        public int? TeacherScore { get; set; }
        public string Note { get; set; }
    }

}
