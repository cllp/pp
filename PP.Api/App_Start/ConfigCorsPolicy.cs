using Microsoft.Owin;
using Microsoft.Owin.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;

namespace PP.Api
{
    public class ConfigCorsPolicy : ICorsPolicyProvider
    {
        private CorsPolicy _policy;

        public ConfigCorsPolicy(string origin)
        {
            // Create a CORS policy.
            _policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true
            };
            // Add allowed origins.
            string[] origins = origin.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string o in origins)
                _policy.Origins.Add(o);
            //_policy.Origins.Add(origin);
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(IOwinRequest request)
        {
            return Task.FromResult(_policy);
        }
    }
}