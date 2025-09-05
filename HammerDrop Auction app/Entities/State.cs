using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HammerDrop_Auction_app.Entities
{
    public class State
    {
        [Key]
        [Ignore]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        // Foreign key to Country
        public int country_id { get; set; }

        public string country_code { get; set; }

        public string country_name { get; set; }


        public double? latitude { get; set; }

        public double? longitude { get; set; }
    }
}
