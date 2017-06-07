using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using collectCoins.Models;
using PagedList;
using System.IO;

namespace collectCoins.Controllers
{
    public class CoinsCollectionsController : Controller
    {
        private CoinsDataEntities db = new CoinsDataEntities();
        private List<int> years = new List<int>();
        // GET: CoinsCollections

        public CoinsCollectionsController()
        {
            for (int i = 2017; i >= 1771; i--)
            {
               
                years.Add(i);
            }
        }

        public ActionResult Index(string yearSortOrder, string currentFilter, string searchYear, string valSearch, int? page)
        {
            ViewBag.CurrentSort = yearSortOrder;
            
            ViewBag.DateSortParam = yearSortOrder == "Date" ? "date_desc" : "Date";
            


            if (searchYear !=null && searchYear!="All years")
            {
                page = 1;
                ViewBag.CurrentFilter = searchYear;
            }
            else
            {
                if (!String.IsNullOrEmpty(currentFilter))
                {
                    if (double.Parse(currentFilter) >= 1771)
                        searchYear = currentFilter;
                    else
                        valSearch = currentFilter;
                }
                       
            }
          
            var coinsCollections = db.CoinsCollections.Include(c => c.EdgeInscription).Include(c => c.GeneralCoin).Include(c => c.PortretDesigner).Include(c => c.ReverseDesigner).Include(c=>c.MyCollection);

            if (coinsCollections != null)
            {
                var getListOfYears = coinsCollections.Select(s => s.Year).Distinct().ToList();
                var getValues = coinsCollections.Select(s => s.GeneralCoin.Value).Distinct().ToList();

                SelectList listOfYears = new SelectList(getListOfYears);
                SelectList listOfValues = new SelectList(getValues);

                ViewBag.listOfYears = listOfYears;
                ViewBag.listOfValues = listOfValues;

                if (!String.IsNullOrEmpty(searchYear))
                {
                    coinsCollections = coinsCollections.Where(s => s.Year.ToString() == searchYear);
                }
                if (!String.IsNullOrEmpty(valSearch))
                { 
                    var a = Double.Parse(valSearch);
                    coinsCollections = coinsCollections.Where(s => s.GeneralCoin.Value == a);
                    ViewBag.CurrentFilter = valSearch;
                }

                switch (yearSortOrder)
                {
                    case "Date":
                        coinsCollections = coinsCollections.OrderBy(s => s.Year);
                        break;
                    case "date_desc":
                        coinsCollections = coinsCollections.OrderByDescending(s => s.Year);
                        break;
                    default:
                        coinsCollections = coinsCollections.OrderBy(s => s.GeneralCoin.Value);
                        break;
                }

                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(coinsCollections.ToPagedList(pageNumber, pageSize));
            }
            else
                return HttpNotFound();

        //    return View(coinsCollections.ToList());
}

        // GET: CoinsCollections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoinsCollection coinsCollection = db.CoinsCollections.Find(id);
            if (coinsCollection == null)
            {
                return HttpNotFound();
            }
            return View(coinsCollection);
        }

        // GET: CoinsCollections/Create
        public ActionResult Create()
        {
            ViewBag.EdgeId = new SelectList(db.EdgeInscriptions.OrderBy(m=>m.Inscription), "EdgeId", "Inscription");
            ViewBag.GeneralCoinId = new SelectList(db.GeneralCoins.OrderBy(m=>m.Value), "GeneralCoinId", "Value");
            ViewBag.PortretDesignerId = new SelectList(db.PortretDesigners.OrderBy(m=>m.Name), "DesignerId", "Name");
            ViewBag.ReverseDesignerId = new SelectList(db.ReverseDesigners.OrderBy(m=>m.Name), "DesignerId", "Name");

          
            SelectList listYears = new SelectList(years.ToList());
            ViewBag.listYears = listYears;

            return View();
        }

        // POST: CoinsCollections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CoinId,GeneralCoinId,PortretDesignerId,ReverseDesignerId,EdgeId,Year,Mintage,EstimatedValues")] CoinsCollection coinsCollection, HttpPostedFileBase Image)
        {
            ViewBag.EdgeId = new SelectList(db.EdgeInscriptions, "EdgeId", "Inscription", coinsCollection.EdgeId);
            ViewBag.GeneralCoinId = new SelectList(db.GeneralCoins, "GeneralCoinId", "Value", coinsCollection.GeneralCoinId);
            ViewBag.PortretDesignerId = new SelectList(db.PortretDesigners, "DesignerId", "Name", coinsCollection.PortretDesignerId);
            ViewBag.ReverseDesignerId = new SelectList(db.ReverseDesigners, "DesignerId", "Name", coinsCollection.ReverseDesignerId);
            SelectList listYears = new SelectList(years.ToList());
            ViewBag.listYears = listYears;

            if (ModelState.IsValid)
            {
                
                coinsCollection.Image = "";
                if  (Image != null)
                {
                    if ((Image.ContentLength) > 0)
                    {
                        string imageName = Path.GetFileName(Image.FileName);
                        string path = Path.Combine(Server.MapPath("~/Content/Coins"), imageName);
                        Image.SaveAs(path);

                        coinsCollection.Image = imageName;
                    }
                }
                else
                    coinsCollection.Image = "- no image -";


                db.CoinsCollections.Add(coinsCollection);
                db.SaveChanges();
               return RedirectToAction("Index");
               
            }

           
            return View(coinsCollection);
        }

        // GET: CoinsCollections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoinsCollection coinsCollection = db.CoinsCollections.Find(id);
            if (coinsCollection == null)
            {
                return HttpNotFound();
            }

         
            ViewBag.EdgeId = new SelectList(db.EdgeInscriptions, "EdgeId", "Inscription", coinsCollection.EdgeId);
           
            
            ViewBag.GeneralCoinId = new SelectList(db.GeneralCoins, "GeneralCoinId", "Value", coinsCollection.GeneralCoinId);
            ViewBag.PortretDesignerId = new SelectList(db.PortretDesigners, "DesignerId", "Name", coinsCollection.PortretDesignerId);
            ViewBag.ReverseDesignerId = new SelectList(db.ReverseDesigners, "DesignerId", "Name", coinsCollection.ReverseDesignerId);

           
            SelectList listYears = new SelectList(years,coinsCollection.Year);
            ViewBag.listOfYears = listYears;

            return View(coinsCollection);
        }

        // POST: CoinsCollections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CoinId,GeneralCoinId,PortretDesignerId,ReverseDesignerId,EdgeId,Year,Mintage,EstimatedValues,Image")] CoinsCollection coinsCollection, HttpPostedFileBase File)
        {
            
            if ((File!=null) && (File.ContentLength > 0 ))
            {
                string imageName = Path.GetFileName(File.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/Coins"), imageName);
                File.SaveAs(path);

                coinsCollection.Image = imageName;
            }


            if (ModelState.IsValid)
            {
                db.Entry(coinsCollection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EdgeId = new SelectList(db.EdgeInscriptions, "EdgeId", "Inscription", coinsCollection.EdgeId);
            ViewBag.GeneralCoinId = new SelectList(db.GeneralCoins, "GeneralCoinId", "Value", coinsCollection.GeneralCoinId);
            ViewBag.PortretDesignerId = new SelectList(db.PortretDesigners, "DesignerId", "Name", coinsCollection.PortretDesignerId);
            ViewBag.ReverseDesignerId = new SelectList(db.ReverseDesigners, "DesignerId", "Name", coinsCollection.ReverseDesignerId);
            return View(coinsCollection);
        }

        // GET: CoinsCollections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoinsCollection coinsCollection = db.CoinsCollections.Find(id);
            if (coinsCollection == null)
            {
                return HttpNotFound();
            }
            db.CoinsCollections.Remove(coinsCollection);
            db.SaveChanges();
            return RedirectToAction("Index",new { href = Request.UrlReferrer});
            
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
