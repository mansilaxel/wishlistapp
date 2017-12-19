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
    [Route("api/WishCategories")]
    public class WishCategoriesController : Controller
    {
        private readonly wishListContext _context;

        public WishCategoriesController(wishListContext context)
        {
            _context = context;
        }

        // GET: api/WishCategories
        [HttpGet]
        public IEnumerable<WishCategory> GetWishCategory()
        {
            return _context.WishCategory;
        }
        // GET: api/users/userid/MyWishCategories
        [HttpGet]
        [Route("~/api/MyWishCategories")]
        public IEnumerable<WishCategory> GetMyWishCategories()
        {
            var id = int.Parse(User.Claims.SingleOrDefault(t => t.Type == "id")?.Value);

            var user = _context.User.Include(u=>u.MyWishCategories)
                .FirstOrDefault(u => u.Id == id);
          
            return user.MyWishCategories;
        }

        // GET: api/WishCategories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWishCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wishCategory = await _context.WishCategory.SingleOrDefaultAsync(m => m.Id == id);

            if (wishCategory == null)
            {
                return NotFound();
            }

            return Ok(wishCategory);
        }

        // PUT: api/WishCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWishCategory([FromRoute] int id, [FromBody] WishCategory wishCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wishCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(wishCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WishCategoryExists(id))
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

        // POST: api/WishCategories
        [HttpPost]
        public async Task<IActionResult> PostWishCategory([FromBody] WishCategory wishCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WishCategory.Add(wishCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWishCategory", new { id = wishCategory.Id }, wishCategory);
        }

        // DELETE: api/WishCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wishCategory = await _context.WishCategory.SingleOrDefaultAsync(m => m.Id == id);
            if (wishCategory == null)
            {
                return NotFound();
            }

            _context.WishCategory.Remove(wishCategory);
            await _context.SaveChangesAsync();

            return Ok(wishCategory);
        }

        private bool WishCategoryExists(int id)
        {
            return _context.WishCategory.Any(e => e.Id == id);
        }
    }
}