using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DN_Seminarski.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace DN_Seminarski.Controllers
{
    public class CourseController : Controller
    {
        public IConfiguration Configuration { get; }

        public CourseController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public ActionResult Index()
        {
            List<Course> courseList = new List<Course>();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SqlDataReader
                connection.Open();

                string sql = "Select * From Course"; SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Course course = new Course
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Code = Convert.ToString(dataReader["Code"]),
                            Name = Convert.ToString(dataReader["Name"]),
                            CollageId = Convert.ToInt32(dataReader["CollageId"])
                        };
                        courseList.Add(course);
                    }
                }
                connection.Close();
            }

            return View(courseList);
        }

        public IActionResult Create()
        {
            List<Collage> collageList = new List<Collage>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SqlDataReader
                connection.Open();

                string sql = "Select * From Collage"; SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Collage city = new Collage
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Code = Convert.ToString(dataReader["Code"]),
                            Name = Convert.ToString(dataReader["Name"])
                        };
                        collageList.Add(city);
                    }
                }
                connection.Close();
            }

            ViewData["Collage"] = new SelectList(collageList, "Id", "Name");

            return View();

        }

        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = $"Insert Into Course (Code, Name, CollageId) Values ('{course.Code}', '{course.Name}', '{course.CollageId}')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    return RedirectToAction("Index");
                }
            }
            else
                return View();
        }


        public IActionResult Update(int id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            Course course = new Course();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"Select ci.Id AS CourseId, " +
                    $"c.Code AS CollageCode, " +
                    $"c.Name AS CollageName, " +
                    $"ci.Code AS CourseCode, " +
                    $"ci.Name AS CourseName " +
                    $"FROM Collage c, Course ci " +
                    $"WHERE ci.CollageId = c.Id AND ci.Id='{id}';";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        course.Id = Convert.ToInt32(dataReader["CourseId"]);
                        course.Code = Convert.ToString(dataReader["CourseCode"]);
                        course.Name = Convert.ToString(dataReader["CourseName"]);
                        Collage collage = new Collage
                        {
                            Code = Convert.ToString(dataReader["CollageCode"]),
                            Name = Convert.ToString(dataReader["CollageName"])
                        };
                        course.Collage = collage;
                    }
                }
                connection.Close();
            }

            List<Collage> collageList = new List<Collage>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SqlDataReader
                connection.Open();

                string sql = "Select * From Collage"; SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Collage collage = new Collage
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Code = Convert.ToString(dataReader["Code"]),
                            Name = Convert.ToString(dataReader["Name"])
                        };
                        collageList.Add(collage);
                    }
                }
                connection.Close();
            }

            ViewData["Collage"] = new SelectList(collageList, "Id", "Name");
            return View(course);

        }

        [HttpPost]
        public IActionResult Update(Course course)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update Course SET Code='{course.Code}', Name='{course.Name}', CollageId='{course.CollageId}' Where Id='{course.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Delete From Course Where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        ViewBag.Result = "Operation got error:" + ex.Message;
                    }
                    connection.Close();
                }
            }
            return RedirectToAction("Index");
        }
    }
}