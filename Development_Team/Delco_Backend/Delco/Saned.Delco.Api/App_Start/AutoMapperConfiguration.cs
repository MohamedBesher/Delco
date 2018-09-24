using AutoMapper;
using Saned.Delco.Api.Infrastructure;


namespace Saned.Delco.Api
{
    public class AutoMapperConfiguration {
        public static void Configure( ) {
            Mapper.Initialize( x => {
                x.AddProfile<DomainToViewModelMappingProfile>( );
            } );
        }
    }
}