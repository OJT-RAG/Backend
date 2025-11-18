namespace YourNamespace.Models.JobDescription
{
    public class UpdateJobDescriptionModel
    {
        public long JobDescriptionId { get; set; }
        public long? JobPositionId { get; set; }
        public string? Level { get; set; }
        public string? JobDescription { get; set; }
        public int? HireQuantity { get; set; }
        public int? AppliedQuantity { get; set; }
    }
}
