using GAME.PhotoRanking.Controllers;
using GAME.PhotoRanking.Repositories.FilesRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAME.PhotoRanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesRepository _repository;

        public FilesController(IFilesRepository repository)
        {

            

            _repository = repository;
        }

        [HttpGet("info/{id}")]
        public async Task<IActionResult> GetFileInfo(string id) =>
            Ok(await _repository.GetShortFileInfo(id));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(string id)
        {
            byte[] bytes = (await _repository.GetFileBytes(id)).GetResult();
            var info = (await _repository.GetShortFileInfo(id)).GetResult();
            return File(bytes, info.ContentType, info.FileName);
        }

        [HttpGet("base64/{id}")]
        public async Task<IActionResult> Base64(string id) =>
            Ok(await _repository.DownloadBase64(id));
    }

}

