using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
       

        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                //codigo de conexion a base de datos
                //...
                datos.setearConsulta("Select A.Id, A.Codigo, A.Nombre, A.Descripcion, A.Precio, A.ImagenUrl, A.IdMarca, A.IdCategoria, C.Descripcion as Categoria, M.Descripcion as Marca From ARTICULOS A, CATEGORIAS C, MARCAS M Where A.IdMarca = M.Id AND A.IdCategoria = C.Id; ");
                datos.ejecutarLectura();



                //código de mapeo de objetos y armado de list
                //...
                while (datos.Lector.Read())
                {
                    Articulo articulo = new Articulo();

                    //mapeo de objeto articulo
                    articulo.Id = (int)datos.Lector["Id"];
                    articulo.Codigo=(string)datos.Lector["Codigo"];
                    articulo.Nombre = (string)datos.Lector["Nombre"];
                    articulo.Descripcion = (string)datos.Lector["Descripcion"];
                    articulo.Precio = (decimal)datos.Lector["Precio"];
                    articulo.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    articulo.Categoria = new Categoria();
                    articulo.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    articulo.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    articulo.Marca = new Marca();
                    articulo.Marca.Id = (int)datos.Lector["IdMarca"];
                    articulo.Marca.Descripcion = (string)datos.Lector["Marca"];


                    lista.Add(articulo);
                }

                datos.cerrarConexion();
            }
            catch (Exception)
            {

                throw;
            }
            return lista;
        }

        public void Agregar() 
        {

        }

        public void Modificar()
        {

        }

        public void Eliminar()
        {

        }


    }
}
