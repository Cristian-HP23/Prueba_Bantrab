using Patito123.Models;
using Patito123.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Patito123.Controllers
{
    public class ImagenController : ApiController
    {
        [HttpPost]
        //[Authorize]
        [Route("api/v1/imagenes")]
        public async Task<MRespuesta> uploadImage([FromUri]string nombre)
        {
            MRespuesta res = new MRespuesta();
            res.codigo = 0;
            res.error = null;
            res.imagenes = null;

            string root = HttpContext.Current.Server.MapPath("~/App_Data");

            var provider = new MultipartFormDataStreamProvider(root);

            if (nombre == "")
            {
                res.mensaje = "Debe enviar el nombre de la imagen";
                return res;
            }

            //multipart

            if (!Request.Content.IsMimeMultipartContent())
            {
                res.mensaje = "NO viene imagen file";
                return res;
            }


            await Request.Content.ReadAsMultipartAsync(provider);

            FileInfo fileInfoimage = null;


            foreach(MultipartFileData fileData in provider.FileData)
            {
                fileInfoimage = new FileInfo(fileData.LocalFileName);
                break;
            }

            if(fileInfoimage != null)
            {
                string extension = fileInfoimage.Extension;
                using(FileStream fs= fileInfoimage.Open(FileMode.Open, FileAccess.Read))
                {
                    string b64 = ConvertB64(fs);

                    if (!ImagenesLogica.validateExtension(fs))
                    {
                        res.mensaje = "Formato de Archivo no Valido";
                        return res;
                    }
                    

                    if (ImagenesLogica.PostImagen(nombre, b64))
                    {
                        res.mensaje = "Imagen Guardada Exitosamente";
                        res.codigo = 1;
                        return res;
                    }
                    else
                    {
                        res.mensaje = "Erro al intentar guarda la Imagen";
                        res.codigo = 0;
                    }
                }
            }

            return res;
        }


        [HttpGet]
        //[Authorize]
        [Route("api/v1/imagenes")]
        public async Task<MRespuesta> getAllImages()
        {
            List<MImage> listaimagenes = null;
            MRespuesta res = new MRespuesta();
            res.codigo = 0;
            res.error = null;
            res.imagenes = null;
            listaimagenes= ImagenesLogica.GetAllImagen();

            if (listaimagenes == null)
            {
                res.mensaje = "Error al obtener las imagenes";
                return res;
            }
             res.imagenes= ImagenesLogica.filtraimagenes(listaimagenes).ToArray();
            res.mensaje = "Exito";
            res.codigo = 1;

            return res;
        }

        public  string ConvertB64(Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }

    }

}
