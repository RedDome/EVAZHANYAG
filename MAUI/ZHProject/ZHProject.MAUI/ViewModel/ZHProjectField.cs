using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHProject.MAUI.ViewModel
{
    public class ZHProjectField : ViewModelBase
    {
        public Int32 X { get; set; }

        public Int32 Y { get; set; }

        public Tuple<Int32, Int32> XY
        {
            get { return new(X, Y); }
        }

        public DelegateCommand? ClickCommand { get; set; }
    }
}
