using ReactiveUI;
using System;
using AvaloniaApplication2.Model;

using Avalonia.Media;

namespace AvaloniaApplication2.ViewModels
{

    public class SecondPageViewModel : PageViewModelBase
    {
        SecondModel second = new SecondModel();
            
        public SecondPageViewModel()
        {
            this.WhenAnyValue(x => x.MailAddress, x => x.Password).Subscribe(_ => UpdateCanNavigateNext());
        }

        private IBrush _messageForeground;
        public IBrush MessageForeground
        {
            get { return _messageForeground; }
            set
            {
                this.RaiseAndSetIfChanged(ref _messageForeground, value);
            }
        }

        private string? _MailAddress;
        public string? MailAddress
        {
            get { return _MailAddress; }
            set { this.RaiseAndSetIfChanged(ref _MailAddress, value); }
        }

        private string? _Password;
        public string? Password
        {
            get { return _Password; }
            set { this.RaiseAndSetIfChanged(ref _Password, value); }
        }
        private string? _messageUser;

        public string? messageUser
        {
            get { return _messageUser; }
            set { this.RaiseAndSetIfChanged(ref _messageUser, value); }
        }

        private bool _CanNavigateNext;

        public override bool CanNavigateNext
        {
            get { return _CanNavigateNext; }
            set { this.RaiseAndSetIfChanged(ref _CanNavigateNext, value); }
        }
        private void UpdateCanNavigateNext()
        {
            if (!string.IsNullOrEmpty(_MailAddress) && !string.IsNullOrEmpty(_Password) && _Password.Length >= 9)
            {

                if (_Password.Length == 11) //second.CheckConnectionDB(_MailAddress, _Password))
                {
                    CanNavigateNext = true;
                    MessageForeground = Brushes.Green;
                    messageUser = "Подключение успешно установлено";
                }
                else
                {
                    CanNavigateNext = false;
                    MessageForeground = Brushes.Red;
                    messageUser = "Не удалось установить соединение!";
                }
            }
        }
    }
}
