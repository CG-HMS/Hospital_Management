namespace Hms.API.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public string PatientName { get; set; }

        public DateTime StartDateTime { get; set; }

        public string ExaminationRoom { get; set; }
    }
}
