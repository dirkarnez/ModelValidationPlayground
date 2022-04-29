using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ModelValidationPlayground
{
    class User
    {
        private const string hkAreaCode = "+852";

        [Display(Name = "Email")]
        [Required(ErrorMessage = "The {0} is required.")]
        [EmailAddress(ErrorMessage = "The {0} is invalid.")]
        public string Email { get; set; }

        private string mobileContact; // field
        
        [Display(Name = "Mobile Contact")]
        [Required]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "The {0} must be at least {2} and not more than {1} characters long.")]
        public string MobileContact   // property
        {
            get { return mobileContact; }
            set {
                if (value == null) {
                    return;
                }

                var temp = (
                    value.StartsWith(hkAreaCode)
                    ?
                    value.Substring(hkAreaCode.Length)
                    :
                    value
                    );

                mobileContact = new string(temp
                    .Where(char.IsNumber)
                    .ToArray());
            }
        }

        [Display(Name = "Age")]
        [Required(ErrorMessage = "The {0} is required.")]
        [Range(18, 100, ErrorMessage = "The {0} of only more than {1} is allowed.")]
        public int Age { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            User u = new User
            {
                Email = "qwerty",
                MobileContact = "+852234-      78s9",
                Age = 15
            };

            var ctx = new ValidationContext(u);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(u, ctx, results, true))
            {
                foreach (var errors in results)
                {
                    Console.WriteLine("{0}", errors);
                }
            }
            else
            {
                Console.WriteLine($"{u.Age} {u.Email} {u.MobileContact}");
            }

            Console.ReadLine();
            return;
        }
    }
}
