using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wishListBackend
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<WishList> MyWishLists { get; set; }
        public List<ParticipantOnWishList> ParticipantOnWishLists { get; set; }
        public List<WishCategory> MyWishCategories { get; set; }
        public List<Wish> MyPurchases { get; set; }
    }
}
