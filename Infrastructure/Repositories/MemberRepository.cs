using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {

        public MemberRepository(DatabaseContext context) : base(context)
        {
            
        }

    }
}