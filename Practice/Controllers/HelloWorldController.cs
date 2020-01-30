using System;
using System.Text.Encodings.Web;

namespace Practice.Controllers
{
    public class HelloWorldController
    {
        // GET /HelloWorld
        public string Index()
        {
            return "This is my default action...";
        }

        // GET /HelloWorld/Welcome
        public string Welcome()
        {
            return "This is the Welcome action method...";
        }

        // GET /HelloWorld/Welcome/[name]?[multipier]
        public string Welcome(string name, int multiplier)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, multipler is: {multiplier}");
        }
    }
}
