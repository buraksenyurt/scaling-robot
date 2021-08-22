using Librarian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Librarian.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Book> Books{ get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
