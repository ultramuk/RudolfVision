using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RudolfApp.Utils
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (() => true);
        }

        public bool CanExecute(object parameter) => _canExecute();

        public async void Execute(object parameter)
        {
            try
            {
                await _execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine("AsyncRelayCommand 실행 중 예외 발생: " + ex.Message);
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
