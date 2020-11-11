using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_budget_api.Models
{
    public class Expense
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }

    }
}
