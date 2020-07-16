using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class CustomerBasketDto
    {

        public string Id { get; set; } //generated in angular
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
