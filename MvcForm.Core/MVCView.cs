//#define USE_CONFIG_FILEPATH
//#define USE_CUSTOM_VIEWMODEL
//#define DEBUG_MODEL_PROPERTYCHANGED
//#define DEBUG_SETTINGS_PROPERTYCHANGED

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Ssepan.Application.Core;
using Ssepan.Io.Core;
using Ssepan.Utility.Core;
using MVCLibrary.Core;
using MVCLibrary.Core.Properties;

namespace MvcForm.Core
{
    /// <summary>
    /// This is the View.
    /// </summary>
    public partial class MVCView :
        Form,
        INotifyPropertyChanged
    {
        #region Declarations
        protected Boolean disposed;
        
        private Boolean _ValueChangedProgrammatically;
        
        //cancellation hook
        Action cancelDelegate = null;
        protected MVCViewModel ViewModel = default(MVCViewModel);
        #endregion Declarations

        #region Constructors
        public MVCView()
        { 
            try
            {
                InitializeComponent();

                ////(re)define default output delegate
                //ConsoleApplication.defaultOutputDelegate = ConsoleApplication.messageBoxWrapperOutputDelegate;

                //subscribe to view's notifications
                this.PropertyChanged += PropertyChangedEventHandlerDelegate;

                InitViewModel();

                BindSizeAndLocation();
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }
        #endregion Constructors

        #region IDisposable
        //~MVCView()
        //{
        //    Dispose(false);
        //}

        //public virtual void Dispose()
        //{
        //    // dispose of the managed and unmanaged resources
        //    Dispose(true);

        //    // tell the GC that the Finalize process no longer needs
        //    // to be run for this object.
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposeManagedResources)
        //{
        //    // process only if mananged and unmanaged resources have
        //    // not been disposed of.
        //    if (!this.disposed)
        //    {
        //        //Resources not disposed
        //        if (disposeManagedResources)
        //        {
        //            // dispose managed resources
        //            //unsubscribe from model notifications
        //            if (this.PropertyChanged != null)
        //            {
        //                this.PropertyChanged -= PropertyChangedEventHandlerDelegate;
        //            }
        //        }
        //        // dispose unmanaged resources
        //        disposed = true;
        //    }
        //    else
        //    {
        //        //Resources already disposed
        //    }
        //}
        #endregion IDisposable

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            try
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            catch (Exception ex)
            {
                ViewModel.ErrorMessage = ex.Message;
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);

                throw;
            }
        }
        #endregion INotifyPropertyChanged

        #region PropertyChangedEventHandlerDelegates
        /// <summary>
        /// Note: model property changes update UI manually.
        /// Note: handle settings property changes manually.
        /// Note: because settings properties are a subset of the model 
        ///  (every settings property should be in the model, 
        ///  but not every model property is persisted to settings)
        ///  it is decided that for now the settigns handler will 
        ///  invoke the model handler as well.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PropertyChangedEventHandlerDelegate
        (
            Object sender,
            PropertyChangedEventArgs e
        )
        {
            try
            {
#region Model
                if (e.PropertyName == "IsChanged")
                {
                    //ConsoleApplication.defaultOutputDelegate(String.Format("{0}", e.PropertyName));
                    ApplySettings();
                }
                //Status Bar
                else if (e.PropertyName == "ActionIconIsVisible")
                {
                    StatusBarActionIcon.Visible = (ViewModel.ActionIconIsVisible);
                }
                else if (e.PropertyName == "ActionIconImage")
                {
                    StatusBarActionIcon.Image = (ViewModel != null ? ViewModel.ActionIconImage : (Image)null);
                }
                else if (e.PropertyName == "StatusMessage")
                {
                    //replace default action by setting control property
                    //skip status message updates after Viewmodel is null
                    StatusBarStatusMessage.Text = (ViewModel != null ? ViewModel.StatusMessage : (String)null);
                    //e = new PropertyChangedEventArgs(e.PropertyName + ".handled");

                    //ConsoleApplication.defaultOutputDelegate(String.Format("{0}", StatusMessage));
                }
                else if (e.PropertyName == "ErrorMessage")
                {
                    //replace default action by setting control property
                    //skip status message updates after Viewmodel is null
                    StatusBarErrorMessage.Text = (ViewModel != null ? ViewModel.ErrorMessage : (String)null);
                    //e = new PropertyChangedEventArgs(e.PropertyName + ".handled");

                    //ConsoleApplication.defaultOutputDelegate(String.Format("{0}", ErrorMessage));
                }
                else if (e.PropertyName == "CustomMessage")
                {
                    //replace default action by setting control property
                    StatusBarCustomMessage.Text = ViewModel.CustomMessage;
                    //e = new PropertyChangedEventArgs(e.PropertyName + ".handled");

                    //ConsoleApplication.defaultOutputDelegate(String.Format("{0}", ErrorMessage));
                }
                else if (e.PropertyName == "ErrorMessageToolTipText")
                {
                    StatusBarErrorMessage.ToolTipText = ViewModel.ErrorMessageToolTipText;
                }
                else if (e.PropertyName == "ProgressBarValue")
                {
                    StatusBarProgressBar.Value = ViewModel.ProgressBarValue;
                }
                else if (e.PropertyName == "ProgressBarMaximum")
                {
                    StatusBarProgressBar.Maximum = ViewModel.ProgressBarMaximum;
                }
                else if (e.PropertyName == "ProgressBarMinimum")
                {
                    StatusBarProgressBar.Minimum = ViewModel.ProgressBarMinimum;
                }
                else if (e.PropertyName == "ProgressBarStep")
                {
                    StatusBarProgressBar.Step = ViewModel.ProgressBarStep;
                }
                else if (e.PropertyName == "ProgressBarIsMarquee")
                {
                    StatusBarProgressBar.Style = (ViewModel.ProgressBarIsMarquee ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks);
                }
                else if (e.PropertyName == "ProgressBarIsVisible")
                {
                    StatusBarProgressBar.Visible = (ViewModel.ProgressBarIsVisible);
                }
                else if (e.PropertyName == "DirtyIconIsVisible")
                {
                    StatusBarDirtyMessage.Visible = (ViewModel.DirtyIconIsVisible);
                }
                else if (e.PropertyName == "DirtyIconImage")
                {
                    StatusBarDirtyMessage.Image = ViewModel.DirtyIconImage;
                }
                //use if properties cannot be databound
                //else if (e.PropertyName == "SomeInt")
                //{
                //    //SettingsController<MVCSettings>.Settings.
                //    ModelController<MVCModel>.Model.IsChanged = true;
                //}
                //else if (e.PropertyName == "SomeBoolean")
                //{
                //    //SettingsController<MVCSettings>.Settings.
                //    ModelController<MVCModel>.Model.IsChanged = true;
                //}
                //else if (e.PropertyName == "SomeString")
                //{
                //    //SettingsController<MVCSettings>.Settings.
                //    ModelController<MVCModel>.Model.IsChanged = true;
                //}
                //else if (e.PropertyName == "SomeOtherBoolean")
                //{
                //    ConsoleApplication.defaultOutputDelegate(String.Format("SomeOtherBoolean: {0}", ModelController<MVCModel>.Model.SomeComponent.SomeOtherBoolean));
                //}
                //else if (e.PropertyName == "SomeOtherString")
                //{
                //    ConsoleApplication.defaultOutputDelegate(String.Format("SomeOtherString: {0}", ModelController<MVCModel>.Model.SomeComponent.SomeOtherString));
                //}
                //else if (e.PropertyName == "SomeComponent")
                //{
                //    ConsoleApplication.defaultOutputDelegate(String.Format("SomeComponent: {0},{1},{2}", ModelController<MVCModel>.Model.SomeComponent.SomeOtherInt, ModelController<MVCModel>.Model.SomeComponent.SomeOtherBoolean, ModelController<MVCModel>.Model.SomeComponent.SomeOtherString));
                //}
                //else if (e.PropertyName == "StillAnotherInt")
                //{
                //    ConsoleApplication.defaultOutputDelegate(String.Format("StillAnotherInt: {0}", ModelController<MVCModel>.Model.StillAnotherComponent.StillAnotherInt));
                //}
                //else if (e.PropertyName == "StillAnotherBoolean")
                //{
                //    ConsoleApplication.defaultOutputDelegate(String.Format("StillAnotherBoolean: {0}", ModelController<MVCModel>.Model.StillAnotherComponent.StillAnotherBoolean));
                //}
                //else if (e.PropertyName == "StillAnotherString")
                //{
                //    ConsoleApplication.defaultOutputDelegate(String.Format("StillAnotherString: {0}", ModelController<MVCModel>.Model.StillAnotherComponent.StillAnotherString));
                //}
                //else if (e.PropertyName == "StillAnotherComponent")
                //{
                //    ConsoleApplication.defaultOutputDelegate(String.Format("StillAnotherComponent: {0},{1},{2}", ModelController<MVCModel>.Model.StillAnotherComponent.StillAnotherInt, ModelController<MVCModel>.Model.StillAnotherComponent.StillAnotherBoolean, ModelController<MVCModel>.Model.StillAnotherComponent.StillAnotherString));
                //}
                else
                {
#if DEBUG_MODEL_PROPERTYCHANGED
                        ConsoleApplication.defaultOutputDelegate(String.Format("e.PropertyName: {0}", e.PropertyName));
#endif
                }
                #endregion Model

                #region Settings
                if (e.PropertyName == "Dirty")
                {
                    //apply settings that don't have databindings
                    ViewModel.DirtyIconIsVisible = (SettingsController<MVCSettings>.Settings.Dirty);
                }
                else
                {
#if DEBUG_SETTINGS_PROPERTYCHANGED
                    ConsoleApplication.defaultOutputDelegate(String.Format("e.PropertyName: {0}", e.PropertyName));
#endif
                }
                #endregion Settings
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }

        #endregion PropertyChangedEventHandlerDelegates

        #region Properties
        private String _ViewName = Program.APP_NAME;
        public String ViewName
        {
            get { return _ViewName; }
            set
            {
                _ViewName = value;
                OnPropertyChanged("ViewName");
            }
        }
        #endregion Properties

        #region Events
        #region Form Events
        /// <summary>
        /// Process Form key presses.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override Boolean ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Boolean returnValue = default(Boolean);
            try
            {
                // Implement the Escape / Cancel keystroke
                if (keyData == Keys.Cancel || keyData == Keys.Escape)
                {
                    //if a long-running cancellable-action has registered 
                    //an escapable-event, trigger it
                    InvokeActionCancel();

                    // This keystroke was handled, 
                    //don't pass to the control with the focus
                    returnValue = true;
                }
                else
                {
                    returnValue = base.ProcessCmdKey(ref msg, keyData);
                }

            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
            return returnValue;
        }

        private void View_Load(Object sender, EventArgs e)
        {
            try
            {
                ViewModel.StatusMessage = String.Format("{0} starting...", ViewName);
                    
                ViewModel.StatusMessage = String.Format("{0} started.", ViewName);

                _Run();
            }
            catch (Exception ex)
            {
                ViewModel.ErrorMessage = ex.Message;
                ViewModel.StatusMessage = String.Empty;

                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }

        private void View_FormClosing(Object sender, FormClosingEventArgs e)
        {
            try
            {
                ViewModel.StatusMessage = String.Format("{0} completing...", ViewName);
                
                DisposeSettings();

                ViewModel.StatusMessage = String.Format("{0} completed.", ViewName);
            }
            catch (Exception ex)
            {
                ViewModel.ErrorMessage = ex.Message.ToString();
                ViewModel.StatusMessage = "";

                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
            finally
            {
                ViewModel = null;
            }
        }
        #endregion Form Events

        #region Menu Events
        private void menuFileNew_Click(Object sender, EventArgs e)
        {
            ViewModel.FileNew();
        }

        private void menuFileOpen_Click(Object sender, EventArgs e)
        {
            ViewModel.FileOpen();
        }

        private void menuFileSave_Click(Object sender, EventArgs e)
        {
            ViewModel.FileSave();
        }

        private void menuFileSaveAs_Click(Object sender, EventArgs e)
        {
            ViewModel.FileSaveAs();
        }

        private void menuFilePrint_Click(object sender, EventArgs e)
        {
            ViewModel.FilePrint();
        }

        private void menuFileExit_Click(Object sender, EventArgs e)
        {
            ViewModel.FileExit();
        }

        private void menuEditUndo_Click(object sender, EventArgs e)
        {
            ViewModel.EditUndo();
        }

        private void menuEditRedo_Click(object sender, EventArgs e)
        {
            ViewModel.EditRedo();
        }

        private void menuEditSelectAll_Click(object sender, EventArgs e)
        {
            ViewModel.EditSelectAll();
        }

        private void menuEditCut_Click(object sender, EventArgs e)
        {
            ViewModel.EditCut();
        }

        private void menuEditCopy_Click(Object sender, EventArgs e)
        {
            ViewModel.EditCopy();
        }

        private void menuEditPaste_Click(object sender, EventArgs e)
        {
            ViewModel.EditPaste();
        }

        private void menuEditDelete_Click(object sender, EventArgs e)
        {
            ViewModel.EditDelete();
        }

        private void menuEditFind_Click(object sender, EventArgs e)
        {
            ViewModel.EditFind();
        }

        private void menuEditReplace_Click(object sender, EventArgs e)
        {
            ViewModel.EditReplace();
        }

        private void menuEditRefresh_Click(object sender, EventArgs e)
        {
            ViewModel.EditRefresh();
        }

        private void menuEditPreferences_Click(object sender, EventArgs e)
        {
            ViewModel.EditPreferences();
        }

        private void menuEditProperties_Click(Object sender, EventArgs e)
        {
            ViewModel.EditProperties();
        }

        private void menuHelpContents_Click(object sender, EventArgs e)
        {
            ViewModel.HelpContents();
        }

        private void menuHelpIndex_Click(object sender, EventArgs e)
        {
            ViewModel.HelpIndex();
        }

        private void menuHelpOnlineHelp_Click(object sender, EventArgs e)
        {
            ViewModel.HelpOnHelp();
        }

        private void menuHelpLicenceInformation_Click(object sender, EventArgs e)
        {
            ViewModel.HelpLicenceInformation();
        }

        private void menuHelpCheckForUpdates_Click(object sender, EventArgs e)
        {
            ViewModel.HelpCheckForUpdates();
        }

        private void menuHelpAbout_Click(Object sender, EventArgs e)
        {
            ViewModel.HelpAbout<AssemblyInfo>();
        }
        #endregion Menu Events

        #region Control Events
        private void cmdRun_Click(Object sender, EventArgs e)
        {
            ViewModel.DoSomething();
            //ViewModel.CustomMessage = "blah";//done in DoSomething
        }
        #endregion Control Events
        #endregion Events

        #region Methods
        #region FormAppBase
        protected void InitViewModel()
        {
            try
            {
                //tell controller how model should notify view about non-persisted properties AND including model properties that may be part of settings
                ModelController<MVCModel>.DefaultHandler = PropertyChangedEventHandlerDelegate;

                //tell controller how settings should notify view about persisted properties
                SettingsController<MVCSettings>.DefaultHandler = PropertyChangedEventHandlerDelegate;

                InitModelAndSettings();

                FileDialogInfo settingsFileDialogInfo =
                    new FileDialogInfo
                    (
                        SettingsController<MVCSettings>.FILE_NEW,
                        null,
                        null,
                        MVCSettings.FileTypeExtension,
                        MVCSettings.FileTypeDescription,
                        MVCSettings.FileTypeName,
                        new String[] 
                    { 
                        "XML files (*.xml)|*.xml", 
                        "All files (*.*)|*.*" 
                    },
                        false,
                        default(Environment.SpecialFolder),
                        Environment.GetFolderPath(Environment.SpecialFolder.Personal).WithTrailingSeparator()
                    );

                //set dialog caption
                settingsFileDialogInfo.Title = this.Text;

                //class to handle standard behaviors
                ViewModelController<Bitmap, MVCViewModel>.New
                (
                    ViewName,
                    new MVCViewModel
                    (
                        this.PropertyChangedEventHandlerDelegate,
                        new Dictionary<String, Bitmap>() 
                        { //TODO:ideally, should get these from library, but items added did not generated into resource class.
                            { "New", MvcForm.Core.Properties.Resources.New }, 
                            { "Open", MvcForm.Core.Properties.Resources.Open },
                            { "Save", MvcForm.Core.Properties.Resources.Save },
                            { "Print", MvcForm.Core.Properties.Resources.Print },
                            { "Undo", MvcForm.Core.Properties.Resources.Undo },
                            { "Redo", MvcForm.Core.Properties.Resources.Redo },
                            { "Cut", MvcForm.Core.Properties.Resources.Cut },
                            { "Copy", MvcForm.Core.Properties.Resources.Copy },
                            { "Paste", MvcForm.Core.Properties.Resources.Paste },
                            { "Delete", MvcForm.Core.Properties.Resources.Delete },
                            { "Find", MvcForm.Core.Properties.Resources.Find },
                            { "Replace", MvcForm.Core.Properties.Resources.Replace },
                            { "Refresh", MvcForm.Core.Properties.Resources.Refresh },
                            { "Preferences", MvcForm.Core.Properties.Resources.Preferences },
                            { "Properties", MvcForm.Core.Properties.Resources.Properties },
                            { "Contents", MvcForm.Core.Properties.Resources.Contents },
                            { "About", MvcForm.Core.Properties.Resources.About }
                        },
                        settingsFileDialogInfo
                    )
                );

                //select a viewmodel by view name
                ViewModel = ViewModelController<Bitmap, MVCViewModel>.ViewModel[ViewName];

                BindFormUi();

                //Init config parameters
                if (!LoadParameters())
                {
                    throw new Exception(String.Format("Unable to load config file parameter(s)."));
                }

                //DEBUG:filename coming in is being converted/passed as DOS 8.3 format equivalent
                //Load
                if ((SettingsController<MVCSettings>.FilePath == null) || (SettingsController<MVCSettings>.Filename == SettingsController<MVCSettings>.FILE_NEW))
                {
                    //NEW
                    ViewModel.FileNew();
                }
                else
                {
                    //OPEN
                    ViewModel.FileOpen(false);
                }

#if debug
            //debug view
            menuEditProperties_Click(sender, e);
#endif

                //Display dirty state
                ModelController<MVCModel>.Model.Refresh();
            }
            catch (Exception ex)
            {
                if (ViewModel != null)
                {
                    ViewModel.ErrorMessage = ex.Message;
                }
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }

        protected void InitModelAndSettings()
        {
            //create Settings before first use by Model
            if (SettingsController<MVCSettings>.Settings == null)
            {
                SettingsController<MVCSettings>.New();
            }
            //Model properties rely on Settings, so don't call Refresh before this is run.
            if (ModelController<MVCModel>.Model == null)
            {
                ModelController<MVCModel>.New();
            }
        }

        protected void DisposeSettings()
        {
            //save user and application settings 
            Properties.Settings.Default.Save();

            if (SettingsController<MVCSettings>.Settings.Dirty)
            {
                //prompt before saving
                DialogResult dialogResult = MessageBox.Show("Save changes?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        {
                            //SAVE
                            ViewModel.FileSave();

                            break;
                        }
                    case DialogResult.No:
                        {
                            break;
                        }
                    default:
                        {
                            throw new InvalidEnumArgumentException();
                        }
                }
            }

            //unsubscribe from model notifications
            ModelController<MVCModel>.Model.PropertyChanged -= PropertyChangedEventHandlerDelegate;
        }

        protected void _Run()
        {
            //MessageBox.Show("running", "MVC", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        #endregion FormAppBase

        #region Utility
        /// <summary>
        /// Bind static Model controls to Model Controller
        /// </summary>
        private void BindFormUi()
        {
            try
            {
                //Form

                //Controls
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Bind Model controls to Model Controller
        /// </summary>
        private void BindModelUi()
        {
            try
            {
                BindField<TextBox, MVCModel>(txtSomeInt, ModelController<MVCModel>.Model, "SomeInt");
                BindField<TextBox, MVCModel>(txtSomeString, ModelController<MVCModel>.Model, "SomeString");
                BindField<CheckBox, MVCModel>(chkSomeBoolean, ModelController<MVCModel>.Model, "SomeBoolean", "Checked");

                BindField<TextBox, MVCModel>(txtSomeOtherInt, ModelController<MVCModel>.Model, "SomeComponent.SomeOtherInt");
                BindField<TextBox, MVCModel>(txtSomeOtherString, ModelController<MVCModel>.Model, "SomeComponent.SomeOtherString");
                BindField<CheckBox, MVCModel>(chkSomeOtherBoolean, ModelController<MVCModel>.Model, "SomeComponent.SomeOtherBoolean", "Checked");
                
                BindField<TextBox, MVCModel>(txtStillAnotherInt, ModelController<MVCModel>.Model, "StillAnotherComponent.StillAnotherInt");
                BindField<TextBox, MVCModel>(txtStillAnotherString, ModelController<MVCModel>.Model, "StillAnotherComponent.StillAnotherString");
                BindField<CheckBox, MVCModel>(chkStillAnotherBoolean, ModelController<MVCModel>.Model, "StillAnotherComponent.StillAnotherBoolean", "Checked");
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }

        private void BindField<TControl, TModel>
        (
            TControl fieldControl,
            TModel model,
            String modelPropertyName,
            String controlPropertyName = "Text",
            Boolean formattingEnabled = false,
            DataSourceUpdateMode dataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged,
            Boolean reBind = true
        )
            where TControl : Control
        {
            try
            {
                fieldControl.DataBindings.Clear();
                if (reBind)
                {
                    fieldControl.DataBindings.Add(controlPropertyName, model, modelPropertyName, formattingEnabled, dataSourceUpdateMode);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Apply Settings to viewer.
        /// </summary>
        private void ApplySettings()
        {
            try
            {
                _ValueChangedProgrammatically = true;

                //apply settings that have databindings
                BindModelUi();

                //apply settings that shouldn't use databindings

                //apply settings that can't use databindings
                Text = Path.GetFileName(SettingsController<MVCSettings>.Filename) + " - " + ViewName;

                //apply settings that don't have databindings
                ViewModel.DirtyIconIsVisible = (SettingsController<MVCSettings>.Settings.Dirty);

                _ValueChangedProgrammatically = false;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Set function button and menu to enable value, and cancel button to opposite.
        /// For now, do only disabling here and leave enabling based on biz logic 
        ///  to be triggered by refresh?
        /// </summary>
        /// <param name="functionButton"></param>
        /// <param name="functionMenu"></param>
        /// <param name="cancelButton"></param>
        /// <param name="enable"></param>
        private void SetFunctionControlsEnable
        (
            Button functionButton,
            ToolStripButton functionToolbarButton,
            ToolStripMenuItem functionMenu,
            Button cancelButton,
            Boolean enable
        )
        {
            try
            {
                //stand-alone button
                if (functionButton != null)
                {
                    functionButton.Enabled = enable;
                }

                //toolbar button
                if (functionToolbarButton != null)
                {
                    functionToolbarButton.Enabled = enable;
                }

                //menu item
                if (functionMenu != null)
                {
                    functionMenu.Enabled = enable;
                }

                //stand-alone cancel button
                if (cancelButton != null)
                {
                    cancelButton.Enabled = !enable;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Invoke any delegate that has been registered 
        ///  to cancel a long-running background process.
        /// </summary>
        private void InvokeActionCancel()
        {
            try
            {
                //execute cancellation hook
                if (cancelDelegate != null)
                {
                    cancelDelegate();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Load from app config; override with command line if present
        /// </summary>
        /// <returns></returns>
        private Boolean LoadParameters()
        {
            Boolean returnValue = default(Boolean);
#if USE_CONFIG_FILEPATH
            String _settingsFilename = default(String);
#endif

            try
            {
                if ((Program.Filename != null) && (Program.Filename != SettingsController<MVCSettings>.FILE_NEW))
                {
                    //got filename from command line
                    //DEBUG:filename coming in is being converted/passed as DOS 8.3 format equivalent
                    if (!RegistryAccess.ValidateFileAssociation(Application.ExecutablePath, "." + MVCSettings.FileTypeExtension))
                    {
                        throw new ApplicationException(String.Format("Settings filename not associated: '{0}'.\nCheck filename on command line.", Program.Filename));
                    }
                    //it passed; use value from command line
                }
                else
                {
#if USE_CONFIG_FILEPATH
                    //get filename from app.config
                    if (!Configuration.ReadString("SettingsFilename", out _settingsFilename))
                    {
                        throw new ApplicationException(String.Format("Unable to load SettingsFilename: {0}", "SettingsFilename"));
                    }
                    if ((_settingsFilename == null) || (_settingsFilename == SettingsController<MVCSettings>.FILE_NEW))
                    {
                        throw new ApplicationException(String.Format("Settings filename not set: '{0}'.\nCheck SettingsFilename in app config file.", _settingsFilename));
                    }
                    //use with the supplied path
                    SettingsController<MVCSettings>.Filename = _settingsFilename;

                    if (Path.GetDirectoryName(_settingsFilename) == String.Empty)
                    {
                        //supply default path if missing
                        SettingsController<MVCSettings>.Pathname = Environment.GetFolderPath(Environment.SpecialFolder.Personal).WithTrailingSeparator();
                    }
#endif
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                //throw;
            }
            return returnValue;
        }

        private void BindSizeAndLocation()
        {
            //Note:Size must be done after InitializeComponent(); do Location this way as well.--SJS
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::MvcForm.Core.Properties.Settings.Default, "Location", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("ClientSize", global::MvcForm.Core.Properties.Settings.Default, "Size", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ClientSize = global::MvcForm.Core.Properties.Settings.Default.Size;
            this.Location = global::MvcForm.Core.Properties.Settings.Default.Location;
        }
        #endregion Utility

        #endregion Methods
    }
}
