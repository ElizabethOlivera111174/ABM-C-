using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace repaso_abm
{
    public partial class Form1 : Form
    {
        const int tam= 20;
        Personas [] per = new Personas[tam];
        //OleDbConnection conexion = new OleDbConnection();
        //OleDbCommand comando = new OleDbCommand();
        Datos datos = new Datos();
        
        int c = 0;
      
        bool Nuevo;
        

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dBF_ABM_alumno_personasDataSet.tipo_documento' Puede moverla o quitarla según sea necesario.
            Habilitar(false);
            Nuevo = false;

            CargarCombo(cboDocumento,"tipo_documento","n_tipo_documento", "id_tipo_documento");
            CargarCombo(cboEstadoCivil,"estado_civil","n_estado_civil", "id_estado_civil");
            cargarLista(lstPersonas, "personas");
            lstPersonas.SelectedIndex = 0;



        }

//CARGAR LISTA
        private void cargarLista(ListBox Lista, String nombreTabla)
        {
           
            lstPersonas.Items.Clear();
            c = 0;



            //conexion.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\usuarios\alumno\Escritorio\Eli Utn\Programacion 2\DBF_ABM_alumno_personas.mdb";
            //conexion.Open();
            //comando.Connection = conexion;
            //comando.CommandType = CommandType.Text;
            //comando.CommandText = "SELECT * FROM "+ nombreTabla;

            //OleDbDataReader lector;
            //lector = comando.ExecuteReader();
            OleDbDataReader lector;
            datos.leerTabla(nombreTabla);
            lector = datos.pLector;

            while (lector.Read())
            {
                Personas p = new Personas();
                if (!lector.IsDBNull(0))
                    p.papellido= lector.GetString(0);
                if (!lector.IsDBNull(1))
                    p.pnombre = lector["nombres"].ToString();
                if (!lector.IsDBNull(2))
                    p.ptipoDocumento = lector.GetInt32(2);
                if (!lector.IsDBNull(3))
                    p.pdocumento = lector.GetInt32(3);
                if (!lector.IsDBNull(4))
                    p.pestadoCivil = lector.GetInt32(4);
                per[c] = p;
                c++;
            }
            datos.pLector.Close();
            lector.Close();
            datos.desconectar();
           
            for (int i = 0; i < c; i++)
            {
                Lista.Items.Add(per[i].pnombre+" "+per[i].papellido);
            }
            

        }
//CARGAR COMBO
        private void CargarCombo(ComboBox combo, String NombreTabla, String Nombre, String pk) {

            //conexion.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\usuarios\alumno\Escritorio\Eli Utn\Programacion 2\DBF_ABM_alumno_personas.mdb";
            //conexion.Open();
            //comando.Connection = conexion;
            //comando.CommandType = CommandType.Text;
            //comando.CommandText = "SELECT * FROM " + NombreTabla;
            DataTable tabla = new DataTable();
            //tabla.Load(comando.ExecuteReader());

            //conexion.Close();
            tabla = datos.consultar(NombreTabla);

            combo.DataSource = tabla;
            combo.DisplayMember = Nombre;
            combo.ValueMember = pk;
            

        }
//HABILITAR 
        private void Habilitar(Boolean x) {
            txtApellido.Enabled=x;
            txtNombres.Enabled = x;
            cboDocumento.Enabled = x;
            txtDocumento.Enabled = x;
            cboEstadoCivil.Enabled = x;
            btnNuevo.Enabled = !x;
            btnGuardar.Enabled = x;
            btnEditar.Enabled = !x;
            btnBorrar.Enabled = !x;
            lstPersonas.Enabled = !x;
            
        }
//LIMPIAR CAMPOS
        private void LimpiarCampos() {
            txtApellido.Clear();
            txtNombres.Clear();
            txtDocumento.Clear();
            
        }
        private bool ValidarDatos()
        {
            if (txtApellido.Text==String.Empty)
            {
                MessageBox.Show("Debe ingresar un apellido");
                txtApellido.Focus();
                return false;
            }

            if (txtNombres.Text==String.Empty)
            {
                MessageBox.Show("Debe ingresar un Nombre");
                txtNombres.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(txtDocumento.Text))
            {
                MessageBox.Show("Debe ingresar un Documento");
                txtDocumento.Focus();
                return false;
            }

            return true;

        }
        private bool existe(int pk)
        {
            bool x = false;
            for (int i = 0; i < c; i++)
            {
                if (per[i].pdocumento == pk)
                {
                    x = true;
                    break;
                }
                else
                {
                    x = false;
                }

            }
           
          
            return x;
        }
        private void cargarCampos(int posicion) {
            try
            {
                txtNombres.Text = per[posicion].pnombre;
                txtApellido.Text = per[posicion].papellido;
                txtDocumento.Text = per[posicion].pdocumento.ToString();
                cboDocumento.SelectedValue = per[posicion].ptipoDocumento;
                cboEstadoCivil.SelectedValue = per[posicion].pestadoCivil;
            }
            catch (Exception)
            {

                throw;
            }
        }
      

        private void btnNuevo_Click(object sender, EventArgs e)
        {
           

            Nuevo = true;
            LimpiarCampos();
            Habilitar(true);
            btnGuardar.Enabled = true;
            txtApellido.Focus();
            lstPersonas.Enabled = false;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (Nuevo)
                {
                    
                    Personas p = new Personas();
                    p.pnombre = txtNombres.Text;
                    p.papellido = txtApellido.Text;
                    p.pdocumento = Int32.Parse(txtDocumento.Text);
                    p.pestadoCivil = Convert.ToInt32(cboEstadoCivil.SelectedValue);
                    p.ptipoDocumento = Convert.ToInt32(cboDocumento.SelectedValue);
                    per[c] = p;
                    //c++;

                    if (!existe(p.pdocumento)) 
                    {
                        datos.Insertar(p);
                        //conexion.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\usuarios\alumno\Escritorio\Eli Utn\Programacion 2\DBF_ABM_alumno_personas.mdb";
                        //conexion.Open();
                        //comando.Connection = conexion;
                        //comando.CommandType = CommandType.Text;
                        //comando.CommandText = "INSERT INTO personas (apellido, nombres, tipo_documento,documento,estado_civil) " +
                        //                                  " VALUES ('" + p.papellido + "','"
                        //                                             + p.pnombre + "',"
                        //                                             + p.ptipoDocumento + ","
                        //                                             + p.pdocumento + ","
                        //                                             + p.pestadoCivil + ")";
                        //comando.ExecuteNonQuery();
                        //conexion.Close();
                    
                    }
                    else
                    {
                        MessageBox.Show("La Persona que esta intentando agregar ya existe");
                        return;
                    }
                   
                }
                else
                {

                  
                    int i = lstPersonas.SelectedIndex;
                    if (i!=-1)
                    {
                        per[i].pnombre = txtNombres.Text;
                        per[i].papellido = txtApellido.Text;
                        per[i].ptipoDocumento = Convert.ToInt32(cboDocumento.SelectedValue);
                        per[i].pestadoCivil = Convert.ToInt32(cboEstadoCivil.SelectedValue);
                        per[i].pdocumento = Convert.ToInt32(txtDocumento.Text);

                        //conexion.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\usuarios\alumno\Escritorio\Eli Utn\Programacion 2\DBF_ABM_alumno_personas.mdb";
                        //conexion.Open();
                        //comando.Connection = conexion;
                        //comando.CommandType = CommandType.Text;
                        //string cadena = "Update personas set apellido='" + per[i].papellido + "',"
                        //                                           + "nombres='" + per[i].pnombre + "',"
                        //                                           + "tipo_documento=" + per[i].ptipoDocumento + ","
                        //                                           + "estado_civil=" + per[i].pestadoCivil
                        //                                           + " WHERE documento=" + per[i].pdocumento;
                        //comando.ExecuteNonQuery();
                        //conexion.Close();
                        datos.Update(per[i]);
                        //datos.ejecutarConsulta(cadena);

                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar alguna persona ya existente de la lista");
                    }


                }
            }

            cargarLista(lstPersonas, "personas");
            Habilitar(false);
            Nuevo = false;

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Habilitar(true);
            txtDocumento.Enabled=false;
            lstPersonas.Enabled = true;
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
           
            if (MessageBox.Show("¿Esta seguro que desea borrar esta Persona?", "Borar", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)== DialogResult.Yes)
            {
                int i = lstPersonas.SelectedIndex;
                try
                {
                    if (existe(per[i].pdocumento))
                    { 
                        
                        //conexion.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\usuarios\alumno\Escritorio\Eli Utn\Programacion 2\DBF_ABM_alumno_personas.mdb";
                        //conexion.Open();
                        //comando.Connection = conexion;
                        //comando.CommandType = CommandType.Text;
                        //comando.CommandText = "Delete from personas where documento=" + per[i].pdocumento;
                        //comando.ExecuteNonQuery();
                        //conexion.Close();
                        string cadena= "Delete from personas where documento=" + per[i].pdocumento;
                        datos.ejecutarConsulta(cadena);

                    }
                    else
                    {
                        MessageBox.Show("La persona que esta intentado borrar no existe o no ha seleccionado una persona de la lista ");
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Seleccione una persona de la lista");
                }
              
              
            }
            else
            {
                Habilitar(true);
            }
            cargarLista(lstPersonas,"personas");
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Habilitar(false);
            LimpiarCampos();
            lstPersonas.SelectedIndex = 0;
            cargarCampos(lstPersonas.SelectedIndex);
           

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lstPersonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarCampos(lstPersonas.SelectedIndex);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("esta seguro de abandonar la aplicacion", "Salir", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

       
    }
}
