using Prism.AppModel;
using Prism.Navigation;

namespace WeatherApp.ViewModels
{
    public class BaseViewModel: INavigationAware, IPageLifecycleAware
    {
        #region Private & Protected

        protected INavigationService _navigationService { get; set; }

        #endregion

        #region Properties

        public string Title { get; set; }
        public bool IsBusy { get; set; }

        #endregion

        #region Constructor

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion

        #region INavigationAware

        public virtual void OnNavigatingTo(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        #endregion INavigationAware

        #region IPageLifecycleAware

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        #endregion IPageLifecycleAware
    }
}
