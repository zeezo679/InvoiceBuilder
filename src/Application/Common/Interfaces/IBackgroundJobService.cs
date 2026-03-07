using System.Linq.Expressions;

namespace Application.Common.Interfaces;

public interface IBackgroundJobService
{
    string Enqueue(Expression<Action> job);
}