using System;
using Microsoft.Win32;

namespace MatchingGame.Logic
{
    public sealed class GameSettings
    {
        private const string KeyPath = "Software\\AdventureWorks\\MatchingGame";
        public static readonly GameSettings Instance = Load();

        private int _bestScore;

        private GameSettings()
        {
        }

        public int BestScore
        {
            get => _bestScore;
            private set
            {
                _bestScore = value;
                Save();
            }
        }

        public void UpdateScore(int newScore)
        {
            if (BestScore == 0 || newScore < BestScore)
                BestScore = newScore;
        }

        private static GameSettings Load()
        {
            var result = new GameSettings();

#pragma warning disable PC001 // API not supported on all platforms
            using (var key = Registry.CurrentUser.OpenSubKey(KeyPath, false))
#pragma warning restore PC001 // API not supported on all platforms
            {
                if (key != null)
                {
                    var value = key.GetValue(nameof(BestScore));
                    if (value != null)
                        result.BestScore = Convert.ToInt32(value);
                }
            }

            return result;
        }

        private void Save()
        {
            using (var key = Registry.CurrentUser.CreateSubKey(KeyPath))
            {
                key.SetValue(nameof(BestScore), BestScore);
            }
        }
    }
}