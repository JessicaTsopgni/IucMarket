using IucMarket.Web.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace IucMarket.Web.Common
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;

        public TokenAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetTokenAsync(string token, int expiry = default)
        {
            if (token == null)
            {
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authToken");
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authTokenDate");
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authTokenExpiry");
            }
            else
            {
                await RefreshTokenAsync(token, expiry);
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task RefreshTokenAsync(string token, int expiry = default)
        {
            await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authToken", token);
            await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authTokenDate", DateTime.Now);
            await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authTokenExpiry", expiry);
        }

        public async Task<string> GetTokenAsync()
        {
            var tokenObj = await _jsRuntime.InvokeAsync<object>("localStorage.getItem", "authToken");
            var dateObj = await _jsRuntime.InvokeAsync<object>("localStorage.getItem", "authTokenDate");
            var expiryObj = await _jsRuntime.InvokeAsync<object>("localStorage.getItem", "authTokenExpiry");
            if (tokenObj != null && expiryObj != null && dateObj != null)
            {
                var date = DateTime.Parse(dateObj.ToString());
                var expiry = int.Parse(expiryObj.ToString());
                if (date.AddSeconds(expiry) > DateTime.Now)
                {
                    await RefreshTokenAsync(tokenObj.ToString(), expiry);
                    return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
                }
                else
                {
                    await SetTokenAsync(null);
                }
            }
            return null;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetTokenAsync();
            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromToken(token), "Server authentication");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private static IEnumerable<Claim> ParseClaimsFromToken(string token)
        {
            var tokenModel = JsonConvert.DeserializeObject<TokenModel>(token);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, tokenModel.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, tokenModel.Token));
            claims.Add(new Claim(ClaimTypes.Name, tokenModel.Name));
            claims.Add(new Claim(ClaimTypes.Expiration, tokenModel.ExpiresIn.ToString()));
            return claims;
            //var payload = token.Split('.')[1];
            //var jsonBytes = ParseBase64WithoutPadding(payload);
            //var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            //return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
