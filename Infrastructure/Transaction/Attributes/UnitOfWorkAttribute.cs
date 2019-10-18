using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.Extensions.Logging;
using ReactDemo.Infrastructure.Repositories;

namespace ReactDemo.Infrastructure.Transaction.Attributes
{
    public class UnitOfWorkAttribute : AbstractInterceptorAttribute
    {
        [FromContainer]
        public DatabaseContext DbContext { get; set; }

        [FromContainer]
        public ILogger<UnitOfWorkAttribute> Logger { get; set; } 

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            Logger.LogInformation("开启事务");
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                await next(context);

                Logger.LogInformation("事务提交");
                transaction.Commit();
            }

            Logger.LogInformation("事务关闭");
        }
    }
}