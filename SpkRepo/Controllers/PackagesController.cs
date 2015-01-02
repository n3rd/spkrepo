using ICSharpCode.SharpZipLib.Tar;
using PeanutButter.INI;
using SpkRepo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;

namespace SpkRepo.Controllers
{
    public class PackagesController : ApiController
    {

        private static string PackageDir
        {
            get
            {
                string dir;

                if(HostingEnvironment.MapPath("~/App_Data/") != null)
                {
                    dir = HostingEnvironment.MapPath("~/App_Data/");
                }
                else
                {
                    dir = ConfigurationManager.AppSettings["SpkRepo:packageDir"];

                    if (string.IsNullOrEmpty(dir))
                        throw new ArgumentNullException("SpkRepo:packageDir appSetting");

                    if (!Path.IsPathRooted(dir))
                        dir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), dir);

                }

                return dir;
            }
        }

        public class RequestParams
        {
            [Required]
            public string arch { get; set; }
            [Required]
            public string language { get; set; }
        }

        [Route("")]
        public IHttpActionResult Post(RequestParams requestParams)
        {
            if (requestParams == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(GetPackages().Where(p => p.arch == requestParams.arch || "noarch".Equals(p.arch, StringComparison.Ordinal)));
        }

        [Route("file/{fileName}", Name="SpkFile")]
        public IHttpActionResult GetSpkFile(string fileName)
        {
            string filePath = Path.Combine(PackageDir, Path.GetFileNameWithoutExtension(fileName) + ".spk");

            if (File.Exists(filePath))
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(File.OpenRead(filePath))
                };

                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                return ResponseMessage(response);
            }
            else
            {
                return NotFound();
            }
        }

        private IEnumerable<Package> GetPackages()
        {
            var spkFiles = GetFiles().ToList();

            foreach (var spkFile in spkFiles)
            {
                var infoFile = spkFile.InfoFile;
                var package = new Package();

                package.package = infoFile.GetValue("", "package");
                package.version = infoFile.GetValue("", "version");
                package.beta = false;
                package.arch = infoFile.GetValue("", "arch");
                package.dname = infoFile.GetValue("", "displayname");
                package.maintainer = infoFile.GetValue("", "maintainer");
                package.desc = infoFile.GetValue("", "description");
                package.deppkgs = infoFile.GetValue("", "install_dep_services");
                package.link = RequestContext.Url.Link("SpkFile", new { fileName = spkFile.FileName });
                package.icon = spkFile.PackageIcon;

                yield return package;
            }
        }

        private IEnumerable<SpkFile> GetFiles()
        {
            foreach (FileInfo fi in new DirectoryInfo(PackageDir).EnumerateFiles("*.spk"))
            {
                using (var tar = File.OpenRead(fi.FullName))
                {
                    using (var tarArchive = new TarInputStream(tar))
                    {
                        TarEntry curEntry = curEntry = tarArchive.GetNextEntry();
                        var infoFile = new INIFile();
                        byte[] iconFile = null;

                        while (curEntry != null)
                        {
                            if (!curEntry.IsDirectory
                                && "INFO".Equals(curEntry.Name))
                            {
                                var buffer = new byte[curEntry.Size];
                                tarArchive.Read(buffer, 0, (int)curEntry.Size);

                                infoFile.Parse(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
                            }
                            else if (!curEntry.IsDirectory
                              && "PACKAGE_ICON.PNG".Equals(curEntry.Name))
                            {
                                iconFile = new byte[curEntry.Size];
                                tarArchive.Read(iconFile, 0, (int)curEntry.Size);
                            }

                            curEntry = tarArchive.GetNextEntry();
                        }

                        yield return new SpkFile
                        {
                            FileName = fi.Name,
                            InfoFile = infoFile,
                            PackageIcon = iconFile,
                        };
                    }
                }
            }
        }

    }
}
