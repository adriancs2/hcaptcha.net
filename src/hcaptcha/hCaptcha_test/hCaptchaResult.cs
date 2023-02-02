using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Web;
using System.Net.Http;

namespace System
{
    public class hCaptchaResult
    {
        public bool success { get; set; }
        public DateTime challenge_ts { get; set; }
        [JsonPropertyName("error-codes")]
        public List<string> error_codes { get; set; }
        public string jsonstr { get; set; }

        public hCaptchaResult()
        {

        }

        public hCaptchaResult(string secretKey)
        {
            // obtain the response token from user input
            // also called "response parameter" or "verification token"
            string hCaptcha_token = HttpContext.Current.Request.Form["h-captcha-response"];

            Initialize(secretKey, hCaptcha_token);
        }

        public hCaptchaResult(string secretKey, string hCaptcha_token)
        {
            Initialize(secretKey, hCaptcha_token);
        }

        public void Initialize(string secretKey)
        {
            string hCaptcha_token = HttpContext.Current.Request.Form["h-captcha-response"];
            Initialize(secretKey, hCaptcha_token);
        }

        public void Initialize(string secretKey, string hCaptcha_token)
        {
            // collecting data for post request
            var nv = new System.Collections.Specialized.NameValueCollection();
            nv["secret"] = "0x0000000000000000000000000000000000000000";
            nv["response"] = hCaptcha_token;

            // A tool that can performs a post request
            WebClient wc = new WebClient();

            // submit a post request to hcaptcha server
            // receiving bytes of data from hcaptcha server
            byte[] ba = wc.UploadValues("https://hcaptcha.com/siteverify", nv);

            // convert the bytes into string
            var jsonstr = System.Text.Encoding.UTF8.GetString(ba);

            // convert JSON string into Class
            var jsonRootElement = System.Text.Json.JsonDocument.Parse(jsonstr).RootElement;

            success = jsonRootElement.GetProperty("success").GetBoolean();

            JsonElement jeDate = new JsonElement();
            if (jsonRootElement.TryGetProperty("challenge_ts", out jeDate))
            {
                challenge_ts = jeDate.GetDateTime();
            }

            JsonElement jeErrors = new JsonElement();
            if (jsonRootElement.TryGetProperty("error-codes", out jeErrors))
            {
                error_codes = new List<string>();
                foreach (var je in jeErrors.EnumerateArray())
                {
                    error_codes.Add(je.GetString());
                }
            }
        }
    }
}