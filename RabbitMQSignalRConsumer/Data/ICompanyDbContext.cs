﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RabbitMQSignalRConsumer.Data
{
    public interface ICompanyDbContext
    {

        DbSet<UserCompanySubscription> UserCompanySubscriptions { get; set; }
        public DbSet<CompanyPrice> CompanyPrices { get; set; }
        public DbSet<CompanyMaster> CompanyMasters { get; set; }
        public DbSet<Users> Users { get; set; }
        void AddOrUpdate<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync();

    }
}
