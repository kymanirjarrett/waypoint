using Waypoint.Models;

namespace Waypoint.Data;

/// <summary>Starter rows inserted through HasData. Changing a row needs a new migration.</summary>
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
            ImagePath = "/images/destinations/kyoto.webp",
            ImageAlt = "A stone path curving through a tunnel of vermilion torii gates at Fushimi Inari Shrine, with a black lantern hanging on the left.",
            ImageCredit = "Stefano Bucciarelli", ImageCreditUrl = "https://unsplash.com/photos/thousands-of-vibrant-orange-torii-gates-at-fushimi-inari-shrine-Vu9O6DRGXQU",
        },
        new()
        {
            Id = 2, Name = "Paris", Slug = "paris", Country = "France", Continent = "Europe",
            Summary = "Museums, cafes, and walkable neighborhoods along the Seine.",
            Description = "Book timed tickets ahead for the Louvre and the Musee d'Orsay, walk the Seine from Notre-Dame to the Eiffel Tower, and climb Montmartre for the view from Sacre-Coeur. The Metro reaches almost everywhere, and a Navigo Easy card saves queueing for single tickets. Plan at least four days.",
            BestTimeToVisit = "April-June, September-October", AverageDailyBudgetUsd = 200m, Rating = 5,
            RecommendedBy = "Kymani", Visited = false,
            ImagePath = "/images/destinations/paris.webp",
            ImageAlt = "The Eiffel Tower under grey clouds, rising above trees and a tour boat on the Seine, with a leafy branch framing the left edge.",
            ImageCredit = "Howard Walsh", ImageCreditUrl = "https://unsplash.com/photos/eiffel-tower-with-boats-on-the-seine-river-nj6rYQh2nvc",
        },
        new()
        {
            Id = 3, Name = "Banff National Park", Slug = "banff", Country = "Canada", Continent = "North America",
            Summary = "Turquoise lakes and Rocky Mountain trails for every fitness level.",
            Description = "Lake Louise and Moraine Lake are the headliners, but book the Parks Canada shuttle because parking fills by sunrise. Hike to the Lake Agnes Tea House, drive the Icefields Parkway, and keep bear spray handy on every trail.",
            BestTimeToVisit = "June-September", AverageDailyBudgetUsd = 180m, Rating = 5,
            RecommendedBy = "Evan", Visited = true,
            ImagePath = "/images/destinations/banff.webp",
            ImageAlt = "Turquoise Moraine Lake below a ring of jagged peaks, seen past pine trees from the rock pile.",
            ImageCredit = "Connor Jolley", ImageCreditUrl = "https://unsplash.com/photos/moraine-lake-and-mountains-in-banff-Y6I4VvyN_sA",
        },
        new()
        {
            Id = 4, Name = "Venice", Slug = "venice", Country = "Italy", Continent = "Europe",
            Summary = "Canals, bridges, and a city with no cars at all.",
            Description = "Ride the vaporetto the length of the Grand Canal, cross the Rialto Bridge early before the crowds, and take the boat out to Burano for its painted houses. Day visitors pay an access fee on peak dates, so check the calendar before you go. Stay at least one night to see the city after the day-trippers leave.",
            BestTimeToVisit = "April-May, September-October", AverageDailyBudgetUsd = 180m, Rating = 4,
            RecommendedBy = "Kymani", Visited = false,
            ImagePath = "/images/destinations/venice.webp",
            ImageAlt = "Late sun on the dome of Santa Maria della Salute and the palaces along the Grand Canal in Venice, with boats on the water below.",
            ImageCredit = "Rafael Peier", ImageCreditUrl = "https://unsplash.com/photos/venice-canal-with-historic-buildings-boats-and-a-grand-dome--AM_3ESREqQ",
        },
        new()
        {
            Id = 5, Name = "Reykjavik", Slug = "reykjavik", Country = "Iceland", Continent = "Europe",
            Summary = "A base camp for waterfalls, glaciers, and the northern lights.",
            Description = "Use Reykjavik as a hub for the Golden Circle and the South Coast waterfalls. Rent a car if you're comfortable with changing weather, soak in a geothermal pool, and check the aurora forecast on clear winter nights. Groceries save a lot of money here.",
            BestTimeToVisit = "June-August for hiking, October-March for auroras", AverageDailyBudgetUsd = 250m, Rating = 4,
            RecommendedBy = "Evan", Visited = false,
            ImagePath = "/images/destinations/reykjavik.webp",
            ImageAlt = "A rainbow painted down a wet street in Reykjavik, leading toward the spire of Hallgrimskirkja fading into fog.",
            ImageCredit = "NIR HIMI", ImageCreditUrl = "https://unsplash.com/photos/rainbow-street-leading-to-hallgrimskirkja-reykjavik-IVNLK9yjd1I",
        },
        new()
        {
            Id = 6, Name = "Marrakech", Slug = "marrakech", Country = "Morocco", Continent = "Africa",
            Summary = "Souks, riads, and a gateway to the Atlas Mountains.",
            Description = "Get lost in the medina souks, watch Jemaa el-Fnaa come alive at night, and stay in a riad with a courtyard. Agree on taxi fares before you get in, and consider a day trip to the Atlas Mountains or an overnight in the Agafay desert.",
            BestTimeToVisit = "March-May, September-November", AverageDailyBudgetUsd = 70m, Rating = 4,
            RecommendedBy = "Riddhi", Visited = false,
            ImagePath = "/images/destinations/marrakech.webp",
            ImageAlt = "A green-framed window with a white iron grille in a pale plaster wall, in a narrow alley of the Marrakech medina.",
            ImageCredit = "JR Harris", ImageCreditUrl = "https://unsplash.com/photos/green-metal-window-frame-on-brown-concrete-building-slWOnYaNYWI",
        },
        new()
        {
            Id = 7, Name = "Queenstown", Slug = "queenstown", Country = "New Zealand", Continent = "Oceania",
            Summary = "The adventure capital, set on a glacial lake.",
            Description = "Bungee jumping started here, but the gondola, the Ben Lomond track, and a day trip to Milford Sound are just as memorable. Fergburger lines are long for a reason. Book adventure activities ahead in peak summer.",
            BestTimeToVisit = "December-February, June-August for skiing", AverageDailyBudgetUsd = 170m, Rating = 5,
            RecommendedBy = "Evan", Visited = false,
            ImagePath = "/images/destinations/queenstown.webp",
            ImageAlt = "Queenstown seen from above, its streets running down to the bays of Lake Wakatipu.",
            ImageCredit = "Donald Wu", ImageCreditUrl = "https://unsplash.com/photos/aerial-view-of-queenstown-and-lake-wakatipu-new-zealand-sU2NHLTnVTI",
        },
        new()
        {
            Id = 8, Name = "Cusco", Slug = "cusco", Country = "Peru", Continent = "South America",
            Summary = "Inca history and the starting point for Machu Picchu.",
            Description = "Give yourself two days to adjust to the altitude (3,400 m) before hiking. Visit Sacsayhuaman and the Sacred Valley, then take the train or hike the Inca Trail to Machu Picchu. Trail permits sell out months ahead, so book early.",
            BestTimeToVisit = "May-September", AverageDailyBudgetUsd = 60m, Rating = 5,
            RecommendedBy = "Riddhi", Visited = false,
            ImagePath = "/images/destinations/cusco.webp",
            ImageAlt = "Cusco's Plaza de Armas under storm clouds, with a colonial church on the left and brown Andean hills behind red-tiled roofs.",
            ImageCredit = "Sophia Simoes", ImageCreditUrl = "https://unsplash.com/photos/plaza-de-armas-in-cusco-with-cathedral-and-mountains-pw7eT19wz4c",
        },
    ];
}
