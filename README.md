# Scaling-Robot (Bir Clean Architecture Macerası)

Clean Architecture'ın basit uygulamalı bir örneğini yapmaya çalışacağım. Asp.Net Core üstünde ilerlemeyi düşünüyorum. .Net 5 tabanlı olmasına özen göstereceğim. Frontend tarafta React veya Vue.Js kullanabilirim. Fikir olarak çok sıradan ama bilindik bir senaryo üstünde ilerleyebilirim. Çalışma odamdaki kitapların sayısı arttı ve hangi kitap hangi rafta bulmakta zorlanıyorum. Kitaplığımı kayıt altına alacağım basit bir uygulama geliştirebilirim. Scaling-Robot isminin kitaplıkla bir ilgisi yok. Github önerdi, hoşuma gitti ;)

__Takip edilen kaynak : Asp.Net Core and Vue.js, Build read-world, scalable, full-stack applications using vue.js 3, Typescript, .NET 5, and Azure, Devlin Basilan Duldulao, Packt__

## Taslak Plan

- [x] Gün 0 - Proje iskeletinin oluşturulması, EF kurulumu ve SQlite migration işleri
- [x] Gün 1 - MediatR Eklenmesi ve Temel Behavior tipleri ile bazı servis sözleşmelerinin oluşturulması
- [ ] Gün 2 -
- [ ] Gün 3 -
- [ ] Gün 4 -
- [ ] Gün 5 -
- [ ] Gün 6 -
- [ ] Gün 7 -

## Çalışma Logları

Projede ilerledikçe neler yaptığımı gün bazında kayıt altına almak iyi bir fikir olabilir.

## Gün 0 - Proje iskeletinin oluşturulması, EF kurulumu ve SQlite migration işleri

Core, Infrastructure ve Presentation katmanlarını içeren Solution ağacı ile temel projelerin oluşturulması.

```bash
mkdir Librarian
cd Librarian

dotnet new sln

mkdir src
cd src

mkdir core
mkdir infrastructure
mkdir presentation

cd core
dotnet new classlib -f netstandard2.1 --name Librarian.Domain
dotnet new classlib -f netstandard2.1 --name Librarian.Application

cd Librarian.Application
dotnet add reference ..\Librarian.Domain\Librarian.Domain.csproj

cd ..
cd ..
cd infrastructure

dotnet new classlib -f net5.0 --name Librarian.Data
dotnet new classlib -f net5.0 --name Librarian.Shared

cd Librarian.Data
dotnet add reference ..\..\core\Librarian.Domain\Librarian.Domain.csproj
dotnet add reference ..\..\core\Librarian.Application\Librarian.Application.csproj

cd ..
cd Librarian.Shared
dotnet add reference ..\..\core\Librarian.Application\Librarian.Application.csproj

cd ..
cd ..
cd presentation

dotnet new webapi --name Librarian.WebApi

cd Librarian.WebApi

dotnet add reference ..\..\core\Librarian.Application\Librarian.Application.csproj
dotnet add reference ..\..\infrastructure\Librarian.Data\Librarian.Data.csproj
dotnet add reference ..\..\infrastructure\Librarian.Shared\Librarian.Shared.csproj

cd ..
cd ..
cd ..

dotnet sln add src/core/Librarian.Domain/Librarian.Domain.csproj
dotnet sln add src/core/Librarian.Application/Librarian.Application.csproj
dotnet sln add src/infrastructure/Librarian.Data/Librarian.Data.csproj
dotnet sln add src/infrastructure/Librarian.Shared/Librarian.Shared.csproj
dotnet sln add src/presentation/Librarian.WebApi/Librarian.WebApi.csproj
```

İskelet kabaca şöyle;

- __core__
  - Librarian.Application _(Domain projesini kullanır)_
  - Librarian.Domain
- __infrastructure__
  - Librarian.Data _(Application ve Domain projelerini kullanır)_
  - Librarian.Shared _(Application projesini kullanır)_
- __presentation__
  - Librarian.WebApi _(Application, Data ve Shared projelerini kullanır)_

```bash
# Domain projesine entity ve enum tipleri ekleniyor
cd src
cd core
cd Librarian.Domain
mkdir Entities
mkdir Enums
cd ..
cd ..

# Entity Framework için gerekli hazırlıklar

dotnet tool install --global dotnet-ef

cd presentation
cd Librarian.WebApi

dotnet add package Microsoft.EntityFrameworkCore.Design

cd ..
cd ..
cd infrastructure
cd Librarian.Data
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

mkdir Contexts

# Sqlite veri tabanı için migration operasyonları

dotnet ef migrations add InitialCreate --startup-project ..\..\presentation\Librarian.WebApi
dotnet ef database update --startup-project ..\..\presentation\Librarian.WebApi
```
## Gün 1 - MediatR Eklenmesi ve Temel Behavior tipleri ile bazı servis sözleşmelerinin oluşturulması

Yavaştan CQRS tarafında önemli görevi olan ve contoller taleplerini doğru command/query nesnelerine yönlendirecek MediatR paketi ile ilgili geliştirmelere başlıyorum.
MediatR katmanında request,handler,response üçgeninde araya girmemizi sağlayan Behavior türevlerinin nasıl kullanıldığını anlamaya çalışıyorum. Talebin ömrü belli bir sınırın üstündeyse uyarı logu atan bir tanesi, çeşitli doğrulama kriterlerini yakalayıp ihlaller için exception toplayan bir diğeri vs

```bash
# MediatR, DependencyInjection ve Log paketlerinin eklenmesi
cd ..
cd ..
cd core
cd Librarian.Application
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
dotnet add package Microsoft.Extensions.Logging.Abstractions
mkdir Common
cd Common
mkdir Behaviors
```

- MediatR üstünden kullanılabilecek farklı davranışlar eklendi. Özellikle request ve handler arasına girilen yerlere eklendiler. Loglama, performans ölçücü, exception kovalayıcı, içerik doğrulayıcı.
- Mesaj doğrulama kısmında çalışacak ValidationBehavior davranışı için FluentValidation paketi eklendi
- Doğrulama ihlalleri için Common altına Exception klasörü açılıp ValidationException sınıfı eklendi. 

```bash
dotnet add package FluentValidation

# Uygulama katmanında EF için bir sözleşme ekleyeceğiz. Öncesinde EF Core paketini
# Librarian.Application projesine ekliyoruz
dotnet add package Microsoft.EntityFrameworkCore
```

Librarian.Application içindeki Common altına Interfaces isimli bir klasör oluşturup içine EF sözleşmesini eklendi. Burada uygulama seviyesindeki hizmetler için servis sözleşmelerini toplayabiliriz. Örneğin email gönderme hizmeti.

EmailService sözleşmesi mail gönderme işini üstlenirken mail bilgisi için Dtos/Email klasöründeki EmailDto sınıfını kullanıyor. Bu nedenle,

```bash
# Librarian.Application projesinde iken aşağıdaki klasör yapısı oluşturuldu.
mkdir Dtos
cd Dtos
mkdir Email

# Sonrasında önce EmailDto isimli Data Transfer Object sınıfı, sonrasında IEmailServise sözleşme sınıfı yazıldı.
```

## Gün 2 - 

## Gün 3 -

## Gün 4 -

## Gün 5 -

## Gün 6 -

## Gün 7 -


