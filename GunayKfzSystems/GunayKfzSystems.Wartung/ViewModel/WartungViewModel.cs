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

namespace GunayKfzSystems.Wartung.ViewModel
{


    [Export]
    public class WartungViewModel : MasterDataViewModelBase
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


        private ObservableCollection<IVorfallKopfdaten> _WartungsCollection;

        public ObservableCollection<IVorfallKopfdaten> WartungCollection
        {
            get { return this._WartungsCollection; }
            set
            {
                if (this._WartungsCollection != value)
                {
                    this._WartungsCollection = value;
                    this.OnPropertyChanged("WartungCollection");
                }
            }
        }

        private IVorfallKopfdaten _selectedWartung;

        public IVorfallKopfdaten SelectedWartung
        {
            get { return this._selectedWartung; }
            set
            {
                if (this._selectedWartung != value)
                {
                    this._selectedWartung = value;
                    this.OnSelectedWartungChanged();
                    this.OnPropertyChanged("SelectedWartung");
                }
            }
        }

        private void OnSelectedWartungChanged()
        {

            this.SelectedWartung.VorfallCollection = new List<IVorfall>();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["VorfallID"] = this.SelectedWartung.ID;
            this.SelectedWartung.VorfallCollection = this._dbMsAccessService.GetAllVorfall(parameters);



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
        public WartungViewModel(ILoggerFacade logger, IMsAccessServiceModule msDatenbank)
        {
            this._logger = logger;
            this._dbMsAccessService = msDatenbank;


            this.RefreshWartungList(this._dbMsAccessService);


        }

        private void RefreshWartungList(IMsAccessServiceModule msDatenbank)
        {
            var bgWorker = new BackgroundWorker();
            bgWorker.RunWorkerCompleted += this.bgWorker_RunWorkerCompleted;
            bgWorker.DoWork += this.bgWorker_DoWork;
            bgWorker.RunWorkerAsync(msDatenbank);
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.WartungCollection = e.Result as ObservableCollection<IVorfallKopfdaten>;

            if (this.WartungCollection != null)
            {
                this.SelectedWartung = this.WartungCollection.FirstOrDefault();
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
            var Wartunge = new ObservableCollection<IVorfallKopfdaten>(db.GetAllVorfallKopfdaten().OrderBy(m => m.ID));
            e.Result = Wartunge;
        }


        protected override void DeleteItemAction()
        {
            this._dbMsAccessService.DeleteWartung(this.SelectedWartung);
            this.SelectedWartung = null;
            this.RefreshWartungList(this._dbMsAccessService);

        }


        protected override void AddItemAction()
        {
            this.SelectedWartung = null;


            var newArtikel = new Database.MsAccess.Data.VorfallKopfdaten();
            {

            };

            this.SelectedWartung = newArtikel;
            this.IsEditing = true;

        }


        protected override void CancelAction()
        {
            this.IsEditing = false;
            this.RefreshWartungList(this._dbMsAccessService);
        }


        protected override void SaveItemAction()
        {
            if (this.SelectedWartung.ID != 0)
            {
                //update
                this._dbMsAccessService.UpdateWartung(this.SelectedWartung);
            }
            else
            {
                //insert
                var id = this._dbMsAccessService.InsertWartung(this.SelectedWartung);
                this.SelectedWartung.ID = id;
            }
            this.RefreshWartungList(this._dbMsAccessService);
        }

        protected override void EditItemAction()
        {
            this.IsEditing = true;
        }

    }

}
