using System;
//using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
//using System.Linq;
//using System.Xml.Serialization;
using System.Reflection;
//using System.Runtime.Serialization;
//using System.Text;
using Ssepan.Application.Core;
using Ssepan.Utility.Core;

namespace MVCLibrary.Core
{
    /// <summary>
    /// component of non-persisted properties; 
    /// run-time model depends on this;
    /// does NOT rely on settings
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MVCModelComponent :
        ModelComponentBase
    {
        #region Declarations
        #endregion Declarations

        #region Constructors
        public MVCModelComponent()
        {
        }

        public MVCModelComponent
        (
            Int32 stillAnotherInt,
            Boolean stillAnotherBoolean,
            String stillAnotherString
        ) :
            this()
        {
            StillAnotherInt = stillAnotherInt;
            StillAnotherBoolean = stillAnotherBoolean;
            StillAnotherString = stillAnotherString;
        }
        #endregion Constructors

        #region IDisposable support
        ~MVCModelComponent()
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

        #region IEquatable<IModelComponent>
        /// <summary>
        /// Compare property values of two specified Settings objects.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override Boolean Equals(IModelComponent other)
        {
            Boolean returnValue = default(Boolean);
            MVCModelComponent otherModel = default(MVCModelComponent);

            try
            {
                otherModel = other as MVCModelComponent;

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
                    else if (this.StillAnotherInt != otherModel.StillAnotherInt)
                    {
                        returnValue = false;
                    }
                    else if (this.StillAnotherBoolean != otherModel.StillAnotherBoolean)
                    {
                        returnValue = false;
                    }
                    else if (this.StillAnotherString != otherModel.StillAnotherString)
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
        #endregion IEquatable<IModelComponent>

        #region Properties

        #region Non-Persisted Properties
        private Int32 _StillAnotherInt = default(Int32);
        public Int32 StillAnotherInt
        {
            get { return _StillAnotherInt; }
            set
            {
                _StillAnotherInt = value;
                OnPropertyChanged("StillAnotherInt");
            }
        }

        private Boolean _StillAnotherBoolean = default(Boolean);
        public Boolean StillAnotherBoolean
        {
            get { return _StillAnotherBoolean; }
            set
            {
                _StillAnotherBoolean = value;
                OnPropertyChanged("StillAnotherBoolean");
            }
        }

        private String _StillAnotherString = String.Empty;//default(String);
        public String StillAnotherString
        {
            get { return _StillAnotherString; }
            set
            {
                _StillAnotherString = value;
                OnPropertyChanged("StillAnotherString");
            }
        }
        #endregion Non-Persisted Properties
        #endregion Properties

    }
}
