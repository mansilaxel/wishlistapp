using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wishListBackend
{
    public class WishList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Wish> Wishes { get; set; }
        public List<ParticipantOnWishList> ParticipantOnWishLists { get; set; }
    }
}
