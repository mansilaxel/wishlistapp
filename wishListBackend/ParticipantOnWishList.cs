using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wishListBackend
{
    public class ParticipantOnWishList
    {
        public int Id { get; set; }

        public int WishListId { get; set; }
        public WishList WishList { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
