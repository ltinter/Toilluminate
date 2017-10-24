using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ToilluminateModel;

namespace ToilluminateModel.Controllers
{
    public class TempletMastersController : ApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/TempletMasters
        public IQueryable<TempletMaster> GetTempletMaster()
        {
            return db.TempletMaster;
        }

        // GET: api/TempletMasters/5
        [ResponseType(typeof(TempletMaster))]
        public async Task<IHttpActionResult> GetTempletMaster(int id)
        {
            TempletMaster templetMaster = await db.TempletMaster.FindAsync(id);
            if (templetMaster == null)
            {
                return NotFound();
            }

            return Ok(templetMaster);
        }

        // PUT: api/TempletMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTempletMaster(int id, TempletMaster templetMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != templetMaster.TempletID)
            {
                return BadRequest();
            }

            templetMaster.UpdateDate = DateTime.Now;
            db.Entry(templetMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TempletMasterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TempletMasters
        [ResponseType(typeof(TempletMaster))]
        public async Task<IHttpActionResult> PostTempletMaster(TempletMaster templetMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            templetMaster.UpdateDate = DateTime.Now;
            templetMaster.InsertDate = DateTime.Now;
            db.TempletMaster.Add(templetMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = templetMaster.TempletID }, templetMaster);
        }

        // DELETE: api/TempletMasters/5
        [ResponseType(typeof(TempletMaster))]
        public async Task<IHttpActionResult> DeleteTempletMaster(int id)
        {
            TempletMaster templetMaster = await db.TempletMaster.FindAsync(id);
            if (templetMaster == null)
            {
                return NotFound();
            }

            db.TempletMaster.Remove(templetMaster);
            await db.SaveChangesAsync();

            return Ok(templetMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TempletMasterExists(int id)
        {
            return db.TempletMaster.Count(e => e.TempletID == id) > 0;
        }
    }
}