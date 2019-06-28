using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ReactDemo.Infrastructure.Security.Authorization
{
    /// <summary>
    /// 自定义授权策略提供程序。
    /// 一个程序中只能有一个策略提供程序。
    /// <see cref="https://docs.microsoft.com/zh-cn/aspnet/core/security/authorization/iauthorizationpolicyprovider?view=aspnetcore-2.2#use-a-custom-iauthorizationpolicyprovider"/>
    /// </summary>
    public class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _provider;

        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _provider = new DefaultAuthorizationPolicyProvider(options);
        }

        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return await _provider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName == null)
            {
                throw new ArgumentNullException(nameof(policyName));
            }
            var policyProperties = policyName.Split('-', 3, StringSplitOptions.RemoveEmptyEntries);
            var policyBuilder = new AuthorizationPolicyBuilder();
            var requirements = new IAuthorizationRequirement[]
            {
                new DefaultRequirementAttribute(policyProperties[1], (ResourceOperation) Enum.Parse(typeof(ResourceOperation), policyProperties[2]), policyProperties[0])
            };
            policyBuilder.AddRequirements(requirements);
            return Task.FromResult(policyBuilder.Build());
        }
    }

    public class DefaultRequirementAttribute : AuthorizeAttribute, IAuthorizationRequirement
    {
        public string Resource { get; }

        public string Operation { get; }

        public DefaultRequirementAttribute(string resource, ResourceOperation operation, string policy = "DefaultAuthorization") : base(policy)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }
            Resource = resource;
            Operation = operation.ToString();

            var policyBuilder = new StringBuilder();
            Policy = policyBuilder.Append(Policy).Append("-").Append(Resource).Append("-").Append(Operation).ToString();
        }
    }

    public enum ResourceOperation
    {
        Add, 
        Delete,
        Edit,
        Query
    }

    public class DefaultAuthorizationHandler : AuthorizationHandler<DefaultRequirementAttribute>
    {
        private readonly ILogger _logger;

        public DefaultAuthorizationHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultRequirementAttribute requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return Task.CompletedTask;
            }

            _logger.LogDebug(context.Resource.ToString());

            var principal = context.User;
            var role = principal.FindFirstValue(ClaimTypes.Role);
            if (role == "admin")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}