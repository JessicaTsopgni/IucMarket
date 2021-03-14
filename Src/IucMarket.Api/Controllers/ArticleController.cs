using IucMarket.Entities;
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

        public ArticleController(ProductService service, IWebHostEnvironment env)
        {
            this.service = service;
            this.env = env;
        }

        private string GetPathTemplate()
        {
            return Request.Scheme + "://" + Request.Host.Value + "/article/image/{0}?contentType={1}";
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
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IEnumerable<IFormFile> pictures)
        {
            Product product = null;
            try
            {
                product = JsonConvert.DeserializeObject<Product>(Request.Form["Product"].ToString());

                Dictionary<string, string> fileNames = await UploadProductFiles(pictures, product);

                product.Pictures = fileNames.Select(x => new FileInfo(x.Key, x.Value));

                return Ok
                (
                    await service.AddAsync
                    (
                        product,
                        GetPathTemplate()
                    )
                );
            }
            catch(DuplicateWaitObjectException ex)
            {
                DeleteProductFiles(product);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                DeleteProductFiles(product);
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        private void DeleteProductFiles(Product product)
        {
            if (product != null && product.Pictures != null)
            {
                foreach (var p in product.Pictures)
                {
                    var path = System.IO.Path.Combine(env.ContentRootPath, "images", p.Name);
                    System.IO.File.Delete(path);
                }
            }
        }

        [HttpPut]
        [Route("{id}/{deleteFile}")]
        public async Task<IActionResult> Put(string id, bool deleteFile, IEnumerable<IFormFile> pictures)
        {
            Product product = null;
            try
            {
                var oldProduct = await service.GetProductAsync
                (
                    id,
                    GetPathTemplate()
                );
                if (oldProduct == null)
                    throw new KeyNotFoundException($"{nameof(Product)} {id} not found !");

                product = JsonConvert.DeserializeObject<Product>(Request.Form["Product"].ToString());
                
                Dictionary<string, string> fileNames = await UploadProductFiles(pictures, product);
                
                if (deleteFile)
                {
                    if (fileNames.Count != 0)
                        DeleteProductFiles(product);

                    product.Pictures = fileNames.Count == 0
                        ? oldProduct.Pictures
                        : fileNames.Select(x => new FileInfo(x.Key, x.Value));
                }
                else
                {
                    product.Pictures = fileNames.Count == 0
                        ? oldProduct.Pictures
                        : oldProduct.Pictures.Concat(fileNames.Select(x => new FileInfo(x.Key, x.Value)));
                }

                await service.EditAsync
                (
                    id,
                    product,
                    GetPathTemplate()
                );
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DuplicateWaitObjectException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        private async Task<Dictionary<string, string>> UploadProductFiles(IEnumerable<IFormFile> pictures, Product product)
        {
            var fileNames = new Dictionary<string, string>();
            if (pictures != null)
            {
                foreach (var picture in pictures)
                {
                    var fileName = System.IO.Path.GetRandomFileName();
                    var path = System.IO.Path.Combine
                    (
                        env.ContentRootPath,
                        "images",
                        fileName
                    );
                    System.IO.FileInfo f = new System.IO.FileInfo(path);
                    if (!f.Directory.Exists)
                        f.Directory.Create();
                    using System.IO.MemoryStream ms = new();
                    await picture.CopyToAsync(ms);
                    await System.IO.File.WriteAllBytesAsync(path, ms.ToArray());
                    fileNames.Add(fileName, product.Pictures.FirstOrDefault(x => x.Name == picture.FileName)?.ContentType ?? "image/jpg");
                }
            }

            return fileNames;
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
                DeleteProductFiles(product);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public FileContentResult Image(string id, string contentType)
        {
            try
            {     
                var path = System.IO.Path.Combine(env.ContentRootPath, "images", id);
                System.IO.FileInfo f = new System.IO.FileInfo(path);
                if (f.Exists)
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
