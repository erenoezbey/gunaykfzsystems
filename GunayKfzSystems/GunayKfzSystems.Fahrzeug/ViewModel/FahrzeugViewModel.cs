using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GunayKfzSystems.Core.AbstractViewModel;
using GunayKfzSystems.Interface.Database.MsAccess.Data;
using GunayKfzSystems.Interface.Database.MsAccess.Service;
using Prism.Logging;

namespace GunayKfzSystems.Fahrzeug.ViewModel
{


    [Export]
    public class FahrzeugViewModel : MasterDataViewModelBase
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


        private ObservableCollection<IFahrzeug> _fahrzeugsCollection;

        public ObservableCollection<IFahrzeug> FahrzeugCollection
        {
            get { return this._fahrzeugsCollection; }
            set
            {
                if (this._fahrzeugsCollection != value)
                {
                    this._fahrzeugsCollection = value;
                    this.OnPropertyChanged("FahrzeugCollection");
                }
            }
        }

        private IFahrzeug _selectedFahrzeug;

        public IFahrzeug SelectedFahrzeug
        {
            get { return this._selectedFahrzeug; }
            set
            {
                if (this._selectedFahrzeug != value)
                {
                    this._selectedFahrzeug = value;
                    this.OnSelectedFahrzeugChanged();
                    this.OnPropertyChanged("SelectedFahrzeug");
                }
            }
        }

        private void OnSelectedFahrzeugChanged()
        {
            this.IsEditing = false;
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


        [ImportingConstructor]
        public FahrzeugViewModel(ILoggerFacade logger, IMsAccessServiceModule msDatenbank)
        {
            this._logger = logger;
            this._dbMsAccessService = msDatenbank;


            this.RefreshFahrzeugList(this._dbMsAccessService);


        }

        private void RefreshFahrzeugList(IMsAccessServiceModule msDatenbank)
        {
            var bgWorker = new BackgroundWorker();
            bgWorker.RunWorkerCompleted += this.bgWorker_RunWorkerCompleted;
            bgWorker.DoWork += this.bgWorker_DoWork;
            bgWorker.RunWorkerAsync(msDatenbank);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.FahrzeugCollection = e.Result as ObservableCollection<IFahrzeug>;

            if (this.FahrzeugCollection != null)
            {
                this.SelectedFahrzeug = this.FahrzeugCollection.FirstOrDefault();
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


            var fahrzeuge = new ObservableCollection<IFahrzeug>(db.GetAllFahrzeug().OrderBy(m => m.ID));
            e.Result = fahrzeuge;
        }


        protected override void DeleteItemAction()
        {
            this._dbMsAccessService.DeleteFahrzeug(this.SelectedFahrzeug);
            this.SelectedFahrzeug = null;
            this.RefreshFahrzeugList(this._dbMsAccessService);

        }


        protected override void AddItemAction()
        {
            this.SelectedFahrzeug = null;


            var newArtikel = new Database.MsAccess.Data.Fahrzeug()
            {
                ID = 0,
                Fabrikat = "Tragen Sie das Fabrikat ein."

            };

            this.SelectedFahrzeug = newArtikel;
            this.IsEditing = true;

        }


        protected override void CancelAction()
        {
            this.IsEditing = false;
            this.RefreshFahrzeugList(this._dbMsAccessService);
        }


        protected override void SaveItemAction()
        {
            if (this.SelectedFahrzeug.ID != 0)
            {
                //update
                this._dbMsAccessService.UpdateFahrzeug(this.SelectedFahrzeug);
            }
            else
            {
                //insert
                var id = this._dbMsAccessService.InsertFahrzeug(this.SelectedFahrzeug);
                this.SelectedFahrzeug.ID = id;
            }
            this.RefreshFahrzeugList(this._dbMsAccessService);
        }

        protected override void EditItemAction()
        {
            this.IsEditing = true;
        }

    }

}
