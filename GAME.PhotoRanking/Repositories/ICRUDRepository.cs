using GAME.PhotoRanking.Models;

namespace GAME.PhotoRanking.Repositories
{
    public interface ICRUDRepository<ResponseDTO, AddRequest, UpdateRequest>
    {
        Task<Response<ResponseDTO>> Add(AddRequest request);
        Task<Response<ResponseDTO>> Update(UpdateRequest request);
        Task<Response<bool>> Delete(string id);

        Task<Response<ResponseDTO>> Get(string id);
        Task<Response<List<ResponseDTO>>> GetAll();
    }
}
