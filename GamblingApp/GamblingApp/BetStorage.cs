using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GamblingApp.Data;
using Windows.Storage;

namespace GamblingApp
{
    public static class BetStorage
    {
        public static List<Bet> bets = new();
        private static string filename = "bets.json";

        public static async Task LoadBets()
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync(filename);
                var json = await FileIO.ReadTextAsync(file);
                bets = JsonSerializer.Deserialize<List<Bet>>(json);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static async Task SaveBets()
        {
            var json = JsonSerializer.Serialize(bets, new JsonSerializerOptions { WriteIndented = true });
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }

        public static async Task AddBet(Bet bet)
        {
            bets.Add(bet);
            await SaveBets();
        }
    }
}
