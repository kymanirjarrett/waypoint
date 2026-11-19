using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Waypoint.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Continent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    BestTimeToVisit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AverageDailyBudgetUsd = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    RecommendedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Visited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Destinations",
                columns: new[] { "Id", "AverageDailyBudgetUsd", "BestTimeToVisit", "Continent", "Country", "Description", "Name", "Rating", "RecommendedBy", "Slug", "Summary", "Visited" },
                values: new object[,]
                {
                    { 1, 150m, "March-May, October-November", "Asia", "Japan", "Japan's former capital has more than 1,600 temples and shrines. Walk the torii tunnels at Fushimi Inari early in the morning, rent a bike to reach the Philosopher's Path, and spend an evening in Gion. Buy an IC card for buses and trains, and plan at least three days.", "Kyoto", 5, "Riddhi", "kyoto", "Temples, gardens, and quiet lanes that reward slow wandering.", true },
                    { 2, 110m, "April-June, September-October", "Europe", "Portugal", "Ride Tram 28 through Alfama, catch sunset from a miradouro, and take the train to Sintra for a day of palaces. Lisbon is one of the more affordable capitals in Western Europe, and most of the center is walkable if you don't mind the hills.", "Lisbon", 5, "Kymani", "lisbon", "Hilly streets, tiled facades, and the best custard tarts anywhere.", true },
                    { 3, 180m, "June-September", "North America", "Canada", "Lake Louise and Moraine Lake are the headliners, but book the Parks Canada shuttle because parking fills by sunrise. Hike to the Lake Agnes Tea House, drive the Icefields Parkway, and keep bear spray handy on every trail.", "Banff National Park", 5, "Evan", "banff", "Turquoise lakes and Rocky Mountain trails for every fitness level.", true },
                    { 4, 80m, "March-May", "North America", "Mexico", "Spend a morning at the National Museum of Anthropology, explore Coyoacan and the Frida Kahlo Museum, and float the canals of Xochimilco on a weekend. Tacos al pastor from a busy stand are a must. The Metro is cheap and fast outside rush hour.", "Mexico City", 4, "Kymani", "mexico-city", "World-class museums and street food on every corner.", true },
                    { 5, 250m, "June-August for hiking, October-March for auroras", "Europe", "Iceland", "Use Reykjavik as a hub for the Golden Circle and the South Coast waterfalls. Rent a car if you're comfortable with changing weather, soak in a geothermal pool, and check the aurora forecast on clear winter nights. Groceries save a lot of money here.", "Reykjavik", 4, "Evan", "reykjavik", "A base camp for waterfalls, glaciers, and the northern lights.", false },
                    { 6, 70m, "March-May, September-November", "Africa", "Morocco", "Get lost in the medina souks, watch Jemaa el-Fnaa come alive at night, and stay in a riad with a courtyard. Agree on taxi fares before you get in, and consider a day trip to the Atlas Mountains or an overnight in the Agafay desert.", "Marrakech", 4, "Riddhi", "marrakech", "Souks, riads, and a gateway to the Atlas Mountains.", false },
                    { 7, 170m, "December-February, June-August for skiing", "Oceania", "New Zealand", "Bungee jumping started here, but the gondola, the Ben Lomond track, and a day trip to Milford Sound are just as memorable. Fergburger lines are long for a reason. Book adventure activities ahead in peak summer.", "Queenstown", 5, "Evan", "queenstown", "The adventure capital, set on a glacial lake.", false },
                    { 8, 60m, "May-September", "South America", "Peru", "Give yourself two days to adjust to the altitude (3,400 m) before hiking. Visit Sacsayhuaman and the Sacred Valley, then take the train or hike the Inca Trail to Machu Picchu. Trail permits sell out months ahead, so book early.", "Cusco", 5, "Riddhi", "cusco", "Inca history and the starting point for Machu Picchu.", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_Slug",
                table: "Destinations",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Destinations");
        }
    }
}
