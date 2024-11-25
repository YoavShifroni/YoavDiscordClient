using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoavDiscordClient
{
    public class RegistrationInfo
    {
        /// <summary>
        /// The username that the user entered
        /// </summary>
        public string Username;

        /// <summary>
        /// The password that the user entered
        /// </summary>
        public string Password;

        /// <summary>
        /// The first name that the user entered
        /// </summary>
        public string FirstName;

        /// <summary>
        /// The last name that the user entered
        /// </summary>
        public string LastName;

        /// <summary>
        /// The server ip that the user entered
        /// </summary>
        public string ServerIp;

        /// <summary>
        /// The email that the user entered
        /// </summary>
        public string Email;

        /// <summary>
        /// The city that the user choosed
        /// </summary>
        public string City;

        /// <summary>
        /// The gender that the user choosed
        /// </summary>
        public string Gender;


        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="serverIp"></param>
        /// <param name="email"></param>
        /// <param name="city"></param>
        /// <param name="gender"></param>
        public RegistrationInfo(string username, string password, string firstName, string lastName, string serverIp, string email, string city,
            string gender)
        {
            this.Username = username;
            this.Password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.ServerIp = serverIp;
            this.Email = email;
            this.City = city;
            this.Gender = gender;
        }
    }
}
