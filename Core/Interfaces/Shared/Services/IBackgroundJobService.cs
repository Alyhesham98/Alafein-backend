using System.Linq.Expressions;

namespace Core.Interfaces.Shared.Services
{
    public interface IBackgroundJobService
    {
        void SetRecurringJob(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression);
        void SetRecurringJob<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression);
        void TriggerRecurringJobNow(string recurringJobId);
        string Enqueue<T>(Expression<Func<T, Task>> job);
    }
}
