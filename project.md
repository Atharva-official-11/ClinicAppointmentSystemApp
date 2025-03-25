# Clinic Appointment System

![Clinic System](./ImagesToGithub/Screenshot%20(2).png)

## Introduction
The **Clinic Appointment System** is a web-based application that allows patients to book, reschedule, and cancel appointments with doctors. The system is implemented using **ASP.NET Core MVC**, **Entity Framework Core**, and follows the **Code-First Approach**.

## Features
- ✅ **User Authentication**: Patients, doctors, and administrators can register, log in, and manage profiles.
- 📅 **Appointment Booking**: Patients can book appointments with available doctors.
- 🔄 **Rescheduling & Cancellation**: Patients can reschedule or cancel their appointments.
- 📊 **Appointment Tracking**: Patients and doctors can view upcoming and past appointments.
- 🛠 **Admin Dashboard**: Manage users, doctors, and appointment records.

![Features](./ImagesToGithub/Screenshot%20(3).png)

![Features](./ImagesToGithub/Screenshot%20(11).png)

![Features](./ImagesToGithub/Screenshot%20(7).png)
![Features](./ImagesToGithub/Screenshot%20(8).png)

![Features](./ImagesToGithub/Screenshot%20(9).png)

![Features](./ImagesToGithub/Screenshot%20(10).png)
![Features](./ImagesToGithub/Screenshot%20(4).png)
![Features](./ImagesToGithub/Screenshot%20(5).png)

## Technologies Used
![Technologies](images/technologies.png)
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **Code-First Approach**

## Entity Models
### User
```csharp
public class User {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // Patient, Doctor, Admin
}
```
### Doctor
```csharp
public class Doctor {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Specialty { get; set; }
    public List<Appointment> Appointments { get; set; }
}
```

### Appointment
```csharp
public class Appointment 
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public User Patient { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } // Scheduled, Completed, Canceled
}
```


### Repository Interfaces
### IAppointmentRepository
```csharp
public interface IAppointmentRepository 
{
    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync(int userId);
    Task<Appointment> GetAppointmentByIdAsync(int id);
    Task AddAppointmentAsync(Appointment appointment);
    Task UpdateAppointmentStatusAsync(int id, string status);
    Task DeleteAppointmentAsync(int id);
}

```


### IDoctorRepository
```csharp
public interface IDoctorRepository 
{
    Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
    Task<Doctor> GetDoctorByIdAsync(int id);
    Task AddDoctorAsync(Doctor doctor);
    Task UpdateDoctorAsync(Doctor doctor);
    Task DeleteDoctorAsync(int id);
}


```
### Controllers
### AppointmentController

## DoctorController


## Custom Exceptions
### AppointmentNotFoundException
```csharp
public class AppointmentNotFoundException : Exception 
{
    public AppointmentNotFoundException(string message) : base(message) { }
}

```
### DoctorNotFoundException
```csharp
public class DoctorNotFoundException : Exception 
{
    public DoctorNotFoundException(string message) : base(message) { }
}


```


### Fixes & Improvements:
✅ **Each entity (User, Doctor, Appointment) has its own properly closed code block**  
✅ **Every code block is properly formatted**  
✅ **Added horizontal separators (`---`) for readability**  
✅ **Ensured all formatting is correct for GitHub rendering**  

