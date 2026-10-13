using Microsoft.AspNetCore.Mvc;
using Project.Web.Models;

namespace Project.Web.Controllers.Dummy
{
    public class DummyController : Controller
    {
        private static readonly List<DummyItem> _items = new()
        {
            new DummyItem { Id = 1, Name = "Sample One", Description = "First dummy record", IsActive = true }
        };

        private static int _nextId = 2;

        [HttpGet]
        public IActionResult Index()
        {
            return View(_items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DummyItem());
        }

        [HttpPost]
        public IActionResult Create(DummyItem model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Id = _nextId++;
            _items.Add(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(DummyItem model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var item = _items.FirstOrDefault(x => x.Id == model.Id);
            if (item == null)
            {
                return NotFound();
            }

            item.Name = model.Name;
            item.Description = model.Description;
            item.IsActive = model.IsActive;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            _items.Remove(item);
            return RedirectToAction(nameof(Index));
        }
    }
}