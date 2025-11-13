using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController(IPizzaService pizzaService) : ControllerBase
{
    private readonly IPizzaService _pizzaService = pizzaService;

    [HttpGet]
    public ActionResult<List<Pizza>> GetAll() =>
        _pizzaService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = _pizzaService.Get(id);
        if (pizza is null)
            return NotFound();
        return pizza;
    }

    [HttpPost]
    public IActionResult Create(Pizza pizza)
    {
        _pizzaService.Add(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Pizza pizza)
    {
        if (id != pizza.Id)
            return BadRequest();

        var existingPizza = _pizzaService.Get(id);
        if (existingPizza is null)
            return NotFound();

        _pizzaService.Update(pizza);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = _pizzaService.Get(id);
        if (pizza is null)
            return NotFound();

        _pizzaService.Delete(id);
        return NoContent();
    }

    [HttpGet("glutenfree")]
    public ActionResult<Task<List<Pizza>>> SinGluten() =>
        _pizzaService.FindGlutenFreePizzas();
}
