using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.PhotoGroupModel;
using GAME.PhotoRanking.Models.Photo;

namespace GAME.PhotoRanking.Repositories.PhotoGroupsRepository
{
    public interface IPhotoGroupsRepository : 
        ICRUDRepository<PhotoGroupModel, AddPhotoGroupRequest, UpdatePhotoGroupRequest>
    {
        Task<Response<PhotoModel>> AddPhoto(AddPhotoRequest request, string imageId);
        Task<Response<bool>> DeletePhoto(string groupId, string photoId);

    }
}
