using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using StudentManagementSystem.Models;

public class WebApiController : Controller
{
    [HttpGet]
    public ActionResult SaveUser()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> SaveUser(string name ,string email,decimal salary,int department)
    {
        var user = new
        {
            Name = name,
            Email = email,
            Salary = salary,
            Department = department
        };

        string jsonData = JsonConvert.SerializeObject(user);

        using (HttpClient client = new HttpClient())
        {
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response =
                await client.PostAsync("https://localhost:7027/api/Users", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Saved via API";
            }
            else
            {
                ViewBag.Message = "API Error";
            }
        }

        return View();
    }
    public async Task<ActionResult> GetUsers()
    {
        List<User> users = new List<User>();

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response =
                await client.GetAsync("https://localhost:7027/api/Users");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<User>>(data);
            }
        }

        return View(users);
    }
}