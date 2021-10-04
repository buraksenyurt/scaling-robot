using AutoMapper;
using Librarian.Application.Dtos.Books;
using Librarian.Domain.Entities;
using System;
using Travel.Application.Common.Mappings;
using Xunit;

namespace Librarian.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _configurationProvider;

        public MappingTests()
        {
            _configurationProvider = new MapperConfiguration(c =>
              {
                  c.AddProfile<MappingProfile>();
              });
            _mapper = _configurationProvider.CreateMapper();
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configurationProvider.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(Book), typeof(BookDto))]
        public void ShouldSupportMappingFromBookToBookDto(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);
            _mapper.Map(instance, source, destination);
        }
    }
}
