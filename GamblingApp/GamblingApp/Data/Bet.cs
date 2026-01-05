using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingApp.Data
{
    public class Bet
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Amount { get; set; }
        public DateTime PlacedAt { get; set; }
        public Game Game { get; set; }
        public Team Team { get; set; }
        public BettingStatus Status { get; set; }

    }
    public enum BettingStatus
    {
        Pending,
        Won,
        Lost,
        Draw
    }
}
