﻿using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using manager;

namespace Actividad_2
{
    public class CategoriaManager
    {

        public List<Categoria> listar()
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Select Id, Descripcion From CATEGORIAS");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Categoria aux = new Categoria();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void agregar (Categoria nuevo)
        {
            AccesoDatos data = new AccesoDatos();

            try
            {
                data.setearConsulta("Insert into CATEGORIAS(Descripcion)values('"+ nuevo.Descripcion +"')");
                data.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }

        }

        public void eliminarCategoria(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM CATEGORIAS WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { datos.cerrarConexion(); }
        }

        public void editarCategoria(Categoria nuevaCategoria)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE CATEGORIAS SET Descripcion = @Descripcion WHERE Id = @Id");
                datos.setearParametro("@Id", nuevaCategoria.Id);
                datos.setearParametro("@Descripcion", nuevaCategoria.Descripcion);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { datos.cerrarConexion(); }


        }
    }
}
