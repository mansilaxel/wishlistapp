﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wishListBackend
{
    public class Wish
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //optional
        public string Picture { get; set; }
        public bool IsBought { get; set; }

        public  WishCategory WishCategory { get; set; }
    }
}
