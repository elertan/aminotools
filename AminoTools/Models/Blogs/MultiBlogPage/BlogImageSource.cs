using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AminoTools.Models.Blogs.MultiBlogPage
{
    public class BlogImageSource : BaseModel
    {
        public Stream Stream { get; set; }
        public string BlogReference { get; set; }
    }
}
