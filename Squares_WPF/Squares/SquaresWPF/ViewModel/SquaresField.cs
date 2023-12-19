using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquaresWPF.ViewModel
{
    public class SquaresField : ViewModelBase
    {
        private Int32 _usage;

        public Int32 Usage
        {
            get { return _usage; }
            set
            {
                if (_usage != value)
                {
                    _usage = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 X { get; set; }

        public Int32 Y { get; set; }

        public Tuple<Int32, Int32> XY
        {
            get { return new(X, Y); }
        }

        public DelegateCommand? ClickCommand { get; set; }
    }
}
