using MudBlazor;
using TastyBits.Components;

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
    }
}
