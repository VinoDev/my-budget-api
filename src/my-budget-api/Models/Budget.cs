﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_budget_api.Models
{
    public class Budget
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }
        
    }
}
