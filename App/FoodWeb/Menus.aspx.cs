using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Support;
using System.Diagnostics;
using System.Data;
using FoodData;
using System.Drawing;

namespace FoodWeb
{
    public partial class Menus : System.Web.UI.Page
    {
        private DataTable menus;
        private DataTable menuItems;

        private DataTable mealPeriods;
        private DataTable ingredients;

        private CultureInfo provider = CultureInfo.InvariantCulture;

        protected void Page_Load(object sender, EventArgs e)
        {
            Logger.WriteTrace("Hello!");

            if (!IsPostBack)
            {
                FillMenusGrid();
                FillMenuItemsGrid(-1);
            }
        }
        public void FillMenusGrid()
        {
            //if (Session["dt"] == null)
            //{
            menus = FoodData.Menu.GetMenusDT();
            mealPeriods = MealPeriod.GetMealPeriodsDT();
            //Session["dt"] = menus;
            //}
            //else
            //{
            //    menus = Session["dt"] as DataTable;
            //}

            if (menus.Rows.Count > 0)
            {
                grdMenus.DataSource = menus;
                grdMenus.DataBind();
            }
            else
            {
                menus.Rows.Add(menus.NewRow());
                grdMenus.DataSource = menus;
                grdMenus.DataBind();

                int TotalColumns = grdMenus.Rows[0].Cells.Count;
                grdMenus.Rows[0].Cells.Clear();
                grdMenus.Rows[0].Cells.Add(new TableCell());
                grdMenus.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                grdMenus.Rows[0].Cells[0].Text = "No Record Found";
            }
        }
        public void FillMenuItemsGrid(int menuId)
        {
            //if (Session["dt"] == null)
            //{
            menuItems = FoodData.MenuItem.GetMenuItemsDT(menuId);
            ingredients = Ingredient.GetIngredientsDT();
            //Session["dt"] = menus;
            //}
            //else
            //{
            //    menus = Session["dt"] as DataTable;
            //}

            if (menuItems.Rows.Count > 0)
            {
                gdMenuItems.DataSource = menuItems;
                gdMenuItems.DataBind();
            }
            else
            {
                menuItems.Rows.Add(menuItems.NewRow());
                gdMenuItems.DataSource = menuItems;
                gdMenuItems.DataBind();

                int TotalColumns = gdMenuItems.Rows[0].Cells.Count;
                gdMenuItems.Rows[0].Cells.Clear();
                gdMenuItems.Rows[0].Cells.Add(new TableCell());
                gdMenuItems.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gdMenuItems.Rows[0].Cells[0].Text = "No Record Found";
            }
        }
        protected void grdMenus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdMenus.EditIndex = -1;
            FillMenusGrid();
        }
        protected void grdMenus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblMealPeriod = (Label)e.Row.FindControl("lblMealPeriod");
                if (lblMealPeriod != null)
                {
                    lblMealPeriod.Text = (string)(mealPeriods.Select(string.Format("ID = {0}", lblMealPeriod.Text))[0]["Name"]);
                }
                DropDownList cmbMealPeriod = (DropDownList)e.Row.FindControl("cmbMealPeriod");
                if (cmbMealPeriod != null)
                {
                    cmbMealPeriod.DataSource = mealPeriods;
                    cmbMealPeriod.DataTextField = "Name";
                    cmbMealPeriod.DataValueField = "ID";
                    cmbMealPeriod.DataBind();
                    cmbMealPeriod.SelectedValue = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "meal_period_id"));
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList cmbNewMealPeriod = (DropDownList)e.Row.FindControl("cmbNewMealPeriod");
                cmbNewMealPeriod.DataSource = mealPeriods;
                cmbNewMealPeriod.DataBind();
            }
        }
        protected void grdMenus_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //DataTable menus = Session["dt"] as DataTable;
            FoodData.Menu menu = new FoodData.Menu();
            menu.ID = Convert.ToInt32(((Label)grdMenus.Rows[e.RowIndex].FindControl("lblID")).Text);
            //menu.Name = ((TextBox)grdMenus.Rows[e.RowIndex].FindControl("txtName")).Text;
            menu.MealPeriodID = Convert.ToInt32(((DropDownList)grdMenus.Rows[e.RowIndex].FindControl("cmbMealPeriod")).SelectedValue) ;
            menu.ServiceDate = DateTime.ParseExact(((TextBox)grdMenus.Rows[e.RowIndex].FindControl("dtServiceDate")).Text, "dd.MM.yyyy HH:mm", provider);
            menu.Save();
            grdMenus.EditIndex = -1;
            FillMenusGrid();

        }
        protected void grdMenus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            FoodData.Menu menu = new FoodData.Menu();
            menu.ID = Convert.ToInt32(((Label)grdMenus.Rows[e.RowIndex].FindControl("lblID")).Text);
            menu.Delete();
            FillMenusGrid();
        }
        protected void grdMenus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Insert"))
            {
                FoodData.Menu menu = new FoodData.Menu();
                menu.ID = -1;
                //menu.Name = ((TextBox)grdMenus.FooterRow.FindControl("txtNewName")).Text;
                menu.MealPeriodID = Convert.ToInt32(((DropDownList)grdMenus.FooterRow.FindControl("cmbNewMealPeriod")).SelectedValue);
                menu.ServiceDate = DateTime.ParseExact(((TextBox)grdMenus.FooterRow.FindControl("dtNewServiceDate")).Text, "dd.MM.yyyy HH:mm", provider);
                menu.Save();
                FillMenusGrid();
            }
        }
        protected void grdMenus_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdMenus.EditIndex = e.NewEditIndex;
            FillMenusGrid();
            //grdMenus.Rows[e.NewEditIndex].FindControl("dtServiceDate").Focus();
        }

        protected void grdMenus_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["menuId"] = grdMenus.DataKeys[grdMenus.SelectedIndex].Values[0].ToString();
            lblSelectedID.Text = Session["menuId"].ToString();
            grdMenus.Rows[grdMenus.SelectedIndex].BackColor = Color.Yellow;
            FillMenuItemsGrid(Convert.ToInt32(grdMenus.DataKeys[grdMenus.SelectedIndex].Values[0]));
        }

        //MenuItems grid
        protected void gdMenuItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gdMenuItems.EditIndex = -1;
            FillMenusGrid();
        }
        protected void gdMenuItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblIngredient = (Label)e.Row.FindControl("lblIngredient");
                if (lblIngredient != null)
                {
                    //lblIngredient.Text = (string)(ingredients.Select(string.Format("ID = {0}", lblIngredient.Text))[0]["Name"]);
                }
                DropDownList cmbIngredient = (DropDownList)e.Row.FindControl("cmbIngredient");
                if (cmbIngredient != null)
                {
                    cmbIngredient.DataSource = ingredients;
                    cmbIngredient.DataTextField = "Name";
                    cmbIngredient.DataValueField = "ID";
                    cmbIngredient.DataBind();
                    cmbIngredient.SelectedValue = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ingredient_id"));
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList cmbNewIngredient = (DropDownList)e.Row.FindControl("cmbNewIngredient");
                cmbNewIngredient.DataSource = ingredients;
                cmbNewIngredient.DataBind();
            }
        }
        protected void gdMenuItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //DataTable menus = Session["dt"] as DataTable;
            int menuId = Session["menuId"] == null ? -1 : Convert.ToInt32(Session["menuId"]);
            FoodData.MenuItem menu_item = new FoodData.MenuItem();
            menu_item.ID = Convert.ToInt32(((Label)gdMenuItems.Rows[e.RowIndex].FindControl("lblID")).Text);
            menu_item.MenuID = menuId;
            menu_item.IngredientID = Convert.ToInt32(((DropDownList)gdMenuItems.FooterRow.FindControl("cmbIngredient")).SelectedValue);
            menu_item.Weight = Convert.ToDecimal(((TextBox)gdMenuItems.FooterRow.FindControl("txtWeight")).Text);
            menu_item.Save();
            gdMenuItems.EditIndex = -1;
            FillMenuItemsGrid(menuId);

        }
        protected void gdMenuItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int menuId = Session["menuId"] == null ? -1 : Convert.ToInt32(Session["menuId"]);
            FoodData.MenuItem menu_item = new FoodData.MenuItem();
            menu_item.ID = Convert.ToInt32(((Label)gdMenuItems.Rows[e.RowIndex].FindControl("lblID")).Text);
            menu_item.Delete();
            FillMenuItemsGrid(menuId);
        }
        protected void gdMenuItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Insert"))
            {
                int menuId = Session["menuId"] == null ? -1 : Convert.ToInt32(Session["menuId"]);
                FoodData.MenuItem menu_item = new FoodData.MenuItem();
                menu_item.ID = -1;
                menu_item.MenuID = menuId;
                menu_item.IngredientID = Convert.ToInt32(((DropDownList)gdMenuItems.FooterRow.FindControl("cmbNewIngredient")).SelectedValue);
                menu_item.Weight = Convert.ToDecimal(((TextBox)gdMenuItems.FooterRow.FindControl("txtNewWeight")).Text);
                menu_item.Save();
                FillMenuItemsGrid(menuId);
            }
        }
        protected void gdMenuItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int menuId = Session["menuId"] == null ? -1 : Convert.ToInt32(Session["menuId"]);
            gdMenuItems.EditIndex = e.NewEditIndex;
            FillMenuItemsGrid(menuId);
        }
    }
}