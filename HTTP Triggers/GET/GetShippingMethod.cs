using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WsiApi.Data;
using WsiApi.Models;

namespace WsiApi.HTTP_Triggers
{
    public class GetShippingMethod
    {
        private readonly string cs;
        public GetShippingMethod(SqlConnectionStringBuilder builder)
        {
            cs = builder.ConnectionString;
        }

        [FunctionName("GetShippingMethod")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "shipping/{code:alpha?}")] HttpRequest req,
            string? code,
            ILogger log)
        {
            if (code != null)
            {
                ShippingMethodModel method = ShippingMethods.GetShippingMethods(code, cs);

                if (method == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(method);
            }

            List<ShippingMethodModel> methods = ShippingMethods.GetShippingMethods(cs);
            return new OkObjectResult(methods);
        }
    }
}
