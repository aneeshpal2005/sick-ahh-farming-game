using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace sick_ahh_farming_game.Models
{
    public class InventoryItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int SeedId { get; set; }
        public int Quantity { get; set; }
        [Ignore]
        public Seed? Seed { get; set; }
    }
}
