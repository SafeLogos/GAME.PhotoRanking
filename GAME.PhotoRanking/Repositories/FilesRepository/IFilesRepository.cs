using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.File;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace GAME.PhotoRanking.Repositories.FilesRepository
{
    public interface IFilesRepository
    {
        Task<Response<string>> AddFile(IFormFile file);
        Task<Response<byte[]>> GetFileBytes(string id);
        Task<Response<GridFSFileInfo<ObjectId>>> GetFullFileInfo(string id);
        Task<Response<ShortFileInfo>> GetShortFileInfo(string id);

    }
}
