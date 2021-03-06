﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cbbmsR3.Models;
using cbbmsRnD.Models.InvMgt;

namespace cbbmsR3.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class InventoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Inventories
        public async Task<ActionResult> Index()
        {
            var inventories = db.Inventories.Include(i => i.AppUser).Include(i => i.Location);
            return View(await inventories.ToListAsync());
        }

        // GET: Inventories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = await db.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: Inventories/Create
        public ActionResult Create()
        {
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName");
            ViewBag.StandbyCustodian = new SelectList(db.AppUsers, "AppUserID", "UserName");
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Description");
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InventoryId,Title,Description,LocationId,AppUserID,StandbyCustodian,CreatedOn,Remarks")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Inventories.Add(inventory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", inventory.AppUserID);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Description", inventory.LocationId);
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = await db.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", inventory.AppUserID);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Description", inventory.LocationId);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InventoryId,Title,Description,LocationId,AppUserID,StandbyCustodian,CreatedOn,Remarks")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AppUserID = new SelectList(db.AppUsers, "AppUserID", "UserName", inventory.AppUserID);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Description", inventory.LocationId);
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = await db.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Inventory inventory = await db.Inventories.FindAsync(id);
            db.Inventories.Remove(inventory);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
