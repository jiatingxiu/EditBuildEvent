using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace Test
{
    public class DataGridSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private DataView myVar;

        public DataView MyProperty
        {
            get { return myVar; }
            set
            {
                myVar = value;
                OnPropertyChanged("MyProperty");
            }
        }
        


    }
}
