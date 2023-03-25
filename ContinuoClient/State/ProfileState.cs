// #pragma warning disable CS8618
// using Microsoft.AspNetCore.Components.Authorization;
// using System.Security.Claims;
// using Blazored.LocalStorage;
// using System.Net.Http;
// using BlazorPracticeApp.Services;
// using BlazorPracticeApp.Models;

// namespace BlazorPracticeApp.State;
// public class ProfileState : AuthenticationStateProvider
// {
//     public ILocalStorageService _localStorageService { get; }
//     public IUserService _userService { get; set; }
//     private readonly HttpClient _httpClient;

//     public ProfileState(ILocalStorageService localStorageService, IUserService userService, HttpClient httpClient)
//     {
//         _localStorageService = localStorageService;
//         _userService = userService;
//         _httpClient = httpClient;
//     }

//     // public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//     // {
//     //     var token = await _localStorageService.GetItemAsync<string>("token");
//     //     ClaimsIdentity identity;

//     //     if (!string.IsNullOrEmpty(token))
//     //     {
//     //         SecureUser getUser = await _userService.GetUserByTokenAsync(token);
//     //         identity = GetClaimsIdentity(getUser);
//     //     }
//     //     else
//     //     {
//     //         identity = new ClaimsIdentity();
//     //     }
//     //     ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
//     //     return await Task.FromResult(new AuthenticationState(claimsPrincipal));
//     // }

//     private ClaimsIdentity GetClaimsIdentity(SecureUser user)
//         {
//             var claimsIdentity = new ClaimsIdentity();

//             if (user.Email != null)
//             { 
//                 claimsIdentity = new ClaimsIdentity(new[]
//                                 {
//                                     new Claim(ClaimTypes.Name, user.Email)                                   
//                                 }, "apiauth_type");
//             }

//             return claimsIdentity;
//         }

//     public async Task MarkUserAsAuthenticated(SecureUser user)
//     {
//         await _localStorageService.SetItemAsync("token", user.Token);
//         await _localStorageService.SetItemAsync("refreshToken", user.RefreshToken);
//         ClaimsIdentity identity = GetClaimsIdentity(user);
//         ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
//         NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
//     }

// }