using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waypoint.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceLisbonMexicoCityWithParisVenice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AverageDailyBudgetUsd", "Country", "Description", "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath", "Name", "Slug", "Summary", "Visited" },
                values: new object[] { 200m, "France", "Book timed tickets ahead for the Louvre and the Musee d'Orsay, walk the Seine from Notre-Dame to the Eiffel Tower, and climb Montmartre for the view from Sacre-Coeur. The Metro reaches almost everywhere, and a Navigo Easy card saves queueing for single tickets. Plan at least four days.", "The Eiffel Tower under grey clouds, rising above trees and a tour boat on the Seine, with a leafy branch framing the left edge.", "Howard Walsh", "https://unsplash.com/photos/eiffel-tower-with-boats-on-the-seine-river-nj6rYQh2nvc", "/images/destinations/paris.webp", "Paris", "paris", "Museums, cafes, and walkable neighborhoods along the Seine.", false });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AverageDailyBudgetUsd", "BestTimeToVisit", "Continent", "Country", "Description", "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath", "Name", "Slug", "Summary", "Visited" },
                values: new object[] { 180m, "April-May, September-October", "Europe", "Italy", "Ride the vaporetto the length of the Grand Canal, cross the Rialto Bridge early before the crowds, and take the boat out to Burano for its painted houses. Day visitors pay an access fee on peak dates, so check the calendar before you go. Stay at least one night to see the city after the day-trippers leave.", "Late sun on the dome of Santa Maria della Salute and the palaces along the Grand Canal in Venice, with boats on the water below.", "Rafael Peier", "https://unsplash.com/photos/venice-canal-with-historic-buildings-boats-and-a-grand-dome--AM_3ESREqQ", "/images/destinations/venice.webp", "Venice", "venice", "Canals, bridges, and a city with no cars at all.", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AverageDailyBudgetUsd", "Country", "Description", "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath", "Name", "Slug", "Summary", "Visited" },
                values: new object[] { 110m, "Portugal", "Ride Tram 28 through Alfama, catch sunset from a miradouro, and take the train to Sintra for a day of palaces. Lisbon is one of the more affordable capitals in Western Europe, and most of the center is walkable if you don't mind the hills.", "A yellow tram climbing a steep, busy street in Lisbon between old stone buildings and iron street lamps.", "Jacek Urbanski", "https://unsplash.com/photos/a-yellow-tram-rides-down-a-street-in-lisbon-0sODcpe2RPo", "/images/destinations/lisbon.webp", "Lisbon", "lisbon", "Hilly streets, tiled facades, and the best custard tarts anywhere.", true });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AverageDailyBudgetUsd", "BestTimeToVisit", "Continent", "Country", "Description", "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath", "Name", "Slug", "Summary", "Visited" },
                values: new object[] { 80m, "March-May", "North America", "Mexico", "Spend a morning at the National Museum of Anthropology, explore Coyoacan and the Frida Kahlo Museum, and float the canals of Xochimilco on a weekend. Tacos al pastor from a busy stand are a must. The Metro is cheap and fast outside rush hour.", "The carved marble arch over the entrance of the Palacio de Bellas Artes, its name spelled out above a sculpted group of figures.", "Julio Lopez", "https://unsplash.com/photos/a-large-building-with-a-statue-on-top-of-it-2zDXbjtCpFc", "/images/destinations/mexico-city.webp", "Mexico City", "mexico-city", "World-class museums and street food on every corner.", true });
        }
    }
}
