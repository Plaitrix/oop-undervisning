using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KurvClass
{

    public partial class Deafult : System.Web.UI.Page
    {

        public Cart cart;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.cart = new Cart();

            if (IsPostBack) return;

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString =
            ConfigurationManager.ConnectionStrings["DatabaseConnectionString1"].ToString();
            SqlCommand
            cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Produkter";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();

            DataTable table = new DataTable();
            SqlDataAdapter dt = new SqlDataAdapter(cmd);
            dt.Fill(table);

            Repeater_Products.DataSource = table;
            Repeater_Products.DataBind();

            HasItems();
        }

        private void HasItems()
        {
            if (this.cart.items.Count != 0)
            {
                right.Visible = true;
            }
        }

        protected void ImageButton_AddToCart_Command(object sender, CommandEventArgs AddToCart)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString =
            ConfigurationManager.ConnectionStrings["DatabaseConnectionString1"].ToString();
            SqlCommand
            cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "SELECT ProduktID, Navn, Pris FROM Produkter WHERE ProduktID = @id";
            cmd.Parameters.AddWithValue("id", AddToCart.CommandArgument);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                cart.AddToCart(Convert.ToInt32(reader["ProduktID"]), reader["Navn"].ToString(), Convert.ToDecimal(reader["Pris"]), 1);
                HasItems();
            }
            conn.Close();

            Label_ProductChosen.Text = "<p class='ProductChosen'>" + "Your order has been added to your cart" + "</p>";
        }
    }
}