namespace OJT_RAG.DTOs.SemesterDTO
{
    public class SemesterUpdateDTO
    {
        public string? Name { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
