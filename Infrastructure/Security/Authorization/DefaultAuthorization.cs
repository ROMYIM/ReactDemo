using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ReactDemo.Infrastructure.Security.Authorization
{
    /// <summary>
    /// 自定义授权策略提供程序。
    /// 一个程序中只能有一个策略提供程序。
    /// <see cref="https://docs.microsoft.com/zh-cn/aspnet/core/security/authorization/iauthorizationpolicyprovider?view=aspnetcore-2.2#use-a-custom-iauthorizationpolicyprovider"/>
    /// </summary>
    public class DefaultAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly Microsoft.AspNetCore.Authorization.DefaultAuthorizationPolicyProvider _provider;

        public DefaultAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _provider = new Microsoft.AspNetCore.Authorization.DefaultAuthorizationPolicyProvider(options);
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
            policyBuilder.AddRequirements(new DefaultRequirementAttribute(policyProperties[1], (ResourceOperation) Enum.Parse(typeof(ResourceOperation), policyProperties[2]), policyProperties[0]));
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
}