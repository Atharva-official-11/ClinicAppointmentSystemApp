namespace ClinicAppointmentSystemApp.Exceptions
{
    public class AppointmentNotFoundException:ApplicationException
    {
        public AppointmentNotFoundException()
        {
            
        }
        public AppointmentNotFoundException(string msg) : base(msg)
        {

        }
    }
}
