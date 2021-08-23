using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librarian.Application.Common.Exceptions
{
    /*
     * 
     * ValidationBehavior tipi tarafından kullanılan Exception türevidir.
     * 
     */
    public class ValidationException
        :Exception
    {
        public IDictionary<string, string[]> Errors { get; }
        public ValidationException()
        {
            Errors = new Dictionary<string, string[]>();
        }
        public ValidationException(IEnumerable<ValidationFailure> errors)
            :this()
        {
            var errorGroups= errors
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var e in errorGroups)
            {
                Errors.Add(e.Key, e.ToArray());
            }
        }

    }
}
