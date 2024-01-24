using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptcent.Cloud.Drive.WebApi.Infrastructure
{
    public static class GetContentTypeUtil
    {
        public static string GetContentType(string fileName)
        {
            if (fileName.Contains(".jpg"))
            {
                return "image/jpg";
            }
            else if (fileName.Contains(".jpeg"))
            {
                return "image/jpeg";
            }
            else if (fileName.Contains(".png"))
            {
                return "image/png";
            }
            else if (fileName.Contains(".gif"))
            {
                return "image/gif";
            }
            else if (fileName.Contains(".pdf"))
            {
                return "application/pdf";
            }
            else if (fileName.Contains(".docx"))
            {
                return "application/msword";
            }
            else if (fileName.Contains(".txt"))
            {
                return "text/plain";
            }
            else
            {
                return "application/octet-stream";
            }
        }
    }
}
