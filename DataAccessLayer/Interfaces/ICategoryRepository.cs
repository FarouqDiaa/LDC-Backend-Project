﻿using LDCBackendProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDCBackendProject.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Category GetById(int id);
    }
}
