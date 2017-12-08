using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using ToilluminateModel;
using ToilluminateModel.Models;

namespace ToilluminateModel.Controllers
{
    public class UserMastersController : BaseApiController
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

        [AllowAnonymous]
        [HttpPost, Route("api/UserMasters/UserLogin")]
        public async Task<IHttpActionResult> UserLogin(UserMaster userMaster)
        {
            var pwForMatch = PublicMethods.MD5(userMaster.Password);
            List<UserMaster> userList = await db.UserMaster.Where(a => a.UserName == userMaster.UserName && a.Password == pwForMatch && a.UseFlag == true).ToListAsync();
            if (userList.Count == 0)
            {
                return NotFound();
            }
            //save ticket in session
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, userList[0].UserName, DateTime.Now,
                             DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", userList[0].UserName, userList[0].Password),
                             FormsAuthentication.FormsCookiePath);
            var EncryptTicketStr = FormsAuthentication.Encrypt(ticket);
            HttpContext.Current.Session[userList[0].UserName] = EncryptTicketStr;

            UserLoginInfo uli = new UserLoginInfo();
            uli.Ticket = EncryptTicketStr;
            uli.UserMaster = userList[0];
            return Ok(uli);
        }

        [AllowAnonymous]
        [HttpPost, Route("api/UserMasters/UserLogout")]
        public async Task<IHttpActionResult> UserLogout(UserMaster userMaster)
        {
            HttpContext.Current.Session.Remove(userMaster.UserName);
            return Ok();
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