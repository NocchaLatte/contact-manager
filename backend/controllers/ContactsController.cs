using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contact>>> GetAll()
        => await db.Contacts.ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Contact>> Get(int id)
    => await db.Contacts.FindAsync(id) is { } c ? c : NotFound(); // tried to use Ternary conditional operator for pattern matching added in C# 8.0

    [HttpPost]
    public async Task<ActionResult<Contact>> Create([FromBody] Contact c)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        db.Contacts.Add(c);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
        {
            return Conflict(new { Message = "A contact with this email already exists." });
        }
        return CreatedAtAction(nameof(Get), new { id = c.Id }, c);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Contact>> Update(int id, [FromBody] Contact c)
    {
        if (id != c.Id) return BadRequest("Contact ID mismatch.");
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        //check if contact exists
        var exists = await db.Contacts.AnyAsync(x => x.Id == id);
        if (!exists) return NotFound();

        db.Entry(c).State = EntityState.Modified;
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
        {
            return Conflict(new { Message = "A contact with this email already exists." });
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await db.Contacts.FindAsync(id);
        if (contact is null) return NotFound();

        db.Contacts.Remove(contact);
        await db.SaveChangesAsync();
        return NoContent();
    }
}