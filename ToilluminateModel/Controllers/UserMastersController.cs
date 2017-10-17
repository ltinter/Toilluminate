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
    public class UserMastersController : ApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/UserMasters
        public IQueryable<UserMaster> GetUserMaster()
        {
            return db.UserMaster;
        }

        // GET: api/UserMasters/5
        [ResponseType(typeof(UserMaster))]
        public async Task<IHttpActionResult> GetUserMaster(int id)
        {
            UserMaster userMaster = await db.UserMaster.FindAsync(id);
            if (userMaster == null)
            {
                return NotFound();
            }

            return Ok(userMaster);
        }

        // PUT: api/UserMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserMaster(int id, UserMaster userMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userMaster.UserID)
            {
                return BadRequest();
            }

            db.Entry(userMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserMasterExists(id))
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

        // POST: api/UserMasters
        [ResponseType(typeof(UserMaster))]
        public async Task<IHttpActionResult> PostUserMaster(UserMaster userMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserMaster.Add(userMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = userMaster.UserID }, userMaster);
        }

        // DELETE: api/UserMasters/5
        [ResponseType(typeof(UserMaster))]
        public async Task<IHttpActionResult> DeleteUserMaster(int id)
        {
            UserMaster userMaster = await db.UserMaster.FindAsync(id);
            if (userMaster == null)
            {
                return NotFound();
            }

            db.UserMaster.Remove(userMaster);
            await db.SaveChangesAsync();

            return Ok(userMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserMasterExists(int id)
        {
            return db.UserMaster.Count(e => e.UserID == id) > 0;
        }
    }
}