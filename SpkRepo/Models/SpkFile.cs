using PeanutButter.INI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpkRepo.Models
{
    public class SpkFile
    {

        public string FileName { get; set; }

        public INIFile InfoFile { get; set; }

        public byte[] PackageIcon { get; set; }

    }
}