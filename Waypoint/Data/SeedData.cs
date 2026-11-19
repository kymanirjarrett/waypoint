using Waypoint.Models;

namespace Waypoint.Data;

/// <summary>Starter rows inserted by the InitialCreate migration (via HasData).</summary>
public static class SeedData
{
    public static readonly Destination[] Destinations =
    [
        new()
        {
            Id = 1, Name = "Kyoto", Slug = "kyoto", Country = "Japan", Continent = "Asia",
            Summary = "Temples, gardens, and quiet lanes that reward slow wandering.",
            Description = "Japan's former capital has more than 1,600 temples and shrines. Walk the torii tunnels at Fushimi Inari early in the morning, rent a bike to reach the Philosopher's Path, and spend an evening in Gion. Buy an IC card for buses and trains, and plan at least three days.",
            BestTimeToVisit = "March-May, October-November", AverageDailyBudgetUsd = 150m, Rating = 5,
            RecommendedBy = "Riddhi", Visited = true,
        },
        new()
        {
            Id = 2, Name = "Lisbon", Slug = "lisbon", Country = "Portugal", Continent = "Europe",
            Summary = "Hilly streets, tiled facades, and the best custard tarts anywhere.",
            Description = "Ride Tram 28 through Alfama, catch sunset from a miradouro, and take the train to Sintra for a day of palaces. Lisbon is one of the more affordable capitals in Western Europe, and most of the center is walkable if you don't mind the hills.",
            BestTimeToVisit = "April-June, September-October", AverageDailyBudgetUsd = 110m, Rating = 5,
            RecommendedBy = "Kymani", Visited = true,
        },
        new()
        {
            Id = 3, Name = "Banff National Park", Slug = "banff", Country = "Canada", Continent = "North America",
            Summary = "Turquoise lakes and Rocky Mountain trails for every fitness level.",
            Description = "Lake Louise and Moraine Lake are the headliners, but book the Parks Canada shuttle because parking fills by sunrise. Hike to the Lake Agnes Tea House, drive the Icefields Parkway, and keep bear spray handy on every trail.",
            BestTimeToVisit = "June-September", AverageDailyBudgetUsd = 180m, Rating = 5,
            RecommendedBy = "Evan", Visited = true,
        },
        new()
        {
            Id = 4, Name = "Mexico City", Slug = "mexico-city", Country = "Mexico", Continent = "North America",
            Summary = "World-class museums and street food on every corner.",
            Description = "Spend a morning at the National Museum of Anthropology, explore Coyoacan and the Frida Kahlo Museum, and float the canals of Xochimilco on a weekend. Tacos al pastor from a busy stand are a must. The Metro is cheap and fast outside rush hour.",
            BestTimeToVisit = "March-May", AverageDailyBudgetUsd = 80m, Rating = 4,
            RecommendedBy = "Kymani", Visited = true,
        },
        new()
        {
            Id = 5, Name = "Reykjavik", Slug = "reykjavik", Country = "Iceland", Continent = "Europe",
            Summary = "A base camp for waterfalls, glaciers, and the northern lights.",
            Description = "Use Reykjavik as a hub for the Golden Circle and the South Coast waterfalls. Rent a car if you're comfortable with changing weather, soak in a geothermal pool, and check the aurora forecast on clear winter nights. Groceries save a lot of money here.",
            BestTimeToVisit = "June-August for hiking, October-March for auroras", AverageDailyBudgetUsd = 250m, Rating = 4,
            RecommendedBy = "Evan", Visited = false,
        },
        new()
        {
            Id = 6, Name = "Marrakech", Slug = "marrakech", Country = "Morocco", Continent = "Africa",
            Summary = "Souks, riads, and a gateway to the Atlas Mountains.",
            Description = "Get lost in the medina souks, watch Jemaa el-Fnaa come alive at night, and stay in a riad with a courtyard. Agree on taxi fares before you get in, and consider a day trip to the Atlas Mountains or an overnight in the Agafay desert.",
            BestTimeToVisit = "March-May, September-November", AverageDailyBudgetUsd = 70m, Rating = 4,
            RecommendedBy = "Riddhi", Visited = false,
        },
        new()
        {
            Id = 7, Name = "Queenstown", Slug = "queenstown", Country = "New Zealand", Continent = "Oceania",
            Summary = "The adventure capital, set on a glacial lake.",
            Description = "Bungee jumping started here, but the gondola, the Ben Lomond track, and a day trip to Milford Sound are just as memorable. Fergburger lines are long for a reason. Book adventure activities ahead in peak summer.",
            BestTimeToVisit = "December-February, June-August for skiing", AverageDailyBudgetUsd = 170m, Rating = 5,
            RecommendedBy = "Evan", Visited = false,
        },
        new()
        {
            Id = 8, Name = "Cusco", Slug = "cusco", Country = "Peru", Continent = "South America",
            Summary = "Inca history and the starting point for Machu Picchu.",
            Description = "Give yourself two days to adjust to the altitude (3,400 m) before hiking. Visit Sacsayhuaman and the Sacred Valley, then take the train or hike the Inca Trail to Machu Picchu. Trail permits sell out months ahead, so book early.",
            BestTimeToVisit = "May-September", AverageDailyBudgetUsd = 60m, Rating = 5,
            RecommendedBy = "Riddhi", Visited = false,
        },
    ];
}
