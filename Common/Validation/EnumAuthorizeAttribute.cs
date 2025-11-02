using Microsoft.AspNetCore.Authorization;
using dotnet.Enums;

namespace dotnet.Common.Validation;

public class EnumAuthorizeAttribute : AuthorizeAttribute
{
    public EnumAuthorizeAttribute(params Role[] roles)
    {
        Roles = string.Join(",", roles.Select(r => r.ToString()));
    }
}
