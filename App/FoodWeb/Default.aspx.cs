using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Support;
using System.Diagnostics;
using System.Data;
using FoodData;

namespace FoodWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        public string path = "";
        private DataTable ingredients;

        protected void Page_Load(object sender, EventArgs e)
        {
            Logger.WriteTrace("Hello!");

            if (!IsPostBack)
            {
                FillGrid();
            }
        }
        public void FillGrid()
        {
            //if (Session["dt"] == null)
            //{
                ingredients = Ingredient.GetIngredientsDT();
                //Session["dt"] = ingredients;
            //}
            //else
            //{
            //    ingredients = Session["dt"] as DataTable;
            //}

            if (ingredients.Rows.Count > 0)
            {
                grdIngredients.DataSource = ingredients;
                grdIngredients.DataBind();
            }
            else
            {
                ingredients.Rows.Add(ingredients.NewRow());
                grdIngredients.DataSource = ingredients;
                grdIngredients.DataBind();

                int TotalColumns = grdIngredients.Rows[0].Cells.Count;
                grdIngredients.Rows[0].Cells.Clear();
                grdIngredients.Rows[0].Cells.Add(new TableCell());
                grdIngredients.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                grdIngredients.Rows[0].Cells[0].Text = "No Record Found";
            }
        }
        protected void grdIngredients_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdIngredients.EditIndex = -1;
            FillGrid();
        }
        protected void grdIngredients_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //DataTable ingredients = Session["dt"] as DataTable;
            Ingredient ingredient = new Ingredient();
            ingredient.ID = Convert.ToInt32(((Label)grdIngredients.Rows[e.RowIndex].FindControl("lblID")).Text);
            ingredient.Name = ((TextBox)grdIngredients.Rows[e.RowIndex].FindControl("txtName")).Text;
            ingredient.CategoryID = 1;
            ingredient.Proteins = Convert.ToDecimal(((TextBox)grdIngredients.Rows[e.RowIndex].FindControl("txtProteins")).Text);
            ingredient.Fats = Convert.ToDecimal(((TextBox)grdIngredients.Rows[e.RowIndex].FindControl("txtFats")).Text);
            ingredient.Carbohydrates = Convert.ToDecimal(((TextBox)grdIngredients.Rows[e.RowIndex].FindControl("txtCarbohydrates")).Text);
            ingredient.Energy = Convert.ToDecimal(((TextBox)grdIngredients.Rows[e.RowIndex].FindControl("txtEnergy")).Text);
            ingredient.Save();
            grdIngredients.EditIndex = -1;
            FillGrid();

        }
        protected void grdIngredients_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Ingredient ingredient = new Ingredient();
            ingredient.ID = Convert.ToInt32(((Label)grdIngredients.Rows[e.RowIndex].FindControl("lblID")).Text);
            ingredient.Delete();
            FillGrid();
        }
        protected void grdIngredients_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Insert"))
            {
                Ingredient ingredient = new Ingredient();
                ingredient.ID = -1;
                ingredient.Name = ((TextBox)grdIngredients.FooterRow.FindControl("txtNewName")).Text;
                ingredient.CategoryID = 1;
                ingredient.Proteins = Convert.ToDecimal(((TextBox)grdIngredients.FooterRow.FindControl("txtNewProteins")).Text);
                ingredient.Fats = Convert.ToDecimal(((TextBox)grdIngredients.FooterRow.FindControl("txtNewFats")).Text);
                ingredient.Carbohydrates = Convert.ToDecimal(((TextBox)grdIngredients.FooterRow.FindControl("txtNewCarbohydrates")).Text);
                ingredient.Energy = Convert.ToDecimal(((TextBox)grdIngredients.FooterRow.FindControl("txtNewEnergy")).Text);
                ingredient.Save();
                FillGrid();
            }
        }
        protected void grdIngredients_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdIngredients.EditIndex = e.NewEditIndex;
            FillGrid();
        }
    }
}
