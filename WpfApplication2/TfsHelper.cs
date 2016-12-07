﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WpfApplication2.Models;

namespace WpfApplication2
{
    public static class TfsHelper
    {
        private static string baseUrl =
            "http://tfstta.int.thomson.com:8080/tfs/DefaultCollection/_apis/tfvc/changesets?searchCriteria.itemPath=";

        public static ItemHistory GetItemHistory(string item)
        {
            try
            {
                var url = new Uri(baseUrl + item);
                //Use Windows credentials
                using (var httpClient = new HttpClient(new HttpClientHandler
                {
                    UseDefaultCredentials = true
                }))
                {
                    var result = httpClient.GetStringAsync(url).Result;
                    var itemHistory = JsonConvert.DeserializeObject<ItemHistory>(result);
                    return itemHistory;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static List<TfsItemViewModel> GetData(string slnPath, string tfsPath)
        {
            var files = new List<string>();
            if (Directory.Exists(slnPath))
            {
                //TODO: limited file types to cs
                files = Directory.GetFiles(slnPath, "*.cs", SearchOption.AllDirectories).ToList();
            }
            else if (File.Exists(slnPath))
            {
                files.Add(slnPath);
            }

            var tfsItems = new List<TfsItemViewModel>();
            Parallel.ForEach(files, file =>
            {
                var tfsFile = tfsPath + file.Replace(slnPath, "").Replace(@"\", "/");
                var result = TfsHelper.GetItemHistory(tfsFile);
                if (result != null)
                {
                    tfsItems.Add(new TfsItemViewModel()
                    {
                        FullPath = file,
                        Name = Path.GetFileName(file),
                        BugCount = Convert.ToInt32(result.Count),
                        Score = 10,
                        Size = new FileInfo(file).Length
                    });
                }
            });
            return tfsItems;
        }
    }
}
