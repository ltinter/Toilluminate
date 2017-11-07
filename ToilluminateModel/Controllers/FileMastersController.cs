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

        [HttpGet, Route("api/FileMasters/GetFilesByFolderID/{FolderID}")]
        public async Task<IQueryable<FileMaster>> GetFilesByFolderID(int FolderID)
        {
            return db.FileMaster.Where(a => a.FolderID == FolderID);
        }

        [HttpPost, Route("api/FileMasters/CutFile/{folderID}")]
        public async Task<IHttpActionResult> CutFile(int folderID, FileMaster fileMaster)
        {
            try
            {
                fileMaster.FolderID = folderID;
                fileMaster.UpdateDate = DateTime.Now;
                db.Entry(fileMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Ok(fileMaster);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        [HttpPost, Route("api/FileMasters/CopyFile/{folderID}")]
        public async Task<IHttpActionResult> CopyFile(int folderID,FileMaster fileMaster)
        {
            try
            {
                // save original file path
                string dirFilePath = Path.Combine(ROOT_PATH + FORLDER);
                if (!Directory.Exists(dirFilePath))
                {
                    Directory.CreateDirectory(dirFilePath);
                }

                //save thumbnail file path
                string dirThumbnailFilePath = Path.Combine(ROOT_PATH + THUMBNAILFORLDER);
                if (!Directory.Exists(dirThumbnailFilePath))
                {
                    Directory.CreateDirectory(dirThumbnailFilePath);
                }

                string fileNameKey = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileMaster.FileName;

                File.Copy(Path.Combine(ROOT_PATH + fileMaster.FileUrl), Path.Combine(dirFilePath, fileNameKey), true);
                File.Copy(Path.Combine(ROOT_PATH + fileMaster.FileThumbnailUrl), Path.Combine(dirThumbnailFilePath, fileNameKey), true);

                FileMaster newFile = new FileMaster();
                newFile.FolderID = folderID;
                newFile.UserID = fileMaster.UserID;
                newFile.FileType = fileMaster.FileType;
                newFile.FileName = fileMaster.FileName;
                newFile.FileUrl = FORLDER + "/" + fileNameKey;
                newFile.FileThumbnailUrl = THUMBNAILFORLDER + "/" + fileNameKey;
                newFile.UpdateDate = DateTime.Now;
                newFile.InsertDate = DateTime.Now;
                db.FileMaster.Add(newFile);
                await db.SaveChangesAsync();

                return Ok(newFile);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost, Route("api/FileMasters/UploadFile")]
        public async Task<IHttpActionResult> UploadFile()
        {
            try
            {
                // save original file path
                string dirFilePath = Path.Combine(ROOT_PATH + FORLDER);
                if (!Directory.Exists(dirFilePath))
                {
                    Directory.CreateDirectory(dirFilePath);
                }

                //save thumbnail file path
                string dirThumbnailFilePath = Path.Combine(ROOT_PATH + THUMBNAILFORLDER);
                if (!Directory.Exists(dirThumbnailFilePath))
                {
                    Directory.CreateDirectory(dirThumbnailFilePath);
                }

                List<FileMaster> fileReturnList = new List<FileMaster>();
                int fileCount = HttpContext.Current.Request.Files.Count;
                HttpFileCollection fileUploadData = HttpContext.Current.Request.Files;
                HttpPostedFile file = fileUploadData[0];
                string fileName = file.FileName.Trim('"');
                string fileNameKey = DateTime.Now.ToString("yyyyMMddHHmmssffff") + file.FileName.Trim('"');
                Image originalImg = Image.FromStream(file.InputStream);
                Image thumbnailImg = ImageHelper.GetThumbnailImage(originalImg, originalImg.Width / 10, originalImg.Height / 10);
                string[] fileNameSplit = fileName.Split('.');
                string filePath = Path.Combine(dirFilePath, fileNameKey);
                string thumbnailFilePath = Path.Combine(dirThumbnailFilePath, fileNameKey);
                originalImg.Save(filePath);
                thumbnailImg.Save(thumbnailFilePath);

                FileMaster fileMaster = new FileMaster();
                fileMaster.FolderID = int.Parse(HttpContext.Current.Request["FolderID"]);
                fileMaster.UserID = int.Parse(HttpContext.Current.Request["UserID"]);
                fileMaster.FileType = fileNameSplit.Length > 0 ? fileNameSplit[fileNameSplit.Length - 1] : "";
                fileMaster.FileName = fileName;
                fileMaster.FileUrl = FORLDER + "/" + fileNameKey;
                fileMaster.FileThumbnailUrl = THUMBNAILFORLDER + "/" + fileNameKey;
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