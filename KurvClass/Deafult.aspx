<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Deafult.aspx.cs" Inherits="KurvClass.Deafult" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="StyleSheet.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
    
        <asp:Repeater ID="Repeater_Products" runat="server">

            <HeaderTemplate>
                <table>
                    <thead>
                        <th><h2>Produktnavn</h2></th>
                        <th><h2>Pris</h2></th>
                        <th><h2>Beskrivelse</h2></th>
                        <th></th>
                    </thead>
            </HeaderTemplate>

            <ItemTemplate>
                <tr>
                    <th><p><%#Eval("Navn")%></p></th>
                    <th><p><%#Eval("Pris")%></p></th>
                    <th><p><%#Eval("Beskrivelse")%></p></th>
                    <th> <asp:ImageButton ID="ImageButton_AddToCart" CommandArgument='<%#Eval("ProduktID")%>' OnCommand="ImageButton_AddToCart_Command" width="50" height="36" runat="server" ImageUrl="img/add-to-cart-dark.png" /> </th>
                </tr>
            </ItemTemplate>


            <FooterTemplate>
                    
                </table>
            </FooterTemplate>

        </asp:Repeater>

            <asp:Label ID="Label_ProductChosen" runat="server" Text=""></asp:Label>

        <br />

        <div id="right" runat="server" visible="false">
            <a href="CheckOut.aspx"> <p><b>Go To Cart</b></p> <img src="img/GoToCart.png" width="77" height="65" onclick="CheckOut_Click"/> </a>
        </div><!-- right slut -->


        </div><!-- wrapper slut -->
    </form></body>
</html>
