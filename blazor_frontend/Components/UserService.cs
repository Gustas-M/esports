using Microsoft.JSInterop;
using System.Threading.Tasks;

public class UserService
{
    private readonly IJSRuntime _jsRuntime;

    private bool _isLoggedIn;

    public UserService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public bool IsLoggedIn()
    {
        return _isLoggedIn;
    }

    public async Task Login(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "access_token", token);
        _isLoggedIn = true;
    }

    public async Task Logout()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "access_token");
        _isLoggedIn = false;
    }
}
