﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
    public class GroupMastersController : ApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/GroupMasters
        public IQueryable<GroupMaster> GetGroupMaster()
        {
            return db.GroupMaster;
        }

        // GET: api/GroupMasters/5
        [ResponseType(typeof(GroupMaster))]
        public async Task<IHttpActionResult> GetGroupMaster(int id)
        {
            GroupMaster groupMaster = await db.GroupMaster.FindAsync(id);
            if (groupMaster == null)
            {
                return NotFound();
            }

            return Ok(groupMaster);
        }

        // PUT: api/GroupMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutGroupMaster(int id, GroupMaster groupMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groupMaster.GroupID)
            {
                return BadRequest();
            }

            db.Entry(groupMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupMasterExists(id))
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

        // POST: api/GroupMasters
        [ResponseType(typeof(GroupMaster))]
        public async Task<IHttpActionResult> PostGroupMaster(GroupMaster groupMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GroupMaster.Add(groupMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = groupMaster.GroupID }, groupMaster);
        }

        // DELETE: api/GroupMasters/5
        [ResponseType(typeof(GroupMaster))]
        public async Task<IHttpActionResult> DeleteGroupMaster(int id)
        {
            GroupMaster groupMaster = await db.GroupMaster.FindAsync(id);
            if (groupMaster == null)
            {
                return NotFound();
            }

            db.GroupMaster.Remove(groupMaster);
            await db.SaveChangesAsync();

            return Ok(groupMaster);
        }

        [HttpGet, Route("api/GroupMasters/GetJSTreeData")]
        public IList<JSTreeDataModel> GetJSTreeData()
        {
            List<JSTreeDataModel> jdmList = new List<JSTreeDataModel>();
            JSTreeDataModel jdm;
            foreach (GroupMaster gm in db.GroupMaster) {
                jdm = new JSTreeDataModel();
                jdm.id = gm.GroupID.ToString();
                jdm.text = gm.GroupName;
                jdm.parent = gm.GroupParentID == null?"#":gm.GroupParentID.ToString();
                jdm.li_attr = gm;
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

        private bool GroupMasterExists(int id)
        {
            return db.GroupMaster.Count(e => e.GroupID == id) > 0;
        }
    }
}