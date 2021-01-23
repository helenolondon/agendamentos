using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agendamentos.Controllers
{
    public class AgendamentosController : Controller
    {
        // GET: AgendamentosController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AgendamentosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AgendamentosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AgendamentosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: AgendamentosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AgendamentosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: AgendamentosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AgendamentosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
