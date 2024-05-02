using AutoMapper;
using AutoMapper.EquivalencyExpression;
using DTO;
using Entities;

namespace Marketplace
{
    public class AutoMapperProfilling : Profile
    {
        public AutoMapperProfilling()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<UserDTO, User>().EqualityComparison((odto, o) => odto.Id == o.id).ReverseMap();
            CreateMap<User, UserDTO>().EqualityComparison((odto, o) => odto.id == o.Id).ReverseMap();
        }
    }
}
