using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HammerDrop_Auction_app.Entities
{
    public class Country
    {
        [Key]
        [Ignore]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        public string currency { get; set; }

        public string currency_name { get; set; }

        public string currency_symbol { get; set; }

        public string tld { get; set; }

        public string native { get; set; }

        public string region { get; set; }

        public int? region_id { get; set; }


        public int? subregion_id { get; set; }

        public string nationality { get; set; }

        public string timezones { get; set; } // JSON-like string — store as string or consider deserializing to object

        public double? latitude { get; set; }

        public double? longitude { get; set; }

        public string emoji { get; set; }

        public string emojiU { get; set; }
    }
}
