using ReactiveUI;

namespace AvaloniaApplication2.ViewModels
{

    public class ThirdPageViewModel : PageViewModelBase
    {
        
        public void OnButtonClicked()
        {
            CanNavigateNext = true;
        }

        public bool _CanNavigateNext; 
        public override bool CanNavigateNext
        {
            get { return _CanNavigateNext; }
            set { this.RaiseAndSetIfChanged(ref _CanNavigateNext, value); }
        }

    }
}
