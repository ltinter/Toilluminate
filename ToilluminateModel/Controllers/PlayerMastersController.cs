using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using ToilluminateModel;

namespace ToilluminateModel.Controllers
{
    public class PlayerMastersController : BaseApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/PlayerMasters
        public IQueryable<PlayerMaster> GetPlayerMaster()
        {
            return db.PlayerMaster;
        }

        // GET: api/PlayerMasters/5
        [AllowAnonymous]
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
        public async Task<IHttpActionResult> PutPlayerMaster(PlayerMaster playerMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            playerMaster.UpdateDate = DateTime.Now;
            db.Entry(playerMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerMasterExists(playerMaster.PlayerID))
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

            playerMaster.UpdateDate = DateTime.Now;
            playerMaster.InsertDate = DateTime.Now;
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
        [HttpGet, Route("api/PlayerMasters/GetPlayerWithChildByGroupID/{GroupID}")]
        public async Task<IHttpActionResult> GetPlayerWithChildByGroupID(int GroupID)
        {
            List<int> GroupIDList = new List<int>();
            GroupIDList.Add(GroupID);
            PublicMethods.GetChildGroupIDs(GroupID, ref GroupIDList, db);
            int[] groupIDs = GroupIDList.ToArray<int>();
            var jsonList = (from pm in db.PlayerMaster
                           join gm in db.GroupMaster on pm.GroupID equals gm.GroupID into ProjectV
                           from pv in ProjectV.DefaultIfEmpty()
                           where groupIDs.Contains((int)pm.GroupID)
                           select new { pm.PlayerID,
                           pm.PlayerName,
                           pm.PlayerAddress,
                           pm.Settings,
                           pm.OnlineFlag,
                           pm.ActiveFlag,
                           pm.Comments,
                           pm.UpdateDate,
                           pv.GroupName,
                           pv.GroupID}).ToList();


            return Json(jsonList);
        }

        [HttpPost, Route("api/PlayerMasters/GetPlayerByGroupID/{GroupID}")]
        public async Task<IQueryable<PlayerMaster>> GetPlayerByGroupID(int GroupID)
        {
            return db.PlayerMaster.Where(a => a.GroupID == GroupID);
        }

        [AllowAnonymous]
        [HttpGet, Route("api/PlayerMasters/SendHeartBeatPkg/{PlayerID}")]
        public async Task<IHttpActionResult> SendHeartBeatPkg(int PlayerID)
        {
            Dictionary<int, string> playerHeartBeatDic = (Dictionary<int, string>)HttpContext.Current.Application["playerHeartBeat"];
            if (playerHeartBeatDic != null) {
                if (playerHeartBeatDic.Keys.Contains(PlayerID))
                {
                    playerHeartBeatDic[PlayerID] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    playerHeartBeatDic.Add(PlayerID, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                }
            }
            return Ok();
        }

        [HttpGet, Route("api/PlayerMasters/GetPlayerStatusReportData")]
        public async Task<IHttpActionResult> GetPlayerStatusReportData()
        {
            List<Models.PlayerStatusData> psdList = new List<Models.PlayerStatusData>();
            psdList.Add(new Models.PlayerStatusData("Online", 0));
            psdList.Add(new Models.PlayerStatusData("Error", 0));
            psdList.Add(new Models.PlayerStatusData("Lost", 0));
            psdList.Add(new Models.PlayerStatusData("Offline", 0));
            psdList.Add(new Models.PlayerStatusData("Total", 0));
            List<PlayerMaster> pmList = db.PlayerMaster.ToList();
            foreach (PlayerMaster pm in pmList)
            {
                //if (PublicMethods.isPlayerActive(pm, db))
                {
                    if (PublicMethods.isPlayerOnline(pm, db))
                    {
                        if (PublicMethods.isPlayerLost(pm.PlayerID, db))
                        {
                            psdList.Find(a => a.statusName == "Lost").counts++;
                        }
                        else
                        {
                            if (pm.ErrorFlag == "1")
                            {
                                psdList.Find(a => a.statusName == "Error").counts++;
                            }
                            else
                            {
                                psdList.Find(a => a.statusName == "Online").counts++;
                            }
                        }
                    }
                    else
                    {
                        psdList.Find(a => a.statusName == "Offline").counts++;
                    }
                    psdList.Find(a => a.statusName == "Total").counts++;
                }
            }
            return Json(psdList);
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