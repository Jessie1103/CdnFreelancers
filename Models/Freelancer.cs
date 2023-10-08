using System.ComponentModel.DataAnnotations;

namespace CdnFreelancers.Models
{
    public class Freelancer
    {
        public int id { get; set; }

        //[Required(ErrorMessage = "Please enter your username")]
        public string username { get; set; }

        //[Required, EmailAddress]
        public string mail { get; set; }

        //[Required(ErrorMessage = "Please enter your phone number")]
        public string phoneNum { get; set; }

        //[Required(ErrorMessage = "Please enter your skill sets")]
        public string skillSets { get; set; }

        public string hobby { get; set; }
    }
}
