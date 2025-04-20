using MySql.Data.MySqlClient;
using System.Configuration;
using AppWeather.Models;

namespace AppWeather.Utilities
{
    public class DatabaseService
    {
        private readonly string _connectionString = "Server = localhost; Database = weather_db; Uid = root; Pwd = root; Port = 3307";
        //ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public bool IsConnectionAvailable()
        {
            try
            {
                using var conn = new MySqlConnection(_connectionString);
                conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<WeatherForecast> GetAllForecasts()
        {
            var forecasts = new List<WeatherForecast>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM WEATHER_FORECAST", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                forecasts.Add(new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(reader.GetDateTime("WEATHER_DATE")),
                    Temperature = reader.GetFloat("TEMPERATURE"),
                    Precipitation = reader.GetBoolean("PRECIPITATION"),
                    Pressure = reader.GetFloat("PRESSURE")
                });
            }
            return forecasts;
        }

        public void AddForecast(WeatherForecast forecast)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("INSERT INTO WEATHER_FORECAST (WEATHER_DATE, TEMPERATURE, PRECIPITATION, PRESSURE)" +
                "VALUES (@WEATHER_DATE, @TEMPERATURE, @PRECIPITATION, @PRESSURE)", conn);
            cmd.Parameters.AddWithValue("@WEATHER_DATE", forecast.Date);
            cmd.Parameters.AddWithValue("@TEMPERATURE", forecast.Temperature);
            cmd.Parameters.AddWithValue("@PRECIPITATION", forecast.Precipitation);
            cmd.Parameters.AddWithValue("@PRESSURE", forecast.Pressure);
            cmd.ExecuteNonQuery();
        }

        public void UpdateForecast(WeatherForecast forecast)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("UPDATE WEATHER_FORECAST " +
                "SET TEMPERATURE = @TEMPERATURE, PRECIPITATION = @PRECIPITATION, PRESSURE = @PRESSURE" +
                "WHERE WEATHER_DATE = @WEATHER_DATE", conn);
            cmd.Parameters.AddWithValue("@WEATHER_DATE", forecast.Date);
            cmd.Parameters.AddWithValue("@TEMPERATURE", forecast.Temperature);
            cmd.Parameters.AddWithValue("@PRECIPITATION", forecast.Precipitation);
            cmd.Parameters.AddWithValue("@PRESSURE", forecast.Pressure);
            cmd.ExecuteNonQuery();
        }

        public void DeleteForecastByDate(DateTime date)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM WEATHER_FORECAST WHERE WEATHER_DATE = @WEATHER_DATE", conn);
            cmd.Parameters.AddWithValue("@WEATHER_DATE", date);
            cmd.ExecuteNonQuery();
        }

        public Admin? GetAdmin(string username, string password)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM ADMINS WHERE USERNAME = @USERNAME AND USER_PASSWORD = @USER_PASSWORD", conn);
            cmd.Parameters.AddWithValue("@USERNAME", username);
            cmd.Parameters.AddWithValue("@USER_PASSWORD", password);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Admin
                {
                    Username = reader.GetString("USERNAME"),
                    Password = reader.GetString("USER_PASSWORD")
                };
            }
            return null;
        }
    }
}