using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLib.DTOs
{
    public class ApplicationSession
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
