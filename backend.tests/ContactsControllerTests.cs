using System.Net;
using System.Net.Http.Json;
using backend.Models;
using FluentAssertions;

namespace backend.tests;

public class ContactsControllerTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory _factory;

    public ContactsControllerTests(CustomWebAppFactory factory)
    {
        _factory = factory;
        // Ensure a clean database for each test run when using the shared factory
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }


    //-------Helper Methods-------
    private static Contact NewContact(string? email = null)=> new()
    {
        Name = "Alice",
        Email = email ?? $"{Guid.NewGuid():N}@example.com",
        Phone = "123-456-7890",
        Note = "note"
    };

    private async Task<Contact> CreateAsync(Contact c)
    {
        var res = await _client.PostAsJsonAsync("/api/contacts", c);
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await res.Content.ReadFromJsonAsync<Contact>();
        created.Should().NotBeNull();
        created!.Id.Should().BeGreaterThan(0);
        // echo check
        created.Email.Should().Be(c.Email);
        return created!;
    }
    // ---------- CREATE / READ ----------

    [Fact]
    public async Task Create_Then_GetAll_Should_Return_One()
    {
        var c = NewContact();
        await CreateAsync(c);

        var list = await _client.GetFromJsonAsync<List<Contact>>("/api/contacts");
        list.Should().NotBeNull();
        list!.Should().HaveCount(1);
        list[0].Email.Should().Be(c.Email);
    }

    [Fact]
    public async Task Get_NotFound_When_Id_Does_Not_Exist()
    {
        var res = await _client.GetAsync("/api/contacts/9999");
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_Should_Validate_Email_Format()
    {
        var bad = NewContact(email: "not-an-email");
        var res = await _client.PostAsJsonAsync("/api/contacts", bad);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await res.Content.ReadAsStringAsync();
        body.Should().Contain("Email");
    }

    [Fact]
    public async Task Duplicate_Email_On_Create_Should_Return_Conflict()
    {
        var email = $"{Guid.NewGuid():N}@example.com";
        await CreateAsync(NewContact(email));
        var res = await _client.PostAsJsonAsync("/api/contacts", NewContact(email));
        res.StatusCode.Should().Be(HttpStatusCode.Conflict);
        (await res.Content.ReadAsStringAsync()).Should().Contain("email");
    }

    // ---------- UPDATE ----------
    [Fact]
    public async Task Update_Should_Modify_Fields()
    {
        var created = await CreateAsync(NewContact());
        created.Name = "Updated Name";
        created.Phone = "000-000-0000";

        var put = await _client.PutAsJsonAsync($"/api/contacts/{created.Id}", created);
        put.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var fetched = await _client.GetFromJsonAsync<Contact>($"/api/contacts/{created.Id}");
        fetched.Should().NotBeNull();
        fetched!.Name.Should().Be("Updated Name");
        fetched.Phone.Should().Be("000-000-0000");
    }

    [Fact]
    public async Task Update_With_Id_Mismatch_Should_Return_BadRequest()
    {
        var created = await CreateAsync(NewContact());
        var wrongId = new Contact
        {
            Id = created.Id + 1, // intentionally wrong ID
            Name = "Wrong ID",
            Email = $"{Guid.NewGuid():N}@example.com",
            Phone = created.Phone,
            Note = created.Note
        };
        var put = await _client.PutAsJsonAsync($"/api/contacts/{created.Id}", wrongId);
        put.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await put.Content.ReadAsStringAsync()).Should().Contain("ID mismatch");
    }

    [Fact]
    public async Task Update_Duplicate_Email_Should_Return_Conflict()
    {
        var a = await CreateAsync(NewContact());
        var b = await CreateAsync(NewContact());

        // try to update b with a's email -> should conflict since email should be unique
        b.Email = a.Email;
        var put = await _client.PutAsJsonAsync($"/api/contacts/{b.Id}", b);
        put.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_NotFound_When_Target_Missing()
    {
        var fake = NewContact();
        fake.Id = 9999;

        var put = await _client.PutAsJsonAsync($"/api/contacts/{fake.Id}", fake);
        put.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ---------- DELETE ----------

    [Fact]
    public async Task Delete_Should_Remove_Item()
    {
        var created = await CreateAsync(NewContact());
        var del = await _client.DeleteAsync($"/api/contacts/{created.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var get = await _client.GetAsync($"/api/contacts/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_NotFound_When_Id_Does_Not_Exist()
    {
        var res = await _client.DeleteAsync("/api/contacts/9999");
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
