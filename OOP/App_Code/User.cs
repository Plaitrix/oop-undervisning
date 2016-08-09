using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserClass
{

/// <summary>
/// Summary description for User
/// </summary>
public class User
{
        public static bool IsUser()
        {
            // en forkortede udgave af stumpen nede under
            //return HttpContext.Current.Session["user"] != null ? true : false;


            bool result = false;

            if (HttpContext.Current.Session["user"] != null)
            {
                result = true;
            }

            return result;
        }

    protected void Page_Load(object sender,EventArgs eq)

}

}