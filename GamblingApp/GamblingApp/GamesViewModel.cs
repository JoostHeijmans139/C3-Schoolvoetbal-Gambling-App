using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GamblingApp.Data;

namespace GamblingApp
{
    public class GamesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Game> _games = new();
        public ObservableCollection<Game> Games
        {
            get => _games;
            set
            {
                _games = value;
                SetPastGames();
                SetFutureGames();
                OnPropertyChanged();
            }
        }

        private void SetPastGames()
        {
            PastGames = new ObservableCollection<Game>(Games.Where(g => g.Start < DateTime.Now));
        }

        private void SetFutureGames()
        {
            FutureGames = new ObservableCollection<Game>(Games.Where(g => g.Start > DateTime.Now));
        }

        private ObservableCollection<Game> _pastGames = new();
        public ObservableCollection<Game> PastGames
        {
            get => _pastGames;
            set
            {
                _pastGames = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Game> _futureGames = new();
        public ObservableCollection<Game> FutureGames
        {
            get => _futureGames;
            set
            {
                _futureGames = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
