using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;

#pragma warning disable IDE0060 // Remove unused parameter
        public HomeController(ILogger<HomeController> logger,INationalParkRepository nationalParkRepository,
#pragma warning restore IDE0060 // Remove unused parameter
            ITrailRepository trailRepository)
        {
           // _logger = logger;
           _nationalParkRepository = nationalParkRepository;
            _trailRepository = trailRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM listofParkAndTrails = new IndexVM()
            { 
                NationalParkList=await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath),
                TrailList=await _trailRepository.GetAllAsync(SD.TrailAPIPath),
            
            };


            return View(listofParkAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
