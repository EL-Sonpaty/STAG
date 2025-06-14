using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace STAG.ViewModels
{
    /// <summary>
	/// The Relay Command is an implementation of ICommand which allows an Action to be bound to a button in xaml such that it can be enabled/disabled 
	/// with a simple boolean or predicate
	/// </summary>
	/// <typeparam name="T">The type of object to call at execution of the button</typeparam>
	public class RelayCommand<T> : ICommand
    {
        readonly Action<T> _execute;
        readonly Predicate<T> _canExecute;

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            if (execute == null) { throw new ArgumentNullException("execute"); }
            _execute = execute;
            _canExecute = canExecute;
        }

        ///<summary>
        ///Defines the method that determines whether the command can execute in its current state.
        ///</summary>
        ///<param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null"/>.</param>
        ///<returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        ///<summary>
        ///Occurs when changes occur that affect whether or not the command should execute.
        ///</summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        ///<summary>
        ///Defines the method to be called when the command is invoked.
        ///</summary>
        ///<param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null"/>.</param>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }

    /// <summary>
    /// A <see cref="RelayCommand"/> which does not require a specific Type
    /// </summary>
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute)
            : base(param => execute()) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
            : base(param => execute(), param => canExecute()) { }
    }
}
