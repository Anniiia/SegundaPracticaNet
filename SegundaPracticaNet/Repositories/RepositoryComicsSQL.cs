using Microsoft.AspNetCore.Http.HttpResults;
using SegundaPracticaNet.Models;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region PROCEDIMIENTOS ALMACENADOS
//create procedure SP_INSERTAR_COMIC (@nombre varchar(50), @imagen varchar(50), @descripcion varchar(50))
//as 
//declare @maxid int
//select @maxid = max(idcomic)+1 from comics
//insert into comics values (@maxid, @nombre, @imagen, @descripcion)
//go
#endregion

namespace SegundaPracticaNet.Repositories
{
    public class RepositoryComicsSQL : IRepositoryComics
    {
        private DataTable tablaComics;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryComicsSQL() { 
        
            string connectionString= @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.tablaComics = new DataTable();
            string sql = "select * from comics";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tablaComics);

        }
        public void Delete(Comic comic)
        {
            string sql = "delete from comics where idcomic = @idcomic";
            this.com.Parameters.AddWithValue("@idcomic", comic.IdComic);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public Comic GetComicId(int idcomic)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() where datos.Field<int>("IDCOMIC") == idcomic select datos;

            var row = consulta.First();

            Comic comic = new Comic();

            comic.IdComic = row.Field<int>("IDCOMIC");
            comic.Nombre = row.Field<string>("NOMBRE");
            comic.Imagen = row.Field<string>("IMAGEN");
            comic.Descripcion = row.Field<string>("DESCRIPCION");

            return comic;
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;

            List<Comic> comics = new List<Comic>();

            foreach (var row in consulta)
            {
                Comic comic = new Comic
                { 
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION"),
                };

                comics.Add(comic);
            }

            return comics;
        }

        public void Insert(Comic comic)
        {
            string sql = "insert into comics values(@idcomic, @nombre, @imagen, @descripcion)";
            int max = this.GetMaximo();
            this.com.Parameters.AddWithValue("@idcomic",max);
            this.com.Parameters.AddWithValue("@nombre", comic.Nombre);
            this.com.Parameters.AddWithValue("@imagen", comic.Imagen);
            this.com.Parameters.AddWithValue("@descripcion", comic.Imagen);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public int GetMaximo() {

            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;

            List<Comic> comics = new List<Comic>();

            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION"),
                };

                comics.Add(comic);
            }

            int max=comics.Max(x => x.IdComic)+1;

            return max;
        }

        public void InsertarProc(Comic comic)
        {
            //int max = this.GetMaximo();
            //this.com.Parameters.AddWithValue("@idcomic", max);
            this.com.Parameters.AddWithValue("@nombre", comic.Nombre);
            this.com.Parameters.AddWithValue("@imagen", comic.Imagen);
            this.com.Parameters.AddWithValue("@descripcion", comic.Imagen);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERTAR_COMIC";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();


        }

        public List<string> GetNombreComics() {
            var consulta = (from datos in this.tablaComics.AsEnumerable() select datos.Field<string>("NOMBRE"));

            List<string> nombres = new List<string>();

            foreach (string nom in consulta)
            {
                nombres.Add(nom);
            }

            return nombres;
        }

        public Comic GetDetails(string nombre) {
        
            var consulta = from datos in this.tablaComics.AsEnumerable() where datos.Field<string>("NOMBRE") == nombre select datos;

            var row = consulta.First();

            Comic comic = new Comic();

            comic.IdComic = row.Field<int>("IDCOMIC");
            comic.Nombre = row.Field<string>("NOMBRE");
            comic.Imagen = row.Field<string>("IMAGEN");
            comic.Descripcion = row.Field<string>("DESCRIPCION");

            return comic;


        }
    }
}
