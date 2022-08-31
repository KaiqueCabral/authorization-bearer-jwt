using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationProject.DTOs;
using AuthenticationProject.Extensions;
using AuthenticationProject.Interfaces;
using AuthenticationProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationProject.Controllers;

[Produces("application/json")]
[Route("v1/Products")]
public class ProductsController : ControllerBase
{
    private readonly IRepository<Product> _repository;
    public ProductsController(IRepository<Product> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [AllowAnonymous]
    public IEnumerable<ProductDetails> Get()
    {
        return _repository.GetAll().Select(x => new ProductDetails()
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price,
            CategoryName = x.Category.Name
        });
    }

    [HttpGet("{id}", Name = "[controller]/Get")]
    [AllowAnonymous]
    public ProductDetails Get(int id)
    {
        var productDetails = _repository.GetById(id);

        return new ProductDetails()
        {
            Id = productDetails.Id,
            Name = productDetails.Name,
            Price = productDetails.Price,
            CategoryName = productDetails.Category.Name
        };
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Admin}, {UserRoles.Manager}")]
    public async Task<ActionResult> Post([FromBody] ProductAdd productDTO)
    {
        var product = new Product()
        {
            Name = productDTO.Name,
            Price = productDTO.Price,
            CategoryId = productDTO.CategoryId
        };

        await _repository.Add(product);

        return Ok(new
        {
            succeeded = true
        });
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Put(int id, [FromBody] ProductUpdate productUpdate)
    {
        var product = _repository.GetById(id);

        if (product == null)
        {
            return NotFound();
        }

        product.Name = productUpdate.Name;
        product.Price = productUpdate.Price;
        product.CategoryId = productUpdate.CategoryId;

        await _repository.Update(product);

        return Ok(new
        {
            succeeded = true
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult> Delete(int id)
    {
        await _repository.Delete(new Product()
        {
            Id = id
        });

        return Ok(new
        {
            succeeded = true
        });
    }
}
