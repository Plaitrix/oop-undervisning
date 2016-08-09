<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Deafult.aspx.cs" Inherits="KurvClass.Deafult" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>ID</td>
                <td> <asp:TextBox ID="TextBox_id" runat="server"></asp:TextBox> </td>
            </tr>
            <tr>
                <td>Navn</td>
                <td> <asp:TextBox ID="TextBox_name" runat="server"></asp:TextBox> </td>
            </tr>
            <tr>
                <td>Pris</td>
                <td> <asp:TextBox ID="TextBox_price" runat="server"></asp:TextBox> </td>
            </tr>
            <tr>
                <td>Antal</td>
                <td> <asp:TextBox ID="TextBox_amount" runat="server"></asp:TextBox> </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="Button_submit" Text="Put i kurv" OnClick="Button_submit_Clicked" runat="server" />
                </td>
            </tr>
        </table>

        <asp:GridView ID="GV_cart" runat="server"></asp:GridView>

    </div>
    </form>
</body>
</html>
