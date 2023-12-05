using AutoMapper;
using Project.Core.Entities.Common.User;
using Project.Core.Entities.Common.User.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Config
{
    public class AppSettings
    {
        public static AppSettings Current;

        public AppSettings()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                FilePath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Uploads";
            }

            Current = this;
        }

        public Cookie Cookie { get; set; }
        public string FilePath { get; set; }
        
    }
    public class Cookie
    {
        public string Name { get; set; }
    }
}
