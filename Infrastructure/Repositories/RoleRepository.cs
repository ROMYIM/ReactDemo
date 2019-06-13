using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.System;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DatabaseContext databaseContext, IHttpContextAccessor httpContextAccessor) : base(databaseContext, httpContextAccessor)
        {
            _entities = databaseContext.Roles;
        }
    }
}