using MudBlazor;
using TastyBits.Components;
using TastyBits.Model;
using TastyBits.Pages.UserDashboard.Components;

namespace TastyBits.Services
{
    public class TastyDialogService
    {
        private readonly IDialogService _mudDialog;

        public TastyDialogService(IDialogService mudDialog)
        {
            _mudDialog = mudDialog;
        }
        public void ShowInfoSuccess(string content)
        {
            var dParameters = new DialogParameters<Dialog>
            {
                { x => x.DialogContent, content }
            };
            _mudDialog.Show<Dialog>("Success!", parameters: dParameters);
        }
        public void ShowInfoError(string content)
        {
            var dParameters = new DialogParameters<Dialog>
            {
                { x => x.DialogContent, content }
            };
            _mudDialog.Show<Dialog>("Error!", parameters: dParameters);
        }
        public void ShowMealDialog(UserMeal selectedMeal)
        {
            var dOptions = new DialogOptions()
            {
                FullWidth = true, MaxWidth=MaxWidth.Medium
            };
            var dParameters = new DialogParameters<MealDialog>
            {
                { x => x.Meal,selectedMeal }
            };
            _mudDialog.Show<MealDialog>(title:"Meal", parameters:dParameters,options: dOptions);
        }
    }
}
