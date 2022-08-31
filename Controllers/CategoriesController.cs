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
[Route("v1/Categories")]
public class CategoriesController : ControllerBase
{
    private readonly IRepository<Category> _repository;
    public CategoriesController(IRepository<Category> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [AllowAnonymous]
    public IEnumerable<CategoryDetails> Get()
    {
        return _repository.GetAll().Select(x => new CategoryDetails()
        {
            Id = x.Id,
            Name = x.Name
        });
    }

    [HttpGet("{id}", Name = "[controller]/Get")]
    [AllowAnonymous]
    public CategoryDetails Get(int id)
    {
        var categoryDetails = _repository.GetById(id);

        return new CategoryDetails()
        {
            Id = categoryDetails.Id,
            Name = categoryDetails.Name
        };
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Admin}, {UserRoles.Manager}")]
    public async Task<ActionResult> Post([FromBody] CategoryAdd categoryDTO)
    {
        var category = new Category()
        {
            Name = categoryDTO.Name
        };

        await _repository.Add(category);

        return Ok(new
        {
            succeeded = true
        });
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Put(int id, [FromBody] CategoryUpdate categoryUpdate)
    {
        var category = _repository.GetById(id);

        if (category == null)
        {
            return NotFound();
        }

        category.Name = categoryUpdate.Name;

        await _repository.Update(category);

        return Ok(new
        {
            succeeded = true
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult> Delete(int id)
    {
        await _repository.Delete(new Category()
        {
            Id = id
        });

        return Ok(new
        {
            succeeded = true
        });
    }
}
