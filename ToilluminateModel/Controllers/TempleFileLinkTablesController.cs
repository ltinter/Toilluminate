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
    public class TempleFileLinkTablesController : ApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/TempleFileLinkTables
        public IQueryable<TempleFileLinkTable> GetTempleFileLinkTable()
        {
            return db.TempleFileLinkTable;
        }

        // GET: api/TempleFileLinkTables/5
        [ResponseType(typeof(TempleFileLinkTable))]
        public async Task<IHttpActionResult> GetTempleFileLinkTable(int id)
        {
            TempleFileLinkTable templeFileLinkTable = await db.TempleFileLinkTable.FindAsync(id);
            if (templeFileLinkTable == null)
            {
                return NotFound();
            }

            return Ok(templeFileLinkTable);
        }

        // PUT: api/TempleFileLinkTables/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTempleFileLinkTable(int id, TempleFileLinkTable templeFileLinkTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != templeFileLinkTable.ID)
            {
                return BadRequest();
            }

            db.Entry(templeFileLinkTable).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TempleFileLinkTableExists(id))
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

        // POST: api/TempleFileLinkTables
        [ResponseType(typeof(TempleFileLinkTable))]
        public async Task<IHttpActionResult> PostTempleFileLinkTable(TempleFileLinkTable templeFileLinkTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TempleFileLinkTable.Add(templeFileLinkTable);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = templeFileLinkTable.ID }, templeFileLinkTable);
        }

        // DELETE: api/TempleFileLinkTables/5
        [ResponseType(typeof(TempleFileLinkTable))]
        public async Task<IHttpActionResult> DeleteTempleFileLinkTable(int id)
        {
            TempleFileLinkTable templeFileLinkTable = await db.TempleFileLinkTable.FindAsync(id);
            if (templeFileLinkTable == null)
            {
                return NotFound();
            }

            db.TempleFileLinkTable.Remove(templeFileLinkTable);
            await db.SaveChangesAsync();

            return Ok(templeFileLinkTable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TempleFileLinkTableExists(int id)
        {
            return db.TempleFileLinkTable.Count(e => e.ID == id) > 0;
        }
    }
}