using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Security.Policy;

namespace hCaptcha_test
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btSubmitLazy_Click(object sender, EventArgs e)
        {
            // obtain the response token from user input
            // also called "response parameter" or "verification token"
            string hCaptcha_token = Request.Form["h-captcha-response"];

            // collecting data for post request
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            dicData["secret"] = "0x0000000000000000000000000000000000000000";
            dicData["response"] = hCaptcha_token;

            // convert dictionary into form data
            FormUrlEncodedContent formData = new FormUrlEncodedContent(dicData);

            HttpClient hc = new HttpClient();

            // perform post request
            var res = hc.PostAsync("https://hcaptcha.com/siteverify", formData);

            // download full request data
            var result = res.Result.Content.ReadAsStringAsync();

            // extract the content, it's json
            var jsonstr = result.Result;

            StringBuilder sb = new StringBuilder();

            var ja = jsonstr.Split(',');

            if (ja[0].Contains("true"))
            {
                // human
                sb.AppendLine("Result: Success");
                sb.AppendLine();
            }
            else
            {
                // bots
                sb.AppendLine("Result: Failed");
                sb.AppendLine();
            }

            sb.AppendLine();

            sb.AppendLine("JSON returned from hCaptcha verification site:");
            sb.AppendLine(jsonstr);

            // display JSON on UI page
            ph1.Controls.Add(new LiteralControl(sb.ToString()));
        }

        protected void btSubmit_Click(object sender, EventArgs e)
        {
            // obtain the response token from user input
            // also called "response parameter" or "verification token"
            string hCaptcha_token = Request.Form["h-captcha-response"];

            // collecting data for post request
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            dicData["secret"] = "0x0000000000000000000000000000000000000000";
            dicData["response"] = hCaptcha_token;

            // convert dictionary into form data
            FormUrlEncodedContent formData = new FormUrlEncodedContent(dicData);

            HttpClient hc = new HttpClient();

            // perform post request
            var res = hc.PostAsync("https://hcaptcha.com/siteverify", formData);

            // download full request data
            var result = res.Result.Content.ReadAsStringAsync();

            // extract the content, it's json
            var jsonstr = result.Result;

            // convert JSON string into Json Element
            var jsonRootElement = JsonDocument.Parse(jsonstr).RootElement;

            StringBuilder sb = new StringBuilder();

            if (jsonRootElement.GetProperty("success").GetBoolean())
            {
                // human
                sb.AppendLine("Result: Success");
                sb.AppendLine();
            }
            else
            {
                // bots
                sb.AppendLine("Result: Failed");
                sb.AppendLine();
            }

            JsonElement jeDate = new JsonElement();
            if (jsonRootElement.TryGetProperty("challenge_ts", out jeDate))
            {
                sb.AppendLine($"Date Time: {jeDate.GetDateTime()}");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine($"Date Time: ");
                sb.AppendLine();
            }

            JsonElement jeErrors = new JsonElement();
            if (jsonRootElement.TryGetProperty("error-codes", out jeErrors))
            {
                sb.AppendLine("Error Codes:");

                foreach (var je in jeErrors.EnumerateArray())
                {
                    sb.AppendLine(je.GetString());
                }
            }

            sb.AppendLine();

            sb.AppendLine("JSON returned from hCaptcha verification site:");
            sb.AppendLine(jsonstr);

            // display JSON on UI page
            ph1.Controls.Add(new LiteralControl(sb.ToString()));
        }

        protected void btSubmitClass_Click(object sender, EventArgs e)
        {
            // replace your secret key here:
            string secretKey = "0x0000000000000000000000000000000000000000";

            // create the class
            hCaptchaResult hr = new hCaptchaResult(secretKey);

            StringBuilder sb = new StringBuilder();

            if (hr.success)
            {
                // human
                sb.AppendLine("Result: Success");
                sb.AppendLine();
            }
            else
            {
                // bots
                sb.AppendLine("Result: Failed");
                sb.AppendLine();
            }

            sb.AppendLine($"Date Time: {hr.challenge_ts}");
            sb.AppendLine();

            if (hr.error_codes != null && hr.error_codes.Count > 0)
            {
                sb.AppendLine("Error Codes:");

                foreach (var er in hr.error_codes)
                {
                    sb.AppendLine(er);
                }
            }

            sb.AppendLine();

            sb.AppendLine("JSON returned from hCaptcha verification site:");
            sb.AppendLine(hr.jsonstr);

            // display JSON on UI page
            ph1.Controls.Add(new LiteralControl(sb.ToString()));
        }

        protected void btSubmitClass2_Click(object sender, EventArgs e)
        {
            // obtain the response token from user input
            // also called "response parameter" or "verification token"
            string hCaptcha_token = Request.Form["h-captcha-response"];

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
            var hr = JsonSerializer.Deserialize<hCaptchaResult>(jsonstr);

            StringBuilder sb = new StringBuilder();

            if (hr.success)
            {
                // human
                sb.AppendLine("Result: Success");
                sb.AppendLine();
            }
            else
            {
                // bots
                sb.AppendLine("Result: Failed");
                sb.AppendLine();
            }


            sb.AppendLine($"Date Time: {hr.challenge_ts}");
            sb.AppendLine();

            if (hr.error_codes != null && hr.error_codes.Count > 0)
            {
                sb.AppendLine("Error Codes:");

                foreach (var er in hr.error_codes)
                {
                    sb.AppendLine(er);
                }
            }

            sb.AppendLine();

            sb.AppendLine("JSON returned from hCaptcha verification site:");
            sb.AppendLine(hr.jsonstr);

            // display JSON on UI page
            ph1.Controls.Add(new LiteralControl(sb.ToString()));
        }
    }
}