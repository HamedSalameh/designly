﻿using Bogus;
using Clients.Domain.Entities;
using Clients.Infrastructure;
using Clients.Infrastructure.Interfaces;
using RestSharp;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("generated_data.csv")
            .CreateLogger();


var _modelFaker = new Faker<Client>()
    .CustomInstantiator(f => new Client(f.Person.FirstName, f.Person.LastName,
    new Clients.Domain.ValueObjects.Address(
        f.Address.City(), 
        f.Address.StreetName(),
        f.Random.Bool() ? f.Address.BuildingNumber() : string.Empty),
    new Clients.Domain.ValueObjects.ContactDetails(
        f.Phone.PhoneNumber("(###) ###-####"),
        f.Random.Bool() ? f.Phone.PhoneNumber("(###) ###-####") : string.Empty,
        f.Internet.Email()),
    Guid.NewGuid()))
    .RuleFor(c => c.Id, f => Guid.NewGuid())
    //.RuleFor(c => c.FirstName, f => f.Name.FirstName())
    //.RuleFor(c => c.FamilyName, f => f.Name.LastName())
    //.RuleFor(c => c.Address, f => new Domain.ValueObjects.Address(f.Address.City(), f.Address.StreetName(), f.Address.BuildingNumber()))
    //.RuleFor(c => c.ContactDetails, f => new Domain.ValueObjects.ContactDetails(f.Phone.PhoneNumber("(###) ###-####"), f.Phone.PhoneNumber("(###) ###-####"), f.Internet.Email()))
    .FinishWith((f, u) =>
    {
        Console.WriteLine($"Generated a user: {u}");
    });


Console.WriteLine("How many user do you want to generate ?");
string? countIput = Console.ReadLine();
int.TryParse(countIput, out int count);

var generatedClients = new List<Client>();
string? userInput = "";

do {
    Console.WriteLine(Environment.NewLine);
    Console.WriteLine($"{count} will be generated. Proceed? [Y/n]");
    userInput = Console.ReadLine();

    if (string.Equals(userInput?.Trim(), "y", StringComparison.OrdinalIgnoreCase))
    {
        generatedClients = _modelFaker.Generate(count);
    }
    else if (string.Equals(userInput?.Trim(), "n", StringComparison.OrdinalIgnoreCase))
    {
        return;
    }
    else
    {
        Console.WriteLine("Invalid input, please type Y or N");
    }
}
while (userInput is null || !userInput.Trim().ToLower().Equals("y", StringComparison.OrdinalIgnoreCase) && !userInput.Trim().ToLower().Equals("n", StringComparison.OrdinalIgnoreCase));

Console.WriteLine(Environment.NewLine);
Console.WriteLine("Do you want to save the generated users? [Y/N]");
do
{
    userInput = Console.ReadLine();

    if (string.Equals(userInput?.Trim(), "y", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Sending all generated user to persistance");

        // use httpclient to send the generated users to the API
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7246");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/json"));
        foreach(var generatedUser in generatedClients)
        {
            try
            {
                var res = client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/api/v1/clients")
                {
                    Content = new StringContent(JsonSerializer.Serialize(generatedUser), Encoding.UTF8, "application/json")
                }).ContinueWith( _ =>
                {
                    Console.WriteLine($"User {generatedUser} sent to persistance");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending user {generatedUser} to persistance : {ex.Message}");
                throw;
            }
        }
    }
    else if (string.Equals(userInput?.Trim(), "n", StringComparison.OrdinalIgnoreCase))
    {
        return;
    }
    else
    {
        Console.WriteLine("Invalid input, please type Y or N");
    }

}
while (userInput is null || !userInput.Trim().ToLower().Equals("y", StringComparison.OrdinalIgnoreCase) && !userInput.Trim().ToLower().Equals("n", StringComparison.OrdinalIgnoreCase));


