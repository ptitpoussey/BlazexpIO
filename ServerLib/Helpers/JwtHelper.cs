using BaseLib.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib.Helpers
{
    public class JwtHelper
    {
        public string? Key { get; set; }
        public string? Issuer {  get; set; }
        public string? Audience { get; set; }
    }
}
