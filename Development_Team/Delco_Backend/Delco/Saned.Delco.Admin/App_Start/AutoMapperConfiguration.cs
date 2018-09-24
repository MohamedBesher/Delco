using AutoMapper;
using Saned.Delco.Admin.Infrastructure;



namespace Saned.Delco.Admin
{
    public class AutoMapperConfiguration {
        public static void Configure( ) {
            Mapper.Initialize( x => {
                x.AddProfile<DomainToViewModelMappingProfile>( );
            } );
        }
    }
}