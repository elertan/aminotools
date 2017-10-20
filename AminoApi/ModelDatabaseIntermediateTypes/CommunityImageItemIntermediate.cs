using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models;
using AminoApi.Models.Community;
using AminoApi.Models.Media;
using SQLiteNetExtensions.Attributes;

namespace AminoApi.ModelDatabaseIntermediateTypes
{
    public class CommunityImageItemIntermediate : DatabaseModelBase
    {
        [ForeignKey(typeof(Community))]
        public string CommunityId { get; set; }
        [ForeignKey(typeof(ImageItem))]
        public int ImageItemId { get; set; }
    }
}
