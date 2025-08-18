using System.Net;
using System.Net.Http.Json;
using backend.Models;
using FluentAssertions;

namespace backend.tests;

public class ContactsControllerTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;

    public ContactsControllerTests(CustomWebAppFactory factory)
    {
        // Ensure a clean database for each test run when using the shared factory
        factory.ResetDatabase();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_Then_GetAll_Should_Return_One()
    {
        var alice = new Contact { Name = "Alice", Email = "alice@example.com", Phone = "123" };
        var create = await _client.PostAsJsonAsync("/api/contacts", alice);
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var list = await _client.GetFromJsonAsync<List<Contact>>("/api/contacts");
        list.Should().NotBeNull();
        list!.Count.Should().Be(1);
        list[0].Email.Should().Be("alice@example.com");
    }

    [Fact]
    public async Task Duplicate_Email_Should_Return_Conflict()
    {
        var c1 = new Contact { Name = "A", Email = "dup@example.com", Phone = "1" };
        var c2 = new Contact { Name = "B", Email = "dup@example.com", Phone = "2" };

        var r1 = await _client.PostAsJsonAsync("/api/contacts", c1);
        r1.StatusCode.Should().Be(HttpStatusCode.Created);

        var r2 = await _client.PostAsJsonAsync("/api/contacts", c2);
        r2.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_Should_Modify_Name()
    {
        var c = new Contact { Name = "Temp", Email = "t@example.com", Phone = "0" };
        var created = await (await _client.PostAsJsonAsync("/api/contacts", c))
            .Content.ReadFromJsonAsync<Contact>();

        created.Should().NotBeNull();
        created!.Name = "Updated";

        var put = await _client.PutAsJsonAsync($"/api/contacts/{created.Id}", created);
        put.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var fetched = await _client.GetFromJsonAsync<Contact>($"/api/contacts/{created.Id}");
        fetched!.Name.Should().Be("Updated");
    }

    [Fact]
    public async Task Delete_Should_Remove_Item()
    {
        var c = new Contact { Name = "Todelete", Email = "del@example.com", Phone = "0" };
        var created = await (await _client.PostAsJsonAsync("/api/contacts", c))
            .Content.ReadFromJsonAsync<Contact>();

        var del = await _client.DeleteAsync($"/api/contacts/{created!.Id}");
        del.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var get = await _client.GetAsync($"/api/contacts/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
