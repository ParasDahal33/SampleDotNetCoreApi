namespace BusReservationSystemApi.Utils;

public static class Constants
{


    public static string GetConfirmEmailHtml(string username, String url)
    {
        return @$"<html lang=""en"" style=""font-family: sans-serif"">
  <head>
    <meta http-equiv=""Content-Type"" content=""text/html charset=UTF-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
  </head>
  <body>
    <table style=""margin: 0; width: 100%"">
      <tbody style=""width: 100%"">
        <tr style=""width: 100%"">
          <td style=""width: 100%"">
            <div
              style=""
                background-color: #f1e6e1A2;
                padding: 24px;
                border-radius: 15px;
                margin: auto;
                width: fit-content;
              ""
            >
              <img
                src=""https://portal.mbnepal.com/assets/mbnepal-portal.a14e6b06.png""
                style=""height: 80px; margin-bottom: 20px""
              />

              <div>Hi {username},</div>
              <p>
                Your email has been registered to MBNepal Portal. Please confirm
                your email by <a href=""{url}"">clicking here</a>.
              </p>

              <div style=""margin-top: 16px"">Regards,</div>
              <span>MicroBanker Nepal Portal</span>

              <p
                style=""
                  font-size: 12px;
                  text-align: center;
                  margin-top: 4em;
                  color: #0e598b;
                ""
              >
                <span style=""font-weight: bold"">
                  This is auto generated email please do not reply.
                </span>

                <span>
                  <br />
                  <br />
                  MicroBanker Nepal Pvt. Ltd. <br />
                  158-Nirmal Lama Marg,<br />
                  Nayabazar, Kathmandu, <br />Nepal Kathmandu – 44600<br />
                  (977)-01-4958647
                </span>
              </p>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </body>
</html>
";
    }


    public static string GetForgetPasswordHtml(string username, string url)
    {

        return @$"<html>
  <body style=""font-family: sans-serif"">
    <table style=""margin: 0; width: 100%"">
      <tbody>
        <tr style=""width: 100%"">
          <td>
            <div
              style=""
                background-color: #f1e6e1A2;
                padding: 24px;
                border-radius: 15px;
                margin: auto;
                width: 600px;
              ""
            >
              <img
                src=""https://portal.mbnepal.com/assets/mbnepal-portal.a14e6b06.png""
                style=""height: 60px; margin-bottom: 20px""
              />

              <div style=""font-size: 1.2em"">
                <p>Hello {username},</p>

                <p>
                  Seems like you forget your password for MicroBanker Nepal. If
                  this is true, Click below to reset your password.
                </p>

                <table style=""width: 100%; margin: 0; margin-top: 4em"">
                  <tbody style=""width: 100%"">
                    <tr style=""width: 100%"">
                      <td style=""width: 100%"">
                        <div style=""margin: 0 auto; width: 200px"">
                          <a
                            href=""{url}""
                            style=""
                              text-decoration: none;
                              font-weight: bold;
                              color: white;
                              background-color: #e58a1d;
                              padding: 1em 26px;
                              letter-spacing: 1px;
                              border-radius: 15px;
                            ""
                            >Reset Password</a
                          >
                        </div>
                      </td>
                    </tr>
                  </tbody>
                </table>

                <div>
                  <p
                    style=""
                      font-size: 12px;
                      text-align: center;
                      margin-top: 6em;
                      color: #0e598b;
                    ""
                  >
                    If you believe this email was sent to you in error, please
                    disregard and accept our apologies for any inconvenience
                    caused.
                  </p>

                  <p
                    style=""text-align: center; color: #0e598b; font-size: 12px""
                  >
                    MicroBanker Nepal Pvt. Ltd. <br />
                    158-Nirmal Lama Marg,<br />
                    Nayabazar, Kathmandu, <br />Nepal Kathmandu – 44600<br />
                    (977)-01-4958647
                  </p>
                </div>
              </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </body>
</html>
      ";

    }
}
