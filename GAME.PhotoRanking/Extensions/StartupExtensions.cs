using AutoMapper;
using GAME.PhotoRanking.DBContext;
using GAME.PhotoRanking.Profiles;
using GAME.PhotoRanking.Repositories.FilesRepository;
using GAME.PhotoRanking.Repositories.PhotoGroupsRepository;
using GAME.PhotoRanking.Services.Domain.PhotoGroupsDomainService;

namespace GAME.PhotoRanking.Extensions
{
    public static class StartupExtensions
    {
        public static void BuildApplication(this WebApplicationBuilder builder)
        {
            AddSwagger(builder);
            AddApplicationServices(builder);
            AddDomainServices(builder);

            string targetDatabase = builder.Configuration["TargetDatabase"]!;
            if (targetDatabase == "Mongo")
            {
                AddMongoDbContext(builder);
                AddMongoRepositories(builder);
            }
            else if (targetDatabase == "Postgre")
            {
                AddPostgreDbContext(builder);
                AddPostgreRepositories(builder);
            }

            builder.Services.AddAutoMapper(typeof(MainProfile));
            
        }

        private static void AddApplicationServices(WebApplicationBuilder builder)
        {
        }

        private static void AddDomainServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IPhotoGroupsDomainService, PhotoGroupsDomainService>();
        }

        private static void AddMongoRepositories(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IPhotoGroupsRepository, MongoPhotoGroupsRepository>();
            builder.Services.AddScoped<IFilesRepository, MongoFilesRepository>();
        }

        private static void AddPostgreRepositories(WebApplicationBuilder builder)
        {
        }

        private static void AddMongoDbContext(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<MongoDBContext>();
        }

        private static void AddPostgreDbContext(WebApplicationBuilder builder)
        {

        }

        private static void AddSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen();
        }


    }
}
