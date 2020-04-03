using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using System.Data.SqlClient;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.IO;


namespace Checkpoint3
{
    public class FileModule : NancyModule
    {
        public FileModule()
        {
            Get("/File", parameters => GetMainDirectory());
            Get("/File/{FileName}", parameters => 
            {
                string root = @".\Root\" + parameters.FileName;
                Files file = new Files()
                {
                    Root = root,
                    Name = parameters.FileName.ToString(),
                    Content = File.ReadAllText(root)
                };
                var response = JsonConvert.SerializeObject(file);
                return response;
            });
            Delete("/File/{FileName}", parameters =>
            {
                string fp = @".\Root\" + parameters.FileName;
                if (File.Exists(fp))
                {
                    File.Delete(fp);
                    return Response.AsJson(System.Net.HttpStatusCode.OK);
                }
                else
                {
                    return Response.AsJson(Nancy.HttpStatusCode.Forbidden);
                }
            });
            Put("/File/{FileName}/{Content}", parameters =>
            {
                string fileName = parameters.FileName;
                string fp = @".\Root\";
                string content = parameters.Content;
                if (!File.Exists(fp+fileName))
                {
                    Files file = new Files()
                    {
                        Name = "File",
                        Root = fp,
                        Content = content
                    };
                    using(StreamWriter writer = File.CreateText(fp+fileName))
                    {
                        writer.Write(content);
                    }
                    return Response.AsJson(Nancy.HttpStatusCode.OK);
                }
                else
                {
                    return Response.AsJson(Nancy.HttpStatusCode.NotFound);
                }
            });
        }

        public string GetMainDirectory()
        {
            string filepath = @".\Root";
            List<String> filenames = new List<string>();
            foreach(string file in Directory.GetFiles(filepath))
            {
                filenames.Add(file);
            }
            return JsonConvert.SerializeObject(filenames);
        }

        public string GetFile(object fileName)
        {
            string root = @".\Root\" + fileName;
            Files file = new Files()
            {
                Root = root,
                Name = fileName.ToString(),
                Content = File.ReadAllText(root) 
            };
            var response = JsonConvert.SerializeObject(file);
            return response;
        }


    }
}
