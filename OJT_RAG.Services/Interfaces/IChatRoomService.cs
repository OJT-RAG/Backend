using OJT_RAG.DTOs.ChatRoomDTO;
using OJT_RAG.ModelView.ChatRoomModelView;

namespace OJT_RAG.Services.Interfaces
{
    public interface IChatRoomService
    {
        Task<IEnumerable<ChatRoomModelView>> GetAll();
        Task<ChatRoomModelView?> GetById(long id);
        Task<IEnumerable<ChatRoomModelView>> GetByUser(long userId);
        Task<bool> Create(CreateChatRoomDTO dto);
        Task<bool> Update(UpdateChatRoomDTO dto);
        Task<bool> Delete(long id);
    }
}
