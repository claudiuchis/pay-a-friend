using System.ComponentModel.DataAnnotations;

namespace Pay.WebApp
{
    public class VerificationModel
    {
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string CityTown { get; set; }
        [Required]
        public string CountyState { get; set; }
        public string Code { get; set; }
        [Required]
        public string Country { get; set; }
    }
}