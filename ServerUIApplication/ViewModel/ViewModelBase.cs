using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerUIApplication.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static event PropertyChangedEventHandler StaticPropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        protected static void OnStaticPropertyChanged([CallerMemberName] string property = null) => StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(property));
    }
}
