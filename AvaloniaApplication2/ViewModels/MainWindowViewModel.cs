using Avalonia.Media;
using DynamicData;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvaloniaApplication2.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        _CurrentPage = Pages[0];
        MessageBackground = brushes[0];
        ContentButton = "Войти";
        isButtonVisible = true;
        var canNavNext = this.WhenAnyValue(x => x.CurrentPage.CanNavigateNext);
        NavigateNextCommand = ReactiveCommand.Create(NavigateNext, canNavNext);
    }
    private string? _ContentButton;
    public string? ContentButton
    {
        get { return _ContentButton; }
        set { this.RaiseAndSetIfChanged(ref _ContentButton, value); }
    }

    private IBrush _MessageBackground;
    public IBrush MessageBackground
    {
        get { return _MessageBackground; }
        set
        {
            this.RaiseAndSetIfChanged(ref _MessageBackground, value);
        }
    }
    private bool _isButtonVisible;
    public bool isButtonVisible
    {
        get { return _isButtonVisible; }
        set { this.RaiseAndSetIfChanged(ref _isButtonVisible, value); }
    }
    

    SolidColorBrush[] brushes = new[] {
    new SolidColorBrush(Colors.Green),
    new SolidColorBrush(Colors.Blue),
    new SolidColorBrush(Colors.Transparent),
    };

    private readonly PageViewModelBase[] Pages =
    {
        new SecondPageViewModel(),
        new ThirdPageViewModel(),
        new FourthPageViewModel()
    };

    private PageViewModelBase _CurrentPage;
    public PageViewModelBase CurrentPage
    {
        get { return _CurrentPage; }
        set { this.RaiseAndSetIfChanged(ref _CurrentPage, value); }
    }

    public ICommand NavigateNextCommand { get; }
    private void NavigateNext()
    {
        var index = Pages.IndexOf(CurrentPage) + 1;
        MessageBackground = brushes[index];
        CurrentPage = Pages[index];
        ContentButton = "Далее";
        if (index == 2)
        { isButtonVisible = false; }

    }
}
