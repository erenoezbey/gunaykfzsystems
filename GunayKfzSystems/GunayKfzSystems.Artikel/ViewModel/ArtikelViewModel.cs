using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using GunayKfzSystems.Core.AbstractViewModel;
using GunayKfzSystems.Interface.Database.MsAccess.Data;
using GunayKfzSystems.Interface.Database.MsAccess.Service;
using Prism.Logging;

namespace GunayKfzSystems.Artikel.ViewModel
{

    [Export]
    public class ArtikelViewModel : MasterDataViewModelBase
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


        private ObservableCollection<IArtikel> _artikelCollection;

        public ObservableCollection<IArtikel> ArtikelCollection
        {
            get { return this._artikelCollection; }
            set
            {
                if (this._artikelCollection != value)
                {
                    this._artikelCollection = value;
                    this.OnPropertyChanged("ArtikelCollection");
                }
            }
        }

        private IArtikel _selectedArtikel;

        public IArtikel SelectedArtikel
        {
            get { return this._selectedArtikel; }
            set
            {
                if (this._selectedArtikel != value)
                {
                    this._selectedArtikel = value;
                    this.OnSelectedArtikelChanged();
                    this.OnPropertyChanged("SelectedArtikel");
                }
            }
        }

        private void OnSelectedArtikelChanged()
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
        public ArtikelViewModel(ILoggerFacade logger, IMsAccessServiceModule msDatenbank)
        {
            this._logger = logger;
            this._dbMsAccessService = msDatenbank;


            this.RefreshArtikelList(this._dbMsAccessService);


        }

        private void RefreshArtikelList(IMsAccessServiceModule msDatenbank)
        {
            var bgWorker = new BackgroundWorker();
            bgWorker.RunWorkerCompleted += this.bgWorker_RunWorkerCompleted;
            bgWorker.DoWork += this.bgWorker_DoWork;
            bgWorker.RunWorkerAsync(msDatenbank);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ArtikelCollection = e.Result as ObservableCollection<IArtikel>;

            if (this.ArtikelCollection != null)
            {
                this.SelectedArtikel = this.ArtikelCollection.FirstOrDefault();
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
            var artikel = new ObservableCollection<IArtikel>(db.GetAllArtikel().OrderBy(m => m.Artikelname));
            e.Result = artikel;
        }


        protected override void DeleteItemAction()
        {
            this._dbMsAccessService.DeleteArtikel(this.SelectedArtikel);
            this.SelectedArtikel = null;
            this.RefreshArtikelList(this._dbMsAccessService);

        }


        protected override void AddItemAction()
        {
            this.SelectedArtikel = null;


            var newArtikel = new Database.MsAccess.Data.Artikel()
            {
                ArtikelNr = 0,
                Artikelname = "Trage hier den neuen Artikelnamen ein",
                Einzelpreis = 0,
                ArtikelNrbeiLieferant = "Trage hier Informationen zum Lieferanten ein",
                Artikelbeschreibung = ""
            };

            this.SelectedArtikel = newArtikel;
            this.IsEditing = true;

        }


        protected override void CancelAction()
        {
            this.IsEditing = false;
            this.RefreshArtikelList(this._dbMsAccessService);
        }


        protected override void SaveItemAction()
        {
            if (this.SelectedArtikel.ArtikelNr != 0)
            {
                //update
                this._dbMsAccessService.UpdateArtikel(this.SelectedArtikel);
            }
            else
            {
                //insert
                var id = this._dbMsAccessService.InsertArtikel(this.SelectedArtikel);
                this.SelectedArtikel.ArtikelNr = id;
            }
            this.RefreshArtikelList(this._dbMsAccessService);
        }

        protected override void EditItemAction()
        {
            this.IsEditing = true;
        }

    }
}
