
using LocationTracking.Data.Dto;
using System.Net.Http.Json;

public static class Program
{
    public static void Main(string[] args)
    {
        var user = new UserDto { Name = "John Doe" };
        var location = new LocationDto { Latitude = 52.123, Longitude = -2.01, Timestamp = DateTime.Now };

        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://locationtracking.api");

        //Create a new user
         client.PostAsJsonAsync("api/user", user);

        while (true)
        {
            var response = client.PostAsJsonAsync("api/location", new { UserId = user.Name, Location = location, Timestamp = DateTime.Now }).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Location updated");
            }
            else
            {
                Console.WriteLine("Error updating location");
            }
            Thread.Sleep(1000);
        }
    }
}