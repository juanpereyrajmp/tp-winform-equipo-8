﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace manager
{
    public class ArticuloManager
    {
        AccesoDatos datos = new AccesoDatos();

        public List<Articulo> ListarArticulos()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion AS ArticuloDescripcion, M.Descripcion AS Marca, C.Descripcion AS Categoria, A.Precio, I.ImagenUrl, C.Id, M.Id FROM ARTICULOS A LEFT JOIN MARCAS M ON A.IdMarca = M.Id LEFT JOIN CATEGORIAS C ON A.IdCategoria = C.Id LEFT JOIN IMAGENES I ON A.Id = I.IdArticulo");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["ArticuloDescripcion"];

                    Marca marca = new Marca();

                    if (!Convert.IsDBNull(datos.Lector["Marca"]))
                    {
                        aux.Marca.Id = (int)datos.Lector["Id"];
                        aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    }
                    else
                    {
                        aux.Marca.Descripcion = "Sin Marca";
                    }

                    Categoria categoria = new Categoria();

                    if (!Convert.IsDBNull(datos.Lector["Categoria"]))
                    {
                        aux.Categoria.Id = (int)datos.Lector["Id"];
                        aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    }
                    else
                    {
                        aux.Categoria.Descripcion = "Sin categoria";
                    }


                    aux.Precio = (decimal)datos.Lector["Precio"];

                    if (!Convert.IsDBNull(datos.Lector["ImagenUrl"]))
                    {
                        aux.Imagen = (string)datos.Lector["ImagenUrl"];
                    }

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void AgregarArticulo(Articulo agregar)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria) VALUES ('" + agregar.Codigo + "', '" + agregar.Nombre + "', '" + agregar.Descripcion + "', " + agregar.Precio + ", @IdMarca, @IdCategoria)");
                datos.setearParametro("@IdMarca", agregar.Marca.Id);
                datos.setearParametro("@IdCategoria", agregar.Categoria.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void agregarImagen(Articulo nuevoArticulo)
        {
            Articulo articulo = new Articulo();
            articulo = ListarArticulos().Last();

            try
            {
                int idArticulo = articulo.Id;
                datos.setearConsulta("INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@IdArticulo, @ImagenUrl)");
                datos.setearParametro("@IdArticulo", idArticulo);
                datos.setearParametro("@ImagenUrl", nuevoArticulo.Imagen);
                datos.cerrarConexion();
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }



        public void EliminarArticulo(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("DELETE FROM ARTICULOS WHERE id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void ModificarArticulo(Articulo modificar)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE ARTICULOS SET Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca, IdCategoria = @idCategoria, Precio = @precio WHERE Id = @Id");
                datos.setearParametro("@codigo", modificar.Codigo);
                datos.setearParametro("@nombre", modificar.Nombre);
                datos.setearParametro("@descripcion", modificar.Descripcion);
                datos.setearParametro("@idMarca", modificar.Marca.Id);
                datos.setearParametro("@IdCategoria", modificar.Categoria.Id);
                datos.setearParametro("@precio", modificar.Precio);
                datos.setearParametro("@Id", modificar.Id); 
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void modificarImagenArticulo(Articulo articulo)
        {
            try
            {
                datos.setearConsulta("UPDATE IMAGENES SET ImagenUrl = @ImagenUrl WHERE Id = (SELECT TOP 1 Id FROM IMAGENES WHERE IdArticulo = @IdA)");
                datos.setearParametro("@IdA", articulo.Id);
                datos.setearParametro("@ImagenUrl", articulo.Imagen);
                datos.cerrarConexion(); 
                datos.ejecutarAccion();


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> listaParaImagenes()
        {
            List<Articulo> listaArticulos = ListarArticulos();

            if (listaArticulos != null && listaArticulos.Count > 0)
            {
                Dictionary<string, Articulo> diccionarioArticulos = new Dictionary<string, Articulo>();

                foreach (var articulo in listaArticulos)
                {
                    if (!diccionarioArticulos.ContainsKey(articulo.Codigo))
                    {
                        diccionarioArticulos.Add(articulo.Codigo, articulo);
                        articulo.Imagenes = new List<string>();
                    }
                    diccionarioArticulos[articulo.Codigo].Imagenes.Add(articulo.Imagen);
                }

                List<Articulo> listaParaDgv = diccionarioArticulos.Values.ToList();
                return listaParaDgv;
            }
            else
            {
                return new List<Articulo>();
            }
        }


    }
}
