

namespace LoginSSOSAML.Controllers;

public class LoginController : Controller
{
    
    private readonly ILogger<LoginController> _logger;
    
    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }
    
    
    // Este método genera el XML de SAML y lo escribe en un archivo de texto
    /*private string GenerateSamlXml()
    {
        // Creamos una instancia del certificado (reemplaza "samlCertificate" con tu certificado real)
            string samlCertificate = @"-----BEGIN CERTIFICATE-----
MIIC4jCCAcoCCQC33wnybT5QZDANBgkqhkiG9w0BAQsFADAyMQswCQYDVQQGEwJV SzEPMA0GA1UECgwGQm94eUhRMRIwEAYDVQQDDAlNb2NrIFNBTUwwIBcNMjIwMjI4 MjE0NjM4WhgPMzAyMTA3MDEyMTQ2MzhaMDIxCzAJBgNVBAYTAlVLMQ8wDQYDVQQK DAZCb3h5SFExEjAQBgNVBAMMCU1vY2sgU0FNTDCCASIwDQYJKoZIhvcNAQEBBQAD ggEPADCCAQoCggEBALGfYettMsct1T6tVUwTudNJH5Pnb9GGnkXi9Zw/e6x45DD0 RuRONbFlJ2T4RjAE/uG+AjXxXQ8o2SZfb9+GgmCHuTJFNgHoZ1nFVXCmb/Hg8Hpd 4vOAGXndixaReOiq3EH5XvpMjMkJ3+8+9VYMzMZOjkgQtAqO36eAFFfNKX7dTj3V pwLkvz6/KFCq8OAwY+AUi4eZm5J57D31GzjHwfjH9WTeX0MyndmnNB1qV75qQR3b 2/W5sGHRv+9AarggJkF+ptUkXoLtVA51wcfYm6hILptpde5FQC8RWY1YrswBWAEZ NfyrR4JeSweElNHg4NVOs4TwGjOPwWGqzTfgTlECAwEAATANBgkqhkiG9w0BAQsF AAOCAQEAAYRlYflSXAWoZpFfwNiCQVE5d9zZ0DPzNdWhAybXcTyMf0z5mDf6FWBW 5Gyoi9u3EMEDnzLcJNkwJAAc39Apa4I2/tml+Jy29dk8bTyX6m93ngmCgdLh5Za4 khuU3AM3L63g7VexCuO7kwkjh/+LqdcIXsVGO6XDfu2QOs1Xpe9zIzLpwm/RNYeX UjbSj5ce/jekpAw7qyVVL4xOyh8AtUW1ek3wIw1MJvEgEPt0d16oshWJpoS1OT8L r/22SvYEo3EmSGdTVGgk3x3s+A0qWAqTcyjr7Q4s/GKYRFfomGwz0TZ4Iw1ZN99M m0eo2USlSRTVl7QHRTuiuSThHpLKQQ==
-----END CERTIFICATE-----";

            // Creamos la solicitud de autenticación
            var request = new AuthRequest(
                //"https://portal.sso.us-east-1.amazonaws.com/saml/assertion/MDUwNDk0MjU2NDA5X2lucy1kYjliMGYxOGE3ZTRmNmE2",
                "https://login.aufsite.com/app/amazon_aws/exkeyrjl32gZbVmUh697/sso/saml",
                "http://loginssosaml.us-east-1.elasticbeanstalk.com/"
            );

            // Convertimos el certificado a un objeto X509Certificate2
            X509Certificate2 cert = new X509Certificate2(Encoding.ASCII.GetBytes(EnsureCertFormat(samlCertificate)));

            // Firmamos la solicitud de autenticación con el certificado
            string samlRequest = request.GetRequest();

            // Guardamos el XML de la solicitud de autenticación en un archivo de texto
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SamlRequest.xml");
            System.IO.File.WriteAllText(filePath, samlRequest);

            return filePath;
    }*/
    private string EnsureCertFormat(string cert)
    {
        // Aseguramos el formato del certificado
        if (!cert.StartsWith("-----BEGIN CERTIFICATE-----"))
        {
            cert = "-----BEGIN CERTIFICATE-----\n" + cert;
        }
        if (!cert.EndsWith("-----END CERTIFICATE-----"))
        {
            cert += "\n-----END CERTIFICATE-----";
        }
        return cert;
    }
    
    
    
    //this example is an ASP.NET Core MVC action method
    public IActionResult Login()
    {
        
        /*// Generar el XML de SAML y obtener la ruta del archivo
        var samlXmlFilePath = GenerateSamlXml();

        // Devolver el archivo XML como una descarga al navegador del usuario
        return PhysicalFile(samlXmlFilePath, "text/xml", "SamlResponse.xml");*/
        
        
        
        //TODO: specify the SAML provider url here, aka "Endpoint"
        var samlEndpoint = "https://portal.sso.us-east-1.amazonaws.com/saml/assertion/MDUwNDk0MjU2NDA5X2lucy1kYjliMGYxOGE3ZTRmNmE2";

        var request = new AuthRequest(
            //TODO: put your app's "entity ID" here
            "https://portal.sso.us-east-1.amazonaws.com/saml/assertion/MDUwNDk0MjU2NDA5X2lucy1kYjliMGYxOGE3ZTRmNmE2",

            //TODO: put Assertion Consumer URL (where the provider should redirect users after authenticating)
            "http://loginssosaml.us-east-1.elasticbeanstalk.com/"
        );

        //now send the user to the SAML provider
        return Redirect(request.GetRedirectUrl(samlEndpoint));
    }

    [HttpPost]
    //ASP.NET Core MVC action method... But you can easily modify the code for old .NET Framework, Web-forms etc.
    public async Task<IActionResult> SamlConsume()
    {
        // 1. TODO: specify the certificate that your SAML provider gave you
        string samlCertificate = @"-----BEGIN CERTIFICATE-----
MIIC4jCCAcoCCQC33wnybT5QZDANBgkqhkiG9w0BAQsFADAyMQswCQYDVQQGEwJV SzEPMA0GA1UECgwGQm94eUhRMRIwEAYDVQQDDAlNb2NrIFNBTUwwIBcNMjIwMjI4 MjE0NjM4WhgPMzAyMTA3MDEyMTQ2MzhaMDIxCzAJBgNVBAYTAlVLMQ8wDQYDVQQK DAZCb3h5SFExEjAQBgNVBAMMCU1vY2sgU0FNTDCCASIwDQYJKoZIhvcNAQEBBQAD ggEPADCCAQoCggEBALGfYettMsct1T6tVUwTudNJH5Pnb9GGnkXi9Zw/e6x45DD0 RuRONbFlJ2T4RjAE/uG+AjXxXQ8o2SZfb9+GgmCHuTJFNgHoZ1nFVXCmb/Hg8Hpd 4vOAGXndixaReOiq3EH5XvpMjMkJ3+8+9VYMzMZOjkgQtAqO36eAFFfNKX7dTj3V pwLkvz6/KFCq8OAwY+AUi4eZm5J57D31GzjHwfjH9WTeX0MyndmnNB1qV75qQR3b 2/W5sGHRv+9AarggJkF+ptUkXoLtVA51wcfYm6hILptpde5FQC8RWY1YrswBWAEZ NfyrR4JeSweElNHg4NVOs4TwGjOPwWGqzTfgTlECAwEAATANBgkqhkiG9w0BAQsF AAOCAQEAAYRlYflSXAWoZpFfwNiCQVE5d9zZ0DPzNdWhAybXcTyMf0z5mDf6FWBW 5Gyoi9u3EMEDnzLcJNkwJAAc39Apa4I2/tml+Jy29dk8bTyX6m93ngmCgdLh5Za4 khuU3AM3L63g7VexCuO7kwkjh/+LqdcIXsVGO6XDfu2QOs1Xpe9zIzLpwm/RNYeX UjbSj5ce/jekpAw7qyVVL4xOyh8AtUW1ek3wIw1MJvEgEPt0d16oshWJpoS1OT8L r/22SvYEo3EmSGdTVGgk3x3s+A0qWAqTcyjr7Q4s/GKYRFfomGwz0TZ4Iw1ZN99M m0eo2USlSRTVl7QHRTuiuSThHpLKQQ==
-----END CERTIFICATE-----";

        // 2. Let's read the data - SAML providers usually POST it into the "SAMLResponse" var
        var samlResponseString = Request.Form["SAMLResponse"].ToString();
        var samlResponseBytes = Convert.FromBase64String(samlResponseString);
        var samlResponse = new Response(samlResponseBytes, samlCertificate);

        // 3. DONE!
        if (samlResponse.IsValid())
        {
            //WOOHOO!!! user is logged in

            //Some more optional stuff
            //let's extract username/firstname etc
            try
            {
                var username = samlResponse.GetNameID();
                //  var email = samlResponse.GetEmail();
                //  var firstname = samlResponse.GetFirstName();
                //  var lastname = samlResponse.GetLastName();

                //or read some custom-named data that you know the IdP sends
                //var officeLocation = samlResponse.GetCustomAttribute("OfficeAddress");

                //Create an authentication cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, username)
                };

                var identity = new ClaimsIdentity(claims, "SAML");
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user
                await HttpContext.SignInAsync(principal);

                // Redirect to the home page
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                //insert error handling code
                //in case some extra attributes are not present in XML, for example
                // return Task.FromResult<IActionResult>(null!);

                // Handle any errors that occur during the login process
                _logger.LogError(ex, "An error occurred while processing the SAML response.");
                return RedirectToAction("Error", "Home");
            }
        }
        
        return await Task.FromResult<IActionResult>(Content("Unauthorized"));
    }
}