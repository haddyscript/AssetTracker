using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AssetTracker.Models;

namespace AssetTracker.Controllers
{
	public class AssetController : Controller
    {
        private static List<Asset> assetList = new List<Asset>()
        {
            new Asset { id = 1, assetName = "Shabu", Category = "SWAP", Quantity = 5, AssignedTo = "Lodi Cakes" },
            new Asset { id = 2, assetName = "Marijuana", Category = "CASH", Quantity = 5, AssignedTo = "Lodi Biscuit" },
            new Asset { id = 3, assetName = "Sigarilyo", Category = "CASH", Quantity = 5, AssignedTo = "Lodhan" },
            new Asset { id = 4, assetName = "Sigamata", Category = "CASH", Quantity = 5, AssignedTo = "Luwaan" }
        };

        public IActionResult Index()
        {
            return View(assetList);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Asset asset)
        {
            if (ModelState.IsValid)
            {
                asset.id = assetList.Count + 1;
                assetList.Add(asset);
                return RedirectToAction("Index");
            }
            return View(asset);
        }

        public ActionResult Edit(int id)
        {
            var asset = assetList.FirstOrDefault(ass => ass.id == id);

            return View(asset);
        }
        public ActionResult EditAction(Asset asset)
        {
            var lubot = assetList.Where(ass => ass.id == asset.id).FirstOrDefault();
            if (lubot == null)
            {
                return BadRequest("Asset not found"); 
            }
            lubot.assetName = asset.assetName;
            lubot.Category = asset.Category;
            lubot.Quantity = asset.Quantity;
            lubot.AssignedTo = asset.AssignedTo;

            return RedirectToAction("Index");

        }

        public ActionResult Delete(int id)
        {
            var asset = assetList.FirstOrDefault(ass => ass.id == id);
            if (id != null || id != 0 && asset != null)
            {
                assetList.Remove(asset);
            }
            return RedirectToAction("Index");
        }
    }
}

