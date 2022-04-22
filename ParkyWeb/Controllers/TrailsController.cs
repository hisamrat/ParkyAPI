using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;

        public TrailsController(INationalParkRepository nationalParkRepository,ITrailRepository trailRepository)
        {
            _nationalParkRepository = nationalParkRepository;
            _trailRepository = trailRepository;
        }
        public IActionResult Index()
        {
            return View(new Trail() {});
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> nplist = await _nationalParkRepository.
                GetAllAsync(SD.NationalParkAPIPath);


            TrailsVM obj = new TrailsVM()
            {

                NationalParkList = nplist.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),Trail=new Trail()
            
            };
            if(id==null)
            {
                //this will be tru for insert and create
                return View(obj);
            }
            //flow come here for update
            obj.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath,
                id.GetValueOrDefault());
            if(obj.Trail == null)
            {
                return NotFound();
            }
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
         if(ModelState.IsValid)
            {
                
                if(obj.Trail.Id==0)
                {
                    await _trailRepository.CreateAsync(
                        SD.TrailAPIPath, obj.Trail);
                }
                else
                {
                    await _trailRepository.UpdateAsync(
                        SD.TrailAPIPath + obj.Trail.Id, obj.Trail);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> nplist = await _nationalParkRepository.
                GetAllAsync(SD.NationalParkAPIPath);


                TrailsVM objVM = new TrailsVM()
                {

                    NationalParkList = nplist.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail

                };
                return View(objVM);
            }
        
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);
            //var status = await _nationalParkRepository.DeleteAsync(SD.NationalParkAPIPath,id);
            if(status)
            {
                return Json(new { success = true, message = "Delete Successfull" });
            }
            return Json(new { success = false, message = "Delete not Successfull" });
        }
        public async Task<IActionResult> GetAllTrail()
        {
            var data = await _trailRepository.
                GetAllAsync(SD.TrailAPIPath);
            return Json(new { data });
        }
    }
}
