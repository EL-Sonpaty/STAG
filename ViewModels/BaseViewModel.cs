using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace STAG.ViewModels
{
    public class BaseViewModel : Rhino.UI.ViewModel
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Checks if a value has changed, updates the value and raises <see cref="PropertyChanged"/>
        /// </summary>
        /// <typeparam name="T">property type</typeparam>
        /// <param name="field">property backing field</param>
        /// <param name="value">new property value</param>
        /// <param name="propertyName">property name used for raising event</param>
        /// <returns></returns>
        protected virtual bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        #endregion

    }
}
