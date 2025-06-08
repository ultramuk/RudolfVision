using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RudolfApp.Utils
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object?, Task> _executeAsync;
        private readonly Predicate<object?>? _canExecute;

        public AsyncRelayCommand(Func<object?, Task> executeAsync, Predicate<object?>? canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public async void Execute(object? parameter)
        {
            try
            {
                await _executeAsync(parameter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("AsyncRelayCommand 실행 중 예외 발생: " + ex.Message);
            }
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
