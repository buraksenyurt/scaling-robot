using FluentAssertions;
using FluentValidation.Results;
using Librarian.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librarian.UnitTests.Common.Exceptions
{
    public class ValidationExceptionTests
    {
        /*
         * Kabul kriterimize göre Core katmanındaki Application projesinde yer alan
         * ValidationException sınıfına ait bir nesne örneğini varsayılan yapıcı ile oluşturursak
         * Errors özelliğinin key değerleri boş olmalı. 
         * Kısacası boş bir Errors dictionary'si olmalı.
         */ 
        [Fact]
        public void ShouldConstructorCreatesAnEmptyErrorDictionary()
        {
            var actual = new ValidationException().Errors;
            actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
        }

        [Fact]
        public void ShouldSingleErrorCreatesASingleElementErrorDictionary()
        {
            var errors = new List<ValidationFailure>
            {
                new ValidationFailure("EMail","E-Posta adresi gereklidir")
            };
            var actual = new ValidationException(errors).Errors;
            actual.Keys.Count().Should().Be(1);
            actual.Keys.Should().BeEquivalentTo("EMail");
            actual["EMail"].Should().BeEquivalentTo("E-Posta adresi gereklidir");
        }
    }
}
