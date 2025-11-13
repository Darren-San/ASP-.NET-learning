using System.Security.Cryptography.X509Certificates;
using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class InDatabasePizzaService(PizzaDbContext context) : IPizzaService
{
    private readonly PizzaDbContext _context = context;

    public List<Pizza> GetAll()
    {
        return _context.Pizzas
            .AsNoTracking()
            .ToList();
    }

    public Pizza? Get(int id)
    {
        return _context.Pizzas
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == id);
    }

    public void Add(Pizza pizza)
    {
        _context.Pizzas.Add(pizza);
        _context.SaveChanges();
    }

    public void Update(Pizza pizza)
    {
        _context.Pizzas.Update(pizza);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var pizza = _context.Pizzas.Find(id);
        if (pizza is null)
            return;

        _context.Pizzas.Remove(pizza);
        _context.SaveChanges();
    }

    public  Task<List<Pizza>> FindGlutenFreePizzas()
    {
        return _context.Pizzas.Where<Pizza>(p => p.IsGlutenFree).ToListAsync();
    }
}