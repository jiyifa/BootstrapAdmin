﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace Bootstrap.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfilesModel : ThemeModel
    {
        /// <summary>
        /// 获得 头像文件大小
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// 获得 头像文件名称
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="controller"></param>
        public ProfilesModel(ControllerBase controller, IHostingEnvironment host) : base(controller)
        {
            if (host != null)
            {
                var fileName = Path.Combine(host.WebRootPath, Icon.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar));

                // 数据库存储的个人图片有后缀 default.jpg?v=1234567
                fileName = fileName.Split('?').FirstOrDefault();
                if (File.Exists(fileName))
                {
                    Size = new FileInfo(fileName).Length;
                    FileName = Path.GetFileName(fileName);
                }
            }
        }
    }
}