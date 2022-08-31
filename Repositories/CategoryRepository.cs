using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationProject.Data;
using AuthenticationProject.Interfaces;
using AuthenticationProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationProject.Repositories;

public class CategoryRepository : IRepository<Category>
{
    private readonly StoreDataContext _context;

    public CategoryRepository(StoreDataContext context)
    {
        _context = context;
    }

    public async Task Add(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Category category)
    {
        _context.Entry(category).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public IEnumerable<Category> GetAll()
    {
        return _context.Categories.AsNoTracking().ToList();
    }

    public Category GetById(int id)
    {
        return _context.Categories.AsNoTracking().FirstOrDefault(x => x.Id == id);
    }
}
