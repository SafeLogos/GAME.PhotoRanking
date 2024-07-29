using AutoMapper;
using GAME.PhotoRanking.Models.PhotoGroupModel;
using GAME.PhotoRanking.Models.Photo;

namespace GAME.PhotoRanking.Profiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {

            photoGroup();
            photoModel();
        }

        private void photoGroup()
        {
            CreateMap<AddPhotoGroupRequest, PhotoGroupModel>();
            CreateMap<UpdatePhotoGroupRequest, PhotoGroupModel>();
        }

        private void photoModel()
        {
            CreateMap<AddPhotoRequest, PhotoModel>();
        }
    }
}
