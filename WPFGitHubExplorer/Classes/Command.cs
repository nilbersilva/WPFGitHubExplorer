using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFGitHubExplorer.Classes
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Func<object, Task> Action;

        public Command(Func<object, Task> action)
        {
            this.Action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (Action != null)
            {
                await Action.Invoke(parameter);
            }
        }
    }
}
