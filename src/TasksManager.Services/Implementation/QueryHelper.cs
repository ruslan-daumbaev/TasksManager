using System;
using System.Linq;
using TasksManager.Services.BusinessObjects;

namespace TasksManager.Services.Implementation
{
    internal static class QueryHelper
    {
        private const string NameSortField = "name";
        private const string PrioritySortField = "priority";

        public static IQueryable<Data.Entities.Task> ApplyFilters(IQueryable<Data.Entities.Task> query, string status)
        {
            if (string.IsNullOrWhiteSpace(status) || !Enum.TryParse(status, out TaskStatus parsedStatus) || parsedStatus == TaskStatus.Deleted)
            {
                return query.Where(t => t.Status != (int)TaskStatus.Deleted);
            }

            return query.Where(r => r.Status == (int)parsedStatus);
        }


        public static IQueryable<Data.Entities.Task> ApplySorting(IQueryable<Data.Entities.Task> query, string sortField, bool asc)
        {
            switch (sortField)
            {
                case NameSortField:
                    return asc ? query.OrderBy(r => r.Name) : query.OrderByDescending(r => r.Name);
                case PrioritySortField:
                    return asc ? query.OrderBy(r => r.Priority) : query.OrderByDescending(r => r.Priority);
                default:
                    return query.OrderBy(r => r.TimeToComplete);
            }
        }
    }
}