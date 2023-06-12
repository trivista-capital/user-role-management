using System.Diagnostics;
using ElasticEmail.Api;
using ElasticEmail.Client;
using ElasticEmail.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Common.Options;
using Trivister.ApplicationServices.Dto;

namespace Trivister.Infrastructure.MailService;

public sealed class MailManager: IMailManager
{
    private static MailOptions _mailOptions;
    private readonly HttpClient _client;
    private readonly ILogger<MailManager> _logger;

    public MailManager(IOptions<MailOptions> mailOptions, HttpClient client, ILogger<MailManager> logger)
    {
        _client = client;
        _logger = logger;
        _mailOptions = mailOptions.Value;
    }
    
    public void BuildOTPMessage(string otp, string email)
    {
        var builder = new MailBuilder();
        var mailObject = builder.WithToEmail(email)
            .WithFromEmail(_mailOptions.From)
            .WithOTPSubject(_mailOptions.OTPMailSubject)
            .WithOTPMessage(_mailOptions.OTPMailTemplate, otp, email)
            .BuildOtpMailDto(); 
        this.SendEmail(mailObject);
    }

    // private async Task SendMail(MailObject dto)
    // {
    //     dto.ApiKey = _mailOptions?.APIKey;
    //     try
    //     {
    //         var body = JsonConvert.SerializeObject(dto);
    //         HttpContent content = new StringContent(body, Encoding.Default, "application/json");
    //         //_client.DefaultRequestHeaders.Add("AUTHORIZATIONS", _mailOptions?.APIKey);
    //         var postUrl = $"v2{_mailOptions.MailSubPath}?apikey={_mailOptions.APIKey}&subject={dto.Subject}&from={dto.From}&fromName=&sender={dto.From}&senderName=&msgFrom={dto.BodyAmp}&msgFromName=&replyTo=&replyToName=&to=&msgTo={dto.MsgTo}&msgCC=&msgBcc=&lists=&segments=&mergeSourceFilename=&dataSource=&channel=&bodyHtml=&bodyText=&charset={dto.CharSet}&charsetBodyHtml=&charsetBodyText=&encodingType=ApiTypes.EncodingType.None&template=&headers_firstname=firstname: myValueHere&postBack=&merge_firstname=John&timeOffSetMinutes=&poolName=My Custom Pool&isTransactional=true&attachments=&trackOpens=true&trackClicks=true&utmSource=source1&utmMedium=medium1&utmCampaign=campaign1&utmContent=content1&bodyAmp=&charsetBodyAmp=";
    //         var response = await _client.PostAsync(postUrl, content);
    //         if (response.IsSuccessStatusCode)
    //         {
    //             var stringResponse = await response.Content.ReadAsStringAsync();
    //             //var serializedResponse = JsonConvert.DeserializeObject<string>(stringResponse);
    //         }
    //         await Task.CompletedTask;
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex);
    //         throw;
    //     } 
    // }

    private void SendEmail(MailObject dto)
    { 
        Configuration config = new Configuration();
        // Configure API key authorization: apikey
        config.ApiKey.Add("X-ElasticEmail-ApiKey", _mailOptions.APIKey);
        var apiInstance = new EmailsApi(config);
        var to = new List<string> { dto.To };
        var recipients = new TransactionalRecipient(to: to);
        EmailTransactionalMessageData emailData = new EmailTransactionalMessageData(recipients: recipients)
        {
             Content = new EmailContent
             {
                 Body = new List<BodyPart>()
             }
        };
        BodyPart htmlBodyPart = new BodyPart
        {
            ContentType = BodyContentType.HTML,
            Charset = "utf-8",
            Content = "<h1>Mail content</h1>"
        };
        BodyPart plainTextBodyPart = new BodyPart
        {
            ContentType = BodyContentType.PlainText,
            Charset = "utf-8",
            Content = dto.BodyAmp
        };
        emailData.Content.Body.Add(htmlBodyPart);
        emailData.Content.Body.Add(plainTextBodyPart);
        emailData.Content.From = dto.From;
        emailData.Content.Subject = dto.Subject;

        try
        {
            // Send Bulk Emails
            var result = apiInstance.EmailsTransactionalPost(emailData);
            Debug.WriteLine(result);
        }
        catch (ApiException  e)
        {
            Debug.Print("Exception when calling EmailsApi.EmailsPost: " + e.Message);
            Debug.Print("Status Code: " + e.ErrorCode);
            Debug.Print(e.StackTrace);
        }
    }
}