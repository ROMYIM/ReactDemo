using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {

        public OrganizationRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
            this._entities = databaseContext.Organizations;
        }

    }
}