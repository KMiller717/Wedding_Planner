using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace Wedding_Planner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context {get; set;}
        public HomeController(MyContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User registered_user)
        {  
            
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u=> u.Email == registered_user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View("Index");
                }
                else{
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    registered_user.Password = Hasher.HashPassword(registered_user, registered_user.Password);
                    _context.Users.Add(registered_user);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", registered_user.UserId);
                    return RedirectToAction("Success");
                }

            }  
            return View("Index");

        }
        [HttpGet("Success")]
        public IActionResult Success()
        {
            User userInDb = _context.Users.FirstOrDefault( u=>u.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            List<Wedding> all_weddings = _context.Weddings.Include(u=>u.Attendees).ThenInclude(w=>w.Guest).OrderByDescending(w=> w.Attendees.Count).ToList();
            ViewBag.Allusers = _context.Users.ToList();
            if (userInDb == null){
                return View("Index");
            }
            else
            {
                ViewBag.User = userInDb;
                return View(all_weddings);
            }
            
            
        }


        [HttpPost("Login")]
        public IActionResult Login(LoginUser log_user)
        {
            if (ModelState.IsValid){
                User userInDb = _context.Users.FirstOrDefault(user => user.Email ==log_user.LoginEmail);
                
                if (userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Email does not exist.");
                    return View("Index");
                }
                else 
                {
                    var hash = new PasswordHasher<LoginUser>();
                    var result = hash.VerifyHashedPassword(log_user, userInDb.Password, log_user.LoginPassword);
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    if (result == 0)
                    {
                        ModelState.AddModelError("LoginPassword","Password does not match"); 
                        return View("Index");
                    }
                return RedirectToAction("Success");
                }  
            }
            else{
                return View("Index");
            }   
        }

    


        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpGet("addevent")]
        public IActionResult Add_Event()
        {
            return View();
        }

        [HttpPost("Event")]
        public IActionResult Event(Wedding new_wedding)
        {
            if (ModelState.IsValid)
            {
                new_wedding.UserId = (int)HttpContext.Session.GetInt32("UserId");
                _context.Weddings.Add(new_wedding);
                _context.SaveChanges();
            }
            else
            {
                return View("Add_Event");
            }
            return Redirect("/Success");

        }

        [HttpGet("view/{weddingid}")]

        public IActionResult View_Wed(int weddingid)
        {
            Wedding one_wed = _context.Weddings.Include(w =>w.Attendees).ThenInclude(r =>r.Guest).FirstOrDefault(w =>w.WeddingId == weddingid);

            return View(one_wed);
        }

        [HttpGet("rsvp/{weddingid}/{userid}")]
        public IActionResult RSVP(int weddingid, int userid)
        {
            RSVP attend = new RSVP();
            attend.WeddingId = weddingid;
            attend.UserId = userid;
            _context.Attendees.Add(attend);
            _context.SaveChanges();
            return RedirectToAction("Success");
        }
        [HttpGet("unrsvp/{weddingid}/{userid}")]
        public IActionResult UN_RSVP(int weddingid, int userid)
        {
            RSVP remove = _context.Attendees.FirstOrDefault( u => u.UserId == userid && u.WeddingId == weddingid);
            _context.Attendees.Remove(remove);
            _context.SaveChanges();
            return RedirectToAction("Success");
        }

        [HttpGet("cancel/{weddingid}")]
        public IActionResult Cancel_Wed(int weddingid)
        {

            Wedding cancel = _context.Weddings.FirstOrDefault(w =>w.WeddingId == weddingid);
            _context.Weddings.Remove(cancel);
            _context.SaveChanges();
            return RedirectToAction("Success");

        }
        

        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
