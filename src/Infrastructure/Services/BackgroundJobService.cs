using System.Linq.Expressions;
using Application.Common.Interfaces;
using Hangfire;

namespace Infrastructure.Services;


//Adapter Design Pattern :>
//this pattern is very useful in clean architecture
public class BackgroundJobService : IBackgroundJobService
{
    public string Enqueue(Expression<Action> job)
    {
        return BackgroundJob.Enqueue(job);
    }
}