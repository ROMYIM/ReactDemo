using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ReactDemo.Application.Dtos;
using ReactDemo.Domain.Models.Conference;
using ReactDemo.Domain.Repositories;

namespace ReactDemo.Infrastructure.Repositories
{
    public class ConferenceRepository : Repository<Conference>, IConferenceRepository
    {
        public ConferenceRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
            this.Entities = _databaseContext.Conferences;
        }
    }
}