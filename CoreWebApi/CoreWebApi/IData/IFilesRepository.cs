using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IFilesRepository
    {
        //void Upload(IFormFileCollection files);
        //object Download(string file);
        string AppendImagePath(string imageName);
    }
}
