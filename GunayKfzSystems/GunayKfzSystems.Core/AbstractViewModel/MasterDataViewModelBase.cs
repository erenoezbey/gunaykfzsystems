using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace GunayKfzSystems.Core.AbstractViewModel
{
    public abstract class MasterDataViewModelBase : BindableBase
    {
        private ICommand _deleteItemCommand;
        private ICommand _addItemCommand;
        private ICommand _cancelCommand;
        private ICommand _saveItemCommand;
        private ICommand _editItemCommand;

        public ICommand DeleteItemCommand
        {
            get
            {
                return this._deleteItemCommand ??
                       (this._deleteItemCommand = new DelegateCommand(this.DeleteItemAction));
            }
        }

        public ICommand AddItemCommand
        {
            get
            {
                return this._addItemCommand ??
                       (this._addItemCommand = new DelegateCommand(this.AddItemAction));
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return this._cancelCommand ??
                       (this._cancelCommand = new DelegateCommand(this.CancelAction));
            }
        }

        public ICommand SaveItemCommand
        {
            get
            {
                return this._saveItemCommand ??
                       (this._saveItemCommand = new DelegateCommand(this.SaveItemAction));
            }
        }

        public ICommand EditItemCommand
        {
            get
            {
                return this._editItemCommand ??
                       (this._editItemCommand = new DelegateCommand(this.EditItemAction));
            }
        }

        protected abstract void DeleteItemAction();
        protected abstract void AddItemAction();
        protected abstract void CancelAction();
        protected abstract void SaveItemAction();
        protected abstract void EditItemAction();
    }

}
