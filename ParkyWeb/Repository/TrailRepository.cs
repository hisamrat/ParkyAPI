using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IHttpClientFactory _clientFactory;
#pragma warning restore IDE0052 // Remove unread private members

        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
