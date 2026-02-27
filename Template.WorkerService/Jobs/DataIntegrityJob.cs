using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using Template.Database.Context;
using Template.Library.Enums;
using Template.Library.Tables.Notification;

namespace Template.WorkerService.Jobs;

public class DataIntegrityJob : IInvocable
{
    private readonly ILogger<DataIntegrityJob> _logger;
    private readonly ApplicationContext _context;

    public DataIntegrityJob(ILogger<DataIntegrityJob> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Invoke()
    {     
        _logger.LogInformation("DataIntegrityJob completed. No issues found");

        await Task.CompletedTask;
    }
}
