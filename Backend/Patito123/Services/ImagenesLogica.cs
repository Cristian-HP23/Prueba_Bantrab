using Newtonsoft.Json;
using Patito123.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Patito123.Services
{
    public class ImagenesLogica
    {
        public static bool validateExtension(Stream stream)
        {
            try
            {
                using (Image img = Image.FromStream(stream))
                {
                    if (img.RawFormat.Equals(ImageFormat.Bmp) ||
                        img.RawFormat.Equals(ImageFormat.Gif) ||
                        img.RawFormat.Equals(ImageFormat.Jpeg) ||
                        img.RawFormat.Equals(ImageFormat.Png))
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool PostImagen(string nombre, string b64)
        {
            try
            {
                HttpClient client = new HttpClient();
                if (client.BaseAddress == null)
                    client.BaseAddress = new Uri(Properties.Settings.Default.url1.ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Add headers
                client.DefaultRequestHeaders.Add("user", Properties.Settings.Default.user.ToString());
                client.DefaultRequestHeaders.Add("password", Properties.Settings.Default.pass.ToString());

                var data = JsonConvert.SerializeObject(new { nombre = nombre, base64=b64 });
                var stringContent = new StringContent(data, Encoding.UTF8, "application/json");
                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage resp = client.PostAsync("api/v1/imagenes", stringContent).Result;
                //HttpResponseMessage resp = client.GetAsync("api/v1/imagenes").Result;
                if (resp.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static List<MImage> GetAllImagen()
        {
            try
            {
                HttpClient client = new HttpClient();
                if (client.BaseAddress == null)
                    client.BaseAddress = new Uri(Properties.Settings.Default.url1.ToString());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Add headers
                client.DefaultRequestHeaders.Add("user", Properties.Settings.Default.user.ToString());
                client.DefaultRequestHeaders.Add("password", Properties.Settings.Default.pass.ToString());
                HttpResponseMessage resp = client.GetAsync("api/v1/imagenes").Result;
                if (resp.IsSuccessStatusCode)
                {
                    var result = resp.Content.ReadAsStringAsync();
                    var deptList = JsonConvert.DeserializeObject<List<MImage>>(result.Result);
                    return deptList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool IsB64(string base64String)
        {
            String BASE64_REGEX_STRING = @"^[a-zA-Z0-9\+/]*={0,3}$";
            Regex _base64RegexPattern = new Regex(BASE64_REGEX_STRING, RegexOptions.Compiled);
            
            var rs = (!string.IsNullOrEmpty(base64String) && !string.IsNullOrWhiteSpace(base64String) && base64String.Length != 0 && base64String.Length % 4 == 0 && !base64String.Contains(" ") && !base64String.Contains("\t") && !base64String.Contains("\r") && !base64String.Contains("\n")) && (base64String.Length % 4 == 0 && _base64RegexPattern.Match(base64String, 0).Success);
            return rs;
        }

        public static List<MImage> filtraimagenes(List<MImage> listaO)
        {
            List<MImage> listAux = new List<MImage>();

            foreach(var img2 in listaO)
            {
                if (IsB64(img2.base64))
                {
                    listAux.Add(img2);
                }
            }

            return listAux;
        }
    }
}