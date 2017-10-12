using System;
using System.Collections.Generic;
using AminoApi.Models.Media;
using Newtonsoft.Json.Linq;

namespace AminoApi.Models.Blog
{
    public class Blog : ModelBase
    {
        private string _blogId;
        private Author _author;
        private int _commentsCount;
        private string _content;
        private int _contentRating;
        private DateTime _createdTime;
        private DateTime? _endTime;
        private DateTime _modifiedTime;
        private int _status;
        private int _style;
        private string _title;
        private int _type;
        private int _votedValue;
        private int _votesCount;
        private IEnumerable<ImageItem> _imageItems;

        public string BlogId
        {
            get => _blogId;
            set
            {
                _blogId = value; 
                OnPropertyChanged();
            }
        }

        public Author Author
        {
            get => _author;
            set
            {
                _author = value; 
                OnPropertyChanged();
            }
        }

        public int CommentsCount
        {
            get => _commentsCount;
            set
            {
                _commentsCount = value; 
                OnPropertyChanged();
            }
        }

        public string Content
        {
            get => _content;
            set
            {
                _content = value; 
                OnPropertyChanged();
            }
        }

        public int ContentRating
        {
            get => _contentRating;
            set
            {
                _contentRating = value; 
                OnPropertyChanged();
            }
        }

        public DateTime CreatedTime
        {
            get => _createdTime;
            set
            {
                _createdTime = value; 
                OnPropertyChanged();
            }
        }

        public DateTime? EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value; 
                OnPropertyChanged();
            }
        }

        public DateTime ModifiedTime
        {
            get => _modifiedTime;
            set
            {
                _modifiedTime = value; 
                OnPropertyChanged();
            }
        }

        public int Status
        {
            get => _status;
            set
            {
                _status = value; 
                OnPropertyChanged();
            }
        }

        public int Style
        {
            get => _style;
            set
            {
                _style = value; 
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value; 
                OnPropertyChanged();
            }
        }

        public int Type
        {
            get => _type;
            set
            {
                _type = value; 
                OnPropertyChanged();
            }
        }

        public int VotedValue
        {
            get => _votedValue;
            set
            {
                _votedValue = value; 
                OnPropertyChanged();
            }
        }

        public int VotesCount
        {
            get => _votesCount;
            set
            {
                _votesCount = value; 
                OnPropertyChanged();
            }
        }

        public IEnumerable<ImageItem> ImageItems
        {
            get => _imageItems;
            set
            {
                _imageItems = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            if (data.ContainsKey("blog"))
            {
                JsonResolve(data["blog"].ToJObject().ToObject<Dictionary<string, object>>());
                return;
            }

            Author = new Author();
            Author.JsonResolve(data["author"].ToJObject().ToObject<Dictionary<string, object>>());

            if (data.ContainsKey("blogId"))  BlogId = Convert.ToString(data["blogId"]);
            if (data.ContainsKey("commentsCount")) CommentsCount = Convert.ToInt32(data["commentsCount"]);
            if (data.ContainsKey("content")) Content = Convert.ToString(data["content"]);
            if (data.ContainsKey("contentRating")) ContentRating = Convert.ToInt32(data["contentRating"]);
            if (data.ContainsKey("createdTime")) CreatedTime = DateTime.Parse(Convert.ToString(data["createdTime"]));
            if (data.ContainsKey("endTime") && data["endTime"] != null)
            {
                EndTime = DateTime.Parse(Convert.ToString(data["endTime"]));
            }
            if (data.ContainsKey("modifiedTime")) ModifiedTime = DateTime.Parse(Convert.ToString(data["modifiedTime"]));
            if (data.ContainsKey("status")) Status = Convert.ToInt32(data["status"]);
            if (data.ContainsKey("style")) Style = Convert.ToInt32(data["style"]);
            if (data.ContainsKey("title")) Title = Convert.ToString(data["title"]);
            if (data.ContainsKey("type")) Type = Convert.ToInt32(data["type"]);
            if (data.ContainsKey("votedValue")) VotedValue = Convert.ToInt32(data["votedValue"]);
            if (data.ContainsKey("votesCount")) VotesCount = Convert.ToInt32(data["votesCount"]);
            if (data.ContainsKey("mediaList"))
            {
                var jArray = (JArray) data["mediaList"];
                if (jArray == null) return;

                var items = new List<ImageItem>();

                foreach (var item in jArray)
                {
                    var array = (JArray) item;
                    var imageItem = new ImageItem();
                    var objects = array.ToObject<object[]>();
                    imageItem.JsonResolveArray(objects);

                    items.Add(imageItem);
                }

                ImageItems = items;
            }
        }
    }
}
