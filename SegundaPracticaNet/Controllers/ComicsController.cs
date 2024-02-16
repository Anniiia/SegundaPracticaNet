using Microsoft.AspNetCore.Mvc;
using SegundaPracticaNet.Models;
using SegundaPracticaNet.Repositories;

namespace SegundaPracticaNet.Controllers
{
    public class ComicsController : Controller
    {

        private IRepositoryComics repo;

        public ComicsController(IRepositoryComics repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();

            return View(comics);
        }

        public IActionResult Create() { 
            return View();
        }

        [HttpPost]

        public IActionResult Create(Comic comic) { 
            this.repo.Insert(comic);

            return RedirectToAction("Index");
        }

        public IActionResult CreateProc()
        {
            return View();
        }

        [HttpPost]

        public IActionResult CreateProc(Comic comic)
        {
            this.repo.InsertarProc(comic);

            return RedirectToAction("Index");
        }

        public IActionResult DatosComic() {
            ViewData["NOMBRES"] = this.repo.GetNombreComics();

            return View();
        }

        [HttpPost]

        public IActionResult DatosComic(string nombre)
        {
            ViewData["NOMBRES"] = this.repo.GetNombreComics();

            Comic com = this.repo.GetDetails(nombre);

            return View(com);
        }

        public IActionResult Delete(int idcomic)
        {
            Comic com = this.repo.GetComicId(idcomic);

            return View(com);

        }

        [HttpPost]

        public IActionResult Delete(Comic comic) {

            this.repo.Delete(comic);

            return RedirectToAction("Index");
        }
    }
}
