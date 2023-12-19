using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHProject.Model;

namespace ZHProject.MAUI.ViewModel
{
    public class GameDifficultyViewModel : ViewModelBase
    {
        private Difficulty _difficulty;

        public Difficulty GameDifficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DifficultyText));
            }
        }

        public string DifficultyText => _difficulty.ToString();
    }
}
