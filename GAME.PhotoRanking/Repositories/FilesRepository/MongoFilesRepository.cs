using AutoMapper;
using GAME.PhotoRanking.DBContext;
using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.File;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace GAME.PhotoRanking.Repositories.FilesRepository
{
    public class MongoFilesRepository : BaseMongoRepository, IFilesRepository
    {
        public MongoFilesRepository(MongoDBContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public Task<Response<string>> AddFile(IFormFile file) =>
            Response<string>.DoAsync(async resp =>
            {
                using (Stream stream = new MemoryStream())
                {
                    string extension = Path.GetExtension(file.FileName);
                    string fileName = $"{Guid.NewGuid()}{extension}";
                    file.CopyTo(stream);
                    stream.Position = 0;

                    GridFSUploadOptions options = new GridFSUploadOptions()
                    {
                         ContentType = file.ContentType 
                    };

                    ObjectId objectId = _db.Files.UploadFromStream(fileName, stream, options);
                    resp.Data = objectId.ToString();
                }
            });

        public Task<Response<byte[]>> GetFileBytes(string id) =>
            Response<byte[]>.DoAsync(async resp =>
            {
                ObjectId objectId = ObjectId.Parse(id);
                resp.Data = await _db.Files.DownloadAsBytesAsync(objectId);
            });

        public Task<Response<GridFSFileInfo<ObjectId>>> GetFullFileInfo(string id) =>
            Response<GridFSFileInfo<ObjectId>>.DoAsync(async resp =>
            {
                ObjectId objectId = ObjectId.Parse(id);
                var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(f => f.Id, objectId);
                var files = await _db.FilesInfo.Find(filter).ToListAsync();
                resp.ThrowIfEmptyArray(files, "File not found");
                resp.Data = files.First();
            });

        public Task<Response<ShortFileInfo>> GetShortFileInfo(string id) =>
            Response<ShortFileInfo>.DoAsync(async resp =>
            {
                var fullInfo = (await GetFullFileInfo(id)).GetResult();
                ShortFileInfo shortFileInfo = new ShortFileInfo()
                {
                    Id = id,
                    FileName = fullInfo.Filename,
                    Size = fullInfo.Length,
                    MD5 = fullInfo.MD5,
                    ContentType = fullInfo.ContentType,
                    UploadDate = fullInfo.UploadDateTime
                };

                resp.Data = shortFileInfo;
            });
    }
}
