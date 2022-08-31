using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationProject.Data;
using AuthenticationProject.Interfaces;
using AuthenticationProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationProject.Repositories;

public class ProductRepository : IRepository<Product>
{
    private readonly StoreDataContext _context;

    public ProductRepository(StoreDataContext context)
    {
        _context = context;
    }

    public async Task Add(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public IEnumerable<Product> GetAll()
    {
        return _context.Products.Include(x => x.Category).ToList();
    }

    public Product GetById(int id)
    {
        return _context.Products.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
    }
}