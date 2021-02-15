using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogApplication.Models
{
    public class BlogPostModel
    {
        public int id { get; set; }

        public string title { get; set; }

        public string postContent { get; set; }

        public string CreatedOn { get; set; }

        public string Comment { get; set; }

        public string tokendata { get; set; }
    }
}