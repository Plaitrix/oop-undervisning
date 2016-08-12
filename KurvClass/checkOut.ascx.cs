using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KurvClass
{
    public partial class checkout : System.Web.UI.UserControl
    {
        public Cart cart;

        protected void Page_Load(object sender, EventArgs e)
        {
            cart = new Cart();

            this.cart = new Cart();

            if (!IsPostBack)
            {
                Refresh();
            }
        }

        public void Refresh()
        {
            cart = new Cart();
            Repeater_checkOut.DataSource = cart.Items;
            Repeater_checkOut.DataBind();
        }

        protected void Repeater_CheckOutDesign_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "plus")
            {
                cart.addAmountOnProduct(id, 1);
            }

            else if (e.CommandName == "minus")
            {
                cart.reduceAmountOnProduct(id, 1);
            }

            else if (e.CommandName == "delete")
            {
                cart.removeProduct(id);
                if (this.cart.items.Count == 0)
                {
                    Response.Redirect("Default.aspx");
                }
            }

            Refresh();
        }

        protected void Button_EmptyCart_Click(object sender, EventArgs e)
        {
            cart.removeallProducts();
            Refresh();

            Response.Redirect("Default.aspx");
        }
    }
}