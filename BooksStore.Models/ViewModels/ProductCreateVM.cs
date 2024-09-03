﻿using BooksStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models.ViewModels
{
    public class ProductCreateVM
    {
        public ProductAddRequest? ProductAddRequest { get; set; }
        public IEnumerable<SelectListItem>? CategoriesList { get; set; }
    }
}
