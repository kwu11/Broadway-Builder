using System;
using System.IO;
using System.Web;

namespace TestUtilites
{
    public class MockPostedFile : HttpPostedFileBase
    {
        public override string ContentType { get; }

        public override int ContentLength { get; }

        public override string FileName { get; }

        public override Stream InputStream { get; }

        public override void SaveAs(string filename)
        {
            File.Create(filename).Close();
        }


        public MockPostedFile(string contentType, int contentLength, string fileName)
        {
            ContentType = contentType;
            ContentLength = contentLength;
            FileName = fileName;
        }
    }
}
