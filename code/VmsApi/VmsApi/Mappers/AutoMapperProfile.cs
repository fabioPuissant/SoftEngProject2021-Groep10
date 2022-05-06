using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using VmsApi.Data.Models;
using VmsApi.ViewModels.PostModels;

namespace VmsApi.Mappers
{
    [ExcludeFromCodeCoverage]
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegistrationModel, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<PostTaskItemModel, TaskItem>();
            CreateMap<TaskItem, PostTaskItemModel>();
            CreateMap<UserUpdateModel, User>();
        }
    }
}
