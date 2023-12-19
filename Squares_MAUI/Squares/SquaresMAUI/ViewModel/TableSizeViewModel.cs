using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Squares.Model;

namespace SquaresMAUI.ViewModel
{
    public class TableSizeViewModel : ViewModelBase
    {
        private TableSize _size;

        public TableSize Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SizeText));
            }
        }

        public string SizeText => _size.ToString();
    }
}
