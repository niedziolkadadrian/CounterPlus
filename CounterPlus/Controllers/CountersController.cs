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
        public async Task<ActionResult> Counter(int id)
        {
            var u = await _userManager.GetUserAsync(User);
            if (u == null) return RedirectToAction("Index", "Counters");

            var c = _db.Counters?.Include(m => m.SubCounters).FirstOrDefault(m => m.User == u && m.Id == id);
            if(c == null) return RedirectToAction("Index", "Counters");
            
            return View(c);
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

            var c = _db.Counters?.FirstOrDefault(m => m.User == u && m.Id == id);
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

            var c = _db.Counters?.FirstOrDefault(m => m.User == u && m.Id == counter.Id);
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
            return View(counter);
        }
        
        [Authorize]
        public ActionResult CreateSubcounter(int counterId)
        {
            TempData["CounterId"] = counterId;
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSubcounter(int counterId, SubCounterModel subCounter)
        {
            subCounter.Count = 0;
            if (!ModelState.IsValid) return View(subCounter);

            var u = await _userManager.GetUserAsync(User);
            if (u == null) return View(subCounter);
            
            var c = _db.Counters?.Where(m => m.User == u && m.Id == counterId).FirstOrDefault();
            if(c == null) return RedirectToAction("Index", "Counters");
            
            try
            {
                c.SubCounters.Add(subCounter);
                await _db.SaveChangesAsync();
            }
            catch
            {
                TempData["err_msg"] = "Wystąpił błąd podczas dodawanie do bazy!";
                TempData["CounterId"] = counterId;
                return View(subCounter);
            }
            
            return RedirectToAction("Counter", "Counters", new{id=counterId});
        }
        
        // GET: Counters/EditSubcounter/[id]
        [Authorize]
        public async Task<ActionResult> EditSubcounter(int counterId, int id)
        {
            Console.WriteLine(counterId+"+++++++++++");
            var u = await _userManager.GetUserAsync(User);
            if (u == null) return RedirectToAction("Counter", "Counters", new{id=counterId});

            var c = _db.Counters?.FirstOrDefault(m => m.User == u && m.Id == counterId);
            if (c == null) return RedirectToAction("Counter", "Counters", new {id = counterId});

            var s = _db.SubCounters?.FirstOrDefault(m => m.Counter == c && m.Id == id);
            if (s == null) return RedirectToAction("Counter", "Counters", new {id = counterId});

            TempData["CounterId"] = counterId;
            return View(s);
        }
        // POST: Counters/EditSubcounter
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSubcounter(int counterId, [Bind("Id, Name")]SubCounterModel subCounter)
        {
            Console.WriteLine(counterId+"!!!!!!!!!!!!!!!!!!!");
            if (!ModelState.IsValid)
            {
                TempData["CounterId"] = counterId;
                return View(subCounter);
            }
            var u = await _userManager.GetUserAsync(User);
            if (u == null)
            {
                Console.WriteLine("Tutaj?");
                return RedirectToAction("Counter", "Counters", new{id=counterId});
            }

            var c = _db.Counters?.FirstOrDefault(m => m.User == u && m.Id == counterId);
            if (c == null)
            {
                Console.WriteLine("A moze tu?");
                return RedirectToAction("Counter", "Counters", new {id = counterId});
            }

            var s = _db.SubCounters?.FirstOrDefault(m => m.Counter == c && m.Id == subCounter.Id);
            if (s == null)
            {
                Console.WriteLine("Nie to tu!");
                return RedirectToAction("Counter", "Counters", new {id = counterId});
            }

            s.Name = subCounter.Name;
            _db.SubCounters?.Update(s);
            await _db.SaveChangesAsync();
            return RedirectToAction("Counter", "Counters", new {id = counterId});
        }
        
        [Authorize]
        public ActionResult DeleteSubcounter(int id)
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSubcounter(SubCounterModel subCounter)
        {

            return View();
        }
        
        [Authorize]
        public ActionResult Widget(int id)
        {
            return View();
        }
    }
}
