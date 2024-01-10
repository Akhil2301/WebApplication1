
namespace WebApplication1.Models
{
    public static class CollegeRepository
    {
        public static List<StudentDTO> Students { get; set; } = new List<StudentDTO>()
        {
            new StudentDTO
            {   Id=1,
                StudentName="name 1",
                Email="akhil@gmail.com",
                Address="hyd,india",
                Age = 18,
                Password="abc",
                confirmpassword="abc",
                AdmissionDate= new DateTime(2024, 01, 08),
            },
              new StudentDTO
            {   Id=2,
                StudentName="name 2",
                Email="akhil2@gmail.com",
                Address="kolktha,india",
                Age = 18,
                Password="abc",
                confirmpassword="abc",
                AdmissionDate= new DateTime(2024, 01, 08),
            },

        };
    }
}
