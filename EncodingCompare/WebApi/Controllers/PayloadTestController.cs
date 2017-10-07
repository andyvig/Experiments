using Microsoft.AspNetCore.Mvc;
using Shared;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for Test Payloads
    /// </summary>
    [Route("api/[controller]")]
    public class PayloadTestController : Controller
    {
        private static string SomeStatic = "value1";

        /// <summary>
        /// Get method comment...
        /// </summary>
        /// <returns>Some values</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { SomeStatic, "value2" };
        }

        // GET api/values/5
        /// <summary>
        /// Payload of mixed data types
        /// </summary>
        /// <param name="count">How many objects to return</param>
        /// <returns>Good stuff</returns>
        [HttpGet("mixed/{count}")]
        public MixedPayload[] Get(int count)
        {
            var seed = ((int)DateTime.Now.Ticks % 10000);
            var list = new List<MixedPayload>(count);
            for(int i = 0; i < count; i++)
            {
                list.Add(MixedPayload.Create(seed + i));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Payload of numbers
        /// </summary>
        /// <param name="count">How many objects to return</param>
        /// <returns>Good stuff</returns>
        [HttpGet("numeric/{count}")]
        public NumericPayload[] GetNumeric(int count)
        {
            var list = new List<NumericPayload>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(NumericPayload.Create(i));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Payload from Facebook
        /// </summary>
        /// <param name="count">How many objects to return</param>
        /// <returns>Good stuff</returns>
        [HttpGet("facebook/{count}")]
        public FacebookPayload[] GetFacebook(int count)
        {
            var list = new List<FacebookPayload>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(FacebookPayload.Create());
            }
            return list.ToArray();
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
            SomeStatic = value;
        }
    }
}
