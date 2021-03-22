using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Ssepan.Utility.Core;
using Ssepan.Application.Core;
using Ssepan.Io.Core;
using MVCLibrary.Core;

namespace MvcForm.Core
{
    /// <summary>
    /// Note: this class can subclass the base without type parameters.
    /// </summary>
    public class MVCViewModel :
        FormsViewModel<Bitmap, MVCSettings, MVCModel, MVCView>
    {
        #region Declarations
        #endregion Declarations

        #region Constructors
        public MVCViewModel() { }//Note: not called, but need to be present to compile--SJS

        public MVCViewModel
        (
            PropertyChangedEventHandler propertyChangedEventHandlerDelegate,
            Dictionary<String, Bitmap> actionIconImages,
            FileDialogInfo settingsFileDialogInfo
        ) :
            base(propertyChangedEventHandlerDelegate, actionIconImages, settingsFileDialogInfo)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }
        #endregion Constructors

        #region Properties
        #endregion Properties

        #region Methods
        /// <summary>
        /// model specific, not generioc
        /// </summary>
        public void DoSomething()
        {
            StatusMessage = String.Empty;
            ErrorMessage = String.Empty;

            try
            {
                StartProgressBar
                (
                    "Doing something...",
                    null,
                    null, //_actionIconImages["Xxx"],
                    true,
                    33
                );

                ModelController<MVCModel>.Model.SomeBoolean = !ModelController<MVCModel>.Model.SomeBoolean;
                ModelController<MVCModel>.Model.SomeInt += 1;
                ModelController<MVCModel>.Model.SomeString = DateTime.Now.ToString();

                ModelController<MVCModel>.Model.SomeComponent.SomeOtherBoolean = !ModelController<MVCModel>.Model.SomeComponent.SomeOtherBoolean;
                ModelController<MVCModel>.Model.SomeComponent.SomeOtherInt += 1;
                ModelController<MVCModel>.Model.SomeComponent.SomeOtherString = DateTime.Now.ToString();

                ModelController<MVCModel>.Model.StillAnotherComponent.StillAnotherBoolean = !ModelController<MVCModel>.Model.StillAnotherComponent.StillAnotherBoolean;
                ModelController<MVCModel>.Model.StillAnotherComponent.StillAnotherInt += 1;
                ModelController<MVCModel>.Model.StillAnotherComponent.StillAnotherString = DateTime.Now.ToString();

                UpdateStatusBarMessages(null, null, DateTime.Now.ToLongTimeString());

                ModelController<MVCModel>.Model.Refresh();
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                StopProgressBar(null, String.Format("{0}", ex.Message));
            }
            finally
            {
                StopProgressBar("Did something.");
            }
        }
        #endregion Methods

    }
}
