using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContactsController(AppDbContext db) : ControllerBase
{
    /// <summary>Get all contacts.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Contact>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Contact>>> GetAll(CancellationToken ct)
        => Ok(await db.Contacts.AsNoTracking().ToListAsync(ct));

    /// <summary>Get a single contact by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Contact>> Get(int id, CancellationToken ct)
        => await db.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct) is { } c
            ? Ok(c)
            : NotFound();

    /// <summary>Create a new contact.</summary>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Contact>> Create([FromBody] ContactCreateDto dto, CancellationToken ct)
    {
        // [ApiController] will automatically validate the model and return 400 if invalid.
        // Map DTO to entity
        var c = new Contact
        {
            Name  = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone ?? string.Empty,
            Note  = dto.Note
        };

        db.Contacts.Add(c);
        await db.SaveChangesAsync(ct); // UNIQUE violation → global handler returns 409
        return CreatedAtAction(nameof(Get), new { id = c.Id }, c);
    }

    /// <summary>Update an existing contact.</summary>
    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] ContactUpdateDto dto, CancellationToken ct)
    {
        if (id != dto.Id)
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Id mismatch.");

        var entity = await db.Contacts.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        // only update fields that are required or can be changed
        entity.Name  = dto.Name;
        entity.Email = dto.Email;
        entity.Phone = dto.Phone ?? string.Empty;
        entity.Note  = dto.Note;

        await db.SaveChangesAsync(ct); // UNIQUE violation → global handler returns 409
        return NoContent();
    }

    /// <summary>Delete a contact by id.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var entity = await db.Contacts.FindAsync([id], ct);
        if (entity is null) return NotFound();

        db.Contacts.Remove(entity);
        await db.SaveChangesAsync(ct);
        return NoContent();
    }
}
