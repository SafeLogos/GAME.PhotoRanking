using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.Photo;
using GAME.PhotoRanking.Repositories.FilesRepository;
using GAME.PhotoRanking.Repositories.PhotoGroupsRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;

namespace GAME.PhotoRanking.Services.Domain.PhotoGroupsDomainService
{
    public class PhotoGroupsDomainService : IPhotoGroupsDomainService
    {
        private readonly IPhotoGroupsRepository _photoGroupsRepository;
        private readonly IFilesRepository _filesRepository;

        public PhotoGroupsDomainService(IPhotoGroupsRepository photoGroupsRepository, IFilesRepository filesRepository)
        {
            _photoGroupsRepository = photoGroupsRepository;
            _filesRepository = filesRepository;
        }

        public Task<Response<PhotoModel>> AddPhoto(IFormFile file, AddPhotoRequest request) =>
            Response<PhotoModel>.DoAsync(async resp =>
            {
                string fileId = (await _filesRepository.AddFile(file)).GetResult();
                resp.Data = (await _photoGroupsRepository.AddPhoto(request, fileId)).GetResult();
            });

        public Task<Response<PhotoModel>> AddPhoto(IFormFile file, HttpContext context) =>
            Response<PhotoModel>.DoAsync(async resp =>
            {
                string json = context.Request.Form["data"];
                AddPhotoRequest request = Newtonsoft.Json.JsonConvert.DeserializeObject<AddPhotoRequest>(json);

                resp.Data = (await AddPhoto(file, request)).GetResult();
            });

    }
}
