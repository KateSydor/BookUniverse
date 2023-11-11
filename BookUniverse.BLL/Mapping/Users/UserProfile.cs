namespace BookUniverse.BLL.Mapping.User
{
    using AutoMapper;
    using BookUniverse.BLL.DTOs;
    using BookUniverse.DAL.Entities;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegistrationDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .AfterMap((src, dest, opt) =>
                {
                    if (opt.Items.TryGetValue("hashedPassword", out var hashedPassword))
                    {
                        dest.Password = (string)hashedPassword;
                    }
                });

            CreateMap<EditUserDto, User>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
