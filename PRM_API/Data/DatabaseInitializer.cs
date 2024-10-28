using Microsoft.EntityFrameworkCore;
using PRM_API.Common.Enum;
using PRM_API.Extensions;
using PRM_API.Models;

    namespace PRM_API.Data
    {
        public interface IDatabaseInitializer
        {
            Task InitializeAsync();
            Task TrySeedAsync();
            Task SeedAsync();
        }
        public class DatabaseInitializer(ApplicationDbContext dbContext) : IDatabaseInitializer
        {

        public async Task InitializeAsync()
            {
                try
                {
                    // Check whether the database exists and can be connected to
                    if (!await dbContext.Database.CanConnectAsync())
                    {
                        // Check for applied migrations
                        var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
                        if (appliedMigrations.Any())
                        {
                            Console.WriteLine("Migrations have been applied.");
                            return;
                        }

                        // Perform migration if necessary
                        await dbContext.Database.MigrateAsync();
                        Console.WriteLine("Database initialized successfully");
                    }
                    else
                    {
                        Console.WriteLine("Database cannot be connected to.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task TrySeedAsync()
            {
                try
                {
                    // Users
                    if (!dbContext.Users.Any()) await SeedUserAsync();
                    // Payment Types
                    //if (!dbContext.BookingFoodBeverages.Any()) await SeedBookingFoodBeverageTypeAsync();
                    //// Shipping fees
                    //if (!dbContext.Bookings.Any()) await SeedBookingAsync();
                    //// Delivery Orders
                    //if (!dbContext.BookingSeats.Any()) await SeedBookingSeatAsync();
                    //// Animal Types
                    if (!dbContext.CinemaHalls.Any()) await SeedCinemaHallAsync();
                    //// Animal
                    if (!dbContext.FoodBeverages.Any()) await SeedFoodBeverageAsync();
                    //// Delivery Order Details
                    if (!dbContext.Movies.Any()) await SeedMovieAsync();
                    //if (!dbContext.Seats.Any()) await SeedSeatAsync();
                    //if (!dbContext.Showtimes.Any()) await SeedShowtimeAsync();

                    // More seeding here...
                    // Each table need to create private method to seeding data

                    await Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        //private async Task SeedShowtimeAsync()
        //{

        //}


        //private async Task SeedSeatAsync()
        //{

        //}

        private async Task SeedMovieAsync()
        {
            var cinemaHalls = await dbContext.CinemaHalls.ToListAsync();
            if (!cinemaHalls.Any()) return; 

            var random = new Random();
            var moviesData = new List<(string Title, string Description, string)>
            {
                // English Movies translated to Vietnamese
                ("Cuộc Phiêu Lưu Vĩ Đại", "Một cuộc hành trình kỳ thú qua những vùng đất chưa được khám phá.", MovieLanguage.English.GetDescription()),
                ("Tình Yêu Ở Paris", "Một câu chuyện tình lãng mạn diễn ra tại trái tim của Paris.", MovieLanguage.English.GetDescription()),
                ("Bí Ẩn Đảo Hoang", "Một câu chuyện bí ẩn đầy kịch tính xảy ra trên một hòn đảo hoang vắng.", MovieLanguage.English.GetDescription()),
        
                // Japanese Movie translated to Vietnamese
                ("Con Đường Của Samurai", "Một câu chuyện về danh dự và trả thù diễn ra tại Nhật Bản thời phong kiến.", MovieLanguage.Japanese.GetDescription()),
        
                // Korean Movie translated to Vietnamese
                ("Biển Lặng", "Một bộ phim khoa học viễn tưởng về nhiệm vụ thu thập những mẫu vật bí ẩn từ mặt trăng.", MovieLanguage.Korean.GetDescription()),
        
                // Thai Movie translated to Vietnamese
                ("Những Đêm Bangkok", "Một bộ phim tội phạm gay cấn diễn ra trên những con phố sầm uất của Bangkok.", MovieLanguage.Thai.GetDescription()),
        
                // Vietnamese Movie (no translation needed)
                ("Hoàng Hôn Sài Gòn", "Một câu chuyện ấm lòng về tình yêu và truyền thống trong Việt Nam hiện đại.", MovieLanguage.Vietnamese.GetDescription()),
        
                // More English Movies translated to Vietnamese
                ("Chiến Tranh Thiên Hà", "Một trận chiến liên thiên hà để sinh tồn.", MovieLanguage.English.GetDescription()),
                ("Những Giấc Mơ Hoạt Hình", "Một bộ phim hoạt hình ấm áp dành cho mọi lứa tuổi.", MovieLanguage.English.GetDescription()),
                ("Ngôi Nhà Ma Ám", "Một câu chuyện kinh hoàng về một ngôi nhà bị ma ám.", MovieLanguage.English.GetDescription())
            };

            var movieGenres = Enum.GetValues(typeof(MovieGenre)).Cast<MovieGenre>()
                .Select(x => x.GetDescription()).ToList();

            var movies = new List<Movie>();

            foreach (var (title, description, language) in moviesData)
            {
                var movie = new Movie
                {
                    Title = title,
                    Description = description,
                    ReleaseDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-random.Next(100, 1000))),
                    Duration = (int)(Math.Round(random.Next(80, 180) / 10.0) * 10), 
                    Rating = Math.Round((decimal)(random.NextDouble() * 4 + 1), 1), 
                    Genre = movieGenres[random.Next(movieGenres.Count)].ToString(),
                    Language = language.ToString()
                };

                var showtimeCount = random.Next(3, 6);
                for (int j = 0; j < showtimeCount; j++)
                {
                    var showDateTime = DateTime.Now.AddDays(random.Next(1, 30)).AddHours(random.Next(10, 22));
                        
                    decimal seatPrice = (showDateTime.Hour >= 6 && showDateTime.Hour < 18) ? 50000M : 70000M;

                    var showtime = new Showtime
                    {
                        ShowDate = showDateTime,
                        Hall = cinemaHalls[random.Next(cinemaHalls.Count)],
                        SeatPrice = seatPrice
                    };
                    movie.Showtimes.Add(showtime);
                }

                movies.Add(movie);
            }

            await dbContext.Movies.AddRangeAsync(movies);
            await dbContext.SaveChangesAsync();
        }

        private async Task SeedFoodBeverageAsync()
        {
            List<FoodBeverage> fabList = new List<FoodBeverage>();
            FoodBeverage fab1 = new FoodBeverage()
            {   Name = "Bắp phô mai nhỏ",
                Description = "size S",
                Price = 20,
            };
            FoodBeverage fab2 = new FoodBeverage()
            {   Name = "Bắp phô mai trung bình",
                Description = "size M",
                Price = 40,
            };
            
            FoodBeverage fab3 = new FoodBeverage()
            {   Name = "Nước ngọt lớn",
                Description = "size L",
                Price = 45,
            };
            FoodBeverage fab4 = new FoodBeverage()
            {   Name = "Nước ngọt nhỏ",
                Description = "size S",
                Price = 20,
            };
            FoodBeverage fab5 = new FoodBeverage()
            {   Name = "Nước ngọt trung bình",
                Description = "size M",
                Price = 30,
            };
            
            FoodBeverage fab6 = new FoodBeverage()
            {   Name = "Nước ngọt lớn",
                Description = "size L",
                Price = 35,
            };
            
            fabList.AddRange(new List<FoodBeverage>()
            {
                fab1,fab2,fab3,fab4,fab5,fab6
            });
            await dbContext.FoodBeverages.AddRangeAsync(fabList);
            await dbContext.SaveChangesAsync();
        }

        // private async Task SeedCinemaHallAsync()
        // {
        //     Random random = new Random();
        //     //List<string> seatTypes = new List<string> { "Ghế thường", "Ghế VIP" };

        //     int seatsPerRow = 12;

        //     List<CinemaHall> cinemaHalls = new()
        //     {
        //         new CinemaHall { HallName = "Phòng A", TotalSeats = 60 },
        //         new CinemaHall { HallName = "Phòng B", TotalSeats = 90 },
        //         new CinemaHall { HallName = "Phòng C", TotalSeats = 110 },
        //         new CinemaHall { HallName = "Phòng D", TotalSeats = 85 },
        //         new CinemaHall { HallName = "Phòng E", TotalSeats = 120 },
        //         new CinemaHall { HallName = "Phòng F", TotalSeats = 95 },
        //         new CinemaHall { HallName = "Phòng G", TotalSeats = 105 },
        //         new CinemaHall { HallName = "Phòng H", TotalSeats = 80 },
        //         new CinemaHall { HallName = "Phòng I", TotalSeats = 115 },
        //         new CinemaHall { HallName = "Phòng J", TotalSeats = 90 }
        //     };

        //     foreach (var hall in cinemaHalls)
        //     {
        //         int numberOfRows = (int)Math.Ceiling((double)hall.TotalSeats / seatsPerRow);

        //         for (int i = 0; i < hall.TotalSeats; i++)
        //         {
        //             int rowNumber = i / seatsPerRow;
        //             int seatInRow = (i % seatsPerRow) + 1;
        //             string rowLabel = ((char)('A' + rowNumber)).ToString();
        //             string seatNumber = $"{rowLabel}{seatInRow:D2}";

        //             // Set seat type based on row
        //             string seatType = (rowNumber == 2 || rowNumber == 3) ? "Ghế VIP" : "Ghế thường";

        //             hall.Seats.Add(new Seat
        //             {
        //                 HallId = hall.HallId,
        //                 SeatNumber = seatNumber,
        //                 SeatType = seatType
        //             });
        //         }
        //     }

        //     dbContext.CinemaHalls.AddRange(cinemaHalls);
        //     await dbContext.SaveChangesAsync();
        // }


        private async Task SeedCinemaHallAsync()
        {
            string[] seatTypes = new []{ SeatType.Normal.GetDescription(), SeatType.VIP.GetDescription() };
            Random random = new Random();
            int seatsPerRow = 12;

            var hallTypes = new Dictionary<string, (int TotalColumns, Dictionary<int, List<int>> EmptySeatsPerRow)>
            {
                ["Type1"] = (12, new Dictionary<int, List<int>>()),
                ["Type2"] = (14, new Dictionary<int, List<int>> { { 1, new List<int> { 3, 4 } }, { 2, new List<int> { 6 } } }), 
                ["Type3"] = (16, new Dictionary<int, List<int>> { { 0, new List<int> { 1, 2, 13 } }, { 1, new List<int> { 5, 10 } } }) 
            };

            List<CinemaHall> cinemaHalls = new()
            {
                new CinemaHall { HallName = "Phòng A", TotalSeats = 60, HallType = "Type1" },
                new CinemaHall { HallName = "Phòng B", TotalSeats = 90, HallType = "Type2" },
                new CinemaHall { HallName = "Phòng C", TotalSeats = 110, HallType = "Type3" },
            };

            foreach (var hall in cinemaHalls)
            {
                int numberOfRows = (int)Math.Ceiling((double)hall.TotalSeats / seatsPerRow);
                var layout = hallTypes[hall.HallType];

                for (int row = 0; row < numberOfRows; row++)
                {
                    string rowLabel = ((char)('A' + row)).ToString();
                    for (int col = 0; col < layout.TotalColumns; col++)
                    {
                        bool isOff = layout.EmptySeatsPerRow.ContainsKey(row) && layout.EmptySeatsPerRow[row].Contains(col);

                        string seatType = (row == 2 || row == 3) 
                            ? seatTypes[1] 
                            : seatTypes[0];
                            
                        string seatNumber = $"{rowLabel}{(col + 1):D2}";

                        hall.Seats.Add(new Seat
                        {
                            HallId = hall.HallId,
                            SeatNumber = seatNumber,
                            SeatType = isOff ? null! : seatType,
                            ColIndex = row,
                            SeatIndex = col,
                            IsOff = isOff,
                            IsSold = false
                        });
                    }
                }
            }

            dbContext.CinemaHalls.AddRange(cinemaHalls);
            await dbContext.SaveChangesAsync();
        }


        //private async Task SeedBookingSeatAsync()
        //{

        //}

        //private async Task SeedBookingAsync()
        //{

        //}

        //private async Task SeedBookingFoodBeverageTypeAsync()
        //{

        //}

        private async Task SeedUserAsync()
            {
                var user1 = new User()
                {
                    Email = "user1@gmail.com",
                    Fullname = "User 1",
                    Password = "123456",
                    PhoneNumber = "0123456789",
                    Username = "user1"
                };
                var user2 = new User()
                {
                    Email = "user2@gmail.com",
                    Fullname = "User 2",
                    Password = "123456",
                    PhoneNumber = "0123456789",
                    Username = "user2"
                };
                var staff = new User()
                {
                    Email = "staff@gmail.com",
                    Fullname = "Staff",
                    Password = "123456",
                    PhoneNumber = "0123456789",
                    Username = "Staff"
                };
                List<User> users = new List<User>()
                {
                    user1, user2,staff
                };
                await dbContext.Users.AddRangeAsync(users);
                await dbContext.SaveChangesAsync();
            }
        }
    }
