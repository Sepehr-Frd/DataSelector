using AmazonMockup.Model.Models;
using RestSharp;

namespace AmazonMockup.ExternalService;

public class RedditMockupService
{
    // TODO: Get base address
    private readonly string _baseAddress = "localhost";

    public async Task<List<User>?> GetPeopleAsync(CancellationToken cancellationToken = default)
    {
        var restClient = new RestClient();

        var loginRequest = new RestRequest($"{_baseAddress}/Login");

        var user = new
        {
            Username = "sepehr_frd",
            Password = "sfr1376",
            RememberMe = true
        };

        loginRequest.AddJsonBody(user);

        await restClient.ExecutePostAsync(loginRequest, cancellationToken);

        var getUsersRequest = new RestRequest($"{_baseAddress}/User");

        var people = await restClient.ExecuteGetAsync<List<User>>(getUsersRequest, cancellationToken);

        return people.Data;
    }
}