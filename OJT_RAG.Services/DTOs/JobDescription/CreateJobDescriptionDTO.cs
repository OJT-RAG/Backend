namespace OJT_RAG.Services.DTOs.JobDescription
{
    public class CreateJobDescriptionDTO
    {
        public long? JobPositionId { get; set; }
        public string? JobDescription { get; set; }
        public int? HireQuantity { get; set; }
        public int? AppliedQuantity { get; set; }
    }
}
