using GAME.PhotoRanking.Models.PhotoGroup;
using GAME.PhotoRanking.Repositories.PhotoGroupsRepository;
using GAME.PhotoRanking.Services.Domain.PhotoGroupsDomainService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAME.PhotoRanking.Controllers
{
    [Route("api/photo-groups")]
    [ApiController]
    public class PhotoGroupsController :
        CRUDController<IPhotoGroupsRepository, PhotoGroupModel, AddPhotoGroupRequest, UpdatePhotoGroupRequest>
    {

        private readonly IPhotoGroupsDomainService _photoGroupsDomainService;
        public PhotoGroupsController(IPhotoGroupsRepository repository, 
            IPhotoGroupsDomainService photoGroupsDomainService) : base(repository)
        {
            _photoGroupsDomainService = photoGroupsDomainService;
        }

        [HttpPost("add-photo")]
        public async Task<IActionResult> AddPhoto(IFormFile file) =>
            Ok(await _photoGroupsDomainService.AddPhoto(file, HttpContext));

        [HttpDelete("delete-photo/{groupId}/{photoId}")]
        public async Task<IActionResult> DeletePhoto(string groupId, string photoId) =>
            Ok(await _repository.DeletePhoto(groupId, photoId));
    }
}
