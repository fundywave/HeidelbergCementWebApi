using HeidelbergCement.Service.Interface;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace HeidelbergCement.Service.Provider
{
    public class DataProvider<T> : IDataProvider<T>
    {
        public T Get(string url)
        {
            try
            {
                using var client = new WebClient();
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                var respond = client.DownloadString(url);
                var result = System.Text.Json.JsonSerializer.Deserialize<T>(respond);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (T)Activator.CreateInstance(typeof(T));
            }
  
        }

        public T Post<A>(string url,A data)
        {
            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    var serializedData= JsonConvert.SerializeObject(data,Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                    streamWriter.Write(serializedData);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result= JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd());
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (T)Activator.CreateInstance(typeof(T));
            }
        }
    }
}
