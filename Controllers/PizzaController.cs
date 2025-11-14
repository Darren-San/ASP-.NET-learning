using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;
[ApiController]
[Route("api/pizzas")]
public class PizzaController : ControllerBase
{
    private readonly IPizzaService _pizzaService;

    public PizzaController(IPizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }

    [HttpGet]
    public ActionResult<List<Pizza>> GetAll() =>
        _pizzaService.GetAll();

    [HttpGet("{id:int}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = _pizzaService.Get(id);
        return pizza is null ? NotFound() : pizza;
    }

    [HttpPost]
    public IActionResult Create([FromBody] Pizza pizza)
    {
        _pizzaService.Add(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Pizza pizza)
    {
        if (id != pizza.Id)
            return BadRequest();

        var exists = _pizzaService.Get(id);
        if (exists is null)
            return NotFound();

        _pizzaService.Update(pizza);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var exists = _pizzaService.Get(id);
        if (exists is null)
            return NotFound();

        _pizzaService.Delete(id);
        return NoContent();
    }

    [HttpGet("glutenfree")]
    public Task<List<Pizza>> GlutenFree() =>
        _pizzaService.FindGlutenFreePizzas();
}
