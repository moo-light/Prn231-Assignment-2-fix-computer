using DTOS.DTOS;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class MyTools
    {
        public static string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            return configuration.GetConnectionString("DefaultDB") ?? "";
        }
        public static LoginDTO GetAdminAccount()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            return new LoginDTO
            {
                Email = configuration["Admin:email"] ?? throw new MissingMemberException("Admin:email"),
                Password = configuration["Admin:password"] ?? throw new MissingMemberException("Admin:password")
            };
        }

    }
}
