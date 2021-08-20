using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace repaso_abm
{
    class Datos
    {
        OleDbConnection conexion;
        OleDbCommand comando;
        private string cadenaCn;
        OleDbDataReader lector;
        
        public string pCadenaCn { get => cadenaCn; set => cadenaCn = value; }
        public OleDbDataReader pLector { get => lector; set => lector = value; }
        public Datos() {
            pCadenaCn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\usuarios\alumno\Escritorio\Eli Utn\Programacion 2\DBF_ABM_alumno_personas.mdb";
            conexion = new OleDbConnection(cadenaCn);
            comando = new OleDbCommand();
        }

        public void conectadb() {
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
        }
        public void desconectar() {
            conexion.Close();
        }

        public DataTable consultar(string NombreTabla) {
            conectadb();
            comando.CommandText = "SELECT * FROM " + NombreTabla;
            DataTable tabla = new DataTable();
            tabla.Load(comando.ExecuteReader());
            desconectar();
            return tabla;
        }

        public void leerTabla(String NombreTabla)
        {
            conectadb();
            comando.CommandText = "SELECT * FROM " + NombreTabla;
            lector = comando.ExecuteReader();
  
        }
      

        public void Insertar(Personas p)
        {
            conectadb();
            //comando.Parameters.Clear();
            comando.CommandText= "insert into personas (apellido, nombres, tipo_documento,documento,estado_civil)" +
                                " values (@apellido,@nombres,tipo_documento,@documento,@estado_civil)";
           
            comando.Parameters.AddWithValue("@apellido", p.papellido);
            comando.Parameters.AddWithValue("@nombres", p.pnombre);
            comando.Parameters.AddWithValue("@tipo_documento", p.ptipoDocumento);
            comando.Parameters.AddWithValue("@documento", p.pdocumento);
            comando.Parameters.AddWithValue("@estado_civil", p.pestadoCivil);

            comando.ExecuteNonQuery();
            desconectar();
        }

        public void ejecutarConsulta(string consulta)
        {
            conectadb();
            comando.CommandText = consulta;
            comando.ExecuteNonQuery();

            desconectar();
        }
        public void Update(Personas p)
        {
            conectadb();
            comando.Parameters.Clear();
            comando.CommandText = "Update personas set apellido = @apellido, nombres= @nombres, tipo_documento= @tipo_documeto, estado_civil=@estado_civil" +
                                                   " where documento=@documento";
                                                  
            comando.Parameters.AddWithValue("@apellido", p.papellido);
            comando.Parameters.AddWithValue("@nombres", p.pnombre);
            comando.Parameters.AddWithValue("@tipo_documento", p.ptipoDocumento);
            comando.Parameters.AddWithValue("@estado_civil", p.pestadoCivil);
            comando.Parameters.AddWithValue("@documento", p.pdocumento);

            comando.ExecuteNonQuery();
            desconectar();
        }
    }
}
