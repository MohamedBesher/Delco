using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Saned.Jawla.Api.ViewModels
{

    public class RegisterExternalBindingModel
    {
        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static string GetPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("fb");
            builder.Append(RandomString(3, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(1, false));
            return builder.ToString();
        }

        [Required]
        public string UserId { get; set; }
        public string UserName { get; set; } = GetPassword();
        public string Name { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public string ExternalAccessToken { get; set; }

    }
}