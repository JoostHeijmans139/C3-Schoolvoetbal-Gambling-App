using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GamblingApp.Data
{
    internal class Game
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("start")]
        public DateTime Start { get; set; }

        [JsonPropertyName("duration_in_minutes")]
        public int DurationInMinutes { get; set; }

        [JsonPropertyName("team_1")]
        public Team Team1 { get; set; }

        [JsonPropertyName("team_2")]
        public Team Team2 { get; set; }

        [JsonPropertyName("tournament")]
        public Tournament Tournament { get; set; }

        [JsonPropertyName("score_team_1")]
        public int? ScoreTeam1 { get; set; }

        [JsonPropertyName("score_team_2")]
        public int? ScoreTeam2 { get; set; }
        public string FormattedStart => Start.ToString("HH:mm ddd d MMM");
    }
}
