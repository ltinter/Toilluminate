﻿using System;
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
    public class PlayerMastersController : ApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/PlayerMasters
        public IQueryable<PlayerMaster> GetPlayerMaster()
        {
            return db.PlayerMaster;
        }

        // GET: api/PlayerMasters/5
        [ResponseType(typeof(PlayerMaster))]
        public async Task<IHttpActionResult> GetPlayerMaster(int id)
        {
            PlayerMaster playerMaster = await db.PlayerMaster.FindAsync(id);
            if (playerMaster == null)
            {
                return NotFound();
            }

            return Ok(playerMaster);
        }

        // PUT: api/PlayerMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPlayerMaster(int id, PlayerMaster playerMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playerMaster.PlayerID)
            {
                return BadRequest();
            }

            db.Entry(playerMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerMasterExists(id))
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

        // POST: api/PlayerMasters
        [ResponseType(typeof(PlayerMaster))]
        public async Task<IHttpActionResult> PostPlayerMaster(PlayerMaster playerMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PlayerMaster.Add(playerMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = playerMaster.PlayerID }, playerMaster);
        }

        // DELETE: api/PlayerMasters/5
        [ResponseType(typeof(PlayerMaster))]
        public async Task<IHttpActionResult> DeletePlayerMaster(int id)
        {
            PlayerMaster playerMaster = await db.PlayerMaster.FindAsync(id);
            if (playerMaster == null)
            {
                return NotFound();
            }

            db.PlayerMaster.Remove(playerMaster);
            await db.SaveChangesAsync();

            return Ok(playerMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerMasterExists(int id)
        {
            return db.PlayerMaster.Count(e => e.PlayerID == id) > 0;
        }
    }
}