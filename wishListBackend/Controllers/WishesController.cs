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
    [Authorize]
    [Produces("application/json")]
    [Route("api/Wishes")]
    public class WishesController : Controller
    {
        private readonly wishListContext _context;

        public WishesController(wishListContext context)
        {
            _context = context;
        }

        // GET: api/Wishes
        [HttpGet]
        public IEnumerable<Wish> GetWish()
        {
            return _context.Wish;
        }

        // GET: api/Wishes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWish([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wish = await _context.Wish.Include(w=>w.WishCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (wish == null)
            {
                return NotFound();
            }

            return Ok(wish);
        }

        // PUT: api/Wishes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWish([FromRoute] int id, [FromBody] Wish wish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wish.Id)
            {
                return BadRequest();
            }
            

            _context.Entry(wish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WishExists(id))
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

        // POST: api/Wishes
        [HttpPost]
        [Route("~/api/WishList/{id}/Wishes")]
        public async Task<IActionResult> PostWish([FromRoute] int id,[FromBody] Wish wish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var wishList = await _context.WishList.Include("Wishes.WishCategory")                
                .Include("ParticipantOnWishLists.User")
                .SingleOrDefaultAsync(m => m.Id == id);

            wishList.Wishes.Add(wish);

            _context.Wish.Add(wish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWish", new { id = wish.Id }, wish);
        }

        // DELETE: api/Wishes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWish([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wish = await _context.Wish.SingleOrDefaultAsync(m => m.Id == id);
            if (wish == null)
            {
                return NotFound();
            }

            _context.Wish.Remove(wish);
            await _context.SaveChangesAsync();

            return Ok(wish);
        }

        private bool WishExists(int id)
        {
            return _context.Wish.Any(e => e.Id == id);
        }
    }
}