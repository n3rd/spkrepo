using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpkRepo.Models
{
    public class Package
    {

        public string package { get; set; }

        public string version { get; set; }

        public bool beta { get; set; }

        public string arch { get; set; }

        public string dname { get; set; }

        public string maintainer { get; set; }

        public string desc { get; set; }

        public string deppkgs { get; set; }

        public string link { get; set; }

        public byte[] icon { get; set; }

    }
}