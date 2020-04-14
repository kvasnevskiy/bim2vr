using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;

namespace BIM2VR.ViewModels.Busy
{
    public class BusyManagerModel : BindableBase, IBusyManager
    {
        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        private int currentValue;
        public int CurrentValue
        {
            get => currentValue;
            set => SetProperty(ref currentValue, value);
        }

        private int minimum;
        public int Minimum
        {
            get => minimum;
            set => SetProperty(ref minimum, value);
        }

        private int maximum;
        public int Maximum
        {
            get => maximum;
            set => SetProperty(ref maximum, value);
        }

        private string currentMessage;
        public string CurrentMessage
        {
            get => currentMessage;
            set => SetProperty(ref currentMessage, value);
        }

        public void SetBusy(int min, int max, string message)
        {
            Minimum = min;
            Maximum = max;
            CurrentValue = 0;
            IsBusy = true;
            CurrentMessage = message;
        }

        public void SetFree()
        {
            Minimum = 0;
            Maximum = 0;
            CurrentValue = 0;
            IsBusy = false;
        }

        public void SetValue(int value)
        {
            if (Application.Current.Dispatcher != null)
                Application.Current.Dispatcher.Invoke(new Action(() => { CurrentValue = value; }));
        }

        public void SetMessage(string message)
        {
            if (Application.Current.Dispatcher != null)
                Application.Current.Dispatcher.Invoke(new Action(() => { CurrentMessage = message; }));
        }
    }
}
