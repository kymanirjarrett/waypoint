using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Waypoint.Migrations
{
    /// <inheritdoc />
    public partial class AddDestinationImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageAlt",
                table: "Destinations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageCredit",
                table: "Destinations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageCreditUrl",
                table: "Destinations",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Destinations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath" },
                values: new object[] { "A stone path curving through a tunnel of vermilion torii gates at Fushimi Inari Shrine, with a black lantern hanging on the left.", "Stefano Bucciarelli", "https://unsplash.com/photos/thousands-of-vibrant-orange-torii-gates-at-fushimi-inari-shrine-Vu9O6DRGXQU", "/images/destinations/kyoto.webp" });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath" },
                values: new object[] { "A yellow tram climbing a steep, busy street in Lisbon between old stone buildings and iron street lamps.", "Jacek Urbanski", "https://unsplash.com/photos/a-yellow-tram-rides-down-a-street-in-lisbon-0sODcpe2RPo", "/images/destinations/lisbon.webp" });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath" },
                values: new object[] { "Turquoise Moraine Lake below a ring of jagged peaks, seen past pine trees from the rock pile.", "Connor Jolley", "https://unsplash.com/photos/moraine-lake-and-mountains-in-banff-Y6I4VvyN_sA", "/images/destinations/banff.webp" });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath" },
                values: new object[] { "The carved marble arch over the entrance of the Palacio de Bellas Artes, its name spelled out above a sculpted group of figures.", "Julio Lopez", "https://unsplash.com/photos/a-large-building-with-a-statue-on-top-of-it-2zDXbjtCpFc", "/images/destinations/mexico-city.webp" });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath" },
                values: new object[] { "A rainbow painted down a wet street in Reykjavik, leading toward the spire of Hallgrimskirkja fading into fog.", "NIR HIMI", "https://unsplash.com/photos/rainbow-street-leading-to-hallgrimskirkja-reykjavik-IVNLK9yjd1I", "/images/destinations/reykjavik.webp" });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath" },
                values: new object[] { "A green-framed window with a white iron grille in a pale plaster wall, in a narrow alley of the Marrakech medina.", "JR Harris", "https://unsplash.com/photos/green-metal-window-frame-on-brown-concrete-building-slWOnYaNYWI", "/images/destinations/marrakech.webp" });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath" },
                values: new object[] { "Queenstown seen from above, its streets running down to the bays of Lake Wakatipu.", "Donald Wu", "https://unsplash.com/photos/aerial-view-of-queenstown-and-lake-wakatipu-new-zealand-sU2NHLTnVTI", "/images/destinations/queenstown.webp" });

            migrationBuilder.UpdateData(
                table: "Destinations",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ImageAlt", "ImageCredit", "ImageCreditUrl", "ImagePath" },
                values: new object[] { "Cusco's Plaza de Armas under storm clouds, with a colonial church on the left and brown Andean hills behind red-tiled roofs.", "Sophia Simoes", "https://unsplash.com/photos/plaza-de-armas-in-cusco-with-cathedral-and-mountains-pw7eT19wz4c", "/images/destinations/cusco.webp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageAlt",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "ImageCredit",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "ImageCreditUrl",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Destinations");
        }
    }
}
