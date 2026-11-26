using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waypoint.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRiddhiDestinations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Visited" },
                values: new object[] { false });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AverageDailyBudgetUsd", "BestTimeToVisit", "Continent", "Country", "Description", "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath", "Name", "Rating", "Slug", "Summary" },
                values: new object[] { 250m, "September-November", "North America", "United States", "Walk or bike across the Golden Gate Bridge, ride a cable car up Nob Hill, and book Alcatraz tickets well ahead because tours sell out. Spend an afternoon in Golden Gate Park, then eat your way through the Mission and Chinatown. Pack layers, since summer days are often cool and foggy, and use a Clipper card for Muni, cable cars, and BART.", "The red Golden Gate Bridge stretching across blue water from a rocky headland, with the San Francisco skyline faint in the distance.", "Maarten van den Heuvel", "https://unsplash.com/photos/golden-gate-bridge-during-daytime-gZXx8lKAb7Y", "/images/destinations/san-francisco.webp", "San Francisco", 5, "san-francisco", "Steep hills, cable cars, and fog rolling under the Golden Gate." });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AverageDailyBudgetUsd", "BestTimeToVisit", "Continent", "Country", "Description", "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath", "Name", "Slug", "Summary" },
                values: new object[] { 100m, "April-May, September-October", "Europe", "Czech Republic", "Cross Charles Bridge early in the morning before the crowds, watch the Astronomical Clock strike the hour in Old Town Square, and climb up to Prague Castle and St. Vitus Cathedral. The center is easy to explore on foot, and trams cover the rest. Prices are lower than in much of Western Europe, so plan three or four days.", "Crowds crossing the lamp-lit Charles Bridge at dusk, with Prague Castle and St. Vitus Cathedral glowing on the hill across the Vltava River.", "William Zhang", "https://unsplash.com/photos/charles-bridge-and-prague-castle-6En4WYsNYXM", "/images/destinations/prague.webp", "Prague", "prague", "Gothic spires, cobbled lanes, and a castle above the river." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Visited" },
                values: new object[] { true });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AverageDailyBudgetUsd", "BestTimeToVisit", "Continent", "Country", "Description", "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath", "Name", "Rating", "Slug", "Summary" },
                values: new object[] { 70m, "March-May, September-November", "Africa", "Morocco", "Get lost in the medina souks, watch Jemaa el-Fnaa come alive at night, and stay in a riad with a courtyard. Agree on taxi fares before you get in, and consider a day trip to the Atlas Mountains or an overnight in the Agafay desert.", "A green-framed window with a white iron grille in a pale plaster wall, in a narrow alley of the Marrakech medina.", "JR Harris", "https://unsplash.com/photos/green-metal-window-frame-on-brown-concrete-building-slWOnYaNYWI", "/images/destinations/marrakech.webp", "Marrakech", 4, "marrakech", "Souks, riads, and a gateway to the Atlas Mountains." });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AverageDailyBudgetUsd", "BestTimeToVisit", "Continent", "Country", "Description", "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath", "Name", "Slug", "Summary" },
                values: new object[] { 60m, "May-September", "South America", "Peru", "Give yourself two days to adjust to the altitude (3,400 m) before hiking. Visit Sacsayhuaman and the Sacred Valley, then take the train or hike the Inca Trail to Machu Picchu. Trail permits sell out months ahead, so book early.", "Cusco's Plaza de Armas under storm clouds, with a colonial church on the left and brown Andean hills behind red-tiled roofs.", "Sophia Simoes", "https://unsplash.com/photos/plaza-de-armas-in-cusco-with-cathedral-and-mountains-pw7eT19wz4c", "/images/destinations/cusco.webp", "Cusco", "cusco", "Inca history and the starting point for Machu Picchu." });
        }
    }
}
