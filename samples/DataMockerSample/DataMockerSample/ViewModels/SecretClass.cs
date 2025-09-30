namespace DefaultNamespace;

public class SecretClass
{
    private const string StripeApiKey = "sk_test_1234567890abcdefghijklmnop";

    public async Task<string> UseKeyAsync()
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + StripeApiKey);

        var response = await client.GetAsync("https://example.com/api/test-auth");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return content;
    }
    
    public string Hash(string input)
    {
        using var md5 = MD5.Create();
        var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}