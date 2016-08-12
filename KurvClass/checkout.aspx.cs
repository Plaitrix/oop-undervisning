using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace KurvClass
{
    public partial class CheckOut : System.Web.UI.Page
    {
        public Cart cart;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.cart = new Cart();


            if (Request.UrlReferrer == null)
            {
                Response.Redirect("Default.aspx");
            }

        }

        protected void Button_SaveSessionToDB_Click(object sender, EventArgs e)
        {
            if (CheckBox_AcceptPurchase.Checked)
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString1"].ToString();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandText = @"
            INSERT INTO [Kundeordre] (FK_KundeID, Dato) VALUES (@FK_KundeID, @Dato); SELECT SCOPE_IDENTITY()";

                cmd.Parameters.AddWithValue("@FK_KundeID", 1);
                cmd.Parameters.AddWithValue("@Dato", DateTime.Now);

                conn.Open();
                int orderId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();


                foreach (CartProduct product in this.cart.Items)
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO [Produktordre] (FK_KundeordreID, FK_ProduktID, Antal, Pris) VALUES (@FK_KundeordreID, @FK_ProduktID, @Antal, @Pris)";

                    cmd.Parameters.AddWithValue("@FK_KundeordreID", orderId);
                    cmd.Parameters.AddWithValue("@FK_ProduktID", product.Id);
                    cmd.Parameters.AddWithValue("@Antal", product.Amount);
                    cmd.Parameters.AddWithValue("@Pris", product.Price);

                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                }

                Response.Redirect("Success.aspx");
            }

            else
            {
                Label_OrderFailed.Text = "Please accept your orderinformation(s)!";
            }
        }
    }
}