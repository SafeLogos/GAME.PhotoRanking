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

        Task<Response<GridFSFileInfo<ObjectId>>> GetFileByMD5(IFormFile file);
        Task<Response<bool>> FileExists(IFormFile file);
        Task<Response<byte[]>> GetMD5(IFormFile file);
        Task<Response<string>> GetMD5String(IFormFile file);
    }
}
