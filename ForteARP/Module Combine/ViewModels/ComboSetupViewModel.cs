using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForteARP.Module_Combine.ViewModels
{
    internal class ComboSetupViewModel : BindableBase
    {

        protected readonly IEventAggregator _eventAggregator;

        private bool _bModify = false;
        public bool BModify
        {
            get => _bModify;
            set => SetProperty(ref _bModify, value);
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
        _saveCommand ?? (_saveCommand =
            new DelegateCommand(SaveCommandExecute).ObservesCanExecute(() => BModify));
        private void SaveCommandExecute()
        {
            BModify = false;
            SaveValues();
        }

        private void SaveValues()
        {
            
        }

        private DelegateCommand _settingsCommand;
        public DelegateCommand SettingsCommand =>
        _settingsCommand ?? (_settingsCommand = new DelegateCommand(SettingsCommandExecute));
        private void SettingsCommandExecute()
        {
            BModify = true;
        }

        public ComboSetupViewModel(Prism.Events.IEventAggregator EventAggregator)
        {
            this._eventAggregator = EventAggregator;


        }
    }
}
