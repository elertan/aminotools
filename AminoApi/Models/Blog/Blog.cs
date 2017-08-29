using System;
using System.Collections.Generic;

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

        public override void JsonResolve(Dictionary<string, object> data)
        {
            if (data.ContainsKey("blog"))
            {
                JsonResolve(data["blog"].ToJObject().ToObject<Dictionary<string, object>>());
                return;
            }

            Author = new Author();
            Author.JsonResolve(data["author"].ToJObject().ToObject<Dictionary<string, object>>());

            BlogId = Convert.ToString(data["blogId"]);
            CommentsCount = Convert.ToInt32(data["commentsCount"]);
            Content = Convert.ToString(data["content"]);
            ContentRating = Convert.ToInt32(data["contentRating"]);
            CreatedTime = DateTime.Parse(Convert.ToString(data["createdTime"]));
            if (data["endTime"] != null)
            {
                EndTime = DateTime.Parse(Convert.ToString(data["endTime"]));
            }
            ModifiedTime = DateTime.Parse(Convert.ToString(data["modifiedTime"]));
            Status = Convert.ToInt32(data["status"]);
            Style = Convert.ToInt32(data["style"]);
            Title = Convert.ToString(data["title"]);
            Type = Convert.ToInt32(data["type"]);
            VotedValue = Convert.ToInt32(data["votedValue"]);
            VotesCount = Convert.ToInt32(data["votesCount"]);
        }
    }
}
