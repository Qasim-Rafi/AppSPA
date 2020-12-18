﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IEmailRepository
    {
        bool Send(string from, string to, string subject, string html);
    }
}
