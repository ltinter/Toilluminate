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
    public class TemplatTypeMastersController : ApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/TemplatTypeMasters
        public IQueryable<TemplatTypeMaster> GetTemplatTypeMaster()
        {
            return db.TemplatTypeMaster;
        }

        // GET: api/TemplatTypeMasters/5
        [ResponseType(typeof(TemplatTypeMaster))]
        public async Task<IHttpActionResult> GetTemplatTypeMaster(int id)
        {
            TemplatTypeMaster templatTypeMaster = await db.TemplatTypeMaster.FindAsync(id);
            if (templatTypeMaster == null)
            {
                return NotFound();
            }

            return Ok(templatTypeMaster);
        }

        // PUT: api/TemplatTypeMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTemplatTypeMaster(int id, TemplatTypeMaster templatTypeMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != templatTypeMaster.TempletTypeID)
            {
                return BadRequest();
            }

            templatTypeMaster.UpdateDate = DateTime.Now;
            db.Entry(templatTypeMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TemplatTypeMasterExists(id))
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

        // POST: api/TemplatTypeMasters
        [ResponseType(typeof(TemplatTypeMaster))]
        public async Task<IHttpActionResult> PostTemplatTypeMaster(TemplatTypeMaster templatTypeMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            templatTypeMaster.UpdateDate = DateTime.Now;
            templatTypeMaster.InsertDate = DateTime.Now;
            db.TemplatTypeMaster.Add(templatTypeMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = templatTypeMaster.TempletTypeID }, templatTypeMaster);
        }

        // DELETE: api/TemplatTypeMasters/5
        [ResponseType(typeof(TemplatTypeMaster))]
        public async Task<IHttpActionResult> DeleteTemplatTypeMaster(int id)
        {
            TemplatTypeMaster templatTypeMaster = await db.TemplatTypeMaster.FindAsync(id);
            if (templatTypeMaster == null)
            {
                return NotFound();
            }

            db.TemplatTypeMaster.Remove(templatTypeMaster);
            await db.SaveChangesAsync();

            return Ok(templatTypeMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TemplatTypeMasterExists(int id)
        {
            return db.TemplatTypeMaster.Count(e => e.TempletTypeID == id) > 0;
        }
    }
}