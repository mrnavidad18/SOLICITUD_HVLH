﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Solicitud_de_Servicio_Interno_HVLH.Class;


namespace Solicitud_de_Servicio_Interno_HVLH.DAO
{
    public class SolicitudDAO
    {
        private static string Cadena = ConfigurationManager.ConnectionStrings["AccessSolicitud"].ConnectionString;
        private static SqlConnection oConexion = new SqlConnection(Cadena);

        #region Acceso
        //Método para registrar un nuevo acceso al sistema:
        public bool registrarAcceso(AccesoClass accesoCls)
        {

            try
            {
                bool resul = false;
                SqlCommand oComando = new SqlCommand("SP_SOLICITUD_REGISTRAR_ACCESO", oConexion);
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@Nombre", accesoCls.Nombre);
                oComando.Parameters.AddWithValue("@Usuario", accesoCls.Usuario);
                oComando.Parameters.AddWithValue("@Contrasena", accesoCls.Contrasena);
                oComando.Parameters.AddWithValue("@DireccionOficina", accesoCls.DireccionOficina);
                oComando.Parameters.AddWithValue("@Estado", "A");
                oComando.Parameters.AddWithValue("@TipoAcceso", accesoCls.TipoAcceso);
                oComando.Parameters.AddWithValue("@AreaEspec", accesoCls.AreaEspec);

                oConexion.Open();
                resul = oComando.ExecuteNonQuery() > 0;
                oConexion.Close();
                return resul;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //Método para verificar el Login al Sistema:
        public AccesoClass ValidarAccesoUser(string usuario, string clave)
        {

            AccesoClass access = null;
            using (SqlCommand oComando = new SqlCommand("SP_ACCESO_VALIDAR", oConexion))
            {
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@Usuario", usuario);
                oComando.Parameters.AddWithValue("@Contrasena", clave);
               
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = oComando.ExecuteReader(CommandBehavior.SingleResult);
                    while (dr.Read())
                    {
                        access = new AccesoClass();
                        access.idAcceso = Convert.ToInt32(dr[0]);
                        access.Nombre = dr[1].ToString();
                        access.Usuario = dr[2].ToString();
                        access.DireccionOficina = dr[4].ToString();
                        access.TipoAcceso = dr[6].ToString();
                        access.AreaEspec = dr[7].ToString();      
                    }
                    dr.Close();
                    oConexion.Close();
                }
                catch (Exception ex)
                {
                    oConexion.Close();
                    throw ex;
                }
                return access;
            }
        }


        #endregion

        #region Solicitud

        //Método para generar el número de ticket:
        public int generarNumTicket(string oficinaSolicitante, string areaSolicitante)
        {
            int numTicket = 0;
            using (SqlCommand oComando = new SqlCommand("SP_SOLICITUD_GENERAR_TICKET", oConexion))
            {
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@Oficina_Solicitante", oficinaSolicitante);
                oComando.Parameters.AddWithValue("@Area_Solicitante", areaSolicitante);
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = oComando.ExecuteReader(CommandBehavior.SingleResult);
                    while (dr.Read())
                    {

                        numTicket = Convert.ToInt32(dr[0]);
                    }
                    dr.Close();
                    oConexion.Close();
                }
                catch (Exception ex)
                {
                    oConexion.Close();
                    throw ex;
                }
                return numTicket;
            }
        }        

        //Método para generar la solicitud:
        public bool generarSolicitud(Solicitud solicitudCls)
        {
            try
            {
                bool resul = false;
                SqlCommand oComando = new SqlCommand("SP_SOLICITUD_GENERAR_NUEVO", oConexion);
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@idAcceso", solicitudCls.idAcceso.idAcceso);
                oComando.Parameters.AddWithValue("@Oficina_Solicitante", solicitudCls.Oficina_Solicitante);
                oComando.Parameters.AddWithValue("@Area_Solicitante", solicitudCls.Area_Solicitante);
                oComando.Parameters.AddWithValue("@IP_Solicitante", solicitudCls.IP_Solicitante);
                oComando.Parameters.AddWithValue("@MAC_Solicitante", solicitudCls.MAC_Solicitante);
                oComando.Parameters.AddWithValue("@NomPC_Solicitante", solicitudCls.NomPC_Solicitante);
                oComando.Parameters.AddWithValue("@NumTicket", solicitudCls.NumTicket);
                oComando.Parameters.AddWithValue("@NumTicketString", solicitudCls.NumTicketString);
                oComando.Parameters.AddWithValue("@Oficina_Destino", solicitudCls.Oficina_Destino);
                oComando.Parameters.AddWithValue("@Area_Destino", solicitudCls.Area_Destino);
                oComando.Parameters.AddWithValue("@Prioridad", solicitudCls.Prioridad);
                oComando.Parameters.AddWithValue("@Estado", solicitudCls.Estado);
                oConexion.Open();
                resul = oComando.ExecuteNonQuery() > 0;
                oConexion.Close();
                return resul;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        //Método para actualizar  el estado de la solicitud:
        public bool actualizarEstadoSolicitud(string estado, string prioridad, string numTicketStrUpdate)
        {
            try
            {
                bool resul = false;
                SqlCommand oComando = new SqlCommand("SP_SOLICITUD_ACTUALIZAR_ESTADO_PRIORIDAD", oConexion);
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@Estado", estado);
                oComando.Parameters.AddWithValue("@Prioridad", prioridad);
                oComando.Parameters.AddWithValue("@NumTicketString", numTicketStrUpdate);

                oConexion.Open();
                resul = oComando.ExecuteNonQuery() > 0;
                oConexion.Close();
                return resul;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Solicitud> listarSolicitudesSalientes(string oficinaSolicitante, string areaSolicitante)
        {

            List<Solicitud> listaSolicitudes = new List<Solicitud>();
            using (SqlCommand oComando = new SqlCommand("SP_SOLICITUD_LISTAR_SALIENTE", oConexion))
            {

                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@Oficina_Solicitante", oficinaSolicitante);
                oComando.Parameters.AddWithValue("@Area_Solicitante", areaSolicitante);
                Solicitud solicitudCls;
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = oComando.ExecuteReader(CommandBehavior.SingleResult);
                    while (dr.Read())
                    {
                        solicitudCls = new Solicitud();
                        solicitudCls.idSolicitud = Convert.ToInt32(dr[0]);
                        solicitudCls.NumTicket = Convert.ToInt32(dr[1]);
                        solicitudCls.NumTicketString = dr[2].ToString();
                        solicitudCls.FechaHoraRegistroReal =Convert.ToDateTime(dr[3]);
                        solicitudCls.Oficina_Destino=dr[4].ToString();
                        solicitudCls.Area_Destino=dr[5].ToString();
                        solicitudCls.Prioridad=dr[6].ToString();
                        solicitudCls.Estado=dr[7].ToString();
                        listaSolicitudes.Add(solicitudCls);
                    }
                    dr.Close();
                    oConexion.Close();
                }
                catch (Exception)
                {
                    oConexion.Close();
                }
                return listaSolicitudes;
            }
        }

        public List<Solicitud> listarSolicitudEntrantes(string oficinaDestino, string areaDestino)
        {

            List<Solicitud> listaSolicitudes = new List<Solicitud>();
            using (SqlCommand oComando = new SqlCommand("SP_SOLICITUD_LISTAR_ENTRANTE", oConexion))
            {

                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@OficinaDestino", oficinaDestino);
                oComando.Parameters.AddWithValue("@AreaDestino", areaDestino);
                Solicitud solicitudCls;
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = oComando.ExecuteReader(CommandBehavior.SingleResult);
                    while (dr.Read())
                    {
                        solicitudCls = new Solicitud();
                        solicitudCls.idSolicitud = Convert.ToInt32(dr[0]);
                        solicitudCls.NumTicket = Convert.ToInt32(dr[1]);
                        solicitudCls.NumTicketString = dr[2].ToString();
                        solicitudCls.FechaHoraRegistroReal = Convert.ToDateTime(dr[3]);
                        solicitudCls.Oficina_Solicitante = dr[4].ToString();
                        solicitudCls.Area_Solicitante = dr[5].ToString();
                        solicitudCls.Prioridad = dr[6].ToString();
                        solicitudCls.Estado = dr[7].ToString();
                        solicitudCls.NomPC_Solicitante = dr[8].ToString();
                        listaSolicitudes.Add(solicitudCls);
                    }
                    dr.Close();
                    oConexion.Close();
                }
                catch (Exception)
                {
                    oConexion.Close();
                }
                return listaSolicitudes;
            }
        }

        #endregion

        #region MovimientoSolicitud
        //Método para generar el movimiento de la solicitud:
        public bool generarMovimientoSolicitud(MovimientoSolicitud MovisolicitudCls)
        {
            try
            {
                bool resul = false;
                SqlCommand oComando = new SqlCommand("SP_MOVIVIMIENTO_SOLICITUD_GENERAR_NUEVO", oConexion);
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@NumTicketString", MovisolicitudCls.Solicitud.NumTicketString);
                oComando.Parameters.AddWithValue("@Estado", MovisolicitudCls.Estado);
                oComando.Parameters.AddWithValue("@MotivoSolicitud", MovisolicitudCls.MotivoSolicitud);
                oComando.Parameters.AddWithValue("@PersonalDesignado", MovisolicitudCls.PersonalDesignado);
                oComando.Parameters.AddWithValue("@DiagnosticoPersonal", MovisolicitudCls.DiagnosticoPersonal);
                oComando.Parameters.AddWithValue("@ReqInsumo", MovisolicitudCls.ReqInsumo);

                oConexion.Open();
                resul = oComando.ExecuteNonQuery() > 0;
                oConexion.Close();
                return resul;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public List<MovimientoSolicitud> listarMovimientoSolicitud(string numTicketSTR)
        {

            List<MovimientoSolicitud> listaMovSolicitud = new List<MovimientoSolicitud>();
            using (SqlCommand oComando = new SqlCommand("SP_MOVIMIENTO_SOLICITUD_LISTAR", oConexion))
            {

                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@NumTicketString", numTicketSTR);
                MovimientoSolicitud movimientoSolicitudCls;
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = oComando.ExecuteReader(CommandBehavior.SingleResult);
                    while (dr.Read())
                    {
                        movimientoSolicitudCls = new MovimientoSolicitud();
                        movimientoSolicitudCls.idMovimiento = Convert.ToInt32(dr[0]);
                        movimientoSolicitudCls.NumTicketString = dr[1].ToString();
                        movimientoSolicitudCls.Estado = dr[2].ToString();
                        movimientoSolicitudCls.MotivoSolicitud = dr[3].ToString();
                        movimientoSolicitudCls.PersonalDesignado = dr[4].ToString();
                        movimientoSolicitudCls.DiagnosticoPersonal= dr[5].ToString();
                        movimientoSolicitudCls.FechaHoraMovimiento =Convert.ToDateTime(dr[6]);

                        movimientoSolicitudCls.ReqInsumo= Convert.ToBoolean(dr[7]);
                        bool req = movimientoSolicitudCls.ReqInsumo;
                        string reqString="";
                        reqString=(req==true)?"SI":"NO";
                        movimientoSolicitudCls.ReqInsumoString = reqString;
                        listaMovSolicitud.Add(movimientoSolicitudCls);
                    }
                    dr.Close();
                    oConexion.Close();
                }
                catch (Exception)
                {
                    oConexion.Close();
                }
                return listaMovSolicitud;
            }
        }


        #endregion

        #region ItemsSIGA

        public List<ItemsSIGA> listarItemsSIGA(string pBusqueda)
        {

            List<ItemsSIGA> listaItemsSiga = new List<ItemsSIGA>();
            using (SqlCommand oComando = new SqlCommand("SP_ITEMS_LISTAR", oConexion))
            {

                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@Pbusqueda", pBusqueda);
                ItemsSIGA itemSIGACls;
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = oComando.ExecuteReader(CommandBehavior.SingleResult);
                    while (dr.Read())
                    {
                        itemSIGACls = new ItemsSIGA();
                        itemSIGACls.TIPO_BIEN = dr[0].ToString();
                        itemSIGACls.CODSIGA = dr[1].ToString();
                        itemSIGACls.NOMBRE_ITEM = dr[2].ToString();
                        itemSIGACls.UNIDAD_MEDIDA = dr[3].ToString();
                        itemSIGACls.NOMBRE_UNIDAD_MEDIDAD = dr[4].ToString();
                        listaItemsSiga.Add(itemSIGACls);
                    }
                    dr.Close();
                    oConexion.Close();
                }
                catch (Exception)
                {
                    oConexion.Close();
                }
                return listaItemsSiga;
            }
        }

        #endregion

        #region MovInsumos

        //Método para generar una lista de Insumos / Materiales:
        public bool agregarMateriales(MovMateriales movMateriales)
        {
            try
            {
                bool resul = false;
                SqlCommand oComando = new SqlCommand("SP_INSUMO_GENERAR", oConexion);
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@NumTicketString", movMateriales.numTicketString);
                oComando.Parameters.AddWithValue("@codSIGA", movMateriales.codSIGA);
                oComando.Parameters.AddWithValue("@ItemNombre", movMateriales.Item_Nombre);
                oComando.Parameters.AddWithValue("@Unidad_Medida", movMateriales.Unidad_Medida);
                oComando.Parameters.AddWithValue("@cantidad", movMateriales.cantidad);
                oComando.Parameters.AddWithValue("@estado", movMateriales.estado);

                oConexion.Open();
                resul = oComando.ExecuteNonQuery() > 0;
                oConexion.Close();
                return resul;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //método para listar los insmos solicitados en primera instancia por la evaluación de la solicitud:
        //
        public List<MovMateriales> listarMaterialesAsginado(string numticketStr)
        {
            List<MovMateriales> listaMaterialesReq = new List<MovMateriales>();
            using (SqlCommand oComando = new SqlCommand("SP_INSUMO_LISTAR_NROTICKET", oConexion))
            {
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@numTicketString", numticketStr);
                MovMateriales materialesCls;
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = oComando.ExecuteReader(CommandBehavior.SingleResult);
                    while (dr.Read())
                    {
                        materialesCls = new MovMateriales();
                        materialesCls.idMovMateriales = Convert.ToInt32(dr[0]);
                        materialesCls.numTicketString = dr[1].ToString();
                        materialesCls.codSIGA = dr[2].ToString();
                        materialesCls.Item_Nombre = dr[3].ToString();
                        materialesCls.Unidad_Medida = dr[4].ToString();
                        materialesCls.cantidad = Convert.ToInt32(dr[5]);
                        materialesCls.estado = dr[6].ToString();                        
                        listaMaterialesReq.Add(materialesCls);
                    }
                    dr.Close();
                    oConexion.Close();
                }
                catch (Exception)
                {
                    oConexion.Close();
                }
                return listaMaterialesReq;
            }
        }


        //LISTAR MATERIALES UTILIZADOS:
        public List<MovMateriales> listarMaterialesUtilizados(string numticketStr)
        {
            List<MovMateriales> listaMaterialesReq = new List<MovMateriales>();
            using (SqlCommand oComando = new SqlCommand("SP_INSUMO_LISTAR_NROTICKET_UTILIZADO", oConexion))
            {
                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@numTicketString", numticketStr);
                MovMateriales materialesCls;
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = oComando.ExecuteReader(CommandBehavior.SingleResult);
                    while (dr.Read())
                    {
                        materialesCls = new MovMateriales();
                        materialesCls.idMovMateriales = Convert.ToInt32(dr[0]);
                        materialesCls.numTicketString = dr[1].ToString();
                        materialesCls.codSIGA = dr[2].ToString();
                        materialesCls.Item_Nombre = dr[3].ToString();
                        materialesCls.Unidad_Medida = dr[4].ToString();
                        materialesCls.cantidad = Convert.ToInt32(dr[5]);
                        materialesCls.estado = dr[6].ToString();
                        listaMaterialesReq.Add(materialesCls);
                    }
                    dr.Close();
                    oConexion.Close();
                }
                catch (Exception)
                {
                    oConexion.Close();
                }
                return listaMaterialesReq;
            }
        }

        #endregion

        #region Informes Solicitud

        //Listar Informe de Atendido:

        public List<InformeAtendido> verInformeAtendido(string numTicketSTR)
        {

            List<InformeAtendido> listaInformeAtendido = new List<InformeAtendido>();
            using (SqlCommand oComando = new SqlCommand("SP_INFORME_ATENDIDO_NUMTICKET", oConexion))
            {

                oComando.CommandType = CommandType.StoredProcedure;
                oComando.Parameters.AddWithValue("@NumTicketString", numTicketSTR);
                InformeAtendido infoAtendidoCls;
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = oComando.ExecuteReader(CommandBehavior.SingleResult);
                    while (dr.Read())
                    {
                        // < ----- Info - Solicitud----
                        infoAtendidoCls = new InformeAtendido();
                        infoAtendidoCls.NombreSolicitante = dr[0].ToString();
                        infoAtendidoCls.Oficina_Solicitante = dr[1].ToString();
                        infoAtendidoCls.Area_Solicitante = dr[2].ToString();
                        infoAtendidoCls.NumTicketString = dr[3].ToString();
                        infoAtendidoCls.FechaHoraRegistroReal = Convert.ToDateTime(dr[4]);
                        infoAtendidoCls.Oficina_Destino = dr[5].ToString();
                        infoAtendidoCls.Area_Destino = dr[6].ToString();
                        infoAtendidoCls.EstadoSolicitud = dr[7].ToString();

                        //Info - Solicitud -----/>


                        // < ----- Movimiento - Solicitud----

                        infoAtendidoCls.MotivoSolicitud = dr[8].ToString();
                        infoAtendidoCls.PersonalDesignado = dr[9].ToString();
                        infoAtendidoCls.DiagnosticoPersonal = dr[10].ToString();
                        infoAtendidoCls.FechaHoraMovimiento = Convert.ToDateTime(dr[11]);
                        infoAtendidoCls.ReqInsumo = Convert.ToBoolean(dr[12]);

                        //Movimiento - Solicitud -----/>



                        // < ----- Materiales - Solicitud----

                        infoAtendidoCls.codSIGA = dr[13].ToString();
                        infoAtendidoCls.ItemNombre = dr[14].ToString();
                        infoAtendidoCls.Unidad_Medida = dr[15].ToString();
                        infoAtendidoCls.cantidad = Convert.ToInt32(dr[16]);
                        infoAtendidoCls.estado = dr[17].ToString();

                        //Materiales - Solicitud -----/>                      
                     
                        listaInformeAtendido.Add(infoAtendidoCls);
                    }
                    dr.Close();
                    oConexion.Close();
                }
                catch (Exception)
                {
                    oConexion.Close();
                }
                return listaInformeAtendido;
            }
        }

        #endregion

    }
}
