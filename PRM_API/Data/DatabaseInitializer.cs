    using Microsoft.EntityFrameworkCore;
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
                    //if (!dbContext.CinemaHalls.Any()) await SeedCinemaHallAsync();
                    //// Animal
                    //if (!dbContext.FoodBeverages.Any()) await SeedFoodBeverageAsync();
                    //// Delivery Order Details
                    //if (!dbContext.Movies.Any()) await SeedMovieAsync();
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

            //private async Task SeedMovieAsync()
            //{

            //}

            //private async Task SeedFoodBeverageAsync()
            //{

            //}

            //private async Task SeedCinemaHallAsync()
            //{

            //}

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
