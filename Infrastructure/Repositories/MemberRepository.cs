using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using ReactDemo.Domain.Models.Party;
using ReactDemo.Domain.Repositories;
using ReactDemo.Infrastructure.Extensions;

namespace ReactDemo.Infrastructure.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {

        public MemberRepository(
            DatabaseContext databaseContext, 
            IHttpContextAccessor httpContextAccessor) : 
            base(databaseContext, httpContextAccessor)
        {
            this._entities = databaseContext.Members;
        }

        Member IMemberRepository.FindMember()
        {
            string memberIdentifier = null;
            if (_httpContext.Request.Cookies.TryGetValue("memberID", out memberIdentifier))
            {
                var member = _httpContext.Session.Get<Member>(memberIdentifier);
                if (member == null)
                {
                    int memberID;
                    if (int.TryParse(memberIdentifier, out memberID))
                    {
                        member = _entities.Find(memberID);
                    }
                    else
                    {
                        throw new InvalidCastException("can not cast the memberId from the cookie");
                    }
                }
                return member;
            }
            else
            {
                throw new NullReferenceException("can not get the memberId from cookie");
            }
        }
    }
}