﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace TodosIntegrationTest.TestSetup
{
    public class TestFixture<TProgram> where TProgram : class
    {
        public TestFixture()
        {
            var type = typeof(TProgram);
            var methodInfo = type.GetMethod("Main", BindingFlags.NonPublic | BindingFlags.Static);
            const string port = "5001";
            string[] args = { port };
            methodInfo.Invoke(null, new[] { args });

            //var directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory());
            //var solutionDirectory = $"{directoryInfo.Parent.Parent.Parent.Parent.FullName}";
            //var projectName = type.FullName.Split('.')[0];
            //var launchSettingsPath = $"{solutionDirectory}\\{projectName}\\Properties\\launchSettings.json";

            //using var file = File.OpenText(launchSettingsPath);
            //using var reader = new JsonTextReader(file);
            //dynamic o2 = JToken.ReadFrom(reader);
            //var port = o2.iisSettings.iisExpress.sslPort;

            Client = new HttpClient { BaseAddress = new Uri($"https://localhost:{port}") };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HttpClient Client { get; set; }

        private string GeneratePort()
        {
            var generator = new Random();
            var r = generator.Next(1, 100000);
            var s = r.ToString().PadLeft(5, '0');
            return s;
        }
    }
}