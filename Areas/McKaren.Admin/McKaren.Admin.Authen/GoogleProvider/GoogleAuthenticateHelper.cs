using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace McKaren.Admin.Authen
{
    public class GoogleAuthenticateHelper
    {
        public const string PORTAL =
        "https://accounts.google.com/o/oauth2/auth?client_id={0}&redirect_uri={1}&scope={2}&response_type=code&access_type=offline";

        public const string API_TOKEN = "https://accounts.google.com/o/oauth2/token";
        public const string API_BASIC_INFO = "https://www.googleapis.com/plus/v1/people/me?access_token={0}";
        public const string SCOPE_BASIC =
            "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email";
        public const string KEY_GOOGLE_CLIENT_ID = "GoogleClientId";
        public const string KEY_GOOGLE_CLIENT_SECRET = "GoogleClientSecret";
        private string _client_id;
        private string _client_secret;

        public GoogleAuthenticateHelper(string clientId, string clientSecret)
        {
            _client_id = clientId;
            _client_secret = clientSecret;
        }
        public static GoogleId GetDefaultGoogleId() {
            return new GoogleId
            {
                ClientId = ConfigurationManager.AppSettings[KEY_GOOGLE_CLIENT_ID],
                ClientSecret = ConfigurationManager.AppSettings[KEY_GOOGLE_CLIENT_SECRET]
            }; 
        }

        public string GetAuthenUrl(string redirectUri)
        {
            var uri =
                string.Format(
                    PORTAL,
                    _client_id,
                    redirectUri,
                    SCOPE_BASIC);
            return uri;
        }

        public GoogleAuthenResult Resolve(GoogleTokenCallbackQuery request, string redirectUri)
        {
            if (request.FullUrl.Contains("#error=access_denied"))
            {
                return null;
            }
            else
            {
                //Check authenticated by using response code
                var code = request.Code;
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["code"] = code;
                    values["client_id"] = _client_id;
                    values["client_secret"] = _client_secret;
                    values["redirect_uri"] = redirectUri;
                    values["grant_type"] = "authorization_code";
                    try
                    {
                        var response = client.UploadValues(API_TOKEN, values);
                        var responseString = Encoding.UTF8.GetString(response);
                        var re = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleAccessTokenResponse>(responseString);
                        if (!string.IsNullOrEmpty(re.access_token))
                        {
                            var apiInfo = string.Format(API_BASIC_INFO, re.access_token);
                            response = client.DownloadData(apiInfo);
                            responseString = Encoding.UTF8.GetString(response);
                            var uInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleApiUserInfoResponse>(responseString);
                            var uEmail = uInfo.emails[0].value;
                            var fullName = uInfo.displayName;
                            return new GoogleAuthenResult
                            {
                                Email = uEmail,
                                FullName = fullName
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
                return null;
            }
        }
        public class GoogleId
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
        }
        private class GoogleAccessTokenResponse
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string id_token { get; set; }
            public string refresh_token { get; set; }
            public string token_type { get; set; }
        }
        private class GoogleApiUserInfoResponse
        {
            public List<GoogleApiUserEmail> emails { get; set; }
            public string displayName { get; set; }
            public class GoogleApiUserEmail
            {
                public string value { get; set; }
                public string type { get; set; }
            }
        }
        public class GoogleAuthenResult
        {
            public string Email { get; set; }
            public string FullName { get; set; }
        }
        public class GoogleTokenCallbackQuery
        {
            public string FullUrl { get; set; }
            public string Code { get; set; }
        }
    }
}
