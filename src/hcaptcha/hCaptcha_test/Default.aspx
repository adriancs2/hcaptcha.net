<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="hCaptcha_test.Default" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
</head>
<body>

    <h1>Using hCaptcha in ASP.NET</h1>

    <form id="form1" runat="server">
    
        <pre>
Test Key Set: Publisher Account
======================================
Site Key    : 10000000-ffff-ffff-ffff-000000000001
Secret Key  : 0x0000000000000000000000000000000000000000

        </pre>

        Replace "data-sitekey" with your sitekey:
        <div class="h-captcha" data-sitekey="10000000-ffff-ffff-ffff-000000000001"></div>

        <asp:Button ID="btSubmit" runat="server" Text="Submit (Without Converting the Returned JSON into Class)" OnClick="btSubmit_Click" />
        <asp:Button ID="btSubmitClass" runat="server" Text="Submit (Convert Returned JSON into Class)" OnClick="btSubmitClass_Click" />

        <br />
        <br />

        <pre><asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder></pre>

        <script src="https://js.hcaptcha.com/1/api.js" async defer></script>

    </form>

</body>
</html>
