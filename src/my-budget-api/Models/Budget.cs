using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_budget_api.Models
{
    public class Budget
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "Amount")]
        public int Amount { get; set; }
        
    }
}
