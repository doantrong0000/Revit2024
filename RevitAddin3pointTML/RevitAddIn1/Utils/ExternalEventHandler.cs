using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

namespace RevitAddIn1.Utils
{
    public class ExternalEventHandler :  IExternalEventHandler
    {
        protected static Action action;
        protected static ExternalEventHandler instance { get; set; }

        public static ExternalEventHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new ExternalEventHandler();
                return instance;
            }
        }

        protected static Autodesk.Revit.UI.ExternalEvent create { get; set; }

        public Autodesk.Revit.UI.ExternalEvent Create()
        {
            if (create == null)
                create = Autodesk.Revit.UI.ExternalEvent.Create((IExternalEventHandler)Instance);
            return create;
        }

        public void SetAction(Action parameter) => action = parameter;

        public async void Run()
        {
            try
            {
                create?.Raise();
                while (create.IsPending)
                    await Task.Delay(10);
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error)
                TaskDialog.Show("Error", ex.Message);
            }
        }

        public void Execute(UIApplication app)
        {
            if (app.ActiveUIDocument == null)
            {
                TaskDialog.Show("Notification", "No document, nothing to do.");
            }
            else
            {
                action?.Invoke();
            }
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

    }
}
