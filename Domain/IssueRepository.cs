using System;
using System.Collections.Generic;
using System.Linq;
using PROG7312_POE_Part1.Domain;

namespace PROG7312_POE_Part1.Domain
{
    /// <summary>
    /// In-memory storage backed by LinkedList<Issue>.
    /// LinkedList is a valid “appropriate data structure” choice for the brief.
    /// </summary>
    public static class IssueRepository
    {
        private static readonly LinkedList<Issue> _issues = new LinkedList<Issue>();

        /// <summary>
        /// Add a new issue to the end of the linked list.
        /// </summary>
        public static void Add(Issue issue)
        {
            _issues.AddLast(issue);
        }

        /// <summary>
        /// Enumerate all issues (read-only).
        /// </summary>
        public static IEnumerable<Issue> GetAll()
        {
            // LinkedList implements IEnumerable, so we just expose it
            foreach (var issue in _issues)
                yield return issue;
        }

        /// <summary>
        /// Find a single issue by Id (linear search).
        /// </summary>
        public static Issue FindById(Guid id)
        {
            var node = _issues.First;
            while (node != null)
            {
                if (node.Value.Id == id)
                    return node.Value;
                node = node.Next;
            }
            return null;
        }

        /// <summary>
        /// Update an issue's status by Id. Returns true if found and updated.
        /// </summary>
        public static bool UpdateStatus(Guid id, IssueStatus newStatus)
        {
            var issue = FindById(id);
            if (issue == null) return false;
            issue.Status = newStatus;
            return true;
        }

        /// <summary>
        /// Remove an issue by Id. Returns true if a node was removed.
        /// </summary>
        public static bool Remove(Guid id)
        {
            var node = _issues.First;
            while (node != null)
            {
                if (node.Value.Id == id)
                {
                    _issues.Remove(node);
                    return true;
                }
                node = node.Next;
            }
            return false;
        }

        /// <summary>
        /// (Optional) Get the most recent N issues efficiently by walking from the tail.
        /// </summary>
        public static IEnumerable<Issue> GetMostRecent(int count)
        {
            var node = _issues.Last;
            int returned = 0;
            while (node != null && returned < count)
            {
                yield return node.Value;
                node = node.Previous;
                returned++;
            }
        }

        /// <summary>
        /// Simple count.
        /// </summary>
        public static int Count => _issues.Count;
    }
}

