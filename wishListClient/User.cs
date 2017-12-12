using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wishListClient
{
    class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string access_token { get; set; }
        public List<WishList> MyWishLists { get; set; }
        public List<ParticipantOnWishList> ParticipantOnWishLists { get; set; }
        public List<WishCategory> MyWishCategories { get; set; }
        public List<Wish> MyPurchases { get; set; }


        public void NewWishCategory(List<WishCategory> wishCategories)
        {
            MyWishCategories = wishCategories;
        }
    }
}
