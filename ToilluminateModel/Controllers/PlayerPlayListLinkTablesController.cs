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
    public class PlayerPlayListLinkTablesController : ApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/PlayerPlayListLinkTables
        public IQueryable<PlayerPlayListLinkTable> GetPlayerPlayListLinkTable()
        {
            return db.PlayerPlayListLinkTable;
        }

        // GET: api/PlayerPlayListLinkTables/5
        [ResponseType(typeof(PlayerPlayListLinkTable))]
        public async Task<IHttpActionResult> GetPlayerPlayListLinkTable(int id)
        {
            PlayerPlayListLinkTable playerPlayListLinkTable = await db.PlayerPlayListLinkTable.FindAsync(id);
            if (playerPlayListLinkTable == null)
            {
                return NotFound();
            }

            return Ok(playerPlayListLinkTable);
        }

        // PUT: api/PlayerPlayListLinkTables/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlayerPlayListLinkTable(int id, PlayerPlayListLinkTable playerPlayListLinkTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playerPlayListLinkTable.ID)
            {
                return BadRequest();
            }

            playerPlayListLinkTable.UpdateDate = DateTime.Now;
            db.Entry(playerPlayListLinkTable).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerPlayListLinkTableExists(id))
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

        // POST: api/PlayerPlayListLinkTables
        [ResponseType(typeof(PlayerPlayListLinkTable))]
        public async Task<IHttpActionResult> PostPlayerPlayListLinkTable(PlayerPlayListLinkTable playerPlayListLinkTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            playerPlayListLinkTable.UpdateDate = DateTime.Now;
            playerPlayListLinkTable.InsertDate = DateTime.Now;
            db.PlayerPlayListLinkTable.Add(playerPlayListLinkTable);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = playerPlayListLinkTable.ID }, playerPlayListLinkTable);
        }

        // DELETE: api/PlayerPlayListLinkTables/5
        [ResponseType(typeof(PlayerPlayListLinkTable))]
        public async Task<IHttpActionResult> DeletePlayerPlayListLinkTable(int id)
        {
            PlayerPlayListLinkTable playerPlayListLinkTable = await db.PlayerPlayListLinkTable.FindAsync(id);
            if (playerPlayListLinkTable == null)
            {
                return NotFound();
            }

            db.PlayerPlayListLinkTable.Remove(playerPlayListLinkTable);
            await db.SaveChangesAsync();

            return Ok(playerPlayListLinkTable);
        }

        [HttpPost, Route("api/PlayerPlayListLinkTables/DeletePlayerPlayListLinkTableByPlayerID/{PlayerID}")]
        public async Task<IHttpActionResult> DeletePlayerPlayListLinkTableByGroupID(int PlayerID)
        {
            List<PlayerPlayListLinkTable> ppltList = db.PlayerPlayListLinkTable.Where(a => a.PlayerID == PlayerID).ToList();
            foreach (PlayerPlayListLinkTable pplt in ppltList)
            {
                if (pplt == null)
                {
                    return NotFound();
                }

                db.PlayerPlayListLinkTable.Remove(pplt);
                await db.SaveChangesAsync();

            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerPlayListLinkTableExists(int id)
        {
            return db.PlayerPlayListLinkTable.Count(e => e.ID == id) > 0;
        }
    }
}