using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KurvClass
{
    [Serializable]

    public class Ordrer
    {

        #region Construntor

        public Ordrer()
        {
            this.products = GrabCart();
        }

        #endregion

        #region Fields

        private int orderId;
        private int userId;
        private DateTime createdDate;
        private List<CartProduct> products;

        #endregion

        #region Proporties

        public int OrderId { get { return this.orderId; } }
        public int UserId { get { return this.userId; } set { this.userId = value; } }
        public DateTime CreatedDate { get { return this.createdDate; } set { this.createdDate = value; } }
        public List<CartProduct> Products { get { return this.products; } }

        #endregion

        #region Methods

        public Ordrer(int userId)
        {
            this.userId = userId;
            this.createdDate = DateTime.Now;
        }

        public void addProduct(CartProduct item)
        {
            this.products.Add(item);
        }

        private List<CartProduct> GrabCart()
        {
            List<CartProduct> cart = new List<CartProduct>();

            if (HttpContext.Current.Session["Cart"] == null)
            {
                HttpContext.Current.Session.Add("Cart", cart);
            }

            cart = (List<CartProduct>)HttpContext.Current.Session["Cart"];

            return cart;
        }

        public CartProduct findProduct(int id)
        {
            foreach (CartProduct product in this.products)
            {
                if (product.Id == id)
                {
                    return product;
                }
            }

            return new CartProduct();
        }

        public void AddToCart(int id, string name, decimal price, int amount)
        {
            CartProduct product = findProduct(id);

            if (product.Id != 0)
            {
                product.Amount += amount;
            }

            else
            {
                this.products.Add(new CartProduct(id, name, price, amount));
            }
        }

        public void saveOrder()
        {
            //Opret en ordre i databasen - Returner id'et
            //Brug id'et fra ordren til at oprette alle ordre linier med.
        }

        #endregion

    }   
}