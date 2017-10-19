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
    public class PlayListMastersController : ApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/PlayListMasters
        public IQueryable<PlayListMaster> GetPlayListMaster()
        {
            return db.PlayListMaster;
        }

        // GET: api/PlayListMasters/5
        [ResponseType(typeof(PlayListMaster))]
        public async Task<IHttpActionResult> GetPlayListMaster(int id)
        {
            PlayListMaster playListMaster = await db.PlayListMaster.FindAsync(id);
            if (playListMaster == null)
            {
                return NotFound();
            }

            return Ok(playListMaster);
        }

        // PUT: api/PlayListMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlayListMaster(int id, PlayListMaster playListMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playListMaster.PlayListID)
            {
                return BadRequest();
            }

            db.Entry(playListMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayListMasterExists(id))
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

        // POST: api/PlayListMasters
        [ResponseType(typeof(PlayListMaster))]
        public async Task<IHttpActionResult> PostPlayListMaster(PlayListMaster playListMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PlayListMaster.Add(playListMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = playListMaster.PlayListID }, playListMaster);
        }

        // DELETE: api/PlayListMasters/5
        [ResponseType(typeof(PlayListMaster))]
        public async Task<IHttpActionResult> DeletePlayListMaster(int id)
        {
            PlayListMaster playListMaster = await db.PlayListMaster.FindAsync(id);
            if (playListMaster == null)
            {
                return NotFound();
            }

            db.PlayListMaster.Remove(playListMaster);
            await db.SaveChangesAsync();

            return Ok(playListMaster);
        }


        //[HttpPost, Route("api/uploadFile")]
        //public async Task<IHttpActionResult> uploadFile()
        //{
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayListMasterExists(int id)
        {
            return db.PlayListMaster.Count(e => e.PlayListID == id) > 0;
        }
    }
}