using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HammerDrop_Auction_app.Entities
{
    public class City
    {
        [Key]
        [Ignore]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [ForeignKey("State")]
        public int state_id { get; set; }

        public string state_code { get; set; }


        public string state_name { get; set; }

        [ForeignKey("Country")]
        public int country_id { get; set; }

        public string country_code { get; set; }

        public string country_name { get; set; }

        public double? latitude { get; set; }
        public double? longitude { get; set; }

        [Name("wikiDataId")]
        public string WikiDataId { get; set; }

        // Optional: Navigation Properties
        public State State { get; set; }
        public Country Country { get; set; }
    }
}
