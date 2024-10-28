using Microsoft.VisualBasic;

namespace PRM_API.Common.Payloads;

public class ApiRoute
{
    public const string Base = "api";

    public static class Movie
    {
        public const string GetAll = Base + "/movies";
        public const string GetById = Base + "/movies/{id}";
        public const string Create = Base + "/movies";
        public const string Update = Base + "/movies";
        public const string Delete = Base + "/movies/{id}";

        public const string GetAllGenre = Base + "/movie-genres";
        public const string GetAllLang = Base + "/movie-languages";
    }

    public static class Seat
    {
        public const string GetAll = Base + "/seats";
    }

    public static class ShowTime
    {
        public const string GetAll = Base + "/showtimes";
    }
    public static class FAB
    {
        public const string GetAll = Base + "/fab";
        public const string GetById = Base + "/fab/{id}";
        public const string GetByBookingId = Base + "/fab/order/{orderId}";
        public const string CreateFABOrder = Base + "/fab/order/{orderId}";
        public const string UpdateFABOrder = Base + "/fab/order/{orderId}";
        public const string DeleteFABOrder = Base + "fab/order/{orderId}";
    }
}