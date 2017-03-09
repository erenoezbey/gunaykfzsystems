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

namespace GunayKfzSystems.Kunden.ViewModel
{


    [Export]
    public class KundenViewModel : MasterDataViewModelBase
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


        private ObservableCollection<IKunden> _kundenCollection;

        public ObservableCollection<IKunden> KundenCollection
        {
            get { return this._kundenCollection; }
            set
            {
                if (this._kundenCollection != value)
                {
                    this._kundenCollection = value;
                    this.OnPropertyChanged("KundenCollection");
                }
            }
        }

        private IKunden _selectedKunden;

        public IKunden SelectedKunden
        {
            get { return this._selectedKunden; }
            set
            {
                if (this._selectedKunden != value)
                {
                    this._selectedKunden = value;
                    this.OnSelectedKundenChanged();
                    this.OnPropertyChanged("SelectedKunden");
                }
            }
        }

        private void OnSelectedKundenChanged()
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
        public KundenViewModel(ILoggerFacade logger, IMsAccessServiceModule msDatenbank)
        {
            this._logger = logger;
            this._dbMsAccessService = msDatenbank;


            this.RefreshKundenList(this._dbMsAccessService);


        }

        private void RefreshKundenList(IMsAccessServiceModule msDatenbank)
        {
            var bgWorker = new BackgroundWorker();
            bgWorker.RunWorkerCompleted += this.bgWorker_RunWorkerCompleted;
            bgWorker.DoWork += this.bgWorker_DoWork;
            bgWorker.RunWorkerAsync(msDatenbank);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.KundenCollection = e.Result as ObservableCollection<IKunden>;

            if (this.KundenCollection != null)
            {
                this.SelectedKunden = this.KundenCollection.FirstOrDefault();
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
            var Kundene = new ObservableCollection<IKunden>(db.GetAllKunden().OrderByDescending(m => m.KundenNr));
            e.Result = Kundene;
        }


        protected override void DeleteItemAction()
        {
            this._dbMsAccessService.DeleteKunden(this.SelectedKunden);
            this.SelectedKunden = null;
            this.RefreshKundenList(this._dbMsAccessService);

        }


        protected override void AddItemAction()
        {
            this.SelectedKunden = null;


            var newArtikel = new Database.MsAccess.Data.Kunden()
            {
                KundenNr = 0,
                KontaktNachname = "Tragen Sie hier den Nachnachmen ein..."

            };

            this.SelectedKunden = newArtikel;
            this.IsEditing = true;

        }


        protected override void CancelAction()
        {
            this.IsEditing = false;
            this.RefreshKundenList(this._dbMsAccessService);
        }


        protected override void SaveItemAction()
        {
            if (this.SelectedKunden.KundenNr != 0)
            {
                //update
                this._dbMsAccessService.UpdateKunden(this.SelectedKunden);
            }
            else
            {
                //insert
                var id = this._dbMsAccessService.InsertKunden(this.SelectedKunden);
                this.SelectedKunden.KundenNr = id;
            }
            this.RefreshKundenList(this._dbMsAccessService);
        }

        protected override void EditItemAction()
        {
            this.IsEditing = true;
        }

    }

}
