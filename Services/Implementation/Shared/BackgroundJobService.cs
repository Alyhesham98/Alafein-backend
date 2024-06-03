using Core.Interfaces.Shared.Services;
using Hangfire;
using System.Linq.Expressions;

namespace Services.Implementation.Shared
{
    internal sealed class BackgroundJobService : IBackgroundJobService
    {
        public string Enqueue<T>(Expression<Func<T, Task>> job)
        {
            return BackgroundJob.Enqueue(job);
        }

        public bool Requeue(string jobId)
        {
            return BackgroundJob.Requeue(jobId);
        }

        public void SetRecurringJob(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression)
        {
            RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression);
        }

        public void SetRecurringJob<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression)
        {
            RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression);
        }

        public void TriggerRecurringJobNow(string recurringJobId)
        {
            RecurringJob.TriggerJob(recurringJobId);
        }
    }
}
