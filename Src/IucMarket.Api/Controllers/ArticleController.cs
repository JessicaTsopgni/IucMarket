using IucMarket.Dtos;
using IucMarket.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IucMarket.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private const string Error = "An error occured. Please try again later.";
        private readonly ProductService service;
        private readonly IWebHostEnvironment env;
        private const string Upload_Folder = "Uploads";
        public ArticleController(ProductService service, IWebHostEnvironment env)
        {
            this.service = service;
            this.env = env;
        }

        private string GetPathTemplate()
        {
            return Request.Scheme + "://" + Request.Host.Value + "/Article/Downlaod/{0}?contentType={1}";
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 100)
        {
            try
            {
                return Ok
                (
                    await service.GetProductsAsync
                    (
                        GetPathTemplate(), 
                        pageIndex, 
                        pageSize
                    )
                );
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
                //return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Categories(string id, int pageIndex = 1, int pageSize = 100)
        {
            try
            {
                return Ok
                (
                    await service.GetProductsByCategoryAsync
                    (
                        id,
                        GetPathTemplate(),
                        pageIndex,
                        pageSize
                    )
                );
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
                //return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                return Ok
                (
                    await service.GetProductAsync
                    (
                        id,
                        GetPathTemplate()
                    )
                );
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
                //return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IEnumerable<IFormFile> pictures)
        {
            ProductAddCommand command = null;
            try
            {
                command = JsonConvert.DeserializeObject<ProductAddCommand>(Request.Form["Product"].ToString());

                command.Pictures = await UploadProductFiles(pictures, null);

                return Ok
                (
                    await service.AddAsync(command, GetPathTemplate())
                );
            }
            catch(DuplicateWaitObjectException ex)
            {
                DeleteProductFiles(command.Pictures?.Select(x => new FileInfoDto(GetPathTemplate(), x.Key, x.Value)));
                return Conflict(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
                //return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                DeleteProductFiles(command.Pictures?.Select(x => new FileInfoDto(GetPathTemplate(), x.Key, x.Value)));
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        private void DeleteProductFiles(IEnumerable<FileInfoDto> pictures)
        {
            if (pictures != null)
            {
                foreach (var p in pictures)
                {
                    var path = GetPath(p.Name);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(string id, [FromForm] IEnumerable<IFormFile> pictures)
        {
            ProductAddCommand command = null;
            try
            {
                var oldProduct = await service.GetProductAsync
                (
                    id,
                    GetPathTemplate()
                );
                if (oldProduct == null)
                    throw new KeyNotFoundException($"Product {id} not found !");

                command = JsonConvert.DeserializeObject<ProductAddCommand>(Request.Form["Product"].ToString());
                
                command.Pictures = await UploadProductFiles (pictures, oldProduct.Pictures);

                await service.EditAsync
                (
                    id,
                    command,
                    GetPathTemplate()
                );
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DuplicateWaitObjectException ex)
            {
                return Conflict(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
                //return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        private async Task<Dictionary<string, string>> UploadProductFiles
            (IEnumerable<IFormFile> pictures,  IEnumerable<FileInfoDto> oldFileInfos)
        {
            var fileNames = new Dictionary<string, string>();
            if (pictures != null)
            {
                foreach (var picture in pictures)
                {
                    var fileName = picture.FileName;
                    if (picture.Length > 0)
                    {
                        fileName = System.IO.Path.GetRandomFileName();
                        var path = GetPath(fileName);
                        System.IO.FileInfo f = new System.IO.FileInfo(path);
                        if (!f.Directory.Exists)
                            f.Directory.Create();
                        using System.IO.MemoryStream ms = new();
                        await picture.CopyToAsync(ms);
                        await System.IO.File.WriteAllBytesAsync(path, ms.ToArray());
                    }
                    fileNames.Add(fileName, pictures.FirstOrDefault(x => x.Name == picture.FileName)?.ContentType ?? "image/jpg");
               }
            }

            if (oldFileInfos != null)
            {
                foreach (var oldf in oldFileInfos)
                {
                    if (!pictures.Any(x=> x.FileName == oldf.Name))
                    {
                        string path = GetPath(oldf.Name);
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                    }
                }
            }

            return fileNames;
        }

        private string GetPath(string oldFileName)
        {
            return System.IO.Path.Combine(env.WebRootPath, Upload_Folder, oldFileName);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var product = await service.GetProductAsync
                (
                    id,
                    GetPathTemplate()
                );
                await service.DeleteAsync(id);
                DeleteProductFiles(product.Pictures);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
                //return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public FileContentResult Downlaod(string id, string contentType)
        {
            try
            {     
                var path = GetPath(id);
                if (System.IO.File.Exists(path))
                {
                    var myfile = System.IO.File.ReadAllBytes(path);
                    return new FileContentResult(myfile, contentType ?? "image/jpg");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
            }
            return null;
        }
    }
}
