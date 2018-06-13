<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="FoodWeb._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Ингредиенты
    </h2>
    <asp:GridView ID="grdIngredients" runat="server" 
     AutoGenerateColumns="False" DataKeyNames="ID, Name" 
     OnRowCancelingEdit="grdIngredients_RowCancelingEdit" 
     OnRowEditing="grdIngredients_RowEditing" 
     OnRowUpdating="grdIngredients_RowUpdating" ShowFooter="True" 
     OnRowCommand="grdIngredients_RowCommand" 
     OnRowDeleting="grdIngredients_RowDeleting">
     <Columns>
        <asp:TemplateField HeaderText="ID"  HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
            </EditItemTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label> 
            </ItemTemplate> 
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Имя" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
            <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("name") %>' Width="95%" ></asp:TextBox> 
            </EditItemTemplate> 
            <FooterTemplate> 
            <asp:TextBox ID="txtNewName" runat="server" Width="95%" ></asp:TextBox> 
            </FooterTemplate> 
            <ItemTemplate> 
            <asp:Label ID="lblName" runat="server" Text='<%# Bind("name") %>'></asp:Label> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Белки" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
            <asp:TextBox ID="txtProteins" runat="server" Text='<%# Bind("proteins") %>' Width="95%" ></asp:TextBox> 
            </EditItemTemplate> 
            <FooterTemplate> 
            <asp:TextBox ID="txtNewProteins" runat="server" Width="95%" ></asp:TextBox> 
            </FooterTemplate> 
            <ItemTemplate> 
            <asp:Label ID="lblProteins" runat="server" Text='<%# Bind("proteins") %>'></asp:Label> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Жиры" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
            <asp:TextBox ID="txtFats" runat="server" Text='<%# Bind("fats") %>' Width="95%"></asp:TextBox> 
            </EditItemTemplate> 
            <FooterTemplate> 
            <asp:TextBox ID="txtNewFats" runat="server" Width="95%" ></asp:TextBox> 
            </FooterTemplate> 
            <ItemTemplate> 
            <asp:Label ID="lblFats" runat="server" Text='<%# Bind("fats") %>'></asp:Label> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Углеводы" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
            <asp:TextBox ID="txtCarbohydrates" runat="server" Text='<%# Bind("carbohydrates") %>' Width="95%"></asp:TextBox> 
            </EditItemTemplate> 
            <FooterTemplate> 
            <asp:TextBox ID="txtNewCarbohydrates" runat="server" Width="95%" ></asp:TextBox> 
            </FooterTemplate> 
            <ItemTemplate> 
            <asp:Label ID="lblCarbohydrates" runat="server" Text='<%# Bind("carbohydrates") %>'></asp:Label> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Каллории" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
            <asp:TextBox ID="txtEnergy" runat="server" Text='<%# Bind("energy") %>' Width="95%"></asp:TextBox> 
            </EditItemTemplate> 
            <FooterTemplate> 
            <asp:TextBox ID="txtNewEnergy" runat="server" Width="95%" ></asp:TextBox> 
            </FooterTemplate> 
            <ItemTemplate> 
            <asp:Label ID="lblEnergy" runat="server" Text='<%# Bind("energy") %>'></asp:Label> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Изменить" ShowHeader="False" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:LinkButton ID="lbkUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Применить"></asp:LinkButton> 
                <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Отмена"></asp:LinkButton> 
            </EditItemTemplate> 
            <FooterTemplate> 
                <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="False" CommandName="Insert" Text="Вставить"></asp:LinkButton> 
            </FooterTemplate> 
            <ItemTemplate> 
                <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Изменить"></asp:LinkButton> 
            </ItemTemplate> 
        </asp:TemplateField> 

        <asp:CommandField HeaderText="Удалить" ShowDeleteButton="True" DeleteText="Удалить" ShowHeader="True" /> 
     </Columns>
    </asp:GridView>
</asp:Content>
