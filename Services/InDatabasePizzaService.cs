using System.Security.Cryptography.X509Certificates;
using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;
public class InDatabasePizzaService : IPizzaService
{
    private readonly PizzaDbContext _context;

    public InDatabasePizzaService(PizzaDbContext context)
    {
        _context = context;
    }

    public List<Pizza> GetAll() =>
        _context.Pizzas.AsNoTracking().ToList();

    public Pizza? Get(int id) =>
        _context.Pizzas.AsNoTracking().FirstOrDefault(p => p.Id == id);

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

    public Task<List<Pizza>> FindGlutenFreePizzas() =>
        _context.Pizzas
            //  retrieve data from the database without enabling change tracking for the returned entities
            .AsNoTracking()
            .Where(p => p.IsGlutenFree)
            .ToListAsync();
}
