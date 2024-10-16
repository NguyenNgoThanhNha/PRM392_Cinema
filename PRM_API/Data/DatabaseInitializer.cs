    using Microsoft.EntityFrameworkCore;
using PRM_API.Common.Enum;
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
                    //if (!dbContext.FoodBeverages.Any()) await SeedFoodBeverageAsync();
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
            if (!cinemaHalls.Any()) return; // Ensure there are cinema halls available.

            var random = new Random();
            var moviesData = new List<(string Title, string Description, MovieLanguage Language)>
            {
                // English Movies translated to Vietnamese
                ("Cuộc Phiêu Lưu Vĩ Đại", "Một cuộc hành trình kỳ thú qua những vùng đất chưa được khám phá.", MovieLanguage.English),
                ("Tình Yêu Ở Paris", "Một câu chuyện tình lãng mạn diễn ra tại trái tim của Paris.", MovieLanguage.English),
                ("Bí Ẩn Đảo Hoang", "Một câu chuyện bí ẩn đầy kịch tính xảy ra trên một hòn đảo hoang vắng.", MovieLanguage.English),
        
                // Japanese Movie translated to Vietnamese
                ("Con Đường Của Samurai", "Một câu chuyện về danh dự và trả thù diễn ra tại Nhật Bản thời phong kiến.", MovieLanguage.Japanese),
        
                // Korean Movie translated to Vietnamese
                ("Biển Lặng", "Một bộ phim khoa học viễn tưởng về nhiệm vụ thu thập những mẫu vật bí ẩn từ mặt trăng.", MovieLanguage.Korean),
        
                // Thai Movie translated to Vietnamese
                ("Những Đêm Bangkok", "Một bộ phim tội phạm gay cấn diễn ra trên những con phố sầm uất của Bangkok.", MovieLanguage.Thai),
        
                // Vietnamese Movie (no translation needed)
                ("Hoàng Hôn Sài Gòn", "Một câu chuyện ấm lòng về tình yêu và truyền thống trong Việt Nam hiện đại.", MovieLanguage.Vietnamese),
        
                // More English Movies translated to Vietnamese
                ("Chiến Tranh Thiên Hà", "Một trận chiến liên thiên hà để sinh tồn.", MovieLanguage.English),
                ("Những Giấc Mơ Hoạt Hình", "Một bộ phim hoạt hình ấm áp dành cho mọi lứa tuổi.", MovieLanguage.English),
                ("Ngôi Nhà Ma Ám", "Một câu chuyện kinh hoàng về một ngôi nhà bị ma ám.", MovieLanguage.English)
            };

            var movieGenres = Enum.GetValues(typeof(MovieGenre)).Cast<MovieGenre>().ToList();

            var movies = new List<Movie>();

            foreach (var (title, description, language) in moviesData)
            {
                var movie = new Movie
                {
                    Title = title,
                    Description = description,
                    ReleaseDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-random.Next(100, 1000))),
                    Duration = random.Next(80, 180), 
                    Rating = Math.Round((decimal)(random.NextDouble() * 4 + 1), 1), 
                    Genre = movieGenres[random.Next(movieGenres.Count)].ToString(),
                    Language = language.ToString()
                };

                var showtimeCount = random.Next(3, 6);
                for (int j = 0; j < showtimeCount; j++)
                {
                    var showtime = new Showtime
                    {
                        ShowDate = DateTime.Now.AddDays(random.Next(1, 30)).AddHours(random.Next(10, 22)),
                        Hall = cinemaHalls[random.Next(cinemaHalls.Count)] 
                    };
                    movie.Showtimes.Add(showtime);
                }

                movies.Add(movie);
            }

            await dbContext.Movies.AddRangeAsync(movies);
            await dbContext.SaveChangesAsync();
        }

        //private async Task SeedFoodBeverageAsync()
        //{

        //}

        private async Task SeedCinemaHallAsync()
        {
            Random random = new Random();
            List<string> seatTypes = new List<string> { "Normal", "VIP", "Couple" };

            List<CinemaHall> cinemaHalls = new()
            {
                new CinemaHall { HallName = "Hall A", TotalSeats = 100 },
                new CinemaHall { HallName = "Hall B", TotalSeats = 90 },
                new CinemaHall { HallName = "Hall C", TotalSeats = 110 },
                new CinemaHall { HallName = "Hall D", TotalSeats = 85 },
                new CinemaHall { HallName = "Hall E", TotalSeats = 120 },
                new CinemaHall { HallName = "Hall F", TotalSeats = 95 },
                new CinemaHall { HallName = "Hall G", TotalSeats = 105 },
                new CinemaHall { HallName = "Hall H", TotalSeats = 80 },
                new CinemaHall { HallName = "Hall I", TotalSeats = 115 },
                new CinemaHall { HallName = "Hall J", TotalSeats = 90 }
            };
            foreach (var hall in cinemaHalls)
            {
                for (int i = 1; i <= hall.TotalSeats; i++)
                {
                    hall.Seats.Add(new Seat
                    {
                        HallId = hall.HallId,
                        SeatNumber = $"S{i:D3}",
                        SeatType = seatTypes[random.Next(seatTypes.Count)]
                    });
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
