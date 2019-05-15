using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DN_Seminarski.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DN_Seminarski.Controllers
{
    public class CityController : Controller
    {
        public IConfiguration Configuration { get; }

        public CityController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // GET: City
        public ActionResult Index()
        {
            List<City> cityList = new List<City>();
          
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SqlDataReader
                connection.Open();

                string sql = "Select * From City";
                SqlCommand command = new SqlCommand(sql, connection);

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

            return View(cityList);
        }

        // GET: City/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: City/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: City/Create
        [HttpPost]
        public IActionResult Create(City city)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = $"Insert Into City (Code, Name) Values ('{city.Code}', '{city.Name}')";
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
            else return View();
        }

        // GET: City/Edit/5
        public ActionResult Update(int id)
        {

            City city = new City();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"Select * FROM City Where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {


                            city.Id = Convert.ToInt32(dataReader["Id"]);
                            city.Code = Convert.ToString(dataReader["Code"]);
                            city.Name = Convert.ToString(dataReader["Name"]);
                            
                        }
                    }
                    connection.Close();

                }
            }

            return View(city);
        }

  
        [HttpPost]
        public IActionResult Update(City city)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update City SET Code='{city.Code}', Name='{city.Name}' Where Id='{city.Id}'";
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
                string sql = $"Delete From City Where Id='{id}'";
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