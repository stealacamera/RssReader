using Carter;

namespace RssReader.API.Modules;

public class UsersModule : CarterModule
{
    public UsersModule() : base("/users")
    {        
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        throw new NotImplementedException();
    }
}
