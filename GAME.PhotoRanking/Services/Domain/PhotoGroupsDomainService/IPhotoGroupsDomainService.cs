using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.Photo;

namespace GAME.PhotoRanking.Services.Domain.PhotoGroupsDomainService
{
    public interface IPhotoGroupsDomainService
    {
        Task<Response<PhotoModel>> AddPhoto(IFormFile file, AddPhotoRequest request);
        Task<Response<PhotoModel>> AddPhoto(IFormFile file, HttpContext context);
    }
}
