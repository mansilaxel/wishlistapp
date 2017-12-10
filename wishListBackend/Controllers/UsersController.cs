using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wishListBackend;
using wishListBackend.Models;
using Microsoft.Extensions.Options;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Identity;

namespace wishListBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly wishListContext _context;

        private readonly JWTSettings _options;
        //private readonly UserManager<User> _userManager;
        //private readonly SignInManager<User> _signInManager;


        public UsersController(wishListContext context, IOptions<JWTSettings> optionsAccessor
            //, UserManager<User> userManager, SignInManager<User> signInManager
            )
        {
            _context = context;
            _options = optionsAccessor.Value;
            //_userManager = userManager;
            //_signInManager = signInManager;
        }
        //dit zijn functies voor auth te doen werken
        private string GetAccessToken(User user)
        {
            var payload = new Dictionary<string, object>
            {
                { "id", user.Id },                
                { "email", user.Email }
            };
            return GetToken(payload);
        }



        private string GetToken(Dictionary<string, object> payload)
        {
            var secret = _options.SecretKey;

            payload.Add("iss", _options.Issuer);
            payload.Add("aud", _options.Audience);
            payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(1)));
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> GetUser()
        {
            return _context.User.Include(u=>u.MyWishLists);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.Include(u=>u.MyWishLists).Include(u=>u.MyWishCategories).Include(u=>u.MyPurchases)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        // GET: api/Users/{id}/purchases
        [HttpGet]
        [Route("~/api/users/{userId}/purchases")]
        public async Task<IActionResult> GetMyPurchases([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Wish> purchases = null;
            var user = await _context.User.Include(u => u.MyPurchases).SingleOrDefaultAsync(m => m.Id == userId);
            if (user.MyPurchases.Count>0)
            {
                purchases = new List<Wish>();
            }
            
            foreach(Wish purchase in user.MyPurchases)
            {                
                purchases.Add(purchase);
            }

            if (purchases == null)
            {
                return NotFound();
            }

            return Ok(purchases);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        //register
        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var test = _userManager.CreateAsync(user, user.Password).Result;
            //if (!test.Succeeded)
            //{
            //    return BadRequest(ModelState);
            //}
            //await _signInManager.SignInAsync(user, false);
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            string accessToken = GetAccessToken(user);

            return new ObjectResult(new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.SecondName,
                access_token = accessToken
            });
            



            //return CreatedAtAction("GetUser", new
            //{
            //    user.Id,
            //    user.Email,
            //    access_token = accessToken
            //}, user);

        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}