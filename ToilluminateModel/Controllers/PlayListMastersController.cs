using System;
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
using ToilluminateModel.Models;

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

        //[HttpPost, Route("api/PlayListMasters/GetOwnPlayListByPlayerID/{PlayerID}")]
        //public async Task<IHttpActionResult> GetOwnPlayListByPlayerID(int PlayerID)
        //{

        //    var jsonList = (from plm in db.PlayListMaster
        //                    join pplt in db.PlayerPlayListLinkTable on plm.PlayListID equals pplt.PlayListID
        //                    select new
        //                    {
        //                        plm.PlayListID,
        //                        plm.PlayListName,
        //                        plm.Settings,
        //                        plm.UpdateDate,
        //                        plm.GroupID
        //                    }).ToList();
        //    return Json(jsonList);
        //}


        [HttpPost, Route("api/PlayListMasters/GetForcedPlayListByGroupID/{GroupID}")]
        public async Task<List<PlayListLinkData>> GetForcedPlayListByGroupID(int GroupID)
        {
            List<int> GroupIDList = new List<int>();
            GroupIDList.Add(GroupID);
            PublicMethods.GetParentGroupIDs(GroupID, ref GroupIDList, db);
            int[] groupIDs = GroupIDList.ToArray<int>();
            string groupIDsStr = string.Join(",", groupIDs.Select(i => i.ToString()).ToArray());
            List<PlayListLinkData> pldList = (from plm in db.PlayListMaster
                                              join gplt in db.GroupPlayListLinkTable on plm.PlayListID equals gplt.PlayListID
                                              join gm in db.GroupMaster on plm.GroupID equals gm.GroupID into ProjectV
                                              from pv in ProjectV.DefaultIfEmpty()
                                              where groupIDs.Contains((int)gplt.GroupID)
                                              orderby gplt.GroupID == GroupID ? 2 : 1, groupIDsStr.IndexOf(gplt.GroupID.ToString()) descending,gplt.Index 
                                              select new PlayListLinkData
                                              {
                                                  PlayListID = plm.PlayListID,
                                                  PlayListName = plm.PlayListName,
                                                  Settings = plm.Settings,
                                                  UpdateDate = (DateTime)plm.UpdateDate,
                                                  GroupID = (int)plm.GroupID,
                                                  GroupName = pv.GroupName,
                                                  BindGroupID = (int)gplt.GroupID
                                              }).ToList();
            return pldList;
        }


        [HttpPost, Route("api/PlayListMasters/GetTotalPlayListByPlayerID/{PlayerID}")]
        public async Task<List<PlayListLinkData>> GetTotalPlayListByPlayerID(int PlayerID)
        {
            PlayerMaster pm = db.PlayerMaster.Find(PlayerID);

            List<int> GroupIDList = new List<int>();
            GroupIDList.Add((int)pm.GroupID);
            PublicMethods.GetParentGroupIDs((int)pm.GroupID, ref GroupIDList, db);
            int[] groupIDs = GroupIDList.ToArray<int>();
            string groupIDsStr = string.Join(",", groupIDs.Select(i => i.ToString()).ToArray());
            List<PlayListLinkData> pldList = (from plm in db.PlayListMaster
                                              join gplt in db.GroupPlayListLinkTable on plm.PlayListID equals gplt.PlayListID
                                              join gm in db.GroupMaster on plm.GroupID equals gm.GroupID into ProjectV
                                              from pv in ProjectV.DefaultIfEmpty()
                                              where groupIDs.Contains((int)gplt.GroupID)
                                              orderby groupIDsStr.IndexOf(gplt.GroupID.ToString()) descending, gplt.Index
                                              select new PlayListLinkData
                                              {
                                                  PlayListID = plm.PlayListID,
                                                  PlayListName = plm.PlayListName,
                                                  Settings = plm.Settings,
                                                  UpdateDate = (DateTime)plm.UpdateDate,
                                                  GroupID = (int)plm.GroupID,
                                                  GroupName = pv.GroupName,
                                                  BindGroupID = (int)gplt.GroupID,
                                                  Index = (int)gplt.Index
                                              }).ToList();

            pldList.AddRange((from plm in db.PlayListMaster
                              join pplt in db.PlayerPlayListLinkTable on plm.PlayListID equals pplt.PlayListID
                              join gm in db.GroupMaster on plm.GroupID equals gm.GroupID into ProjectV
                              from pv in ProjectV.DefaultIfEmpty()
                              where pplt.PlayerID == PlayerID
                              orderby pplt.Index
                              select new PlayListLinkData
                              {
                                  PlayListID = plm.PlayListID,
                                  PlayListName = plm.PlayListName,
                                  Settings = plm.Settings,
                                  UpdateDate = (DateTime)plm.UpdateDate,
                                  GroupID = (int)plm.GroupID,
                                  GroupName = pv.GroupName,
                                  Index = (int)pplt.Index
                              }).ToList());
            return pldList;
        }

        [HttpGet, Route("api/PlayListMasters/GetOwnPlayListWithInheritByGroupID/{GroupID}")]
        public async Task<List<PlayListLinkData>> GetOwnPlayListWithInheritByGroupID(int GroupID)
        {
            List<int> GroupIDList = new List<int>();
            GroupIDList.Add(GroupID);
            PublicMethods.GetParentGroupIDs(GroupID, ref GroupIDList, db);
            int[] groupIDs = GroupIDList.ToArray<int>();
            List<PlayListLinkData> pldList = (from plm in db.PlayListMaster
                            join gm in db.GroupMaster on plm.GroupID equals gm.GroupID into ProjectV
                            from pv in ProjectV.DefaultIfEmpty()
                            where groupIDs.Contains((int)plm.GroupID)
                            select new PlayListLinkData
                            {
                                PlayListID = plm.PlayListID,
                                PlayListName = plm.PlayListName,
                                Settings = plm.Settings,
                                UpdateDate = (DateTime)plm.UpdateDate,
                                GroupID = (int)plm.GroupID,
                                GroupName = pv.GroupName
                            }).ToList();
            return pldList;
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