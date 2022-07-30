using System.ComponentModel.DataAnnotations;

namespace WitchApp.Models
{
    public class VillagerModel
    {
        [Required(ErrorMessage = "Please Enter Name...")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Age of Death...")]
        [Display(Name = "Age of Death")]
        public int AgeOfDeath { get; set; }

        [Required(ErrorMessage = "Please Enter Year of Death...")]
        [Display(Name = "Year of Death")]
        public int YearOfDeath { get; set; }

        [Display(Name = "Year")]
        public int Year { get; set; }

        [Display(Name = "Number of People Killed")]
        public int NumberOfPeopleKilled { get; set; }
    }
}
