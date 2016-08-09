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
            List<CartProduct> cart = new List<CartProduct>();

            cart = ShopCart(cart);

            addToCart(cart);

            ShowCart(cart);
        }

        private void ShowCart(List<CartProduct> cart)
        {
            GV_cart.DataSource = cart;
            GV_cart.DataBind();
        }

        private void addToCart(List<CartProduct> cart)
        {
            bool newProduct = true;

            foreach (CartProduct product in cart)
            {
                if (product.Id == Convert.ToInt32(TextBox_id.Text))
                {
                    newProduct = false;
                    product.Amount += Convert.ToInt32(TextBox_amount.Text);
                    product.TotalPrice = product.Amount * product.Price;
                }
            }
            if (newProduct)
            {
                cart.Add(new CartProduct(
                    Convert.ToInt32(TextBox_id.Text),
                    TextBox_name.Text,
                    Convert.ToDecimal(TextBox_price.Text),
                    Convert.ToInt32(TextBox_amount.Text)
                    ));
            }
        }

        private List<CartProduct> ShopCart(List<CartProduct> cart)
        {
            if (Session["Cart"] == null)
            {
                Session.Add("Cart", cart);
            }

            cart = Session["Cart"] as List<CartProduct>;
            return cart;
        }
    }
}