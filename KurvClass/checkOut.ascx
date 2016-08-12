<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CheckOut.ascx.cs" Inherits="KurvClass.checkOut" %>

<asp:Repeater ID="Repeater_checkOut" runat="server" OnItemCommand="Repeater_CheckOutDesign_ItemCommand">

    <HeaderTemplate>
        <table id="CartView">
            <thead>
                <th>Produktnavn</th>
                <th>Pris (Stk.)</th>
                <th>Antal</th>
                <th>Moms</th>
                <th>Ialt</th>
            </thead>
        <tbody>
    </HeaderTemplate>





    <ItemTemplate>    
        <th><br /></th>
        <tr>
            <th><%#Eval("Name")%></th>
            <th><%#String.Format("{0:C2}", Eval("Price"))%></th>
            <th>
                <asp:Button ID="Button_Minus" runat="server" Text="-" CommandArgument='<%#Eval("id") %>' CommandName='minus'/>
                    <%#Eval("Amount")%>
                <asp:Button ID="Button_Plus" runat="server" Text="+" CommandArgument='<%#Eval("id") %>' CommandName='plus'/>
            </th>
            <th><%#String.Format("{0:C2}", Eval("Vat"))%></th>
            <th><%#String.Format("{0:C2}", Eval("Inklmoms"))%></th>
            <th>
                <asp:Button ID="Button_removeProduct" runat="server" Text="Slet" CommandName="delete" CommandArgument='<%#Eval("id")%>'/>
            </th>
        </tr>
    </ItemTemplate>

    <FooterTemplate>
        <tr>
            <td colspan="5">
            </td>
        </tr>
            </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>

    <br />

    <asp:Button ID="Button_EmptyCart" runat="server" Text="Tøm Indkøbskurv" OnClick="Button_EmptyCart_Click" />

    <br /><br />

    <a href="Default.aspx">Back to front-page</a>