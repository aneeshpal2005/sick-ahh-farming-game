using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace sick_ahh_farming_game.Models
{
    public class PlayerStat
    {
        [PrimaryKey]
        public int Id { get; set; } = 1;

        public int Coins { get; set; }

        public int PlantsHarvested { get; set; }
    }
}
