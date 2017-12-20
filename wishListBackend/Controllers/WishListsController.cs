using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wishListBackend;
using wishListBackend.Models;
using Microsoft.AspNetCore.Authorization;

namespace wishListBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/WishLists")]
    [Authorize]
    public class WishListsController : Controller
    {
        private readonly wishListContext _context;

        public WishListsController(wishListContext context)
        {
            _context = context;
        }

        // GET: api/WishLists
        [HttpGet]
        public IEnumerable<WishList> GetWishList()
        {
            return _context.WishList;
        }

        [HttpGet]
        [Route("~/api/WishLists/{id}/Wishes")]
        public IEnumerable<Wish> GetWishesOnWishList([FromRoute] int id)
        {
            //var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);
            var wishlist = _context.WishList.Include("Wishes.WishCategory").FirstOrDefault(w => w.Id == id);

            return wishlist.Wishes;
        }

        [HttpGet]
        [Route("~/api/WishLists/{id}/Participants")]
        public IEnumerable<User> GetParticipantsOnWishList([FromRoute] int id)
        {
            //var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);
            var participantsOnWishList = _context.ParticipantOnWishList.Include(w=>w.User).Where(w => w.WishListId == id);

            List<User> participants = new List<User>();
            foreach(var p in participantsOnWishList)
            {
                participants.Add(p.User);
            }

            return participants;
        }
        // DELETE: participants
        [HttpDelete]
        [Route("~/api/WishLists/ParticipantToRemove/{id}")]
        public async Task<IActionResult> DeleteParticipantFromWishList([FromRoute] int id)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var participant = _context.ParticipantOnWishList.Include(w => w.User).FirstOrDefaultAsync(w => w.UserId == id).Result;
            
            
            if (participant == null)
            {
                return NotFound();
            }
            _context.ParticipantOnWishList.Remove(participant);

            await _context.SaveChangesAsync();

            return Ok(participant);
        }

        [HttpPost]
        [Route("~/api/WishLists/ParticipantToAdd/{email}")]
        public ParticipantOnWishList PostPartcipantOnWishList([FromRoute] string email,[FromBody] ParticipantOnWishList ponw )
        {
            
            var user = _context.User.FirstOrDefaultAsync(w => w.Email == email).Result;
            
            ponw.User = user;
            
            ponw.UserId = user.Id;
           

            var wishlist = _context.WishList.FirstOrDefaultAsync(w => w.Id == ponw.WishListId).Result;
            
        
            

            _context.ParticipantOnWishList.Add(ponw);
            _context.SaveChanges();

            return ponw;
        }

        [HttpGet]
        [Route("~/api/MyWishLists")]
        public IEnumerable<WishList> GetMyWishLists()
        {
            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);

            var user = _context.User.Include("MyWishLists.Wishes")
                .Include("MyWishLists.ParticipantOnWishLists.User")
                .FirstOrDefault(u => u.Id == id);


            return user.MyWishLists;
        }

        [HttpGet]
        [Route("~/api/ParticipatingOnWishLists")]
        public IEnumerable<WishList> GetWishListWhereIParticipateOn()
        {
            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);

            var user = _context.User
                .Include("ParticipantOnWishLists.WishList")
                .FirstOrDefault(u => u.Id == id);

            var particpatingonthislist = new List<WishList>();

            if (user.ParticipantOnWishLists != null)
            {
                foreach(var ponwishlist in user.ParticipantOnWishLists)
                {
                    particpatingonthislist.Add(ponwishlist.WishList);
                }
            }
            

            return particpatingonthislist;
        }

        // GET: api/WishLists/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWishList([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wishList = await _context.WishList.Include("Wishes.WishCategory")
                //.Include(w => w.ParticipantOnWishLists)
                .Include("ParticipantOnWishLists.User")
                .SingleOrDefaultAsync(m => m.Id == id);
           

            if (wishList == null)
            {
                return NotFound();
            }

            return Ok(wishList);
        }

        // PUT: api/WishLists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWishList([FromRoute] int id, [FromBody] WishList wishList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wishList.Id)
            {
                return BadRequest();
            }

            _context.Entry(wishList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WishListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WishLists
        [HttpPost]
        public async Task<IActionResult> PostWishList([FromBody] WishList wishList)
        {
            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);

            var user = _context.User.Include("MyWishLists.Wishes")
                .Include("MyWishLists.ParticipantOnWishLists.User")
                .FirstOrDefault(u => u.Id == id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.AddWishList(wishList);

            _context.WishList.Add(wishList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWishList", new { id = wishList.Id }, wishList);
        }

        // DELETE: api/WishLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishList([FromRoute] int id)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wishList = await _context.WishList.SingleOrDefaultAsync(m => m.Id == id);
            if (wishList == null)
            {
                return NotFound();
            }

            _context.WishList.Remove(wishList);
            await _context.SaveChangesAsync();

            return Ok(wishList);
        }

        private bool WishListExists(int id)
        {
            return _context.WishList.Any(e => e.Id == id);
        }
    }
}