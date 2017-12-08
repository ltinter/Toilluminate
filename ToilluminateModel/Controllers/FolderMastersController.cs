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
using ToilluminateModel.Models;

namespace ToilluminateModel.Controllers
{
    public class FolderMastersController : BaseApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/FolderMasters
        public IQueryable<FolderMaster> GetFolderMaster()
        {
            return db.FolderMaster;
        }

        // GET: api/FolderMasters/5
        [ResponseType(typeof(FolderMaster))]
        public async Task<IHttpActionResult> GetFolderMaster(int id)
        {
            FolderMaster folderMaster = await db.FolderMaster.FindAsync(id);
            if (folderMaster == null)
            {
                return NotFound();
            }

            return Ok(folderMaster);
        }

        // PUT: api/FolderMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFolderMaster(int id, FolderMaster folderMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != folderMaster.FolderID)
            {
                return BadRequest();
            }

            folderMaster.UpdateDate = DateTime.Now;
            db.Entry(folderMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FolderMasterExists(id))
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

        // POST: api/FolderMasters
        [ResponseType(typeof(FolderMaster))]
        public async Task<IHttpActionResult> PostFolderMaster(FolderMaster folderMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            folderMaster.UpdateDate = DateTime.Now;
            folderMaster.InsertDate = DateTime.Now;
            db.FolderMaster.Add(folderMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = folderMaster.FolderID }, folderMaster);
        }

        // DELETE: api/FolderMasters/5
        [ResponseType(typeof(FolderMaster))]
        public async Task<IHttpActionResult> DeleteFolderMaster(int id)
        {
            if (db.FolderMaster.Where(a => a.FolderParentID == id).Count() > 0)
                return BadRequest("Can not delete folder with child.");
            if (db.FileMaster.Where(a => a.FolderID == id).Count() > 0)
                return BadRequest("Can not delete folder with files.");

            FolderMaster folderMaster = await db.FolderMaster.FindAsync(id);
            if (folderMaster == null)
            {
                return NotFound();
            }

            db.FolderMaster.Remove(folderMaster);
            await db.SaveChangesAsync();

            return Ok(folderMaster);
        }

        [HttpGet, Route("api/FolderMasters/GetFolderJSTreeDataByGroupID/{GroupID}")]
        public async Task<IList<DataModel>> GetFolderJSTreeDataByGroupID(int GroupID)
        {
            List<DataModel> jdmList = new List<DataModel>();
            DataModel jdm;
            List<FolderMaster> fmList = db.FolderMaster.Where(a => a.GroupID == GroupID).ToList();
            foreach (FolderMaster fm in fmList)
            {
                jdm = new DataModel();
                jdm.id = fm.FolderID.ToString();
                jdm.text = fm.FolderName;
                jdm.parent = fm.FolderParentID == null ? "#" : fm.FolderParentID.ToString();
                jdm.li_attr = fm;
                jdmList.Add(jdm);
            }
            return jdmList;
        }

        [HttpPost, Route("api/FolderMasters/GetJSTreeNodeDataByCreate")]
        public async Task<DataModel> GetJSTreeNodeDataByCreate(FolderMaster folderMaster)
        {

            folderMaster.UpdateDate = DateTime.Now;
            folderMaster.InsertDate = DateTime.Now;
            db.FolderMaster.Add(folderMaster);
            await db.SaveChangesAsync();
            DataModel jdm = new DataModel();
            jdm.id = folderMaster.FolderID.ToString();
            jdm.text = folderMaster.FolderName;
            jdm.parent = folderMaster.FolderParentID == null ? "#" : folderMaster.FolderParentID.ToString();
            //jdm.li_attr = fm;
            return jdm;
        }
        [HttpPost, Route("api/FolderMasters/EditTreeNodeFolder")]
        public async Task<DataModel> EditTreeNodeFolder(FolderMaster folderMaster)
        {

            folderMaster.UpdateDate = DateTime.Now;
            db.Entry(folderMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            DataModel jdm = new DataModel();
            jdm.id = folderMaster.FolderID.ToString();
            jdm.text = folderMaster.FolderName;
            jdm.parent = folderMaster.FolderParentID == null ? "#" : folderMaster.FolderParentID.ToString();
            //jdm.li_attr = fm;
            return jdm;
        }

        [HttpGet, Route("api/FolderMasters/GetFolderJSTreeNodeWithInheritForcedByGroupID/{GroupID}")]
        public async Task<IList<DataModel>> GetFolderJSTreeNodeWithInheritForcedByGroupID(int GroupID)
        {
            List<int> GroupIDList = new List<int>();
            GroupIDList.Add(GroupID);
            PublicMethods.GetParentGroupIDs(GroupID, ref GroupIDList, db);
            int[] groupIDs = GroupIDList.ToArray<int>();
            var jsonList = (from fm in db.FolderMaster
                            join gm in db.GroupMaster on fm.GroupID equals gm.GroupID into ProjectV
                            from pv in ProjectV.DefaultIfEmpty()
                            where groupIDs.Contains((int)fm.GroupID)
                            select new
                            {
                                fm,
                                pv.GroupName,
                                pv.GroupID
                            }).ToList();


            List<DataModel> jdmList = new List<DataModel>();
            DataModel jdm;
            StateForJsonModel sfjm = new StateForJsonModel();
            sfjm.opened = false;
            foreach (var item in jsonList)
            {
                jdm = new DataModel();
                jdm.id = item.fm.FolderID.ToString();
                jdm.text = item.fm.FolderName;
                jdm.parent = item.fm.FolderParentID == null ? "#" : item.fm.FolderParentID.ToString();
                jdm.state = sfjm;
                jdm.li_attr = item;
                jdmList.Add(jdm);
            }
            return jdmList;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FolderMasterExists(int id)
        {
            return db.FolderMaster.Count(e => e.FolderID == id) > 0;
        }
    }
}