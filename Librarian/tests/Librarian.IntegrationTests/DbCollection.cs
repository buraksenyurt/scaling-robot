using Xunit;

namespace Librarian.IntegrationTests
{
    /*
     * Bu projedeki birim testlerin tamamının bu koleksiyon nesnesine eklenen DbFixture örneğini kullanacağını belirtmiş olur.
     * Her test için DbFixture'u düşünmemize gerek kalmaz.
     * Testler sonlandığında, DbFixture nesne yok edilme sürecine girer nitekim IDisposable arayüzünü uygulamaktadır.
     */
    [CollectionDefinition("DbCollection")]
    public class DbCollection
        :ICollectionFixture<DbFixture>
    {
    }
}
