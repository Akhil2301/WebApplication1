using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController:ControllerBase
    {
        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult <IEnumerable<StudentDTO> >GetStudents()
        {

            //var students=new List<StudentDTO>();
            //  foreach (var student in CollegeRepository.Students)
            //  {
            //      StudentDTO obj = new StudentDTO()
            //      {
            //         Id = student.Id,
            //         StudentName = student.StudentName,
            //          Address = student.Address,
            //          Email = student.Email,
            //      };
            //      students.Add(obj)
            //  }
            var students = CollegeRepository.Students.Select(n => new StudentDTO()
            {
                       Id = n.Id,
                       StudentName = n.StudentName,
                       Address = n.Address,
                       Email = n.Email,
                       AdmissionDate = n.AdmissionDate,
                       Age = n.Age,
                       Password = n.Password,

            });
            return Ok(students);
        }


        [HttpGet("{Id:int}", Name = "GetStudentById")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentById(int Id)
        {
            if (Id <= 0)

                return BadRequest();//400

            var student= CollegeRepository.Students.Where(n => n.Id == Id).FirstOrDefault();

            if (student == null)

                return NotFound($"The student with id {Id} is not found");

            var students = new StudentDTO
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Address = student.Address,
                Email = student.Email,
                AdmissionDate = student.AdmissionDate,
                Age = student.Age,
                Password = student.Password,
            };

            return Ok(students);
        }
        [HttpGet]
        [Route("{StudentName}",Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult <StudentDTO> GetStudentByName(string StudentName)
        {
            if (string.IsNullOrEmpty(StudentName))

                return  BadRequest();
            var student= CollegeRepository.Students.Where(n => n.StudentName == StudentName).FirstOrDefault();
            if (student == null)
                return NotFound($"The student with name {StudentName} is not found");
            var students = new StudentDTO
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Address = student.Address,
                Email = student.Email,
                AdmissionDate = student.AdmissionDate,
                Password = student.Password,
                Age=student.Age,
                
            };

            return Ok(student);
        }
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult <StudentDTO> CreateStudent([FromBody]StudentDTO model)
        {
           /*if(!ModelState.IsValid)
                return BadRequest(ModelState);*/
            if(model == null)
            {
                return BadRequest(ModelState);
            }
            /* if (model.AdmissionDate < DateTime.Now)
             {
                 //1. Directly adding error message to model state
                 //2. Using cusom attribute
                 ModelState.AddModelError("AdmissionDate Error", "Admission date must be greater than or equal to todays date");
                 return BadRequest(ModelState);
             }*/
            int newId = CollegeRepository.Students.LastOrDefault().Id + 1;
            StudentDTO student = new StudentDTO
            {
                Id = newId,
                StudentName = model.StudentName,
                Address = model.Address,
                Email = model.Email,
                Age = model.Age,
                Password=model.Password,
                AdmissionDate = model.AdmissionDate,

            };
            CollegeRepository.Students.Add(student);
            model.Id = student.Id;
            //status  201
            //created link https://localhost:7262/api/Student/3
            //new student details
            return CreatedAtRoute("GetStudentById", new { model.Id }, model);
            return Ok(model);
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent([FromBody]StudentDTO model)
        {
            if (model == null||model.Id<=0) 
            {
                return BadRequest();
            }

            var existingStudent=CollegeRepository.Students.Where(s=>s.Id==model.Id).FirstOrDefault();

            if(existingStudent==null)
               return NotFound();
            existingStudent.Id = model.Id;
            existingStudent.Address = model.Address;
            existingStudent.StudentName=model.StudentName;
            existingStudent.Email = model.Email;
            existingStudent.Age = model.Age;
            existingStudent.AdmissionDate = model.AdmissionDate;

            return NoContent();
        }

        [HttpPatch]
        [Route("{Id}/UpdateParticular")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateParticular(int Id,[FromBody] JsonPatchDocument <StudentDTO> patchDocument)
        {
            if (patchDocument == null || Id <= 0)
            {
                return BadRequest();
            }

            var existingStudent = CollegeRepository.Students.Where(s => s.Id == Id).FirstOrDefault();

            if (existingStudent == null)
                return NotFound();

            var StudentDTO = new StudentDTO
            {
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
                Address = existingStudent.Address,
                Email= existingStudent.Email,

            };
            patchDocument.ApplyTo(StudentDTO,ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            existingStudent.Address = StudentDTO.Address;
            existingStudent.StudentName = StudentDTO.StudentName;
            existingStudent.Email = StudentDTO.Email;


            return NoContent();
        }
        [HttpDelete]
        [Route("Delete/{Id}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult <bool> DeleteStudentById(int Id)
        {
            if (Id <= 0)
        
                return BadRequest();

            var student = CollegeRepository.Students.Where(n => n.Id == Id).FirstOrDefault();
            if (student == null)
                return NotFound();
             CollegeRepository.Students.Remove(student);

            return Ok();
        }
    }
}
