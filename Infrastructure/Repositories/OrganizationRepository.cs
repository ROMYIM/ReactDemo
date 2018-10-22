using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly DatabaseContext _context;

        public OrganizationRepository(DatabaseContext context)
        {
            this._context = context;
        }

        void IRepository<Organization>.Add(Organization entity)
        {
            _context.Organization.Add(entity);
        }

        void IRepository<Organization>.Delete(Organization entity)
        {
            _context.Organization.Remove(entity);
        }

        IList<Organization> IRepository<Organization>.FindList(Expression<Func<Organization, bool>> predicate)
        {
            return _context.Organization.Where(predicate).ToList();
        }

        Organization IRepository<Organization>.FindOne(Expression<Func<Organization, bool>> predicate)
        {
            return _context.Organization.Single(predicate);
        }

        void IRepository<Organization>.Update(Organization entity)
        {
            _context.Organization.Update(entity);
        }
    }
}