using AutoMapper;
using GAME.PhotoRanking.DBContext;

namespace GAME.PhotoRanking.Repositories
{
    public abstract class BaseMongoRepository
    {
        protected readonly MongoDBContext _db;
        protected readonly IMapper _mapper;
        protected BaseMongoRepository(MongoDBContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
    }
}
