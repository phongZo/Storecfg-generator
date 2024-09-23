// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.RelayCommand
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using System;
using System.Windows.Input;

namespace StorecfgGenerator
{
  public class RelayCommand : ICommand
  {
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
      this._execute = execute ?? throw new ArgumentNullException(nameof (execute));
      this._canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
      add => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
      return this._canExecute == null || this._canExecute(parameter);
    }

    public void Execute(object parameter) => this._execute(parameter);
  }
}
