using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AminoApi.Models.Blog
{
    public class BlogList : ModelBase
    {
        private List<Blog> _blogs;

        public List<Blog> Blogs
        {
            get => _blogs;
            set
            {
                _blogs = value;
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            Blogs = new List<Blog>();
            var jArray = (JArray) data["blogList"];
            var enumerable = jArray.ToObject<IEnumerable<Dictionary<string, object>>>();
            foreach (var item in enumerable)
            {
                var b = new Blog();
                b.JsonResolve(item);
                Blogs.Add(b);
            }
        }
    }
}
