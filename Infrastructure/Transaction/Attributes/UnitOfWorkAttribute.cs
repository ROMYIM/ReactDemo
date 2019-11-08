using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ReactDemo.Infrastructure.Repositories;
using ReactDemo.Infrastructure.Event.Helpers;

namespace ReactDemo.Infrastructure.Transaction.Attributes
{
    public class UnitOfWorkAttribute : AbstractInterceptorAttribute
    {

        [FromContainer]
        public ILogger<UnitOfWorkAttribute> Logger { get; set; }

		[FromContainer]
		public IHttpContextAccessor ContextAccessor { get; set; }

		public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
			DatabaseContext dbContext = context.ServiceProvider.GetService<DatabaseContext>();

			if (dbContext.EventHelper == null)
			{
				dbContext.EventHelper = context.ServiceProvider.GetService<IEventHelper>();
			}

			await next(context);
			await dbContext.SaveChangesAsync();

            // Logger.LogInformation("开启事务");
			// var transaction = dbContext.Database.BeginTransaction();
			// try
			// {
			// 	await next(context);

			// 	Logger.LogInformation("事务提交");
			// 	transaction.Commit();
			// }
			// catch (System.Exception)
			// {
			// 	transaction.Rollback();
			// 	Logger.LogInformation("事务回滚");
				
			// 	var httpContext = ContextAccessor.HttpContext;
			// 	httpContext.Response.StatusCode = 500;
			// }
			// finally
			// {
			// 	transaction.Dispose();
			// 	Logger.LogInformation("事务关闭");
			// }
            
        }
    }
}