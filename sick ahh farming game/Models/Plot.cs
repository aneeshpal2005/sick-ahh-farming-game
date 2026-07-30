using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace sick_ahh_farming_game.Models
{
    public class Plot
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? SeedId { get; set; }
        public DateTime? PlantedTime { get; set; }
        [Ignore]
        public Seed? Seed { get; set; }
    }
}
