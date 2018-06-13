<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Menus.aspx.cs" Inherits="FoodWeb.Menus" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Приемы пищи
    </h2>
    <asp:GridView ID="grdMenus" runat="server" 
     AutoGenerateColumns="False" DataKeyNames="ID" 
     OnRowDataBound="grdMenus_RowDataBound" 
     OnRowCancelingEdit="grdMenus_RowCancelingEdit" 
     OnRowEditing="grdMenus_RowEditing" 
     OnRowUpdating="grdMenus_RowUpdating" ShowFooter="True" 
     OnRowCommand="grdMenus_RowCommand" 
     OnRowDeleting="grdMenus_RowDeleting" 
     OnSelectedIndexChanged="grdMenus_SelectedIndexChanged">
     <EditRowStyle CssClass="selectedRowStyle" />
     <Columns>
        <asp:TemplateField HeaderText="ID"  HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
            </EditItemTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label> 
            </ItemTemplate> 
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Дата/Время" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:TextBox ID="dtServiceDate" CssClass="MyDatePicker" runat="server" Text='<%# Bind("service_date", "{0:dd.MM.yyyy HH:mm}") %>' Width="95%" ></asp:TextBox> 
            </EditItemTemplate> 
            <FooterTemplate> 
                <asp:TextBox ID="dtNewServiceDate" CssClass="MyDatePicker" runat="server" Width="95%" ></asp:TextBox> 
            </FooterTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblServiceDate" runat="server" Text='<%# Bind("service_date", "{0:dd.MM.yyyy HH:mm}") %>'></asp:Label> 
            </ItemTemplate>
        </asp:TemplateField>
<%--        <asp:TemplateField HeaderText="Имя" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("name") %>' Width="95%" ></asp:TextBox> 
            </EditItemTemplate> 
            <FooterTemplate> 
                <asp:TextBox ID="txtNewName" runat="server" Width="95%" ></asp:TextBox> 
            </FooterTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblName" runat="server" Text='<%# Bind("name") %>'></asp:Label> 
            </ItemTemplate>
        </asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Тип" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:DropDownList ID="cmbMealPeriod" runat="server" DataTextField="Name" DataValueField="ID"> </asp:DropDownList> 
            </EditItemTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblMealPeriod" runat="server" Text='<%# Eval("meal_period_id") %>'></asp:Label> 
            </ItemTemplate> 
            <FooterTemplate> 
                <asp:DropDownList ID="cmbNewMealPeriod" runat="server" DataTextField="Name" DataValueField="ID"> </asp:DropDownList> 
            </FooterTemplate> 
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Белки" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:Label ID="lblProteins" runat="server" Text='<%# Bind("proteins") %>'></asp:Label> 
            </EditItemTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblProteins" runat="server" Text='<%# Bind("proteins") %>'></asp:Label> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Жиры" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:Label ID="lblFats" runat="server" Text='<%# Bind("fats") %>'></asp:Label>  
            </EditItemTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblFats" runat="server" Text='<%# Bind("fats") %>'></asp:Label> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Углеводы" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:Label ID="lblCarbohydrates" runat="server" Text='<%# Bind("carbohydrates") %>'></asp:Label> 
            </EditItemTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblCarbohydrates" runat="server" Text='<%# Bind("carbohydrates") %>'></asp:Label> 
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Каллории" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:Label ID="lblEnergy" runat="server" Text='<%# Bind("energy") %>'></asp:Label> 
            </EditItemTemplate> 
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
         <asp:CommandField ShowSelectButton="True" />
     </Columns>
    </asp:GridView>
    <br />
    <asp:Label ID="lblSelectedID" runat="server"></asp:Label>

    <asp:GridView ID="gdMenuItems" runat="server" 
     AutoGenerateColumns="False" DataKeyNames="ID" 
     OnRowDataBound="gdMenuItems_RowDataBound" 
     OnRowCancelingEdit="gdMenuItems_RowCancelingEdit" 
     OnRowEditing="gdMenuItems_RowEditing" 
     OnRowUpdating="gdMenuItems_RowUpdating" ShowFooter="True" 
     OnRowCommand="gdMenuItems_RowCommand" 
     OnRowDeleting="gdMenuItems_RowDeleting">
     <Columns>
        <asp:TemplateField HeaderText="ID"  HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
            </EditItemTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label> 
            </ItemTemplate> 
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Ингредиент" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:DropDownList ID="cmbIngredient" runat="server" DataTextField="Name" DataValueField="ID"> </asp:DropDownList> 
            </EditItemTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblIngredient" runat="server" Text='<%# Eval("ingredient_id") %>'></asp:Label> 
            </ItemTemplate> 
            <FooterTemplate> 
                <asp:DropDownList ID="cmbNewIngredient" runat="server" DataTextField="Name" DataValueField="ID"> </asp:DropDownList> 
            </FooterTemplate> 
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Масса" HeaderStyle-HorizontalAlign="Left"> 
            <EditItemTemplate> 
                <asp:TextBox ID="txtWeight" runat="server" Text='<%# Bind("weight") %>' Width="95%" ></asp:TextBox> 
            </EditItemTemplate> 
            <FooterTemplate> 
                <asp:TextBox ID="txtNewWeight" runat="server" Width="95%" ></asp:TextBox> 
            </FooterTemplate> 
            <ItemTemplate> 
                <asp:Label ID="lblWeight" runat="server" Text='<%# Bind("weight") %>'></asp:Label> 
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

    <script type="text/javascript">
        $(function () {
            $('.MyDatePicker').datetimepicker({
                stepMinute: 10
            });
        });
    </script>
</asp:Content>
