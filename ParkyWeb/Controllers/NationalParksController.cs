﻿using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;

        public NationalParksController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() {});
        }

        public async Task<IActionResult> Upsert(int? id)
        {

            NationalPark obj = new NationalPark();
            if(id==null)
            {
                //this will be tru for insert and create
                return View(obj);
            }
            //flow come here for update
            obj = await _nationalParkRepository.GetAsync(SD.NationalParkAPIPath,
                id.GetValueOrDefault());
            if(obj==null)
            {
                return NotFound();
            }
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
         if(ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if(files.Count>0)
                {
                    byte[] p1 = null;
                    using (var fs1=files[0].OpenReadStream())
                    {
                        using var ms1 = new MemoryStream();
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    obj.Picture = p1;
                }
                else
                {
                    var objFromDb = await _nationalParkRepository.GetAsync(
                        SD.NationalParkAPIPath, obj.Id);
                    obj.Picture = objFromDb.Picture;
                }
                if(obj.Id==0)
                {
                    await _nationalParkRepository.CreateAsync(
                        SD.NationalParkAPIPath, obj);
                }
                else
                {
                    await _nationalParkRepository.UpdateAsync(
                        SD.NationalParkAPIPath+obj.Id, obj);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _nationalParkRepository.DeleteAsync(SD.NationalParkAPIPath, id);
            //var status = await _nationalParkRepository.DeleteAsync(SD.NationalParkAPIPath,id);
            if(status)
            {
                return Json(new { success = true, message = "Delete Successfull" });
            }
            return Json(new { success = false, message = "Delete not Successfull" });
        }
        public async Task<IActionResult> GetAllNationalPark()
        {
            var data = await _nationalParkRepository.
                GetAllAsync(SD.NationalParkAPIPath);
            return Json(new { data });
        }
    }
}
