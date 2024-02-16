using Microsoft.AspNetCore.Http.HttpResults;
using Oracle.ManagedDataAccess.Client;
using SegundaPracticaNet.Models;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region PROCEDIMIENTOS ALMACENADOS
//create or replace procedure SP_INSERTAR_COMIC (p_nombre comics.nombre%type, p_imagen comics.imagen%type, p_descripcion comics.descripcion%type)
//as
//maxid number;
//begin
//  select max((idcomic) +1) into maxid from comics;
//insert into comics (idcomic, nombre, imagen, descripcion) values (maxid, p_nombre, p_imagen, p_descripcion);
//commit;
//end;
#endregion

namespace SegundaPracticaNet.Repositories
{
    public class RepositoryComicsOracle : IRepositoryComics
    {

        private DataTable tablaComics;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryComicsOracle()
        {

            string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True;User Id=system;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from comics";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            this.tablaComics = new DataTable();
            ad.Fill(this.tablaComics);

        }
        public void Delete(Comic comic)
        {
            string sql = "delete from comics where idcomic =:idcomic";
            OracleParameter pamIdComic = new OracleParameter(":idcomic", comic.IdComic);
            this.com.Parameters.Add(pamIdComic);
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

        public Comic GetDetails(string nombre)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() where datos.Field<string>("NOMBRE") == nombre select datos;

            var row = consulta.First();

            Comic comic = new Comic();

            comic.IdComic = row.Field<int>("IDCOMIC");
            comic.Nombre = row.Field<string>("NOMBRE");
            comic.Imagen = row.Field<string>("IMAGEN");
            comic.Descripcion = row.Field<string>("DESCRIPCION");

            return comic;
        }

        public int GetMaximo()
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

            int max = comics.Max(x => x.IdComic) + 1;

            return max;
        }

        public List<string> GetNombreComics()
        {
            var consulta = (from datos in this.tablaComics.AsEnumerable() select datos.Field<string>("NOMBRE"));

            List<string> nombres = new List<string>();

            foreach (string nom in consulta)
            {
                nombres.Add(nom);
            }

            return nombres;
        }

        public void Insert(Comic comic)
        {
            string sql = "insert into comics values(:idcomic, :nombre, :imagen, :descripcion)";
            int max = this.GetMaximo();

            OracleParameter pamIdComic = new OracleParameter(":idcomic", max);
            this.com.Parameters.Add(pamIdComic);
            OracleParameter pamNombre = new OracleParameter(":nombre", comic.Nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":imagen", comic.Imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescripcion = new OracleParameter(":descripcion", comic.Descripcion);
            this.com.Parameters.Add(pamDescripcion);

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void InsertarProc(Comic comic)
        {
            OracleParameter pamNombre = new OracleParameter(":nombre", comic.Nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":imagen", comic.Imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescripcion = new OracleParameter(":descripcion", comic.Descripcion);
            this.com.Parameters.Add(pamDescripcion);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERTAR_COMIC";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
