# Using hCaptcha in ASP.NET Web Forms

Blob Post: https://adriancs.com/c-sharp/927/using-hcaptcha-in-asp-net-web-forms/

First of all, register an account at hCaptcha, then obtain the following values:

- Site Key (you can create multiple site keys)
- Secret Key (developer’s key)

At the front end (HTML page), add a DIV for loading hCaptcha:
```html
<div class="h-captcha" data-sitekey="your site key"></div>
```
Then, import the javascript file:
```html
<script src="https://js.hcaptcha.com/1/api.js" async defer></script>
```
Example of a front end page:
```html
<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        Username: <input type="text" name="username" /> <br />
        Password: <input type="password" name="password" /> <br />
        <div class="h-captcha" data-sitekey="your site key"></div>
        <asp:Button ID="btLogin" runat="server" Text="Login" OnClick="btLogin_Click" />
    </form>
    <script src="https://js.hcaptcha.com/1/api.js" async defer></script>
</body>
</html>
```
To verify the result of hCaptcha challenge, submit 2 values to hCaptcha verification site:
```
https://hcaptcha.com/siteverify
```
Perform a POST request with the following collection of values:

- secret – your secret key
- response – the unique response token for each specific hCaptcha challenge

In C#, at code behind, the POST request can be carried out by using WebClient:

```c#
using System.Net;

protected async void btLogin_Click(object sender, EventArgs e)
{
    // obtain the response token from user input
    // also called "response parameter" or "verification token"
    string hCaptcha_token = Request.Form["h-captcha-response"];

    // collect data for post request
    Dictionary<string, string> dicData = new Dictionary<string, string>();
    dicData["secret"] = "your secret key";
    dicData["response"] = hCaptcha_token;

    // convert dictionary into form data
    FormUrlEncodedContent formData = new FormUrlEncodedContent(dicData);

    string url = "https://hcaptcha.com/siteverify";

    HttpClient hc = new HttpClient();

    // perform post request
    var res = await hc.PostAsync(url, formData);

    // download full request data, extract content, it's json
    var jsonstr = await res.Content.ReadAsStringAsync();
}
```
hCaptcha server will return a JSON string. Here’s a typical example:

```json
{
   "success": (boolean), 
   "challenge_ts": (DateTime),
   "hostname": (string),
   "error-codes": (List<string>)
}
```

Values Explained:

- "success" – indicates the challenge was a success (human) or failure (bots detected)
- "challenge_ts" – the time that the challenge was taken place
- "hostname" – the hostname of the site where the challenge was solved
- "error-codes" – the reasons why the challenge was a failure. If the challenge is a success, this value will be empty

Explanation of Error Codes:

- `missing-input-secret`: secret key is missing
- `invalid-input-secret`: secret key is invalid or malformed
- `missing-input-response`: The response parameter (verification token) is missing
- `invalid-input-response`: The response parameter (verification token) is invalid or malformed
- `bad-request`: The request is invalid or malformed
- `invalid-or-already-seen-response`: The response parameter has already been checked, or has another issue
- `not-using-dummy-passcode`: You have used a testing sitekey but have not used its matching secret
- `sitekey-secret-mismatch`: The sitekey is not registered with the provided secret

**Convert JSON into a Class Object.**

Install Nuget Package of System.Text.JSON.

Create a Class Object:

```c#
using System.Text.Json.Serialization;

public class hCaptchaResult
{
    public bool success { get; set; }
    public DateTime challenge_ts { get; set; }
    public string hostname { get; set; }
    [JsonPropertyName("error-codes")]
    public List<string> error_codes { get; set; }
}
```
Converts the JSON string into Class:

```c#
using System.Text.Json;

protected async void btLogin_Click(object sender, EventArgs e)
{
    // obtain the response token from user input
    // also called "response parameter" or "verification token"
    string hCaptcha_token = Request.Form["h-captcha-response"];

    // collect data for post request
    Dictionary<string, string> dicData = new Dictionary<string, string>();
    dicData["secret"] = "your secret key";
    dicData["response"] = hCaptcha_token;

    // convert dictionary into form data
    FormUrlEncodedContent formData = new FormUrlEncodedContent(dicData);

    string url = "https://hcaptcha.com/siteverify";

    HttpClient hc = new HttpClient();

    // perform post request
    var res = await hc.PostAsync(url, formData);

    // download full request data, extract content, it's json
    var jsonstr = await res.Content.ReadAsStringAsync();
    
    // convert JSON string into Class
    var hcaptcha = JsonSerializer.Deserialize<hCaptchaResult>(jsonstr);
    
    if (hcaptcha.success)
    {
        // success (human)
    }
    else
    {
        // fail (bots detected)
    }
}
```

hCaptcha is not allowed to run on localhost, but however, you can use the developer test key to run in local development environment:

- Site Key: `10000000-ffff-ffff-ffff-000000000001`
- Secret Key: `0x0000000000000000000000000000000000000000`
