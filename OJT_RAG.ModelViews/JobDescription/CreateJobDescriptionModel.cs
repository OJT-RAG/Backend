namespace YourNamespace.Models.JobDescription
{
    public class CreateJobDescriptionModel
    {
        public long JobPositionId { get; set; }
        public string Level { get; set; }  // job_level_enum
        public string JobDescription { get; set; }
        public int HireQuantity { get; set; }
        public int AppliedQuantity { get; set; }
    }
}
