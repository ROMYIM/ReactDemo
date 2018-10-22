using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DatabaseContext _context;

        public MemberRepository(DatabaseContext context)
        {
            this._context = context;
        }

        void IRepository<Member>.Add(Member entity)
        {
            _context.Member.Add(entity);

        }

        void IRepository<Member>.Delete(Member entity)
        {
            _context.Member.Remove(entity);
        }

        IList<Member> IRepository<Member>.FindList(Expression<Func<Member, bool>> predicate)
        {
            return _context.Member.Where(predicate).ToList();
        }

        Member IRepository<Member>.FindOne(Expression<Func<Member, bool>> predicate)
        {
            return _context.Member.Single(predicate);
        }

        void IRepository<Member>.Update(Member entity)
        {
            _context.Member.Update(entity);
        }
    }
}