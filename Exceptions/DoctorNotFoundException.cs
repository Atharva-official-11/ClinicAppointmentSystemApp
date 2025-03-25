namespace ClinicAppointmentSystemApp.Exceptions
{
    public class DoctorNotFoundException:ApplicationException
    {

        public DoctorNotFoundException()
        {
            
        }

        public DoctorNotFoundException(string msg) : base(msg)
        {

        }
    }
}
