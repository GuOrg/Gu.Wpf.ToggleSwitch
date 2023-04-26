namespace Gu.Wpf.ToggleSwitch.Demo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    public class Vm : INotifyPropertyChanged
    {
        private bool _boolProp;

        private bool? _nullableBoolProp;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public bool BoolProp
        {
            get
            {
                return this._boolProp;
            }
            set
            {
                if (value.Equals(this._boolProp))
                {
                    return;
                }
                this._boolProp = value;
                this.OnPropertyChanged();
            }
        }

        public bool? NullableBoolProp
        {
            get
            {
                return this._nullableBoolProp;
            }
            set
            {
                if (value.Equals(this._nullableBoolProp))
                {
                    return;
                }
                this._nullableBoolProp = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
