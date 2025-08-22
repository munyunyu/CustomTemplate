using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Business.Interfaces.System;
using Template.Library.Constants;
using Template.Library.Enums;
using Template.Library.Extensions;
using Template.Library.Tables.Notification;

namespace Template.Business.Services.System
{
    public class CommunicationService : ICommunicationService
    {
        private readonly ILogger<CommunicationService> logger;
        private readonly IDatabaseService databaseService;

        public CommunicationService(ILogger<CommunicationService> logger, IDatabaseService databaseService)
        {
            this.logger = logger;
            this.databaseService = databaseService;
        }
        public async Task SendConfirmEmailAsync(string to, string template_name, string token, string config_name = "")
        {
            try
            {
                if(string.IsNullOrEmpty(config_name)) config_name = EmailConfig.Default;

                var template = await databaseService.GetAsync<TblEmailTemplate>(x => x.Name == template_name);

                if (template == null)
                {
                    logger.LogWarning("Failed to get email template: {template_name}", template_name);

                    return;
                }

                var configs = await databaseService.GetAsync<TblEmailConfig>(x => x.Name == config_name);

                if (configs == null)
                {
                    logger.LogWarning("Failed to get email configuration");

                    return;
                }

                var table = new TblEmailQueue
                {
                    Id = Guid.NewGuid(),
                    Subject = template.Subject,
                    Body = template.Body?.Replace("[ConfirmationLink]", token.Base64Encode()),
                    FromEmailAddress = configs.SmtpUser,
                    CCEmailAddresses = string.Empty,
                    ToEmailAddresses = to,
                    SendAttempts = 0,
                    Status =  Status.Pending,
                    Priority = Priority.High,

                    CreatedById = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedById = Guid.NewGuid(),

                };

                await databaseService.AddAsync(table);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending conffirmation email to: {email}", to);
            }
        }
    }
}
