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



namespace Solicitud_de_Servicio_Interno_HVLH.Vista.Admin
{
    public partial class Frm_RegistrarAcceso : Form
    {
        public Frm_RegistrarAcceso()
        {
            InitializeComponent();
            LlenarDirOficina();
          
        }

        private void txtBusquedaTrabajador_TextChanged(object sender, EventArgs e)
        {
            BuscarTrabajador(txtBusquedaTrabajador.Text.Trim());
        }

        private void BuscarTrabajador(string PBusqueda)
        {
            TrabajadorPLHDAO trabajadorDao = new TrabajadorPLHDAO();
            var listado = trabajadorDao.ListarTrabajador(PBusqueda);
            dgvTrabajadores.DataSource = listado;
            dgvTrabajadores.Columns.Clear();
            dgvTrabajadores.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn Name = new DataGridViewTextBoxColumn();
            Name.HeaderText = "Nombres Completos";
            Name.DataPropertyName = "NombreCompleto";
            Name.Width = 300;
            dgvTrabajadores.Columns.Add(Name);

            DataGridViewTextBoxColumn DNI = new DataGridViewTextBoxColumn();
            DNI.HeaderText = "Nº DNI";
            DNI.DataPropertyName = "DNI";
            DNI.Width = 110;
            dgvTrabajadores.Columns.Add(DNI);

            DataGridViewTextBoxColumn DireccionOficina = new DataGridViewTextBoxColumn();
            DireccionOficina.HeaderText = "Oficina";
            DireccionOficina.DataPropertyName = "Direccion_Oficina";
            DireccionOficina.Width = 300;
            dgvTrabajadores.Columns.Add(DireccionOficina);           
        }

        private void LlenarDirOficina()
        {
            TrabajadorPLHDAO TrabajadorDao = new TrabajadorPLHDAO();

            var listadoDireccionOfi = TrabajadorDao.ListarDireccionOficina();
            this.cboOficinaAccess.DataSource = listadoDireccionOfi;
            this.cboOficinaAccess.ValueMember = "CODIGO_DIRECCIONOFICINA";
            this.cboOficinaAccess.DisplayMember = "NOMBRE_DIRECCIONOFICINA";
            
            this.cboOficinaAccess.AutoCompleteCustomSource.AddRange(new TrabajadorPLHDAO().listaDireccionOficiString().ToArray());
            this.cboOficinaAccess.AutoCompleteMode = AutoCompleteMode.Suggest;
            this.cboOficinaAccess.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.cboOficinaAccess.SelectedIndex = -1;
            
        }
        private void dgvTrabajadores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            panel1.Enabled = false;
            if (dgvTrabajadores.RowCount <= 0) return;

            
            //Generar Acceso:
            txtNombresAccess.Text= dgvTrabajadores.CurrentRow.Cells[0].Value.ToString();

            txtUsuarioAccess.Text=dgvTrabajadores.CurrentRow.Cells[1].Value.ToString();

            txtContrasenaAccess.Text = dgvTrabajadores.CurrentRow.Cells[1].Value.ToString();


            cboOficinaAccess.Text = dgvTrabajadores.CurrentRow.Cells[2].Value.ToString();

            cboTipoAccess.SelectedIndex = 1;

            llenarAreaEspecXOficina(Convert.ToInt32(cboOficinaAccess.SelectedValue));

            panel1.Enabled = true;

        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {

            SolicitudDAO soliDao = new SolicitudDAO();

            //Validar Campos:
            if(txtNombresAccess.Text.Trim()=="" || txtUsuarioAccess.Text.Trim()=="" || txtContrasenaAccess.Text.Trim()==""
                || cboOficinaAccess.SelectedItem == null || cboTipoAccess.SelectedItem == null || cboAreaEspec.SelectedItem==null)
            {
                MessageBox.Show("No puede dejar campos vacíos","Mensaje al Usuario",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            AccesoClass acceso = new AccesoClass();
            acceso.Nombre = txtNombresAccess.Text;
            acceso.Usuario = txtUsuarioAccess.Text;
            acceso.Contrasena = txtContrasenaAccess.Text;
            acceso.DireccionOficina = cboOficinaAccess.Text;
            if (cboTipoAccess.Text == "Administrador")
                acceso.TipoAcceso = "A";
            else
                acceso.TipoAcceso = "U";

            acceso.AreaEspec = cboAreaEspec.Text;
            if (soliDao.registrarAcceso(acceso))
                MessageBox.Show("El Acceso ha sido registrado","Mensaje al Usuario",MessageBoxButtons.OK,MessageBoxIcon.Information);
            else
            {
                MessageBox.Show("Ocurrió un error al registrar", "Mensaje al Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(cboOficinaAccess.SelectedValue.ToString());

            TrabajadorPLHDAO areaEspecDao = new TrabajadorPLHDAO();

            cboAreaEspec.DataSource = areaEspecDao.ListarAreaEspecifica(Convert.ToInt32(cboOficinaAccess.SelectedValue));
            this.cboAreaEspec.DisplayMember = "NOMBRE_AREA";
            this.cboAreaEspec.ValueMember = "CODIGO_AREAESPECIFICA";
     
        }
        private void llenarAreaEspecXOficina(int codOficina)
        {
            TrabajadorPLHDAO areaEspecDao = new TrabajadorPLHDAO();
            cboAreaEspec.DataSource = areaEspecDao.ListarAreaEspecifica(codOficina);
            this.cboAreaEspec.DisplayMember = "NOMBRE_AREA";
            this.cboAreaEspec.ValueMember = "CODIGO_AREAESPECIFICA";
        }

        private void dgvTrabajadores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

           /* if (e.Value != null && e.ColumnIndex == 2)
            {
                if (e.Value.ToString().Equals("OFICINA DE LOGISTICA"))
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.BackColor = Color.Black;
                }
            }*/

            if (this.dgvTrabajadores.Rows[e.RowIndex].Cells[2].Value.Equals("OFICINA DE LOGISTICA"))
            {                
                foreach (DataGridViewCell celda in
                this.dgvTrabajadores.Rows[e.RowIndex].Cells)
                {
                    celda.Style.BackColor = Color.Lime;
                    celda.Style.ForeColor = Color.Black;
                }
            }
        }

    }
}
