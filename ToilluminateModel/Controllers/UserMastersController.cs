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

            userMaster.UpdateDate = DateTime.Now;
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

            userMaster.UpdateDate = DateTime.Now;
            userMaster.InsertDate = DateTime.Now;
            userMaster.Password = PublicMethods.MD5(userMaster.Password);
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

        [HttpPost, Route("api/UserMasters/MatchUserInfo")]
        public async Task<IHttpActionResult> MatchUserInfo(UserMaster userMaster)
        {
            var pwForMatch = PublicMethods.MD5(userMaster.Password);
            List<UserMaster> userList = await db.UserMaster.Where(a => a.UserName == userMaster.UserName && a.Password == pwForMatch).ToListAsync();
            if (userList.Count == 0)
            {
                return NotFound();
            }

            return Ok(userList[0]);
        }

        [HttpGet, Route("api/UserMasters/GetUserByName/{userName}")]
        public async Task<List<UserMaster>> GetUserByName(string userName)
        {
            List<UserMaster> userList = await db.UserMaster.Where(a => a.UserName == userName).ToListAsync();
            return userList;
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