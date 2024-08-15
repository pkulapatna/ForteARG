using ForteArg.Services;
using ForteARP.Module_FieldsSelect.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ForteARP.Module_FieldsSelect.ViewModels
{
    public class SelectItemsViewModel : BindableBase
    {
        private SelectItemModel _selItemModel;
        public Action CloseAction { get; set; }

        public DelegateCommand LoadedPageICommand { get; set; } //Load Page
        public DelegateCommand ClosedPageICommand { get; set; } //Close page
        public DelegateCommand SaveUpdateCommand { get; set; }  //Save Button
        public DelegateCommand ModifyCommand { get; set; }      //Modify Button
        public DelegateCommand CancelCommand { get; set; }  //Close Button
        public DelegateCommand OnCheckCommand { get; set; }     //Check Checkbox item in listbox 

        private ObservableCollection<string> _selectedHdrList;
        public ObservableCollection<string> SelectedHdrList
        {
            get { return _selectedHdrList; }
            set { SetProperty(ref _selectedHdrList, value); }
        }

        private ObservableCollection<CheckedListItem> _hdrListboxList;
        public ObservableCollection<CheckedListItem> AvailableHdrList
        {
            get { return _hdrListboxList; }
            set { SetProperty(ref _hdrListboxList, value); }
        }


        private List<string> sqlHdrList;
        public List<string> SqlHdrList
        {
            get { return sqlHdrList; }
            set { SetProperty(ref sqlHdrList, value); }
        }


        private bool _bModSetup = true;
        public bool BModifySetup
        {
            get { return _bModSetup; }
            set { SetProperty(ref _bModSetup, value); }
        }

        private bool _bOpenSetup = false;
        public bool OpenSetup
        {
            get { return _bOpenSetup; }
            set { SetProperty(ref _bOpenSetup, value); }
        }

        public SelectItemsViewModel()
        {
            //Main Window
            LoadedPageICommand = new DelegateCommand(LoadPageExecute, LoadPageCanExecute);
            ClosedPageICommand = new DelegateCommand(ClosedPageExecute, ClosedPageCanExecute);

            ModifyCommand = new DelegateCommand(ModifyCommandExecute, ModifyCommandCanExecute);

            SaveUpdateCommand = new DelegateCommand(SaveModExecute, SaveModCanExecute).ObservesProperty(() => OpenSetup);
            CancelCommand = new DelegateCommand(CancelCommandExecute, CancelCommandCanExecute).ObservesProperty(() => OpenSetup);
            OnCheckCommand = new DelegateCommand(OnCheckExecute, OnCheckCanExecute).ObservesProperty(() => OpenSetup);

        }

        private bool ModifyCommandCanExecute()
        {
            return true;
        }

        private void ModifyCommandExecute()
        {
            OpenSetup = true;
        }

        private bool CancelCommandCanExecute()
        {
            return OpenSetup;
        }

        private void CancelCommandExecute()
        {
            CloseAction();
        }

        private bool OnCheckCanExecute()
        {
            return OpenSetup;
        }

        private void OnCheckExecute()
        {
            ObservableCollection<string> NewList = new ObservableCollection<string>();
            ObservableCollection<string> orgList = SelectedHdrList;

            for (int i = 0; i < AvailableHdrList.Count; i++)
            {
                if (AvailableHdrList[i].IsChecked == true) NewList.Add(AvailableHdrList[i].Name);
            }

            if (orgList.Count > NewList.Count) //Remove item
            {
                IEnumerable<string> ItemRemove = orgList.Except(NewList);
                SelectedHdrList = _selItemModel.RemoveHdrItem(orgList, ItemRemove.ElementAt(0).ToString());
            }
            else //add item
            {
                IEnumerable<string> ItemAdd = NewList.Except(orgList);
                SelectedHdrList = _selItemModel.AddHdrItem(orgList, ItemAdd.ElementAt(0).ToString());
            }
        }

        private bool SaveModCanExecute()
        {
            return OpenSetup;
        }

        private void SaveModExecute()
        {
            _selItemModel.SaveModified_setting();
            _selItemModel.SaveXmlcolumnList(SelectedHdrList);
            CloseAction();
        }

        private bool ClosedPageCanExecute()
        {
            return true;
        }

        private void ClosedPageExecute()
        {
            OpenSetup = false;
        }

        private bool LoadPageCanExecute()
        {
            return true;
        }

        private void LoadPageExecute()
        {
            try
            {
                if (_selItemModel == null)
                    _selItemModel = new SelectItemModel();

                SelectedHdrList = new ObservableCollection<string>();
                SelectedHdrList = _selItemModel.GetSelectHrdCheckList();
                AvailableHdrList = _selItemModel.AvailableItemList;

                BModifySetup = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in SelectItemsViewModel LoadPageExecute " + ex.Message);
            }
        }
    }
}
