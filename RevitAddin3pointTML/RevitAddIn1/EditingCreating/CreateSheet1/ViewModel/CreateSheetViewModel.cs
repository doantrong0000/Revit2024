using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitAddIn1.EditingCreating.CreateSheet1.Model;
using RevitAddIn1.EditingCreating.CreateSheet1.View;
using RevitAddIn1.Parameter.RenameSheet.Model;

namespace RevitAddIn1.EditingCreating.CreateSheet1.ViewModel
{
    public class CreateSheetViewModel : ObservableObject
    {
        public CreateSheetView  RenameSheetView { get; set; }
        private List<CreateSheetModel> CreateSheetModels = new List<CreateSheetModel>();
      
        private List<CreateSheetModel> existingSheets = new List<CreateSheetModel>();
      

     

        public RelayCommand OkCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        private Document doc;

        public CreateSheetViewModel(Document doc)
        {
            this.doc = doc;
            OkCommand = new RelayCommand(Run);
            CloseCommand = new RelayCommand(Close);

            GetData();
        }

        void GetData()
        {
           

        }

        void Run()
        {
           
        }

        void Close()
        {
           
        }
    }
}
