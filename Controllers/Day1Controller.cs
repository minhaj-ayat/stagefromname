using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Day1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Day1Controller : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get([FromQuery]string? name)
        {
            //string name = Console.ReadLine();
            string stage = string.Empty;
            
            var url = "https://api.agify.io?name="+name;//Paste url here  

            WebRequest request = HttpWebRequest.Create(url);

            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());

            string responseText = reader.ReadToEnd();

            var responseage = -1;

            JObject json = JObject.Parse(responseText);
            Console.WriteLine(json);
            //Console.WriteLine(json["age"].ToString().Length);

            if(json["age"].ToString().Length > 0)
                responseage = Int32.Parse(json["age"].ToString());
            //Console.WriteLine(responseage);

            if (responseage <= 12) stage = "child";
            else if (responseage >= 13 && responseage<=19) stage = "teen";
            else if (responseage >= 20 && responseage <= 25) stage = "young adult";
            else if (responseage >= 25 && responseage <= 50) stage = "middle aged";
            else if (responseage >= 50) stage = "adult";

   

            if (responseage != -1)
            {
                Day1 d = new Day1
                {
                    name = name,
                    stage = stage
                };
                return Ok(d);
            }

            else
            {
                Err e = new Err
                {
                    error = "Unable to determine age for " + name
                };
                return BadRequest(e);
            }
        }
    }
}
