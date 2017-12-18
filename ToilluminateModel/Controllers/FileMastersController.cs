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
    public class FileMastersController : BaseApiController
    {
        private ToilluminateEntities db = new ToilluminateEntities();

        // GET: api/FileMasters
        public IQueryable<FileMaster> GetFileMaster()
        {
            return db.FileMaster.Where(a=>a.UseFlag == true);
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

        // PUT: api/FileMasters
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFileMaster(FileMaster fileMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            fileMaster.UpdateDate = DateTime.Now;
            db.Entry(fileMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(fileMaster);
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
            fileMaster.UseFlag = true;
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
            return db.FileMaster.Where(a => a.FolderID == FolderID && a.UseFlag == true);
        }

        [HttpPost, Route("api/FileMasters/GetFilesByFolderIDArray")]
        public async Task<IHttpActionResult> GetFilesByFolderIDArray(string [] folderIDs)
        {

            var jsonList = (from fm in db.FileMaster
                            join gm in db.GroupMaster on fm.GroupID equals gm.GroupID into ProjectV
                            from pv in ProjectV.DefaultIfEmpty()
                            where folderIDs.Contains(fm.FolderID.ToString()) && fm.UseFlag == true
                            select new
                            {
                                fm.FolderID,
                                fm.FileName,
                                fm.FileID,
                                fm.FileExtension,
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
            return db.FileMaster.Where(a => fileIDs.Contains(a.FileID.ToString()) && a.UseFlag == true);
        }

        [HttpPost, Route("api/FileMasters/CutFile/{folderID}")]
        public async Task<IHttpActionResult> CutFile(int folderID, FileMaster fileMaster)
        {
            try
            {
                FolderMaster folderMaster = await db.FolderMaster.FindAsync(folderID);
                if (!(bool)folderMaster.UseFlag) { return NotFound(); }

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
        public async Task<IHttpActionResult> CopyFile(int folderID, FileMaster fileMaster)
        {
            try
            {
                FolderMaster folderMaster = await db.FolderMaster.FindAsync(folderID);
                if (!(bool)folderMaster.UseFlag) { return NotFound(); }
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
                string filePathName = fileNameKey + fileMaster.FileExtension;
                string thumbnailFilePathName = filePathName.ToLower().Replace(".mp4", ".gif");
                File.Copy(Path.Combine(ROOT_PATH + fileMaster.FileUrl), Path.Combine(dirFilePath, filePathName), true);
                File.Copy(Path.Combine(ROOT_PATH + fileMaster.FileThumbnailUrl), Path.Combine(dirThumbnailFilePath, thumbnailFilePathName), true);

                FileMaster newFile = new FileMaster();
                newFile.GroupID = fileMaster.GroupID;
                newFile.FolderID = folderID;
                newFile.UserID = fileMaster.UserID;
                newFile.FileExtension = fileMaster.FileExtension;
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
                int userID = int.Parse(HttpContext.Current.Request["UserID"]);
                int groupID = int.Parse(HttpContext.Current.Request["GroupID"]);
                int folderID = int.Parse(HttpContext.Current.Request["FolderID"]);
                FolderMaster folderMaster = await db.FolderMaster.FindAsync(folderID);
                GroupMaster groupMaster = await db.GroupMaster.FindAsync(groupID);
                if (!(bool)folderMaster.UseFlag || !(bool)groupMaster.UseFlag) { return NotFound(); }

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
                string fileExtension = fileInfo.Extension.ToLower();
                string fileType = "";
                string fileNameKey = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string filePathName = fileNameKey + fileExtension;
                string thumbnailFilePathName = filePathName;
                string filePath = Path.Combine(dirFilePath, filePathName);
                string thumbnailFilePath = Path.Combine(dirThumbnailFilePath, thumbnailFilePathName);
                if (fileExtension == ".mp4" ||
                    fileExtension == ".wmv" ||
                    fileExtension == ".mpg" ||
                    fileExtension == ".avi" ||
                    fileExtension == ".mpeg" ||
                    fileExtension == ".flv" ||
                    fileExtension == ".mkv" ||
                    fileExtension == ".mov") {
                    // save video file
                    fileType = "video";
                    file.SaveAs(filePath);
                    // save gif from mp4
                    string ffmpeg = Path.Combine(ROOT_PATH, "bin/ffmpeg.exe");
                    if (!System.IO.File.Exists(ffmpeg))
                    {
                        return BadRequest();
                    }
                    thumbnailFilePathName = thumbnailFilePathName.ToLower().Replace(fileExtension, ".gif");
                    Process pcs = new Process();
                    pcs.StartInfo.FileName = ffmpeg;
                    pcs.StartInfo.Arguments = " -i " + filePath + " -ss 00:00:00.000 -pix_fmt rgb24 -r 10 -s 320x240 -t 00:00:10.000 " + thumbnailFilePath.Replace(fileExtension, ".gif");
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
                    fileType = "image";
                    Image originalImg = Image.FromStream(file.InputStream);
                    int thumbnailMaxWidth = 200;
                    int thumbnailMaxHeight = 150;
                    Image thumbnailImg;
                    if (originalImg.Width > thumbnailMaxWidth && originalImg.Height <= thumbnailMaxHeight)//宽度比目的图片宽度大，长度比目的图片长度小
                    {
                        thumbnailImg = ImageHelper.GetThumbnailImage(originalImg, thumbnailMaxWidth, (thumbnailMaxWidth * originalImg.Height) / originalImg.Width);
                    }
                    else if (originalImg.Width <= thumbnailMaxWidth && originalImg.Height > thumbnailMaxHeight)//宽度比目的图片宽度小，长度比目的图片长度大
                    {
                        thumbnailImg = ImageHelper.GetThumbnailImage(originalImg, (thumbnailMaxHeight * originalImg.Width) / originalImg.Height, thumbnailMaxHeight);
                    }
                    else if (originalImg.Width <= thumbnailMaxWidth && originalImg.Height <= thumbnailMaxHeight) //长宽比目的图片长宽都小
                    {
                        thumbnailImg = ImageHelper.GetThumbnailImage(originalImg, originalImg.Width, originalImg.Height);
                    }
                    else
                    {
                        if ((thumbnailMaxWidth * originalImg.Height) / originalImg.Width > thumbnailMaxHeight)
                        {
                            thumbnailImg = ImageHelper.GetThumbnailImage(originalImg, (thumbnailMaxHeight * originalImg.Width) / originalImg.Height, thumbnailMaxHeight);
                        }
                        else
                        {
                            thumbnailImg = ImageHelper.GetThumbnailImage(originalImg, thumbnailMaxWidth, (thumbnailMaxWidth * originalImg.Height) / originalImg.Width);
                        }
                    }

                    originalImg.Save(filePath);
                    thumbnailImg.Save(thumbnailFilePath);
                }

                FileMaster fileMaster = new FileMaster();
                fileMaster.GroupID = groupID;
                fileMaster.FolderID = folderID;
                fileMaster.UserID = userID;
                fileMaster.FileExtension = fileExtension;
                fileMaster.FileType = fileType;
                fileMaster.FileName = fileName;
                fileMaster.FileUrl = FORLDER  + filePathName;
                fileMaster.FileThumbnailUrl = THUMBNAILFORLDER + thumbnailFilePathName;
                fileMaster.UseFlag = true;
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
            return db.FileMaster.Count(e => e.FileID == id && e.UseFlag == true) > 0;
        }
    }
}