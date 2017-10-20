using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoApi.Models;
using AminoApi.Models.Chat;
using AminoApi.Models.User;
using SQLiteNetExtensions.Attributes;

namespace AminoApi.ModelDatabaseIntermediateTypes
{
    public class ChatUserProfileIntermediate : DatabaseModelBase
    {
        [ForeignKey(typeof(Chat))]
        public string ThreadId { get; set; }
        [ForeignKey(typeof(UserProfile))]
        public string UserProfileId { get; set; }
    }
}
