using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using ToilluminateModel;

namespace ToilluminateModel.Controllers
{
    public class FileMastersController : ApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();
        private static readonly string FORLDER = "/FileResource/" + DateTime.Now.ToString("yyyyMMdd") + "/";
        private static readonly string THUMBNAILFORLDER = FORLDER + "/Thumbnail/";
        private static readonly string ROOT_PATH = HttpContext.Current.Server.MapPath("~/");

        // GET: api/FileMasters
        public IQueryable<FileMaster> GetFileMaster()
        {
            return db.FileMaster;
        }

        // GET: api/FileMasters/5
        [ResponseType(typeof(FileMaster))]
        public async Task<IHttpActionResult> GetFileMaster(int id)
        {
            FileMaster fileMaster = await db.FileMaster.FindAsync(id);
            if (fileMaster == null)
            {
                return NotFound();
            }

            return Ok(fileMaster);
        }

        // PUT: api/FileMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFileMaster(int id, FileMaster fileMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fileMaster.FileID)
            {
                return BadRequest();
            }

            fileMaster.UpdateDate = DateTime.Now;
            db.Entry(fileMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileMasterExists(id))
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

        // POST: api/FileMasters
        [ResponseType(typeof(FileMaster))]
        public async Task<IHttpActionResult> PostFileMaster(FileMaster fileMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            fileMaster.UpdateDate = DateTime.Now;
            fileMaster.InsertDate = DateTime.Now;
            db.FileMaster.Add(fileMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = fileMaster.FileID }, fileMaster);
        }

        // DELETE: api/FileMasters/5
        [ResponseType(typeof(FileMaster))]
        public async Task<IHttpActionResult> DeleteFileMaster(int id)
        {
            FileMaster fileMaster = await db.FileMaster.FindAsync(id);
            if (fileMaster == null)
            {
                return NotFound();
            }

            File.Delete(ROOT_PATH + fileMaster.FileUrl);
            File.Delete(ROOT_PATH + fileMaster.FileThumbnailUrl);
            db.FileMaster.Remove(fileMaster);
            await db.SaveChangesAsync();

            return Ok(fileMaster);
        }

        [HttpPost, Route("api/FileMasters/UploadFile")]
        public async Task<IHttpActionResult> UploadFile()
        {
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                // extract file name and file contents
                var fileNameParam = provider.Contents[0].Headers.ContentDisposition.Parameters
                    .FirstOrDefault(p => p.Name.ToLower() == "filename");
                string fileName = (fileNameParam == null) ? "" : (DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileNameParam.Value.Trim('"'));
                byte[] file = await provider.Contents[0].ReadAsByteArrayAsync();
                Image originalImg = ImageHelper.BytesToImage(file);
                Image thumbnailImg = ImageHelper.GetThumbnailImage(originalImg, originalImg.Width / 10, originalImg.Height / 10);
                byte[] thumbnailFile = ImageHelper.ImageToBytes(thumbnailImg);

                string[] fileNameSplit = fileName.Split('.');

                // save original file path
                string dirFilePath = Path.Combine(ROOT_PATH + FORLDER);
                if (!Directory.Exists(dirFilePath))
                {
                    Directory.CreateDirectory(dirFilePath);
                }
                string filePath = Path.Combine(dirFilePath, fileName);
                File.WriteAllBytes(filePath, file);
                //save thumbnail file path
                string dirThumbnailFilePath = Path.Combine(ROOT_PATH + THUMBNAILFORLDER);
                if (!Directory.Exists(dirThumbnailFilePath))
                {
                    Directory.CreateDirectory(dirThumbnailFilePath);
                }
                string thumbnailFilePath = Path.Combine(dirThumbnailFilePath, fileName);
                File.WriteAllBytes(thumbnailFilePath, thumbnailFile);


                FileMaster fileMaster = new FileMaster();
                fileMaster.FolderID = int.Parse(HttpContext.Current.Request["FolderID"]);
                fileMaster.UserID = int.Parse(HttpContext.Current.Request["UserID"]);
                fileMaster.FileType = fileNameSplit.Length > 0 ? fileNameSplit[fileNameSplit.Length - 1] : "";
                fileMaster.FileName = fileName;
                fileMaster.FileUrl = FORLDER + "/" + fileName;
                fileMaster.FileThumbnailUrl = THUMBNAILFORLDER + "/" + fileName;
                fileMaster.UpdateDate = DateTime.Now;
                fileMaster.InsertDate = DateTime.Now;
                db.FileMaster.Add(fileMaster);
                await db.SaveChangesAsync();

                return Ok(fileMaster);
            }
            catch
            {
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FileMasterExists(int id)
        {
            return db.FileMaster.Count(e => e.FileID == id) > 0;
        }
    }
}