using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab03
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String servidor = txtServidor.Text;
            String bd = txtBaseDatos.Text;
            String user = txtUsuario.Text;
            String pwd = txtPassword.Text;

            String str = "Server= "+servidor+";DataBase="+bd+";";

            if (chkAutenticacion.Checked)
                str += "Integrated Security=true";
            else
                str += "User Id="+user+";Password"+pwd+";";
            try
            {
                conn = new SqlConnection(str);
                conn.Open();
                MessageBox.Show("Conectado Exitosamente");
                btnDesconectar.Enabled = true;
            }
            catch (Exception ex){
                MessageBox.Show("Error al conectar el servidor: \n"+ ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    MessageBox.Show("Conexión cerrada satisfactoriamente");
                }
                else
                {
                    MessageBox.Show("La conexión ya esta cerrada");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al cerrar la conexión: \n"+ex.ToString());
            }
        }

        private void btnEstado_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                    MessageBox.Show("Estado del servidor: " + conn.State +
                                   "\nVersión del servidor: "+ conn.ServerVersion+
                                   "\nBase de datos: "+conn.Database);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Imposible determinar el estado del servidor: \n"+ex.ToString());
            }
        }

        private void chkAutenticacion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutenticacion.Checked)
            {
                txtUsuario.Enabled = false;
                txtPassword.Enabled = false;
            }
            else {
                txtUsuario.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        private void btnPersonas_Click(object sender, EventArgs e)
        {
            Persona persona = new Persona(conn);
            persona.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String users = txtUsuario.Text;
            String pass = txtPassword.Text;

            String sql = "select* from tbl_usuario where usuario_nombre = '" + users + "' and usuario_password = '" + pass + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);
            if (dt.Rows[0][0].ToString() == "1")
            {
                MessageBox.Show("Inicio de sesion correcto");
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas");
            }
        }
    }
}
