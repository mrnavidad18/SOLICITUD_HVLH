﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Solicitud_de_Servicio_Interno_HVLH.Class;
using Solicitud_de_Servicio_Interno_HVLH.DAO;
using Solicitud_de_Servicio_Interno_HVLH.Vista.User.Informes;


namespace Solicitud_de_Servicio_Interno_HVLH.Vista.User
{

    public partial class ConcluirSolicitudUser : Form
    {
        public string nroTicket = "";
        public string oficinaAreaSolicitante = "";
        public string motivoSolicitud = "";
        public string diagnosticoPersonal = "";
        public string personalDesignado = "";

        public string prioridadConcluir = "";        

        //lista de materiales:

        MovMateriales newItemMaterial = null;

        List<MovMateriales> lista_AddMateriales = new List<MovMateriales>();
        public ConcluirSolicitudUser()
        {
            InitializeComponent();          
        }
        private void ConcluirSolicitudUser_Load(object sender, EventArgs e)
        {
            txtNumTicket_.Text = nroTicket;
            txtOficinaAreaSolicita.Text = oficinaAreaSolicitante;
            txtMotivoSolicitud_.Text = motivoSolicitud;
            txtDiagnosticoPerso_.Text = diagnosticoPersonal;
            txtListaPersonal.Text = personalDesignado;

            llenarInsumosAsginados(nroTicket);
           
        }
        private void txtBuscarPersonal__TextChanged(object sender, EventArgs e)
        {
            BuscarTrabajadorPorOficinaPBusqueda(txtBuscarPersonal_.Text.Trim(), VarGlobal.userAccesLogueado.DireccionOficina); 
        }
        private void BuscarTrabajadorPorOficinaPBusqueda(string pBusqueda_, string pOficina)
        {
            TrabajadorPLHDAO trabDAo = new TrabajadorPLHDAO();
            dgvTrabajadorDesig.DataSource = trabDAo.ListarTrabajadorXOficina(pBusqueda_, pOficina);

            dgvTrabajadorDesig.Columns.Clear();
            dgvTrabajadorDesig.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn Name = new DataGridViewTextBoxColumn();
            Name.HeaderText = "Nombres Completos";
            Name.DataPropertyName = "NombreCompleto";
            Name.Width = 300;
            dgvTrabajadorDesig.Columns.Add(Name);

            DataGridViewTextBoxColumn DNI = new DataGridViewTextBoxColumn();
            DNI.HeaderText = "Nº DNI";
            DNI.DataPropertyName = "DNI";
            DNI.Width = 110;
            dgvTrabajadorDesig.Columns.Add(DNI);

            DataGridViewTextBoxColumn DireccionOficina = new DataGridViewTextBoxColumn();
            DireccionOficina.HeaderText = "Oficina";
            DireccionOficina.DataPropertyName = "Direccion_Oficina";
            DireccionOficina.Width = 300;
            dgvTrabajadorDesig.Columns.Add(DireccionOficina);
        }
        private void llenarInsumosAsginados(string numTickeTStr)
        {
            SolicitudDAO solDAO = new SolicitudDAO();

            lista_AddMateriales = solDAO.listarMaterialesAsginado(numTickeTStr);

            dgvMaterialesAsignados.DataSource = lista_AddMateriales;

            dgvMaterialesAsignados.Columns.Clear();
            dgvMaterialesAsignados.AutoGenerateColumns = false;

           /* DataGridViewTextBoxColumn idMovMateriales = new DataGridViewTextBoxColumn();
            idMovMateriales.HeaderText = "idMovMateriales";
            idMovMateriales.DataPropertyName = "idMovMateriales";
            idMovMateriales.Width = 80;
            idMovMateriales.Visible = false;
            dgvMaterialesAsignados.Columns.Add(idMovMateriales);*/

            DataGridViewTextBoxColumn codSIGA = new DataGridViewTextBoxColumn();
            codSIGA.HeaderText = "COD. SIGA";
            codSIGA.DataPropertyName = "codSIGA";
            codSIGA.Width = 90;
            dgvMaterialesAsignados.Columns.Add(codSIGA);

            DataGridViewTextBoxColumn Item_Nombre = new DataGridViewTextBoxColumn();
            Item_Nombre.HeaderText = "ITEM";
            Item_Nombre.DataPropertyName = "Item_Nombre";
            Item_Nombre.Width = 550;
            dgvMaterialesAsignados.Columns.Add(Item_Nombre);

            DataGridViewTextBoxColumn Unidad_Medida = new DataGridViewTextBoxColumn();
            Unidad_Medida.HeaderText = "U. MEDIDA";
            Unidad_Medida.DataPropertyName = "Unidad_Medida";
            Unidad_Medida.Width = 85;
            dgvMaterialesAsignados.Columns.Add(Unidad_Medida);


            DataGridViewTextBoxColumn cantidad = new DataGridViewTextBoxColumn();
            cantidad.HeaderText = "CANTIDAD";
            cantidad.DataPropertyName = "cantidad";
            cantidad.Width = 80;
            dgvMaterialesAsignados.Columns.Add(cantidad);

            DataGridViewButtonColumn colBotones = new DataGridViewButtonColumn();
            colBotones.Name = "colBtnDelete";
            colBotones.HeaderText = "Eliminar Material";
            colBotones.Width = 100;
            this.dgvMaterialesAsignados.Columns.Add(colBotones);
        }
        private void eliminarInsumosDGV(int index)
        {
            lista_AddMateriales = (List<MovMateriales>)dgvMaterialesAsignados.DataSource;
            lista_AddMateriales.RemoveAt(index);
            dgvMaterialesAsignados.DataSource = lista_AddMateriales;

            dgvMaterialesAsignados.Columns.Clear();
            dgvMaterialesAsignados.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn codSIGA = new DataGridViewTextBoxColumn();
            codSIGA.HeaderText = "COD. SIGA";
            codSIGA.DataPropertyName = "codSIGA";
            codSIGA.Width = 90;
            dgvMaterialesAsignados.Columns.Add(codSIGA);

            DataGridViewTextBoxColumn Item_Nombre = new DataGridViewTextBoxColumn();
            Item_Nombre.HeaderText = "ITEM";
            Item_Nombre.DataPropertyName = "Item_Nombre";
            Item_Nombre.Width = 550;
            dgvMaterialesAsignados.Columns.Add(Item_Nombre);

            DataGridViewTextBoxColumn Unidad_Medida = new DataGridViewTextBoxColumn();
            Unidad_Medida.HeaderText = "U. MEDIDA";
            Unidad_Medida.DataPropertyName = "Unidad_Medida";
            Unidad_Medida.Width = 85;
            dgvMaterialesAsignados.Columns.Add(Unidad_Medida);


            DataGridViewTextBoxColumn cantidad = new DataGridViewTextBoxColumn();
            cantidad.HeaderText = "CANTIDAD";
            cantidad.DataPropertyName = "cantidad";
            cantidad.Width = 80;
            dgvMaterialesAsignados.Columns.Add(cantidad);

            DataGridViewButtonColumn colBotones = new DataGridViewButtonColumn();
            colBotones.Name = "colBtnDelete";
            colBotones.HeaderText = "Eliminar Material";
            colBotones.Width = 100;
            this.dgvMaterialesAsignados.Columns.Add(colBotones);
        }
        private void txtBuscarMateriales_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Enter)
            {
                if (txtBuscarMateriales.Text.Trim() == "")
                    return;

                bloquearInfoMateriales();
                buscarItemsSIGA(txtBuscarMateriales.Text.Trim());
            }
        }

        //metodos para agregar materiales:
        private void bloquearInfoMateriales()
        {
            btnAddMaterial.Visible = false;
            txtCantidadSelected.Enabled = false;        
        }
        private void desbloquearInfoMateriales()
        {


            btnAddMaterial.Visible = true;
            txtCantidadSelected.Enabled = true;
            txtCantidadSelected.Text = "";
            txtCantidadSelected.Focus();
        }
        private void buscarItemsSIGA(string pBusqueda)
        {
            SolicitudDAO soliDAO = new SolicitudDAO();
            dgvItemsSIGA.DataSource = soliDAO.listarItemsSIGA(pBusqueda);

            dgvItemsSIGA.Columns.Clear();
            dgvItemsSIGA.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn CODSIGA = new DataGridViewTextBoxColumn();
            CODSIGA.HeaderText = "COD SIGA";
            CODSIGA.DataPropertyName = "CODSIGA";
            CODSIGA.Width = 150;
            dgvItemsSIGA.Columns.Add(CODSIGA);

            DataGridViewTextBoxColumn NOMBRE_ITEM = new DataGridViewTextBoxColumn();
            NOMBRE_ITEM.HeaderText = "ITEM";
            NOMBRE_ITEM.DataPropertyName = "NOMBRE_ITEM";
            NOMBRE_ITEM.Width = 450;
            dgvItemsSIGA.Columns.Add(NOMBRE_ITEM);

            DataGridViewTextBoxColumn NOMBRE_UNIDAD_MEDIDAD = new DataGridViewTextBoxColumn();
            NOMBRE_UNIDAD_MEDIDAD.HeaderText = "UNIDAD DE MEDIDA";
            NOMBRE_UNIDAD_MEDIDAD.DataPropertyName = "NOMBRE_UNIDAD_MEDIDAD";
            NOMBRE_UNIDAD_MEDIDAD.Width = 150;
            dgvItemsSIGA.Columns.Add(NOMBRE_UNIDAD_MEDIDAD);
        }        
        private void btnFinalizarTrabajo_Click(object sender, EventArgs e)
        {
            //preguntar por diagnostico:

            if (txtDiagnosticoPerso_.Text.Trim().Equals(diagnosticoPersonal))
            {
                MessageBox.Show("debe cambiar el diagnostico para finalizar. Indique detalles de la finalización de su trabajo.");
                return;
            }


            MovimientoSolicitud nuevoMovSolici = new MovimientoSolicitud();
            Solicitud solicitudClass = new Solicitud();
            SolicitudDAO solidao = new SolicitudDAO();
            bool banderaMateriales = false;            
            solicitudClass.NumTicketString = nroTicket;
            nuevoMovSolici.Solicitud = solicitudClass;
            nuevoMovSolici.Estado = "Atendido";
            nuevoMovSolici.MotivoSolicitud = txtMotivoSolicitud_.Text.Trim();
            nuevoMovSolici.PersonalDesignado = txtListaPersonal.Text.Trim();
            nuevoMovSolici.DiagnosticoPersonal = txtDiagnosticoPerso_.Text.Trim();
            if(dgvMaterialesAsignados.Rows.Count>0)
            nuevoMovSolici.ReqInsumo = true;
            else
                nuevoMovSolici.ReqInsumo = false;
           
            if (solidao.generarMovimientoSolicitud(nuevoMovSolici))
            {

                if (dgvMaterialesAsignados.Rows.Count>0)
                {
                    foreach (var item in lista_AddMateriales)
                    {
                        item.estado = "UTILIZADO";
                        if (solidao.agregarMateriales(item))
                        {
                            banderaMateriales = true;
                        }
                        else
                        {
                            banderaMateriales = false;

                        }
                    }

                }

                MessageBox.Show("Trabajo terminado.", "Mensaje al Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ocurriò un error al concluir el trabajo","Mensaje al Usuario",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            if (banderaMateriales)
                MessageBox.Show("Se agregaron los materiales", "Mensaje al Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("no se agregaron los materiales correctamente", "Mensaje al Usuario", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (solidao.actualizarEstadoSolicitud(nuevoMovSolici.Estado, prioridadConcluir, nroTicket))
                MessageBox.Show("Se actualizó la solicitud", "Mensaje al Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("no se pudo actualizar el estado", "Mensaje al Usuario", MessageBoxButtons.OK, MessageBoxIcon.Error);

            
            Frm_Inf_SolicitudAtendida openInforme = new Frm_Inf_SolicitudAtendida();
            openInforme.ListaSolicitudAtendida = solidao.verInformeAtendido(nroTicket);
            openInforme.ShowDialog();

            ShowSolicitudes_Users frmAsignaR = new ShowSolicitudes_Users();

            frmAsignaR.estadoSolicitudSelectedEntrante = nuevoMovSolici.Estado;
            this.Close();

        }
        private void btnCerrarConcluirTrabajo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtBuscarPersonal__KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Enter)
            {
                if (dgvTrabajadorDesig.Rows.Count <= 0)
                    return;

                if (txtListaPersonal.Text.Trim().Contains(dgvTrabajadorDesig.CurrentRow.Cells[0].Value.ToString()))
                    return;
                txtListaPersonal.Text += Environment.NewLine;
                txtListaPersonal.Text += "· " + dgvTrabajadorDesig.CurrentRow.Cells[0].Value.ToString();
               
            }
        }
        private void btnAddList_Click(object sender, EventArgs e)
        {
            if (dgvTrabajadorDesig.Rows.Count <= 0)
                return;

            if (txtListaPersonal.Text.Trim().Contains(dgvTrabajadorDesig.CurrentRow.Cells[0].Value.ToString()))
                return;
            txtListaPersonal.Text += Environment.NewLine;
            txtListaPersonal.Text += "· " + dgvTrabajadorDesig.CurrentRow.Cells[0].Value.ToString();
           
        }
        private void btnLimpiarList_Click(object sender, EventArgs e)
        {
            txtListaPersonal.Text = "";
            txtBuscarPersonal_.Text = "";
            txtBuscarPersonal_.Focus();
        }
        private void dgvItemsSIGA_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItemsSIGA.Rows.Count <= 0)
                return;


            txtItemSelected.Text = dgvItemsSIGA.CurrentRow.Cells[0].Value.ToString();
            txtUMSelected.Text = dgvItemsSIGA.CurrentRow.Cells[2].Value.ToString();
            desbloquearInfoMateriales();
        }
        private void btnAddMaterial_Click(object sender, EventArgs e)
        {
            if (txtCantidadSelected.Text.Trim() == "")
                return;
            dgvMaterialesAsignados.Enabled = true;

            newItemMaterial = new MovMateriales();
            newItemMaterial.numTicketString = nroTicket;
            newItemMaterial.codSIGA = dgvItemsSIGA.CurrentRow.Cells[0].Value.ToString();
            newItemMaterial.Item_Nombre = dgvItemsSIGA.CurrentRow.Cells[1].Value.ToString();
            newItemMaterial.Unidad_Medida = dgvItemsSIGA.CurrentRow.Cells[2].Value.ToString();
            newItemMaterial.cantidad = Convert.ToInt32(txtCantidadSelected.Text);
            newItemMaterial.estado = "UTILIZADO";

            foreach (var item in lista_AddMateriales)
            {
                if (item.Item_Nombre.Equals(newItemMaterial.Item_Nombre) || item.codSIGA.Equals(newItemMaterial.codSIGA))
                    return;
            }

            lista_AddMateriales.Add(newItemMaterial);

            dgvMaterialesAsignados.DataSource = lista_AddMateriales;
            dgvMaterialesAsignados.Columns.Clear();
            dgvMaterialesAsignados.AutoGenerateColumns = false;


            DataGridViewTextBoxColumn codSIGA = new DataGridViewTextBoxColumn();
            codSIGA.HeaderText = "COD. SIGA";
            codSIGA.DataPropertyName = "codSIGA";
            codSIGA.Width = 90;
            dgvMaterialesAsignados.Columns.Add(codSIGA);

            DataGridViewTextBoxColumn Item_Nombre = new DataGridViewTextBoxColumn();
            Item_Nombre.HeaderText = "ITEM";
            Item_Nombre.DataPropertyName = "Item_Nombre";
            Item_Nombre.Width = 550;
            dgvMaterialesAsignados.Columns.Add(Item_Nombre);

            DataGridViewTextBoxColumn Unidad_Medida = new DataGridViewTextBoxColumn();
            Unidad_Medida.HeaderText = "U. MEDIDA";
            Unidad_Medida.DataPropertyName = "Unidad_Medida";
            Unidad_Medida.Width = 85;
            dgvMaterialesAsignados.Columns.Add(Unidad_Medida);


            DataGridViewTextBoxColumn cantidad = new DataGridViewTextBoxColumn();
            cantidad.HeaderText = "CANTIDAD";
            cantidad.DataPropertyName = "cantidad";
            cantidad.Width = 80;
            dgvMaterialesAsignados.Columns.Add(cantidad);

            DataGridViewButtonColumn colBotones = new DataGridViewButtonColumn();
            colBotones.Name = "colBtnDelete";
            colBotones.HeaderText = "Eliminar Material";
            colBotones.Width = 100;
            this.dgvMaterialesAsignados.Columns.Add(colBotones);

            lblMaterialesUtilziar.Visible = true;
        }
        private void txtCantidadSelected_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtCantidadSelected.Text.Trim() == "")
                return;

            if (e.KeyChar == (int)Keys.Enter)
            {
                dgvMaterialesAsignados.Enabled = true;

                newItemMaterial = new MovMateriales();
                newItemMaterial.numTicketString = nroTicket;
                newItemMaterial.codSIGA = dgvItemsSIGA.CurrentRow.Cells[0].Value.ToString();
                newItemMaterial.Item_Nombre = dgvItemsSIGA.CurrentRow.Cells[1].Value.ToString();
                newItemMaterial.Unidad_Medida = dgvItemsSIGA.CurrentRow.Cells[2].Value.ToString();
                newItemMaterial.cantidad = Convert.ToInt32(txtCantidadSelected.Text);
                newItemMaterial.estado = "UTILIZADO";

                foreach (var item in lista_AddMateriales)
                {
                    if (item.Item_Nombre.Equals(newItemMaterial.Item_Nombre) || item.codSIGA.Equals(newItemMaterial.codSIGA))
                        return;
                }

                lista_AddMateriales.Add(newItemMaterial);

                dgvMaterialesAsignados.DataSource = lista_AddMateriales;
                dgvMaterialesAsignados.Columns.Clear();
                dgvMaterialesAsignados.AutoGenerateColumns = false;


                DataGridViewTextBoxColumn codSIGA = new DataGridViewTextBoxColumn();
                codSIGA.HeaderText = "COD. SIGA";
                codSIGA.DataPropertyName = "codSIGA";
                codSIGA.Width = 90;
                dgvMaterialesAsignados.Columns.Add(codSIGA);

                DataGridViewTextBoxColumn Item_Nombre = new DataGridViewTextBoxColumn();
                Item_Nombre.HeaderText = "ITEM";
                Item_Nombre.DataPropertyName = "Item_Nombre";
                Item_Nombre.Width = 550;
                dgvMaterialesAsignados.Columns.Add(Item_Nombre);

                DataGridViewTextBoxColumn Unidad_Medida = new DataGridViewTextBoxColumn();
                Unidad_Medida.HeaderText = "U. MEDIDA";
                Unidad_Medida.DataPropertyName = "Unidad_Medida";
                Unidad_Medida.Width = 85;
                dgvMaterialesAsignados.Columns.Add(Unidad_Medida);


                DataGridViewTextBoxColumn cantidad = new DataGridViewTextBoxColumn();
                cantidad.HeaderText = "CANTIDAD";
                cantidad.DataPropertyName = "cantidad";
                cantidad.Width = 80;
                dgvMaterialesAsignados.Columns.Add(cantidad);

                DataGridViewButtonColumn colBotones = new DataGridViewButtonColumn();
                colBotones.Name = "colBtnDelete";
                colBotones.HeaderText = "Eliminar Material";
                colBotones.Width = 100;
                this.dgvMaterialesAsignados.Columns.Add(colBotones);

                lblMaterialesUtilziar.Visible = true;
            }
        }
        private void dgvMaterialesAsignados_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.ColumnIndex >= 0 && this.dgvMaterialesAsignados.Columns[e.ColumnIndex].Name == "colBtnDelete" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                DataGridViewButtonCell celBoton = this.dgvMaterialesAsignados.Rows[e.RowIndex].Cells["colBtnDelete"] as DataGridViewButtonCell;
                Icon icoAtomico = new Icon(Environment.CurrentDirectory + @"\deleteGrid.ico");
                e.Graphics.DrawIcon(icoAtomico, e.CellBounds.Left + 35, e.CellBounds.Top + 1);

                this.dgvMaterialesAsignados.Rows[e.RowIndex].Height = icoAtomico.Height + 3;
                //  this.dgvMaterialesAsignados.Columns[e.ColumnIndex].Width = icoAtomico.Width + 3;

                e.Handled = true;
            }
        }
        private void dgvMaterialesAsignados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMaterialesAsignados.Rows.Count <= 0)
                return;

            if (this.dgvMaterialesAsignados.Columns[e.ColumnIndex].Name == "colBtnDelete")
            {
                eliminarInsumosDGV(dgvMaterialesAsignados.CurrentRow.Index);
            }

        }


    }
}
