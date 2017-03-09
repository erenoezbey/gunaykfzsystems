using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GunayKfzSystems.Core.AbstractViewModel;
using GunayKfzSystems.Database.MsAccess.Data;
using GunayKfzSystems.Interface.Database.MsAccess.Data;
using GunayKfzSystems.Interface.Database.MsAccess.Service;
using Prism.Logging;

namespace GunayKfzSystems.Services.ViewModel
{
    [Export]
    public class ServicesViewModel : MasterDataViewModelBase

    {

        private ILoggerFacade _logger;


        private IMsAccessServiceModule _dbMsAccessService;


        private bool _isBusy;

        public bool IsBusy
        {
            get { return this._isBusy; }
            set
            {
                if (this._isBusy != value)
                {
                    this._isBusy = value;
                    this.OnPropertyChanged("IsBusy");
                }
            }
        }


        private ObservableCollection<IService> _serviceCollection;

        public ObservableCollection<IService> ServiceCollection
        {
            get { return this._serviceCollection; }
            set
            {
                if (this._serviceCollection != value)
                {
                    this._serviceCollection = value;
                    this.OnPropertyChanged("ServiceCollection");
                }
            }
        }

        private IService _selectedService;

        public IService SelectedService
        {
            get { return this._selectedService; }
            set
            {
                if (this._selectedService != value)
                {
                    this._selectedService = value;
                    this.OnSelectedServicesChanged();
                    this.OnPropertyChanged("SelectedService");
                }
            }
        }




        private bool _isEditing;

        public bool IsEditing
        {
            get { return this._isEditing; }
            set
            {
                if (this._isEditing != value)
                {
                    this._isEditing = value;
                    this.OnPropertyChanged("IsEditing");
                }
            }
        }


        private void OnSelectedServicesChanged()
        {
            this.IsEditing = false;
        }



        [ImportingConstructor]
        public ServicesViewModel(ILoggerFacade logger, IMsAccessServiceModule msDatenbank)
        {
            this._logger = logger;
            this._dbMsAccessService = msDatenbank;
            this.RefreshServiceList(this._dbMsAccessService);

        }

        private void RefreshServiceList(IMsAccessServiceModule dbMsAccessService)
        {
            var bgWorker = new BackgroundWorker();
            bgWorker.RunWorkerCompleted += this.bgWorker_RunWorkerCompleted;
            bgWorker.DoWork += this.bgWorker_DoWork;
            bgWorker.RunWorkerAsync(dbMsAccessService);
        }


        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ServiceCollection = e.Result as ObservableCollection<IService>;

            if (this.ServiceCollection != null)
            {
                this.SelectedService = this.ServiceCollection.FirstOrDefault();
            }
        }


        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var db = e.Argument as IMsAccessServiceModule;
            if (db == null)
            {
                e.Result = null;
                return;
            }
            var artikel = new ObservableCollection<IService>(db.GetAllServices().OrderBy(m => m.AtionName));
            e.Result = artikel;
        }


        #region Overrides of MasterDataViewModelBase

        protected override void DeleteItemAction()

        {
            this._dbMsAccessService.DeleteService(this.SelectedService);
            this.RefreshServiceList(this._dbMsAccessService);
        }

        protected override void AddItemAction()
        {

            var newService = new Service()
            {
                AtionName = "Bitte hier den Servicenamen eintragen!",
                Aktionpreis = 0,
                Bemerkung = "Bitte trage hier optional einen Preis ein oder lösche diese Bemerkung!"
                };
            this.SelectedService = newService;
            this.IsEditing = true;
        }

        protected override void CancelAction()
        {
            this.IsEditing = false;
            this.RefreshServiceList(this._dbMsAccessService);
        }


        protected override void SaveItemAction()
        {
            if (this.SelectedService.Id != 0)
            {
                //update
                this._dbMsAccessService.UpdateService(this.SelectedService);
            }
            else
            {
                //insert
                var id = this._dbMsAccessService.InsertService(this.SelectedService);
                this.SelectedService.Id = id;
            }
            this.RefreshServiceList(this._dbMsAccessService);
        }
        protected override void EditItemAction()
        {
            this.IsEditing = true;
        }

        #endregion
    }
}
