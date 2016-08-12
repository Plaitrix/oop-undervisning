<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="checkOut.aspx.cs" Inherits="KurvClass.checkout" %>

<%@ Register Src="~/CheckOut.ascx" TagPrefix="uc1" TagName="CheckOutDesign" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cart</title>
    <link href="StyleSheet.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper2">

            <uc1:checkOut runat="server" ID="CheckOutDesign" />

            <br /><br />

            <div id="position_right">
                <asp:CheckBox ID="CheckBox_AcceptPurchase" runat="server" Text="Accepter dit køb"/>

                <br /><br />

                <asp:Button ID="Button_SaveSessionToDB" runat="server" Text="Accepter" OnClick="Button_SaveSessionToDB_Click" />

                <br /><br />

            </div><!-- position_right slut -->

            <asp:Label ID="Label_OrderFailed" runat="server" Text=""></asp:Label>

        </div><!-- wrapper slut -->
    </form>
</body>
</html>