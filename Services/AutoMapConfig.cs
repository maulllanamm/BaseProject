using AutoMapper;

namespace Marketplace
{
    public class AutoMapConfig : MapperConfiguration
    {
        public AutoMapConfig(MapperConfigurationExpression cfg) : base(cfg)
        {
            cfg.AddProfile<AutoMapperProfilling>();

            cfg.AllowNullCollections = true;
            cfg.AllowNullDestinationValues = true;

        }
    }
}
