using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DN_Seminarski.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace DN_Seminarski.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }

        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {

            List<Collage> collageList = new List<Collage>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Collage";

                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Collage collage = new Collage
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = Convert.ToString(dataReader["Name"]),
                            Code = Convert.ToString(dataReader["Code"]),
                            CityId = Convert.ToInt32(dataReader["CityId"])
                        };

                        collageList.Add(collage);
                    }
                }

                connection.Close();
            }
            return View(collageList);
        }

        public IActionResult Create()
        {
            List<Collage> cityList = new List<Collage>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SqlDataReader
                connection.Open();

                string sql = "Select * From City"; SqlCommand command = new SqlCommand(sql, connection);
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
                        cityList.Add(city);
                    }
                }
                connection.Close();
            }

            ViewData["City"] = new SelectList(cityList, "Id", "Name");

            return View();

        }

        [HttpPost]
        public IActionResult Create(Collage collage)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = $"Insert Into Collage (Code, Name, CityId) Values ('{collage.Code}', '{collage.Name}', '{collage.CityId}')";
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
            Collage collage = new Collage();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"Select c.Id AS CollageId, " +
                    $"c.Code AS CollageCode, " +
                    $"c.Name AS CollageName, " +
                    $"ci.Code AS CityCode, " +
                    $"ci.Name AS CityName " +
                    $"FROM Collage c, City ci " +
                    $"WHERE c.CityId = ci.Id AND c.Id='{id}';";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        collage.Id = Convert.ToInt32(dataReader["CollageId"]);
                        collage.Code = Convert.ToString(dataReader["CollageCode"]);
                        collage.Name = Convert.ToString(dataReader["CollageName"]);
                        City city = new City
                        {
                            Code = Convert.ToString(dataReader["CityCode"]),
                            Name = Convert.ToString(dataReader["CityName"])
                        };
                        collage.City = city;
                    }
                }
                connection.Close();
            }

            List<City> cityList = new List<City>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SqlDataReader
                connection.Open();

                string sql = "Select * From City"; SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        City city = new City
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Code = Convert.ToString(dataReader["Code"]),
                            Name = Convert.ToString(dataReader["Name"])
                        };
                        cityList.Add(city);
                    }
                }
                connection.Close();
            }

            ViewData["City"] = new SelectList(cityList, "Id", "Name");
            return View(collage);

        }

        [HttpPost]
        public IActionResult Update(Collage collage)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update Collage SET Code='{collage.Code}', Name='{collage.Name}', CityId='{collage.CityId}' Where Id='{collage.Id}'";
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
                string sql = $"Delete From Collage Where Id='{id}'";
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



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
