using CsvHelper;
using CsvHelper.Configuration;
using HammerDrop_Auction_app.Entities;
using Microsoft.EntityFrameworkCore;



namespace HammerDrop_Auction_app.SeedData
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;

        public DatabaseSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetCountryNamesAsync()
        {
            var existingCountries = await _context.Countries
                .Select(c => c.name.ToLower().Trim())
                .ToListAsync();
            return existingCountries;
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            var countriesInDb = await _context.Countries.ToListAsync();
            return countriesInDb;
        }

        public async Task<List<State>> GetAllStatesAsync()
        {
            var statesInDb = await _context.States.ToListAsync();
            return statesInDb;
        }

        public async Task<List<dynamic>> GetExistingCityKeysAsync()
        {
            var existingCityKeys = await _context.Cities
                .Select(c => new { c.name, c.state_code, c.country_code })
                .ToListAsync();
            return existingCityKeys.Cast<dynamic>().ToList();
        }
    }
}








    //public class DatabaseSeeder
    //{
    //    private readonly AppDbContext _context;
    //        private const int BatchSize = 1000;

//        public DatabaseSeeder(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task SeedAllAsync(string countriesCsvPath, string statesCsvPath, string citiesCsvPath)
//        {
//            Console.WriteLine("Starting database seeding...");
//            await SeedCountriesAsync(countriesCsvPath);
//            await SeedStatesAsync(statesCsvPath);
//            await SeedCitiesAsync(citiesCsvPath);

//            Console.WriteLine("Database seeding completed.");
//        }



//        private async Task SeedCountriesAsync(string csvFilePath)
//        {
//            var countries = await ReadCsvAsync<Country>(csvFilePath);
//            var existingCountries = await _context.Countries.Select(c => c.name.ToLower().Trim()).ToListAsync();
//          //  var newCountries = countries.Where(c => !existingCountries.Contains(c.name.ToLower().Trim())).ToList();
//          //  newCountries.ForEach(c => c.id = 0);
//          //  await InsertInBatchesAsync(newCountries, _context.Countries);
//        }

//        private async Task SeedStatesAsync(string csvFilePath)
//        {
//            var states = await ReadCsvAsync<State>(csvFilePath);
//            states.ForEach(s => s.id = 0);

//            var countriesInDb = await _context.Countries.ToListAsync();

//            var countryLookup = countriesInDb
//                .GroupBy(c => c.name.Trim().ToLower())
//                .ToDictionary(g => g.Key, g => g.First());

//            var validStates = new List<State>();

//            foreach (var state in states)
//            {
//                string key = (state.country_name ?? "").Trim().ToLower();
//                if (countryLookup.TryGetValue(key, out var matchedCountry))
//                {
//                    state.country_id = matchedCountry.id;
//                    validStates.Add(state);
//                }
//                else
//                {
//                    Console.WriteLine($"Warning: No matching country found for state '{state.name}' with country_name '{state.country_name}'");
//                    // Optionally skip or handle differently
//                }
//            }

//            await InsertInBatchesAsync(validStates, _context.States);
//        }

//        private async Task SeedCitiesAsync(string csvFilePath)
//        {

//            var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
//            {
//                HeaderValidated = null,
//                MissingFieldFound = null
//            };

//            List<City> cities;
//            using (var reader = new StreamReader(csvFilePath))
//            using (var csv = new CsvReader(reader, config))
//            {
//              //  csv.Context.RegisterClassMap<CityMap>(); // your CityMap mapping
//                cities = csv.GetRecords<City>().ToList();
//            }

//            // Load Countries and States from DB for matching
//            var countriesInDb = await _context.Countries.ToListAsync();
//            var statesInDb = await _context.States.ToListAsync();

//            // Build lookups by code or name (choose consistent keys)
//            var countryLookup = countriesInDb
//                .GroupBy(c => c.name.Trim().ToLower())
//                .ToDictionary(g => g.Key, g => g.First());

//            var stateLookup = statesInDb
//                .GroupBy(s => (s.name.Trim().ToLower(), s.country_id))
//                .ToDictionary(g => g.Key, g => g.First());

//            var validCities = new List<City>();

//            foreach (var city in cities)
//            {
//                //city.id = 0;
//                //city.state_code ??= "N/A";
//                //city.state_name ??= "N/A";
//                //city.country_code ??= "N/A";
//                //city.country_name ??= "N/A";
//                //city.WikiDataId ??= "N/A";
//                //city.latitude ??= 0;
//                //city.longitude ??= 0;

//                // Find Country
//                string countryKey = (city.country_name ?? "").Trim().ToLower();
//                if (!countryLookup.TryGetValue(countryKey, out var matchedCountry))
//                {
//                    Console.WriteLine($"Warning: No country found for city '{city.name}' with country_name '{city.country_name}'");
//                    continue; // skip city
//                }

//                city.country_id = matchedCountry.id;

//                // Find State - use (state_name, country_id) tuple as key
//                string stateKey = (city.state_name ?? "").Trim().ToLower();
//                var stateLookupKey = (stateKey, city.country_id);

//                if (!stateLookup.TryGetValue(stateLookupKey, out var matchedState))
//                {
//                    Console.WriteLine($"Warning: No state found for city '{city.name}' with state_name '{city.state_name}' and country_id '{city.country_id}'");
//                    continue; // skip city
//                }

//                city.state_id = matchedState.id;

//                validCities.Add(city);
//            }

//            if (validCities.Count == 0)
//            {
//                Console.WriteLine("No valid cities to insert.");
//                return;
//            }

//            // Avoid duplicates based on name, state_code, country_code
//            var existingCityKeys = await _context.Cities
//                .Select(c => new { c.name, c.state_code, c.country_code })
//                .ToListAsync();

//            var newCities = validCities
//                .Where(city => !existingCityKeys.Any(ec =>
//                    ec.name == city.name &&
//                    ec.state_code == city.state_code &&
//                    ec.country_code == city.country_code))
//                .ToList();

//            if (newCities.Count == 0)
//            {
//                Console.WriteLine("No new cities to insert.");
//                return;
//            }

//            await InsertInBatchesAsync(newCities, _context.Cities);

//            Console.WriteLine($"Seeded {newCities.Count} new cities.");
//        }


//        private async Task<List<T>> ReadCsvAsync<T>(string path)
//        {
//            using var reader = new StreamReader(path);
//            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
//            return csv.GetRecords<T>().ToList();
//        }

//        private async Task InsertInBatchesAsync<T>(List<T> data, DbSet<T> dbSet) where T : class
//        {
//            int totalBatches = (int)Math.Ceiling(data.Count / (double)BatchSize);

//            for (int i = 0; i < data.Count; i += BatchSize)
//            {
//                var batch = data.Skip(i).Take(BatchSize).ToList();
//                await dbSet.AddRangeAsync(batch);
//                await _context.SaveChangesAsync();

//               // Console.WriteLine($"Inserted batch {(i / BatchSize) + 1} of {totalBatches}");
//            }
//        }
//    }

//    // CityMap to correctly map CSV columns to City entity properties
//    //public sealed class CityMap : ClassMap<City>
//    //{
//    //    public CityMap()
//    //    {

//    //        Map(m => m.country_name).Name("country_name");
//    //        Map(m => m.country_id).Name("country_id");
//    //        Map(m => m.name).Name("name");
//    //        Map(m => m.state_name).Name("state_name");
//    //        Map(m => m.state_id).Name("state_id");
//    //        // Replace "CityName" with actual CSV column name
//    //        Map(m => m.state_code).Name("state_code");  // Replace "StateCode" with actual CSV column name
//    //        Map(m => m.country_code).Name("country_code"); // Replace "CountryCode" with actual CSV column name

//    //        // Map other City properties if needed, for example:
//    //        Map(m => m.latitude).Name("latitude");
//    //        Map(m => m.longitude).Name("longitude");
//    //        Map(m => m.WikiDataId).Name("wikiDataId");
//    //    }
//    }

