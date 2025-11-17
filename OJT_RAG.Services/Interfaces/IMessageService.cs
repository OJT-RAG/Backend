using OJT_RAG.DTOs.MessageDTO;
using OJT_RAG.ModelView.MessageModelView;

namespace OJT_RAG.Services.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageModelView>> GetAll();
        Task<MessageModelView?> GetById(long id);
        Task<IEnumerable<MessageModelView>> GetByChatRoom(long chatRoomId);
        Task<bool> Create(CreateMessageDTO dto);
        Task<bool> Update(UpdateMessageDTO dto);
        Task<bool> Delete(long id);
    }
}
