﻿using Domain.Core.Model;
using Infrastructure.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data
{
    public class NewsContextSeed
    {
        public void Seed(NewsContext newsContext)
        {
            if (!newsContext.Categories.Any())
            {
                newsContext.AddRange(GetCaregories());
                newsContext.SaveChanges();
            }

        }


        private IEnumerable<Category> GetCaregories()
        {
            return new List<Category>()
            {
                new Category() { Title = "Новости"},
                new Category() { Title = "Развлечения" },
                new Category() { Title = "Мода" },
                new Category() { Title = "Кулинария" }
            };
        }
    }
}
