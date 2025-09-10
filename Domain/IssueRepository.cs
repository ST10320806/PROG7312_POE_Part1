using System;
using System.Collections.Generic;
using System.Linq;
using PROG7312_POE_Part1.Domain;

namespace PROG7312_POE_Part1.Domain
{
    
    public static class IssueRepository
    {
        //Creating linked-list
        private static readonly LinkedList<Issue> _issues = new LinkedList<Issue>();

        
        //Adding issue to list
        public static void Add(Issue issue)
        {
            _issues.AddLast(issue);
        }

        
        public static IEnumerable<Issue> GetAll()
        {
            foreach (var issue in _issues)
                yield return issue;
        }

        //Code for fetching a reported issue by its id (to be implemented later)
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

        //Updating the status on a reported issue (to be implemented later)
        public static bool UpdateStatus(Guid id, IssueStatus newStatus)
        {
            var issue = FindById(id);
            if (issue == null) return false;
            issue.Status = newStatus;
            return true;
        }

        //code for removal of a reported issue (to be implemented later)
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

        //Getting most recent issues (to be implemented later)
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

        
        public static int Count => _issues.Count;
    }
}
//******************************************************************End Of File******************************************************************

