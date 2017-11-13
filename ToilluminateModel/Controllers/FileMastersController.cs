using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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

        [HttpPost, Route("api/FileMasters/GetFilesByFolderIDArray")]
        public async Task<IHttpActionResult> GetFilesByFolderIDArray(string [] folderIDs)
        {

            var jsonList = (from fm in db.FileMaster
                            join gm in db.GroupMaster on fm.GroupID equals gm.GroupID into ProjectV
                            from pv in ProjectV.DefaultIfEmpty()
                            where folderIDs.Contains(fm.FolderID.ToString())
                            select new
                            {
                                fm.FolderID,
                                fm.FileName,
                                fm.FileID,
                                fm.FileType,
                                fm.FileUrl,
                                fm.FileThumbnailUrl,
                                fm.Comments,
                                pv.GroupName,
                                pv.GroupID
                            }).ToList();

            return Json(jsonList);
        }

        [HttpPost, Route("api/FileMasters/GetFilesByFileIDArray")]
        public async Task<IQueryable<FileMaster>> GetFilesByFileIDArray(string[] fileIDs)
        {
            return db.FileMaster.Where(a => fileIDs.Contains(a.FileID.ToString()));
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

        private static readonly string FORLDER = "/FileResource/" + DateTime.Now.ToString("yyyyMMdd") + "/";
        private static readonly string THUMBNAILFORLDER = FORLDER + "Thumbnail/";
        private static readonly string ROOT_PATH = HttpContext.Current.Server.MapPath("~/");
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

                string fileNameKey = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string filePathName = fileNameKey + fileMaster.FileType;
                string thumbnailFilePathName = filePathName.ToLower().Replace(".mp4",".gif");
                File.Copy(Path.Combine(ROOT_PATH + fileMaster.FileUrl), Path.Combine(dirFilePath, filePathName), true);
                File.Copy(Path.Combine(ROOT_PATH + fileMaster.FileThumbnailUrl), Path.Combine(dirThumbnailFilePath, thumbnailFilePathName), true);

                FileMaster newFile = new FileMaster();
                newFile.GroupID = fileMaster.GroupID;
                newFile.FolderID = folderID;
                newFile.UserID = fileMaster.UserID;
                newFile.FileType = fileMaster.FileType;
                newFile.FileName = fileMaster.FileName;
                newFile.FileUrl = FORLDER + filePathName;
                newFile.FileThumbnailUrl = THUMBNAILFORLDER + thumbnailFilePathName;
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

                HttpFileCollection fileUploadData = HttpContext.Current.Request.Files;
                HttpPostedFile file = fileUploadData[0];
                string fileName = file.FileName.Trim('"');
                FileInfo fileInfo = new FileInfo(fileName);
                string fileType = fileInfo.Extension;
                string fileNameKey = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string filePathName = fileNameKey + fileType;
                string thumbnailFilePathName = filePathName;
                string filePath = Path.Combine(dirFilePath, filePathName);
                string thumbnailFilePath = Path.Combine(dirThumbnailFilePath, thumbnailFilePathName);
                if (fileType == ".mp4") {
                    // save mp4 file
                    file.SaveAs(filePath);
                    // save gif from mp4
                    string ffmpeg = Path.Combine(ROOT_PATH, "bin/ffmpeg.exe");
                    if (!System.IO.File.Exists(ffmpeg))
                    {
                        return BadRequest();
                    }
                    thumbnailFilePathName = thumbnailFilePathName.ToLower().Replace(".mp4", ".gif");
                    Process pcs = new Process();
                    pcs.StartInfo.FileName = ffmpeg;
                    pcs.StartInfo.Arguments = " -i " + filePath + " -ss 00:00:00.000 -pix_fmt rgb24 -r 10 -s 320x240 -t 00:00:10.000 " + thumbnailFilePath.Replace(".mp4",".gif");
                    pcs.StartInfo.UseShellExecute = false;
                    pcs.StartInfo.RedirectStandardError = true;
                    pcs.StartInfo.CreateNoWindow = false;
                    try
                    {
                        pcs.Start();
                        pcs.BeginErrorReadLine();
                        pcs.WaitForExit();
                    }
                    catch
                    {
                        return BadRequest();
                    }
                    finally
                    {
                        pcs.Close();
                        pcs.Dispose();

                    }
                }
                else {
                    // save image file
                    Image originalImg = Image.FromStream(file.InputStream);
                    Image thumbnailImg = ImageHelper.GetThumbnailImage(originalImg, originalImg.Width / 10, originalImg.Height / 10);
                    originalImg.Save(filePath);
                    thumbnailImg.Save(thumbnailFilePath);
                }

                FileMaster fileMaster = new FileMaster();
                fileMaster.GroupID = int.Parse(HttpContext.Current.Request["GroupID"]);
                fileMaster.FolderID = int.Parse(HttpContext.Current.Request["FolderID"]);
                fileMaster.UserID = int.Parse(HttpContext.Current.Request["UserID"]);
                fileMaster.FileType = fileType;
                fileMaster.FileName = fileName;
                fileMaster.FileUrl = FORLDER  + filePathName;
                fileMaster.FileThumbnailUrl = THUMBNAILFORLDER + thumbnailFilePathName;
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