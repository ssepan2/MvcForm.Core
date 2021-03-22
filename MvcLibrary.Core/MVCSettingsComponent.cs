using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Ssepan.Application.Core;
using Ssepan.Utility.Core;

namespace MVCLibrary.Core
{
    /// <summary>
    /// component of persisted settings; 
    /// run-time model depends on this;
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class MVCSettingsComponent :
        SettingsComponentBase
    {
        #region Declarations
        #endregion Declarations

        #region Constructors
        public MVCSettingsComponent()
        {
        }

        public MVCSettingsComponent
        (
            Int32 someOtherInt,
            Boolean someOtherBoolean,
            String someOtherString
        ) :
            this()
        {
            SomeOtherInt = someOtherInt;
            SomeOtherBoolean = someOtherBoolean;
            SomeOtherString = someOtherString;
        }
        #endregion Constructors

        #region IDisposable support
        ~MVCSettingsComponent()
        {
            Dispose(false);
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
            
        #region IEquatable<ISettingsComponent>
        /// <summary>
        /// Compare property values of two specified Settings objects.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override Boolean Equals(ISettingsComponent other)
        {
            Boolean returnValue = default(Boolean);
            MVCSettingsComponent otherModel = default(MVCSettingsComponent);

            try
            {
                otherModel = other as MVCSettingsComponent;

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
                    else if (this.SomeOtherInt != otherModel.SomeOtherInt)
                    {
                        returnValue = false;
                    }
                    else if (this.SomeOtherBoolean != otherModel.SomeOtherBoolean)
                    {
                        returnValue = false;
                    }
                    else if (this.SomeOtherString != otherModel.SomeOtherString)
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
        #endregion IEquatable<ISettingsComponent>

        #region Properties
        [XmlIgnore]
        public override Boolean Dirty
        {
            get
            {
                Boolean returnValue = default(Boolean);

                try
                {
                    if (base.Dirty)
                    {
                        returnValue = true;
                    }
                    else if (_SomeOtherInt != __SomeOtherInt)
                    {
                        returnValue = true;
                    }
                    else if (_SomeOtherBoolean != __SomeOtherBoolean)
                    {
                        returnValue = true;
                    }
                    else if (_SomeOtherString != __SomeOtherString)
                    {
                        returnValue = true;
                    }
                    else
                    {
                        returnValue = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                    throw;
                }

                return returnValue;
            }
        }

        #region Persisted Properties
        private Int32 __SomeOtherInt = default(Int32);
        private Int32 _SomeOtherInt = default(Int32);
        public Int32 SomeOtherInt
        {
            get { return _SomeOtherInt; }
            set 
            {
                _SomeOtherInt = value;
                OnPropertyChanged("SomeOtherInt");
            }
        }

        private Boolean __SomeOtherBoolean = default(Boolean);
        private Boolean _SomeOtherBoolean = default(Boolean);
        public Boolean SomeOtherBoolean
        {
            get { return _SomeOtherBoolean; }
            set 
            { 
                _SomeOtherBoolean = value;
                OnPropertyChanged("SomeOtherBoolean");
            }
        }

        private String __SomeOtherString = String.Empty;//default(String);
        private String _SomeOtherString = String.Empty;//default(String);
        public String SomeOtherString
        {
            get { return _SomeOtherString; }
            set 
            { 
                _SomeOtherString = value;
                OnPropertyChanged("SomeOtherString");
            }
        }
        #endregion Persisted Properties
        #endregion Properties

        #region Methods
        /// <summary>
        /// Copies property values from source working fields to detination working fields, then optionally syncs destination.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="sync"></param>
        public override void CopyTo(ISettingsComponent destination, Boolean sync)
        {
            MVCSettingsComponent destinationSettings = default(MVCSettingsComponent);

            try
            {
                destinationSettings = destination as MVCSettingsComponent;

                destinationSettings.SomeOtherInt = this.SomeOtherInt;
                destinationSettings.SomeOtherBoolean = this.SomeOtherBoolean;
                destinationSettings.SomeOtherString = this.SomeOtherString;

                base.CopyTo(destination, sync);//also checks and optionally performs sync
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }

        /// <summary>
        /// Syncs property values by copying from working fields to reference fields.
        /// </summary>
        public override void Sync()
        {
            try
            {
                __SomeOtherInt = _SomeOtherInt;
                __SomeOtherBoolean = _SomeOtherBoolean;
                __SomeOtherString = _SomeOtherString;

                base.Sync();

                if (Dirty)
                {
                    throw new ApplicationException("Sync failed.");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, MethodBase.GetCurrentMethod(), EventLogEntryType.Error);
                throw;
            }
        }
        #endregion Methods
    }
}
