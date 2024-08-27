﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Project.InfrastructureLayer.Entities;
using Project.InfrastructureLayer.Interfaces;
using Project.InfrastructureLayer.Migrations;

namespace Project.InfrastructureLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;


        public CustomerRepository(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            return await _context.Set<Customer>().SingleOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Set<Customer>().AddAsync(customer);
        }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            var cacheKey = $"Customer-{email}";
            Customer customer;
            if (!_cache.TryGetValue(cacheKey, out customer))
            {
                customer = await _context.Customers.AsNoTracking()
                                                   .Where(c => c.Email == email)
                                                   .SingleOrDefaultAsync();

                if (customer != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    };

                    _cache.Set(cacheKey, customer, cacheOptions);
                }
            }

            return customer;
        }


        public async Task<bool> CustomerExistsAsync(string email)
        {
            var cacheKey = $"CustomerExists-{email}";
            if (!_cache.TryGetValue(cacheKey, out bool exists))
            {
                exists = await _context.Customers
                                       .AsNoTracking()
                                       .AnyAsync(c => c.Email == email);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                _cache.Set(cacheKey, exists, cacheOptions);

            }

            return exists;
        }
        public async Task<bool> CustomerExistsWithIdAsync(Guid customerId)
        {
            var cacheKey = $"CustomerExists-{customerId}";
            if (!_cache.TryGetValue(cacheKey, out bool exists))
            {
                exists = await _context.Customers
                                       .AsNoTracking()
                                       .AnyAsync(c => c.CustomerId == customerId);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                _cache.Set(cacheKey, exists, cacheOptions);

            }

            return exists;
        }
        public async Task<bool> IsAdmin(Guid id)
        {
            var cacheKey = $"IsAdmin-{id}";

            if (!_cache.TryGetValue(cacheKey, out bool isAdmin))
            {
                isAdmin = await _context.Customers.AsNoTracking()
                                                  .Where(c => c.CustomerId == id)
                                                  .Select(c => c.IsAdmin)
                                                  .FirstOrDefaultAsync();

                if (isAdmin)
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    };

                    _cache.Set(cacheKey, isAdmin, cacheOptions);
                }
            }

            return isAdmin;
        }

    }
}
