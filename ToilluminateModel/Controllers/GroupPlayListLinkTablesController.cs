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
    public class GroupPlayListLinkTablesController : ApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/GroupPlayListLinkTables
        public IQueryable<GroupPlayListLinkTable> GetGroupPlayListLinkTable()
        {
            return db.GroupPlayListLinkTable;
        }

        // GET: api/GroupPlayListLinkTables/5
        [ResponseType(typeof(GroupPlayListLinkTable))]
        public async Task<IHttpActionResult> GetGroupPlayListLinkTable(int id)
        {
            GroupPlayListLinkTable groupPlayListLinkTable = await db.GroupPlayListLinkTable.FindAsync(id);
            if (groupPlayListLinkTable == null)
            {
                return NotFound();
            }

            return Ok(groupPlayListLinkTable);
        }

        // PUT: api/GroupPlayListLinkTables/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGroupPlayListLinkTable(int id, GroupPlayListLinkTable groupPlayListLinkTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groupPlayListLinkTable.ID)
            {
                return BadRequest();
            }

            db.Entry(groupPlayListLinkTable).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupPlayListLinkTableExists(id))
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

        // POST: api/GroupPlayListLinkTables
        [ResponseType(typeof(GroupPlayListLinkTable))]
        public async Task<IHttpActionResult> PostGroupPlayListLinkTable(GroupPlayListLinkTable groupPlayListLinkTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GroupPlayListLinkTable.Add(groupPlayListLinkTable);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = groupPlayListLinkTable.ID }, groupPlayListLinkTable);
        }

        // DELETE: api/GroupPlayListLinkTables/5
        [ResponseType(typeof(GroupPlayListLinkTable))]
        public async Task<IHttpActionResult> DeleteGroupPlayListLinkTable(int id)
        {
            GroupPlayListLinkTable groupPlayListLinkTable = await db.GroupPlayListLinkTable.FindAsync(id);
            if (groupPlayListLinkTable == null)
            {
                return NotFound();
            }

            db.GroupPlayListLinkTable.Remove(groupPlayListLinkTable);
            await db.SaveChangesAsync();

            return Ok(groupPlayListLinkTable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupPlayListLinkTableExists(int id)
        {
            return db.GroupPlayListLinkTable.Count(e => e.ID == id) > 0;
        }
    }
}