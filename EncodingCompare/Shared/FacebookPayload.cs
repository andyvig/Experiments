using ProtoBuf;
using System;

namespace Shared
{
    /*{
       "id": "106181569450743",
       "name": "Codesamplez",
       "picture": "http://profile.ak.fbcdn.net/hprofile-ak-snc4/hs478.snc4/50552_106181569450743_960056_s.jpg",
       "link": "http://www.facebook.com/pages/Codesamplez/106181569450743",
       "category": "Website",
       "website": "http://codesamplez.com/",
       "founded": "2010",
       "company_overview": "codesamplez.com is a dedicated blog site for discussions on programming and developments. This blog will contain rich and informative contents day by day to help programmers/developers(from geek to professionals). This site doesn't limit its discussions on a specific programming language domain, rather discussions on this site will include help topics from all languages whenever possible.",
       "likes": 7
    }*/

    /// <summary>
    /// Example of a payload from the Facebook Graph
    /// </summary>
    [ProtoContract]
    public class FacebookPayload
    {
        [ProtoMember(1)]
        public long Id;
        [ProtoMember(2)]
        public string Name;
        [ProtoMember(3)]
        public string PictureUri;
        [ProtoMember(4)]
        public string Link;
        [ProtoMember(5)]
        public string Category;
        [ProtoMember(6)]
        public string Website;
        [ProtoMember(7)]
        public int Founded;
        [ProtoMember(8)]
        public string CompanyOverview;
        [ProtoMember(9)]
        public int Likes;

        public static FacebookPayload Create()
        {
            var id = DateTime.Now.Ticks;
            var name = RandomWords.GetWords(2);
            var payload = new FacebookPayload()
            {
                Id = id,
                Name = name,
                PictureUri = $"http://profile.ak.fbcdn.net/hprofile-ak-snc4/hs478.snc4/{id}.jpg",
                Link = $"http://www.facebook.com/pages/Codesamplez/{id}",
                Category = "Website",
                Website = $"http://{name}.com/",
                Founded = 1980 + ((int)DateTime.Now.Ticks % 40),
                CompanyOverview = RandomWords.GetWords(50),
                Likes = ((int)DateTime.Now.Ticks % 10000000)
            };
            return payload;
        }
    }
}
