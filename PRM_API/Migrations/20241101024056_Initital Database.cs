using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRM_API.Migrations
{
    /// <inheritdoc />
    public partial class InititalDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CinemaHall",
                columns: table => new
                {
                    hall_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hall_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    hall_type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    total_seats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaHall", x => x.hall_id);
                });

            migrationBuilder.CreateTable(
                name: "FoodBeverage",
                columns: table => new
                {
                    food_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodBeverage", x => x.food_id);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    movie_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    release_date = table.Column<DateOnly>(type: "date", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: true),
                    rating = table.Column<decimal>(type: "decimal(2,1)", nullable: true),
                    genre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    language = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LinkTrailer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.movie_id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: false),
                    password = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: false),
                    fullname = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: true),
                    email = table.Column<string>(type: "nvarchar(155)", maxLength: 155, nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    seat_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    hall_id = table.Column<int>(type: "int", nullable: false),
                    seat_number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    seat_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_off = table.Column<bool>(type: "bit", nullable: false),
                    is_sold = table.Column<bool>(type: "bit", nullable: false),
                    SeatIndex = table.Column<int>(type: "int", nullable: false),
                    ColIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.seat_id);
                    table.ForeignKey(
                        name: "FK_Seat_Hall",
                        column: x => x.hall_id,
                        principalTable: "CinemaHall",
                        principalColumn: "hall_id");
                });

            migrationBuilder.CreateTable(
                name: "Showtime",
                columns: table => new
                {
                    showtime_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    movie_id = table.Column<int>(type: "int", nullable: false),
                    hall_id = table.Column<int>(type: "int", nullable: false),
                    show_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    seat_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Showtime", x => x.showtime_id);
                    table.ForeignKey(
                        name: "FK_Showtime_Hall",
                        column: x => x.hall_id,
                        principalTable: "CinemaHall",
                        principalColumn: "hall_id");
                    table.ForeignKey(
                        name: "FK_Showtime_Movie",
                        column: x => x.movie_id,
                        principalTable: "Movie",
                        principalColumn: "movie_id");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    showtime_id = table.Column<int>(type: "int", nullable: false),
                    booking_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    total_price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.booking_id);
                    table.ForeignKey(
                        name: "FK_Booking_Showtime",
                        column: x => x.showtime_id,
                        principalTable: "Showtime",
                        principalColumn: "showtime_id");
                    table.ForeignKey(
                        name: "FK_Booking_User",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "BookingFoodBeverage",
                columns: table => new
                {
                    booking_food_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    food_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingFoodBeverage", x => x.booking_food_id);
                    table.ForeignKey(
                        name: "FK_BookingFoodBeverage_Booking",
                        column: x => x.booking_id,
                        principalTable: "Booking",
                        principalColumn: "booking_id");
                    table.ForeignKey(
                        name: "FK_BookingFoodBeverage_Food",
                        column: x => x.food_id,
                        principalTable: "FoodBeverage",
                        principalColumn: "food_id");
                });

            migrationBuilder.CreateTable(
                name: "BookingSeat",
                columns: table => new
                {
                    booking_seat_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_id = table.Column<int>(type: "int", nullable: true),
                    seat_id = table.Column<int>(type: "int", nullable: false),
                    booking_seat_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingSeat", x => x.booking_seat_id);
                    table.ForeignKey(
                        name: "FK_BookingSeat_Booking",
                        column: x => x.booking_id,
                        principalTable: "Booking",
                        principalColumn: "booking_id");
                    table.ForeignKey(
                        name: "FK_BookingSeat_Seat",
                        column: x => x.seat_id,
                        principalTable: "Seat",
                        principalColumn: "seat_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_showtime_id",
                table: "Booking",
                column: "showtime_id");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_user_id",
                table: "Booking",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingFoodBeverage_booking_id",
                table: "BookingFoodBeverage",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingFoodBeverage_food_id",
                table: "BookingFoodBeverage",
                column: "food_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeat_booking_id",
                table: "BookingSeat",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeat_seat_id",
                table: "BookingSeat",
                column: "seat_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_hall_id",
                table: "Seat",
                column: "hall_id");

            migrationBuilder.CreateIndex(
                name: "IX_Showtime_hall_id",
                table: "Showtime",
                column: "hall_id");

            migrationBuilder.CreateIndex(
                name: "IX_Showtime_movie_id",
                table: "Showtime",
                column: "movie_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingFoodBeverage");

            migrationBuilder.DropTable(
                name: "BookingSeat");

            migrationBuilder.DropTable(
                name: "FoodBeverage");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "Showtime");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "CinemaHall");

            migrationBuilder.DropTable(
                name: "Movie");
        }
    }
}
