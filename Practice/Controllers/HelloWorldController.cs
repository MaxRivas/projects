using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Practice.Models;

namespace Practice.Controllers
{
    public class HelloWorldController : Controller
    {
        // GET /HelloWorld
        /*public string Index()
        {
            return "This is my default action...";
        }*/

        // GET /HelloWord using razor files
        public IActionResult Index()
        {
            return View();
        }

        // GET /HelloWorld/Welcome
        /*public string Welcome()
        {
            return "This is the Welcome action method...";
        }*/

        // GET /HelloWorld/Welcome/[name]?[multipier] as a query string
        public IActionResult Welcome(string name, int multiplier = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["Multiplier"] = multiplier;

            return View();
            
            //return HtmlEncoder.Default.Encode($"Hello {name}, multiplier is: {multiplier}");
        }

        // GET /HelloWord/Welcome/?id?parameter where paremeter is optional
        /*public string Welcome(string name, int Id = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, your ID  is {Id}");
        }*/
    }
}
