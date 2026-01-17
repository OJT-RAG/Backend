using System.ComponentModel.DataAnnotations;

namespace OJT_RAG.Services.DTOs.JobDescription
{
    public class CreateJobDescriptionDTO
    {
        [Required(ErrorMessage = "jobPositionId là bắt buộc")]
        public long JobPositionId { get; set; }

        [Required(ErrorMessage = "jobDescription là bắt buộc")]
        public string JobDescription { get; set; } = null!;

        [Required(ErrorMessage = "hireQuantity là bắt buộc")]
        public int HireQuantity { get; set; }

        [Required(ErrorMessage = "appliedQuantity là bắt buộc")]
        public int AppliedQuantity { get; set; }
    }
}
