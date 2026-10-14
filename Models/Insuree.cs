namespace CarInsurance.Models
{
    // This class represents an insured person and their vehicle.
    public class Insuree
    {
        // Unique identifier for each insuree.
        public Guid Id { get; set; }

        // Insuree's first name.
        public string FirstName { get; set; }

        // Insuree's last name.
        public string LastName { get; set; }

        // Insuree's email address.
        public string EmailAddress { get; set; }

        // Insuree's date of birth.
        public DateTime DateOfBirth { get; set; }

        // Year the insured vehicle was manufactured.
        public int CarYear { get; set; }

        // Make of the insured vehicle.
        public string CarMake { get; set; }

        // Model of the insured vehicle.
        public string CarModel { get; set; }

        // Indicates whether the insuree has ever had a DUI.
        public bool DUI { get; set; }

        // Number of speeding tickets the insuree has.
        public int SpeedingTickets { get; set; }

        // Type of insurance coverage.
        public string CoverageType { get; set; }

        // Monthly insurance quote calculated by the application.
        public decimal Quote { get; set; }
    }
}