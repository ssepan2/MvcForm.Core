using System;   
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
//using System.Threading.Tasks;
using Ssepan.Application.Core;
using Ssepan.Utility.Core;

namespace MVCLibrary.Core
{
    /// <summary>
    /// run-time model; relies on settings
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MVCModel :
        ModelBase
    {
        #region Declarations
        #endregion Declarations

        #region Constructors
        public MVCModel()
        {
            //init SomeComponent property backed by setting component
            if (SettingsController<MVCSettings>.Settings == null)
            {
                //ensures that there is a new instance of Settings backing persisted properties in model
                SettingsController<MVCSettings>.New();
 
            }
            Debug.Assert(SettingsController<MVCSettings>.Settings != null, "SettingsController<MVCSettings>.Settings != null");

            //init some other component property NOT backed by settings, but backed by model component
           StillAnotherComponent = new MVCModelComponent();
        }

        public MVCModel
        (
            Int32 someInt,
            Boolean someBoolean,
            String someString
        ) :
            this()
        {
            SomeInt = someInt;
            SomeBoolean = someBoolean;
            SomeString = someString;

        }

        public MVCModel
        (
            Int32 someInt,
            Boolean someBoolean,
            String someString,
            Int32 someOtherInt,
            Boolean someOtherBoolean,
            String someOtherString
        ) :
            this(someInt,someBoolean,someString)
        {
            SomeComponent.SomeOtherInt = someOtherInt;
            SomeComponent.SomeOtherBoolean = someOtherBoolean;
            SomeComponent.SomeOtherString = someOtherString;

        }

        public MVCModel
        (
            Int32 someInt,
            Boolean someBoolean,
            String someString,
            Int32 someOtherInt,
            Boolean someOtherBoolean,
            String someOtherString,
            Int32 stillAnotherInt,
            Boolean stillAnotherBoolean,
            String stillAnotherString
        ) :
            this(someInt,someBoolean,someString, someOtherInt, someOtherBoolean, someOtherString)
        {
            StillAnotherComponent.StillAnotherInt = stillAnotherInt;
            StillAnotherComponent.StillAnotherBoolean = stillAnotherBoolean;
            StillAnotherComponent.StillAnotherString = stillAnotherString;

        }
        #endregion Constructors


        #region IDisposable support
        ~MVCModel()
        {
            Dispose(false);
            //base.Finalize();//not called directly in C#; called by Destructor
        }

        //inherited; override if additional cleanup needed
        protected override void Dispose(Boolean disposeManagedResources)
        {
            // process only if mananged and unmanaged resources have
            // not been disposed of.
            if (!disposed)
            {
                try
                {
                    //Resources not disposed
                    if (disposeManagedResources)
                    {
                        // dispose managed resources
                        StillAnotherComponent = null;
                    }

                    disposed = true;
                }
                finally
                {
                    // dispose unmanaged resources
                    base.Dispose(disposeManagedResources);
                }
            }
            else
            {
                //Resources already disposed
            }
        }
        #endregion

        #region IEquatable<IModel>
        /// <summary>
        /// Compare property values of two specified Model objects.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override Boolean Equals(IModelComponent other)
        {
            Boolean returnValue = default(Boolean);
            MVCModel otherModel = default(MVCModel);

            try
            {
                otherModel = other as MVCModel;

                if (this == otherModel)
                {
                    returnValue = true;
                }
                else
                {
                    if (!base.Equals(other))
                    {
                        returnValue = false;
                    }
                    else if (this.SomeInt != otherModel.SomeInt)
                    {
                        returnValue = false;
                    }
                    else if (this.SomeBoolean != otherModel.SomeBoolean)
                    {
                        returnValue = false;
                    }
                    else if (this.SomeString != otherModel.SomeString)
                    {
                        returnValue = false;
                    }
                    else
                    {
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }

            return returnValue;
        }
        #endregion IEquatable<IModel>

        #region Properties
        private String[] _Args = default(String[]);
        public String[] Args
        {
            get {  return _Args; }
            set 
            {
                _Args = value;
                OnPropertyChanged("Args");
            }
        }

        public MVCSettingsComponent SomeComponent
        {
            get { return SettingsController<MVCSettings>.Settings.SomeComponent; }
            set 
            {

                SettingsController<MVCSettings>.Settings.SomeComponent = value;

                //OnPropertyChanged("SomeComponent");//not needed if fired by settings
            }
        }

        private MVCModelComponent _StillAnotherComponent = default(MVCModelComponent);
        public MVCModelComponent StillAnotherComponent
        {
            get { return _StillAnotherComponent; }
            set 
            {

                if (ModelController<MVCModel>.DefaultHandler != null)
                {
                    if (_StillAnotherComponent != null)
                    {
                        _StillAnotherComponent.PropertyChanged -= ModelController<MVCModel>.DefaultHandler;
                    }
                }

                _StillAnotherComponent = value;

                if (ModelController<MVCModel>.DefaultHandler != null)
                {
                    if (_StillAnotherComponent != null)
                    {
                        _StillAnotherComponent.PropertyChanged += ModelController<MVCModel>.DefaultHandler;
                    }
                }

                OnPropertyChanged("StillAnotherComponent");//needed because NOT backed by settings
            }
        }

        public Int32 SomeInt
        {
            get { return SettingsController<MVCSettings>.Settings.SomeInt; }
            set 
            { 
                SettingsController<MVCSettings>.Settings.SomeInt = value;
                //OnPropertyChanged("SomeInt");//not needed if fired by settings
            }
        }

        public Boolean SomeBoolean
        {
            get { return SettingsController<MVCSettings>.Settings.SomeBoolean; }
            set 
            { 
                SettingsController<MVCSettings>.Settings.SomeBoolean = value;
                //OnPropertyChanged("SomeBoolean");//not needed if fired by settings
            }
        }

        public String SomeString
        {
            get { return SettingsController<MVCSettings>.Settings.SomeString; }
            set 
            { 
                SettingsController<MVCSettings>.Settings.SomeString = value;
                //OnPropertyChanged("SomeString");//not needed if fired by settings
            }
        }
        #endregion Properties

        #region Methods
        #endregion Methods
    }
}
