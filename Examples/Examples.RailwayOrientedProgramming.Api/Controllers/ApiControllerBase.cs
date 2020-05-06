using System.Net;
using System.Net.Http;
using Examples.RailwayOrientedProgramming.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Examples.RailwayOrientedProgramming.Api.Controllers
{
    public class ApiControllerBase : Controller
    {
        protected IActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }
        
        protected IActionResult Ok()
        {
            
            return Ok( Envelope.Ok());
        }

        protected new IActionResult Ok<T>(T result)
        {
           
            return Ok(Envelope.Ok(result));
        }
      
    }
}