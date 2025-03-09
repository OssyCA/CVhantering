using CVhantering.Dtos;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace CVhantering.Services
{
    public class GithubService
    {
        private HttpClient client;

        public GithubService(HttpClient _client)
        {
            client = _client;
        }
        public async Task<List<GithubDto>> GetGithubData(string user)
        {
            client.DefaultRequestHeaders.Add("User-Agent", "CvHantering");

            var response = await client.GetAsync($"https://api.github.com/users/{user}/repos");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<List<GithubDto>>(json, option);

            foreach (var item in data) // going through list of item
            {
                item.Language ??= "Unknown";
                item.Description ??= "No description";
            }
            return data;

        }
    }
}
