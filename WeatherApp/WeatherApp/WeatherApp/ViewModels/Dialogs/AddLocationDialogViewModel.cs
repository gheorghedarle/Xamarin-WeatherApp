using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace WeatherApp.ViewModels.Dialogs
{
    public class AddLocationDialogViewModel : BindableBase, IDialogAware
    {
        public AddLocationDialogViewModel()
        {

        }
        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
