﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
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

            playListMaster.UpdateDate = DateTime.Now;
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

            playListMaster.UpdateDate = DateTime.Now;
            playListMaster.InsertDate = DateTime.Now;
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


        [HttpPost, Route("api/PlayListMasters/GetPlayListByGroupID/{GroupID}")]
        public async Task<IQueryable<PlayListMaster>> GetPlayListByGroupID(int GroupID)
        {
            return db.PlayListMaster.Where(a=>a.GroupID == GroupID);
        }
        
        [HttpPost, Route("api/PlayListMasters/GetOwnPlayListByPlayerID/{PlayerID}")]
        public async Task<IQueryable<PlayListMaster>> GetOwnPlayListByPlayerID(int PlayerID)
        {
            return db.PlayListMaster
                    .Where(q => db.PlayerPlayListLinkTable
                    .Where(s => s.PlayListID == q.PlayListID && s.PlayerID == PlayerID).Count() > 0);
        }


        [HttpPost, Route("api/PlayListMasters/GetForcedPlayListByGroupID/{GroupID}")]
        public async Task<IQueryable<PlayListMaster>> GetForcedPlayListByGroupID(int GroupID)
        {
            List<int> GroupIDList = new List<int>();
            GroupIDList.Add(GroupID);
            PublicMethods.GetParentGroupIDs(GroupID, ref GroupIDList, db);
            int[] groupIDs = GroupIDList.ToArray<int>();
            return db.PlayListMaster
                    .Where(q => db.GroupPlayListLinkTable
                    .Where(s => s.PlayListID == q.PlayListID && groupIDs.Contains((int)s.GroupID)).Count() > 0);
        }


        [HttpPost, Route("api/PlayListMasters/GetTotalPlayListByPlayerID/{PlayerID}")]
        public async Task<List<PlayListMaster>> GetTotalPlayListByPlayerID(int PlayerID)
        {
            PlayerMaster pm = db.PlayerMaster.Find(PlayerID);
            List<PlayListMaster> pmList = db.PlayListMaster
                    .Where(q => db.PlayerPlayListLinkTable
                    .Where(s => s.PlayListID == q.PlayListID && s.PlayerID == PlayerID).Count() > 0).ToList();

            List<int> GroupIDList = new List<int>();
            GroupIDList.Add((int)pm.GroupID);
            PublicMethods.GetParentGroupIDs((int)pm.GroupID, ref GroupIDList, db);
            int[] groupIDs = GroupIDList.ToArray<int>();
            pmList.AddRange(db.PlayListMaster
                    .Where(q => db.GroupPlayListLinkTable
                    .Where(s => s.PlayListID == q.PlayListID && groupIDs.Contains((int)s.GroupID)).Count() > 0).ToList());
            return pmList;
        }

        [HttpGet, Route("api/PlayListMasters/GetOwnPlayListWithInheritByGroupID/{GroupID}")]
        public async Task<IHttpActionResult> GetOwnPlayListWithInheritByGroupID(int GroupID)
        {
            List<int> GroupIDList = new List<int>();
            GroupIDList.Add(GroupID);
            PublicMethods.GetParentGroupIDs(GroupID, ref GroupIDList, db);
            int[] groupIDs = GroupIDList.ToArray<int>();
            var jsonList = (from plm in db.PlayListMaster
                            join gm in db.GroupMaster on plm.GroupID equals gm.GroupID into ProjectV
                            from pv in ProjectV.DefaultIfEmpty()
                            where groupIDs.Contains((int)plm.GroupID)
                            select new
                            {
                                plm.PlayListID,
                                plm.PlayListName,
                                plm.Settings,
                                plm.UpdateDate,
                                pv.GroupName,
                                pv.GroupID
                            }).ToList();
            return Json(jsonList);
        }

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