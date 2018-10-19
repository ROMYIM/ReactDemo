using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Reporsitories;

namespace ReactDemo.Infrastructure.Reporistories
{
    public class MemberReporistory : IMemberReporisitory
    {
        private readonly DatabaseContext _context;

        int IReporistory<Member>.Add(Member entity)
        {
            _context.Member.Add(entity);
            return _context.SaveChanges();
        }

        int IReporistory<Member>.Delete(Member entity)
        {
            _context.Member.Remove(entity);
            return _context.SaveChanges();
        }

        IList<Member> IReporistory<Member>.FindList(Expression<Predicate<Member>> expression)
        {
            
        }

        Member IReporistory<Member>.FindOne(int id)
        {
            throw new NotImplementedException();
        }

        int IReporistory<Member>.Update(Member entity)
        {
            throw new NotImplementedException();
        }
    }
}