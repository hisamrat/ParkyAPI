using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class NationalParkRepository: Repository<NationalPark>,INationalParkRepository
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IHttpClientFactory _clientFactory;
#pragma warning restore IDE0052 // Remove unread private members

        public NationalParkRepository(IHttpClientFactory clientFactory):base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
