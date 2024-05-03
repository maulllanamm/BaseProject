using DTO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services.Interface;
using System.Net;

namespace Middleware
{
    public class BaseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtDTO _jwt;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BaseMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory, IOptions<JwtDTO> jwt)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
            _jwt = jwt.Value;
        }

        public async Task<bool> InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                var isPermitted = await authService.IsRequestPermitted();
                if (isPermitted.IsSuccess)
                {
                    await _next(context);
                    return true;
                }
                else//NOT PERMITTED
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return false;
                }
            }
        }
    }
}
