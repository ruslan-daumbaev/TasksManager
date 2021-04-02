using System;
using System.Linq;
using TasksManager.Data.Entities;
using TasksManager.Services.BusinessObjects;

namespace TasksManager.Services.Implementation
{
    internal static class QueryHelper
    {
        private const string NameSortField = "name";
        private const string PrioritySortField = "priority";

        public static IQueryable<TodoTask> FilterByStatus(this IQueryable<TodoTask> query, string status)
        {
            if (string.IsNullOrWhiteSpace(status) || !Enum.TryParse(status, out TaskStatus parsedStatus) || parsedStatus == TaskStatus.Deleted)
            {
                return query.Where(t => t.Status != (int)TaskStatus.Deleted);
            }

            return query.Where(r => r.Status == (int)parsedStatus);
        }


        public static IQueryable<TodoTask> OrderByField(this IQueryable<TodoTask> query, string sortField, bool asc)
        {
            return sortField switch
            {
                NameSortField => asc ? query.OrderBy(r => r.Name) : query.OrderByDescending(r => r.Name),
                PrioritySortField => asc ? query.OrderBy(r => r.Priority) : query.OrderByDescending(r => r.Priority),
                _ => query.OrderBy(r => r.TimeToComplete),
            };
        }
    }
}