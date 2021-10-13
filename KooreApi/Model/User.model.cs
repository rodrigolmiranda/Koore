using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace KooreApi.Model
{
    public class AuthenticateResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_Token")]
        public string RefreshToken { get; set; }

        public AuthenticateResponse(string _accessToken, string _refreshToken)
        {
            AccessToken = _accessToken;
            RefreshToken = _refreshToken;
        }
    }
    public class UserSession
    {
        [JsonIgnore]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonIgnore]
        [JsonPropertyName("salt")]
        public string Salt { get; set; }
        [JsonIgnore]
        [JsonPropertyName("refresh_Token")]
        public string RefreshToken { get; set; }
        [JsonIgnore]
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }



    }
    public class User
    {
        [JsonIgnore]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonIgnore]
        [JsonPropertyName("salt")]
        public string Salt { get; set; }
        [JsonIgnore]
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }
        [JsonPropertyName("profile")]
        public Profile Profile { get; set; }

    }
    public class Profile
    {
        [JsonIgnore]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("firstName")]
        public string first_name { get; set; }
        [JsonPropertyName("lastName")]
        public string last_name { get; set; }
        [JsonIgnore]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }


    }
   
}
