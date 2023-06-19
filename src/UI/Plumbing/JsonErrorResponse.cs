using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Plumbing
{
    public class JsonErrorResponse
    {
        public string Error { get; set; }
        public string Issue { get; set; }
        public string StackTrace { get; set; }
    }
}