using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Login1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Intenta abrir una conexión a la base de datos
            VerificarConexionBaseDeDatos();
        }

        private void VerificarConexionBaseDeDatos()
        {
            string conectar = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection sqlConectar = new SqlConnection(conectar))
            {
                try
                {
                    sqlConectar.Open();
                    // Si llegamos aquí, la conexión se estableció con éxito
                    // Puedes agregar código adicional si lo deseas
                }
                catch (Exception ex)
                {
                    // Mostrar el mensaje de error en el control Label
                    lblMensajeError.Visible = true;
                    lblMensajeError.Text = "Error al conectar a la base de datos: " + ex.Message;
                }
            }
        }

        string patron = "PatronDeIncriptacion";

        protected void BtnIngresar_Click(object sender, EventArgs e)
        {
            string conectar = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
            SqlConnection sqlConectar = new SqlConnection(conectar);
            SqlCommand cmd = new SqlCommand("SP_ValidarUsuarios", sqlConectar)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Connection.Open();
            cmd.Parameters.Add("@Usuario", SqlDbType.VarChar, 50).Value = tbUsuario.Text;
            cmd.Parameters.Add("@Contrasenia", SqlDbType.VarChar, 50).Value = tbPassword.Text; // Cambio aquí
            cmd.Parameters.Add("@Patron", SqlDbType.VarChar, 50).Value = patron;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                //agregar una sesion de usuarios
                Response.Redirect("Index.aspx");
            }
            else
            {
                // Mostrar un mensaje en la página
                lblMensajeError.Visible = true;
                lblMensajeError.Text = "La condición no se cumplió.";
            }

            cmd.Connection.Close();
        }
    }
}
