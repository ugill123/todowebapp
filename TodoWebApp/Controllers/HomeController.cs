using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;
using TodoWebApp.Models;

namespace TodoWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ViewResult AddTodo() => View();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Todo>? todoList = new List<Todo>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:5142/api/Tasks"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    todoList = JsonConvert.DeserializeObject<List<Todo>>(apiResponse);
                }

            }
            return View(todoList);
        }
        [HttpPost]
        public async Task<IActionResult> AddTodo(Todo todo)
        {
            Todo? receivedTodo = new();
            using (var httpClient = new HttpClient())
            {
                String test = todo.Id.ToString();
                StringContent content = new(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json");
                using var response = await httpClient.PostAsync("http://localhost:5142/api/Tasks/", content);

                _logger.LogInformation(response.ToString());
                string apiResponse = await response.Content.ReadAsStringAsync();
                receivedTodo = JsonConvert.DeserializeObject<Todo>(apiResponse);
            }
            
            return RedirectToAction("Index");
        }
        // update  methods 
        public async Task<IActionResult> UpdateTodo(int id)
        {
            Todo? todo = new Todo();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:5142/api/Tasks/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    todo = JsonConvert.DeserializeObject<Todo>(apiResponse);
                }
            }
            return View(todo);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTodo(Todo todo)
        {
            Todo receivedTodo = new Todo();
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:5142/api/Tasks/{todo.Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            string apiResponse = await response.Content.ReadAsStringAsync();
            ViewBag.Result = "Success";
            receivedTodo = JsonConvert.DeserializeObject<Todo>(apiResponse);
            return RedirectToAction("Index");
        }

        // Delete Method
        [HttpPost]
        public async Task<IActionResult> DeleteTodo(int todoId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("http://localhost:5142/api/Tasks/" + todoId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
             
            
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}