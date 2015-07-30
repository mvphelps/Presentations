using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SoulService.Controllers
{
    public class SoulsController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            var bs = new BlobStorage("CloudStorageConnectionString", "blob");
            return bs.ListBlobs();
        }

        
        // POST api/values
        public void Post([FromBody]byte[] value)
        {
            string blobName = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFF") + ".bmp";
            var bs = new BlobStorage("CloudStorageConnectionString", "blob");
            bs.PutBlob(value, blobName, "application/octet-stream");
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        // GET api/values/5
        public string Get(int id)
        {
            return "Not implemented";
        }

    }
}