using SegundaPracticaNet.Models;

namespace SegundaPracticaNet.Repositories
{
    public interface IRepositoryComics
    {
        List<Comic> GetComics();

        void Insert(Comic comic);

        void InsertarProc(Comic comic);

        void Delete(Comic comic);

        Comic GetComicId (int idComic);

        int GetMaximo();

        List<string> GetNombreComics();

        Comic GetDetails(string nombre);
    }
}
