using CounterPlus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CounterPlus.Controllers
{
    public class CountersController : Controller
    {
        private readonly CPdbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public CountersController(CPdbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // GET: CountersController
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var u = await _userManager.GetUserAsync(User);
            if (u == null) return View(new List<CounterModel>());
            
            await _db.Counters?.Where(m => m.User == u).LoadAsync()!;
            return View(u.Counters);
        }

        // GET: Counters/Counter/[id]
        [Authorize]
        public ActionResult Counter(int id)
        {
            return View();
        }
        
        // GET: Counters/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Counters/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CounterModel counter)
        {
            counter.Count = 0;
            counter.CreatedAt = DateTime.Now;
            var u = await _userManager.GetUserAsync(User);
            if (u == null) return View(counter);
            if (!ModelState.IsValid) return View(counter);
            try
            {
                u.Counters.Add(counter);
                await _db.SaveChangesAsync();
            }
            catch
            {
                TempData["err_msg"] = "Wystąpił błąd podczas dodawanie do bazy!";
                return View(counter);
            }
            return RedirectToAction("Index", "Counters");
        }

        // GET: Counters/Edit/[id]
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var u = await _userManager.GetUserAsync(User);
            if (u == null) return RedirectToAction("Index", "Counters");

            var c = _db.Counters?.Where(m => m.User == u && m.Id == id).FirstOrDefault();
            if(c == null) return RedirectToAction("Index", "Counters");
            return View(c);
            
        }

        // POST: Counters/Edit
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id, Name")]CounterModel counter)
        {
            if (!ModelState.IsValid) return View(counter);
            
            var u = await _userManager.GetUserAsync(User);
            if (u == null) return RedirectToAction("Index", "Counters");

            var c = _db.Counters?.Where(m => m.User == u && m.Id == counter.Id).FirstOrDefault();
            if(c == null) return RedirectToAction("Index", "Counters");

            c.Name = counter.Name;
            _db.Counters?.Update(c);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Counters");
        }

        // GET: Counters/Delete/[id]
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CountersController/Delete
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CounterModel counter)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
