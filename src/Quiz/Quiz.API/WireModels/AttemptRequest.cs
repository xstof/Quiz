using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.API.WireModels
{
    public class AttemptRequest
    {
        public string Email { get; set; }

        public bool IsValid()
        {
            if (!string.IsNullOrWhiteSpace(Email))
            {
                return true;
            }
            else return false;
        }
    }
}