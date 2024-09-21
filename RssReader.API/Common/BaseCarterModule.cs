using Carter;
using System.IdentityModel.Tokens.Jwt;

namespace RssReader.API.Common;

public abstract class BaseCarterModule : CarterModule
{
    protected BaseCarterModule(string path) : base(path)
    {
    }

    protected int GetRequesterId(HttpContext httpContext)
    {
        var id = httpContext.User
                            .Claims
                            .Where(x => x.Type == JwtRegisteredClaimNames.Sub)
                            .FirstOrDefault()?
                            .Value;

        return int.Parse(id!);
    }
}
