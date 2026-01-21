using System.ComponentModel.DataAnnotations;
using OJT_RAG.Repositories.Enums;

namespace OJT_RAG.Services.DTOs.User
{
    public class UpdateUserStatusDTO
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public AccountStatusEnum AccountStatus { get; set; }
    }
}
