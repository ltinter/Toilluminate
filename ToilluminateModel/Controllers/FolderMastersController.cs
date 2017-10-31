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
    public class FolderMastersController : ApiController
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
            FolderMaster folderMaster = await db.FolderMaster.FindAsync(id);
            if (folderMaster == null)
            {
                return NotFound();
            }

            db.FolderMaster.Remove(folderMaster);
            await db.SaveChangesAsync();

            return Ok(folderMaster);
        }

        [HttpGet, Route("api/FolderMasters/GetJSTreeData/{GroupID}")]
        public async Task<IList<JSTreeDataModel>> GetJSTreeData(int GroupID)
        {
            List<JSTreeDataModel> jdmList = new List<JSTreeDataModel>();
            JSTreeDataModel jdm;
            foreach (FolderMaster fm in db.FolderMaster.Where(a=>a.GroupID == GroupID))
            {
                jdm = new JSTreeDataModel();
                jdm.id = fm.FolderID.ToString();
                jdm.text = fm.FolderName;
                jdm.parent = fm.FolderParentID == null ? "#" : fm.FolderParentID.ToString();
                jdm.li_attr = fm;
                jdmList.Add(jdm);
            }
            return jdmList;
        }

        [HttpPost, Route("api/FolderMasters/GetJSTreeNodeDataByCreate")]
        public async Task<JSTreeDataModel> GetJSTreeNodeDataByCreate(FolderMaster folderMaster)
        {

            folderMaster.UpdateDate = DateTime.Now;
            folderMaster.InsertDate = DateTime.Now;
            db.FolderMaster.Add(folderMaster);
            await db.SaveChangesAsync();
            JSTreeDataModel jdm = new JSTreeDataModel();
            jdm.id = folderMaster.FolderID.ToString();
            jdm.text = folderMaster.FolderName;
            jdm.parent = folderMaster.FolderParentID == null ? "#" : folderMaster.FolderParentID.ToString();
            //jdm.li_attr = fm;
            return jdm;
        }
        [HttpPost, Route("api/FolderMasters/EditTreeNodeFolder")]
        public async Task<JSTreeDataModel> EditTreeNodeFolder(FolderMaster folderMaster)
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
            JSTreeDataModel jdm = new JSTreeDataModel();
            jdm.id = folderMaster.FolderID.ToString();
            jdm.text = folderMaster.FolderName;
            jdm.parent = folderMaster.FolderParentID == null ? "#" : folderMaster.FolderParentID.ToString();
            //jdm.li_attr = fm;
            return jdm;
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