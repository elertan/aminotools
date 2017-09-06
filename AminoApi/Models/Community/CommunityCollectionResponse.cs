using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AminoApi.Models.Community
{
    public class CommunityCollectionResponse : ModelBase
    {
        private CommunityCollection _communityCollection;
        private IEnumerable<CommunityCollectionSection> _sections;

        public CommunityCollection CommunityCollection
        {
            get => _communityCollection;
            set
            {
                _communityCollection = value; 
                OnPropertyChanged();
            }
        }

        public IEnumerable<CommunityCollectionSection> Sections
        {
            get => _sections;
            set
            {
                _sections = value; 
                OnPropertyChanged();
            }
        }

        public override void JsonResolve(Dictionary<string, object> data)
        {
            ResolveCommunityCollection(data);
            ResolveSections(data);
        }

        private void ResolveSections(Dictionary<string, object> data)
        {
            var jArray = (JArray) data["communityCollectionSections"];
            var enumerable = jArray.ToObject<IEnumerable<Dictionary<string, object>>>();
            var communityCollectionSections = new List<CommunityCollectionSection>();

            foreach (var dictionary in enumerable)
            {
                var communityCollectionSection = new CommunityCollectionSection();

                communityCollectionSection.JsonResolve(dictionary);

                communityCollectionSections.Add(communityCollectionSection);
                break;
            }

            Sections = communityCollectionSections;
        }

        private void ResolveCommunityCollection(Dictionary<string, object> data)
        {
            var jObject = (JObject)data["communityCollection"];
            var dictionary = jObject.ToObject<Dictionary<string, object>>();

            var communityCollection = new CommunityCollection();
            communityCollection.JsonResolve(dictionary);
            CommunityCollection = communityCollection;
        }
    }
}