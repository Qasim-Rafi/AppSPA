using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IFilesRepository
    {
        string SaveFile(IFormFile file);
        //object Download(string file);
        string AppendImagePath(string imageName);
        string AppendDocPath(string fileName);
        string GetBinaryFile(IFormFile file);
    }
}
