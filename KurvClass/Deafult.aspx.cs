using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KurvClass
{
    public partial class Deafult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button_submit_Clicked(object sender, EventArgs e)
        {


            addToCart(cart);

            ShowCart(cart);
        }

        private void ShowCart(List<CartProduct> cart)
        {
            GV_cart.DataSource = cart;
            GV_cart.DataBind();
        }
    }
}