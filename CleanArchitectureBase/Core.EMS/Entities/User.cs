using Core.Utils.Entities;
using System;
using System.Collections.Generic;

#nullable disable

namespace Core.EMS.Entities
{
    public partial class User:BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
