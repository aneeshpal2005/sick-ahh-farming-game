using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace sick_ahh_farming_game.Models
{
    public class Seed
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
        public int Cost { get; set; }
        public int SellValue { get; set; }
        public int GrowthDurationSeconds { get; set; }
    }
}
