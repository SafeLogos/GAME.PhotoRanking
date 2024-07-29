using AutoMapper;
using GAME.PhotoRanking.DBContext;
using GAME.PhotoRanking.Models;
using GAME.PhotoRanking.Models.PhotoGroupModel;
using GAME.PhotoRanking.Models.Photo;
using MongoDB.Driver;
using MongoDB.Bson;

namespace GAME.PhotoRanking.Repositories.PhotoGroupsRepository
{
    public class MongoPhotoGroupsRepository : BaseMongoRepository, IPhotoGroupsRepository
    {
        public MongoPhotoGroupsRepository(MongoDBContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public Task<Response<PhotoGroupModel>> Add(AddPhotoGroupRequest request) =>
            Response<PhotoGroupModel>.DoAsync(async resp =>
            {
                PhotoGroupModel dbModel = _mapper.Map<PhotoGroupModel>(request);
                dbModel.Photos = new List<PhotoModel>();
                 _db.PhotoGroups.InsertOne(dbModel);
                resp.Data = dbModel;
            });

       

        public Task<Response<bool>> Delete(string id) =>
            Response<bool>.DoAsync(async resp =>
            {
                var filter = Builders<PhotoGroupModel>.Filter.Where(pg => pg.Id == id);
                _db.PhotoGroups.DeleteOne(filter);
                resp.Data = true;
            });

        public Task<Response<PhotoGroupModel>> Get(string id) =>
            Response<PhotoGroupModel>.DoAsync(async resp =>
            {
                var filter = Builders<PhotoGroupModel>.Filter.Where(pg => pg.Id == id);
                PhotoGroupModel dbModel = _db.PhotoGroups.Find(filter).FirstOrDefault();
                resp.ThrowIfNull(dbModel, "Group not found");
                resp.Data = dbModel;
            });

        public Task<Response<List<PhotoGroupModel>>> GetAll() =>
            Response<List<PhotoGroupModel>>.DoAsync(async resp =>
            {
                var filter = Builders<PhotoGroupModel>.Filter.Where(pg => !pg.IsDeleted);
                List<PhotoGroupModel> list = _db.PhotoGroups.Find(filter).ToList();
                resp.Data = list;
            });

        public Task<Response<PhotoGroupModel>> Update(UpdatePhotoGroupRequest request) =>
            Response<PhotoGroupModel>.DoAsync(async resp =>
            {
                var filter = Builders<PhotoGroupModel>.Filter.Where(pg => pg.Id == request.Id);
                PhotoGroupModel dbModel = _db.PhotoGroups.Find(filter).FirstOrDefault();

                _mapper.Map(request, dbModel);

                _db.PhotoGroups.ReplaceOne(filter, dbModel);
                resp.Data = dbModel;
            });

        public Task<Response<PhotoModel>> AddPhoto(AddPhotoRequest request, string imageId) =>
            Response<PhotoModel>.DoAsync(async resp =>
            {
                PhotoGroupModel group = (await Get(request.GroupId)).GetResult();
                PhotoModel photoModel = _mapper.Map<PhotoModel>(request);
                photoModel.ImageId = imageId;
                photoModel.Id = ObjectId.GenerateNewId().ToString();

                if(group.Photos == null)
                    group.Photos = new List<PhotoModel>();

                group.Photos.Add(photoModel); 
                

                var filter = Builders<PhotoGroupModel>.Filter.Where(pg => pg.Id == group.Id);
                _db.PhotoGroups.ReplaceOne(filter, group);

                resp.Data = photoModel;

            });


        public Task<Response<bool>> DeletePhoto(string groupId, string photoId) =>
            Response<bool>.DoAsync(async resp =>
            {
                PhotoGroupModel group = (await Get(groupId)).GetResult();
                group.Photos = group.Photos.Where(p => p.Id != photoId).ToList();


                var filter = Builders<PhotoGroupModel>.Filter.Where(pg => pg.Id == group.Id);
                _db.PhotoGroups.ReplaceOne(filter, group);

                resp.Data = true;

            });


    }
}
