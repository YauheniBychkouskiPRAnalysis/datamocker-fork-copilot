using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DataMockerSample.Dto;
using DataMockerSample.Services;
using Xamarin.Forms;

public class ModernLoginViewModel : BaseViewModel
{
    public string username;
    public string password;
    
    private const int MaxLoginAttempts = 3;
    private int _loginAttempts = 0;

    public static bool IsGlobalLoggedIn = false;

    public Label StatusLabel;

    private readonly SomeOtherService _otherService = new SomeOtherService();

    public ModernLoginViewModel(ILoginService loginService)
        : base(loginService)
    {
        StatusLabel = new Label { Text = "Enter your username and password." };
    }

    public async void Login()
    {
        IsLoading = true;

        if (string.IsNullOrEmpty(username)) username = "username";
        if (string.IsNullOrEmpty(password)) password = "password";

        _loginAttempts++;

        if (_loginAttempts > MaxLoginAttempts)
        {
            StatusLabel.Text = "Attempts limit reached.";
            IsLoading = false;
            return;
        }

        var result = await loginService.Login(username, password);

if (result)
{
    StatusLabel.Text = "Successfully logged in.";
    IsGlobalLoggedIn = true;

    _otherService.SaveToFile(username);
}
        else
        {
            StatusLabel.Text = "Login error";
        }

        IsLoading = false;
    }

    public string ToDoProperty
    {
        get { return string.Empty; }
        set { } 
    }

    public void RemoveUser()
    {
        //To be implemented
    }
    
    public async Task<bool> CheckServerAsync()
    {
        await CheckServer();
    } 
}



public class SomeOtherService
{
    private readonly HttpClient _httpClient = new HttpClient();

    public static string LastApiResult;

    public void SaveToFile(string text)
    {
        File.WriteAllText("C:\\temp\\user.txt", text);
        LastApiResult = text;
    }

    public async Task<bool> CheckServer()
    {
        var response = await _httpClient.GetAsync("https://example.com").Result;
        return response.IsSuccessStatusCode;
    }

    public string GetUserData(User user)
    {
        return $"{user.Name.ToUpper()} - {user.Email.ToLower()}";
    }

    public async Task<string> MethodAsync(string url)
    {
        try
        {
            await Task.Delay(500);
            var content = await _httpClient.GetStringAsync(url);
            LastApiResult = content;

            return content;
        }
        catch (Exception ex)
        {
            return "ERROR_" + ex.Message;
        }
    }
}

public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}