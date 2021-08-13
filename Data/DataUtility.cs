using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbiraAddressBook.Data
{
    public static class DataUtility
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            //The defailt connectionstring will come from appSettings like usual
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //It will be automatically overwritten if we are running on Heroku
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);

        }

        public static string BuildConnectionString(string databaseUrl)
        {
            //Provides an object  representation of a uniform resource identifier(URI) and easy
            var DatabaseUri = new Uri(databaseUrl);
            var UserInfo = DatabaseUri.UserInfo.Split(':');


            //Provides a simple way to create and manage the contents of connection strings used 
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = DatabaseUri.Host,
                Port = DatabaseUri.Port,
                Username = UserInfo[0],
                Password = UserInfo[1],
                Database = DatabaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            };

            return builder.ToString();
            }
        }
    }







    

