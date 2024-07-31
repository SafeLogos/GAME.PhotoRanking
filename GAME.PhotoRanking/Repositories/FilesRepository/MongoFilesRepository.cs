using AutoMapper;
using GAME.PhotoRanking.DBContext;
using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.File;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Security.Cryptography;
using System.Text;

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
                var md5File = await GetFileByMD5(file);
                if (md5File.Code == 0)
                {
                    resp.Data = md5File.GetResult().Id.ToString();
                    resp.Message = "Файл уже существовал, поэтому он не будет записан заново";
                    return;
                }

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

        public Task<Response<GridFSFileInfo<ObjectId>>> GetFileByMD5(IFormFile file) =>
            Response<GridFSFileInfo<ObjectId>>.DoAsync(async resp =>
            {
                string md5 = (await GetMD5String(file)).GetResult();
                var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(f => f.MD5, md5);
                var files = _db.FilesInfo.Find(filter).ToList();
                resp.ThrowIfEmptyArray(files, "Файл не найден");

                resp.Data = files.First();
            });

        public Task<Response<bool>> FileExists(IFormFile file) =>
            Response<bool>.DoAsync(async resp =>
            {
                string md5 = (await GetMD5String(file)).GetResult();
                var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(f => f.MD5, md5);
                resp.Data = _db.FilesInfo.Find(filter).Any();
            });

        public Task<Response<byte[]>> GetMD5(IFormFile file) =>
            Response<byte[]>.DoAsync(async resp =>
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        resp.Data = md5.ComputeHash(stream);
                    }
                }
            });

        public Task<Response<string>> GetMD5String(IFormFile file) =>
            Response<string>.DoAsync(async resp =>
            {
                byte[] md5 = (await GetMD5(file)).GetResult();


                var builder = new StringBuilder(md5.Length * 2);
                for (var i = 0; i < md5.Length; i++)
                {
                    builder.Append(md5[i].ToString("x2"));
                }

                resp.Data = builder.ToString();
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
