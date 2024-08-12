﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDCBackendProject.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public bool isRemoved { get; set; } = false;
    }
}
