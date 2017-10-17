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
    public class FolderMastersController : ApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/FolderMasters
        public IQueryable<FolderMaster> GetFolderMaster()
        {
            return db.FolderMaster;
        }

        // GET: api/FolderMasters/5
        [ResponseType(typeof(FolderMaster))]
        public async Task<IHttpActionResult> GetFolderMaster(int id)
        {
            FolderMaster folderMaster = await db.FolderMaster.FindAsync(id);
            if (folderMaster == null)
            {
                return NotFound();
            }

            return Ok(folderMaster);
        }

        // PUT: api/FolderMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFolderMaster(int id, FolderMaster folderMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != folderMaster.FolderID)
            {
                return BadRequest();
            }

            db.Entry(folderMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FolderMasterExists(id))
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

        // POST: api/FolderMasters
        [ResponseType(typeof(FolderMaster))]
        public async Task<IHttpActionResult> PostFolderMaster(FolderMaster folderMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FolderMaster.Add(folderMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = folderMaster.FolderID }, folderMaster);
        }

        // DELETE: api/FolderMasters/5
        [ResponseType(typeof(FolderMaster))]
        public async Task<IHttpActionResult> DeleteFolderMaster(int id)
        {
            FolderMaster folderMaster = await db.FolderMaster.FindAsync(id);
            if (folderMaster == null)
            {
                return NotFound();
            }

            db.FolderMaster.Remove(folderMaster);
            await db.SaveChangesAsync();

            return Ok(folderMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FolderMasterExists(int id)
        {
            return db.FolderMaster.Count(e => e.FolderID == id) > 0;
        }
    }
}