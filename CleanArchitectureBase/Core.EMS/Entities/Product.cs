using Core.Utils.Entities;
using System;
using System.Collections.Generic;

namespace Core.EMS.Entities
{
    public partial class Product:BaseEntity
    {
        public string ProductName { get; set; }
        public decimal ProductCost { get; set; }
        public string ProductDescription { get; set; }
        public int ProductStock { get; set; }
    }
}
