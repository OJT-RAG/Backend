using OJT_RAG.DTOs.ChatRoomDTO;
using OJT_RAG.ModelView.ChatRoomModelView;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IChatRoomRepository _repo;

        public ChatRoomService(IChatRoomRepository repo)
        {
            _repo = repo;
        }

        private ChatRoomModelView Map(ChatRoom x)
        {
            return new ChatRoomModelView
            {
                ChatRoomId = x.ChatRoomId,
                UserId = x.UserId,
                ChatRoomTitle = x.ChatRoomTitle,
                Description = x.Description,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            };
        }

        public async Task<IEnumerable<ChatRoomModelView>> GetAll()
        {
            return (await _repo.GetAllAsync()).Select(Map);
        }

        public async Task<ChatRoomModelView?> GetById(long id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item == null ? null : Map(item);
        }

        public async Task<IEnumerable<ChatRoomModelView>> GetByUser(long userId)
        {
            return (await _repo.GetByUserIdAsync(userId)).Select(Map);
        }

        public async Task<bool> Create(CreateChatRoomDTO dto)
        {
            var entity = new ChatRoom
            {
                UserId = dto.UserId,
                ChatRoomTitle = dto.ChatRoomTitle,
                Description = dto.Description,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };

            await _repo.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(UpdateChatRoomDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.ChatRoomId);
            if (entity == null) return false;

            entity.UserId = dto.UserId;
            entity.ChatRoomTitle = dto.ChatRoomTitle;
            entity.Description = dto.Description;
            entity.UpdateAt = DateTime.UtcNow;

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
