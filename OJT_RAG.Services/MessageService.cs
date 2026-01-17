using OJT_RAG.DTOs.MessageDTO;
using OJT_RAG.ModelView.MessageModelView;
using OJT_RAG.Repositories.Entities;
using OJT_RAG.Repositories.Interfaces;
using OJT_RAG.Services.Interfaces;

namespace OJT_RAG.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repo;

        public MessageService(IMessageRepository repo)
        {
            _repo = repo;
        }

        private MessageModelView Map(Message x)
        {
            return new MessageModelView
            {
                MessageId = x.MessageId,
                ChatRoomId = x.ChatRoomId,
                Content = x.Content,
                FromAi = x.FromAi,
                Useful = x.Useful,
                Sources = x.Sources,
                CreatedAt = x.CreatedAt
            };
        }

        public async Task<IEnumerable<MessageModelView>> GetAll()
        {
            return (await _repo.GetAllAsync()).Select(Map);
        }

        public async Task<MessageModelView?> GetById(long id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : Map(x);
        }

        public async Task<IEnumerable<MessageModelView>> GetByChatRoom(long chatRoomId)
        {
            return (await _repo.GetByChatRoomIdAsync(chatRoomId)).Select(Map);
        }

        public async Task<bool> Create(CreateMessageDTO dto)
        {
            var entity = new Message
            {
                ChatRoomId = dto.ChatRoomId,
                Content = dto.Content,
                FromAi = dto.FromAi,
                Useful = dto.Useful,
                Sources = dto.Sources,
                CreatedAt = DateTime.UtcNow.ToLocalTime()
            };

            await _repo.AddAsync(entity);
            return true;
        }

        public async Task<bool> Update(UpdateMessageDTO dto)
        {
            var entity = await _repo.GetByIdAsync(dto.MessageId);
            if (entity == null) return false;

            entity.ChatRoomId = dto.ChatRoomId;
            entity.Content = dto.Content;
            entity.FromAi = dto.FromAi;
            entity.Useful = dto.Useful;
            entity.Sources = dto.Sources;

            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
