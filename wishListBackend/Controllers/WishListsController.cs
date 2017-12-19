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
        [Route("~/api/MyWishLists")]
        public IEnumerable<WishList> GetMyWishLists()
        {
            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);

            var user = _context.User.Include("MyWishLists.Wishes")
                .Include("MyWishLists.ParticipantOnWishLists.User")
                .FirstOrDefault(u => u.Id == id);


            return user.MyWishLists;
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