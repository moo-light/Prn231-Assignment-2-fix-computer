namespace FUCarRentingSystem_RazorPage.Utils
{
    public class Constants
    {
        public static int PageSize { get; internal set; } = 4;

        public class ApiRoute
        {
            public const string DefaultPath = "https://localhost:5000/api";
            public const string SignIn = $"{DefaultPath}/auth/sign-in";  
            public const string CarsApi = $"{DefaultPath}/cars";  
            public const string CustomersApi = $"{DefaultPath}/customers";
            public const string CarRentalsApi = $"{DefaultPath}/carrentals";
            public const string ReviewsApi = $"{DefaultPath}/reviews";
            public const string ProducersApi = $"{DefaultPath}/CarProducers";

        }
        public class Role
        {
            public const string Admin = "Admin";
            public const string Customer = "User";
        }
    }
}
