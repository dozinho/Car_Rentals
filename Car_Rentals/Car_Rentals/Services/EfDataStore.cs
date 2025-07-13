using Car_Rentals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Rentals.Services
{
    public class EfDataStore : IDataStore<Item>
    {
        private readonly CarRentalDbContext _context;

        public EfDataStore()
        {
            _context = new CarRentalDbContext();
            _context.Database.EnsureCreated();
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            _context.Items.Add(item);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            _context.Items.Update(item);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_context.Items.ToList());
        }
    }
} 