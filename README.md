# Scaling-Robot (Bir Clean Architecture Macerası)

Clean Architecture'ın basit uygulamalı bir örneğini yapmaya çalışacağım. Asp.Net Core üstünde ilerlemeyi düşünüyorum. .Net 5 tabanlı olmasına özen göstereceğim. Frontend tarafta React veya Vue.Js kullanabilirim. Fikir olarak çok sıradan ama bilindik bir senaryo üstünde ilerleyebilirim. Çalışma odamdaki kitapların sayısı arttı ve hangi kitap hangi rafta bulmakta zorlanıyorum. Kitaplığımı kayıt altına alacağım basit bir uygulama geliştirebilirim. Scaling-Robot isminin kitaplıkla bir ilgisi yok. Github önerdi, hoşuma gitti ;)

__Takip edilen kaynak : [Asp.Net Core and Vue.js, Build read-world, scalable, full-stack applications using vue.js 3, Typescript, .NET 5, and Azure, Devlin Basilan Duldulao, Packt](https://www.amazon.com.tr/ASP-NET-Core-Vue-js-real-world-applications/dp/1800206690)__

## Taslak Plan

- [x] Gün 00 - Proje iskeletinin oluşturulması, EF kurulumu ve SQlite migration işleri
- [x] Gün 01 - MediatR Eklenmesi ve Temel Behavior tipleri ile bazı servis sözleşmelerinin oluşturulması
- [x] Gün 02 - AutoMapper ve CSV Export Özelliğinin Kazandırılması
- [x] Gün 03 - İlk Query Tiplerinin(ExportBooksQuery, GetBooksQuery) Yazılması
- [x] Gün 04 - Kitap Oluşturma, Güncelleme ve Silme operasyonlarına ait Command Nesnelerinin Oluşturulması
- [x] Gün 05 - Dependency Injection yürütücü, Mail gönderici ve CSV dosya üretici sınıflarının yazılması.
- [x] Gün 06 - Web API Projesindeki Controller'ların Tamamlanması ve Diğer
- [x] Gün 07 - Serilog Entegrasyonu ve Yapısal Log'lamaya Geçiş
- [x] Gün 08 - Cache Yapısının Kurgulanması ve Redis Entegrasyonu
- [x] Gün 09 - JWT Bazlı Güvenlik Politikasının Eklenmesi
- [x] Gün 10 - Önyüz Uygulamasının Vue.js ile Geliştirilmesi
- [x] Gün 11 - Vue Tarafında Authentication Mekanizmasının Uygulanması
- [x] Gün 12 - Validation Kontrollerinin Eklenmesi
- [x] Gün 13 - SQL Server Göçü
- [x] Gün 14 - Unit Test ile Entegrasyon Testlerinin Yazılması

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
  - Librarian.Domain
  - Librarian.Application _(Domain projesini kullanır)_
- __infrastructure__
  - Librarian.Data _(Application ve Domain projelerini kullanır)_
  - Librarian.Identity _(Application projesini kullanır)_
  - Librarian.Shared _(Application projesini kullanır)_
- __presentation__
  - Librarian.WebApi _(Application, Identity, Data ve Shared projelerini kullanır)_
  - librarian-app _(Vue.js uygulamasıdır ve WebApi projesi ile Single Page Application bütünlüğü içinde çalışır)_

Kabaca aşağıdaki gibi bir şeyler ortaya çıkacak gibi duruyor.

![./Assets/screenshot_21.png](./Assets/screenshot_21.png)

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
FluentValidation.DependencyInjectionExtensions

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

## Gün 2 - AutoMapper ve CSV Export Özelliğinin Kazandırılması

Kitap listesinin CSV olarak exprot edilmesini istediğimizi düşünelim. Book'un bazı alanlarını taşıyan ayrı bir nesne, çıktı için bir ViewModel tasarlanıyor. Aralardaki nesneden nesneye dönüşümlerde AutoMapper kullanılıyor.

```bash
# Librarian.Application projesine AutoMapper paketi eklenir.

dotnet add package AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

Bunu takiben Common altına Mappings isimli bir klasör açıldı ve MappingProfile sınıfı ile IMapFrom sözleşmesi eklendi. _(Bunların nereden işe yarayacağını kitabın ilerleyen kısımlarından anlayacağım sanırım)_

```bash
# Librarian.Application klasöründeyken
mkdir Books
cd Books
mkdir Commands
mkdir Queries
cd Queries
mkdir ExportBooks
mkdir GetBooks
```

- ExportBooks klasörüne gidildi ve BookRecord ile ExportBooksViewModel isimli sınıflar eklendi. 
- Ayrıca CSV dosya çıktısı için ICsvBuilder isimli bir servis sözleşmesi Common/Interfaces klasörüne eklendi.

## Gün 3 - İlk Query Tiplerinin(ExportBooksQuery, GetBooksQuery) Yazılması

Librarian.Application üstünde ilk Query tipini yazmaya başladım.

- Dtos altına Books isimli bir klasör açıp içine BookDto sınıfı eklendi.
- Aynı projenin Queries/ExportBooks klasörü altına ExportBooksQuery ve ExportBooksQueryHandler isimli sınıflar eklendi. Kitap listesini CSV'te export eden Query ve Handler tipleri.
- Kitap listesini çekmek için gerekli GetBooksQuery ve GetBooksQueryHandler isimli Query sınıfları eklendi. 
- Her iki Query Handler tipi de çevrilen DTO nesne listelerini birer ViewModel içerisinde kullanıp geri veriyor. _CSV çıktısı ile ilgili olan ExportBooksViewModel, kitap listesinin çekmek ile ilgili olan da BooksViewModel_

QueryHandler tipleri ViewModel nesneleri döndürüyor. Book tipinin tüm özelliklerini döndürmüyoruz veya dönüştürerek döndürdüğümüz özellikleri var. Bu nedenle ViewModel'ler içerisinde DTO nesneleri kullanılıyor. Handler sınıflarının Execute fonksiyonlarında EF Context üstünden gelen Book tipinin Mapper üzerinden ilgili DTO'lara çevrimi söz konusu.

## Gün 4 - Kitap Oluşturma, Güncelleme ve Silme operasyonlarına ait Command Nesnelerinin Oluşturulması

Bunun için Librarian.Application projesindeki Books/Commands klasöründe çalışacağız. Her komut için birer alt klasör açarak ilerlemek lazım.

```bash
mkdir CreateBook
mkdir UpdateBook
mkdir DeleteBook
```

- Kitap oluşturma işini üstlenen CreateBookCommand ve Handler sınıfı eklendi.
- Kitap oluşturma işi sırasında doğrulama işini üstlenen CreateBookCommandValidator sınıfı eklendi.
- Kitap bilgilerini güncelleme işini üstlenen UpdateBookCommand ve Handler sınıfı eklendi.
- Kitap güncellerken de doğrulama işlemleri gerekiyor. Bunun için de UpdateBookCommandValidator sınıfı ilave edildi.
- Güncelleme sırasında aranan kitap bulunamadığında ortama BookNotFoundException istisnası fırlatılmak istendiğinden Common/Exceptions altına da bu sınıf eklendi.
- Envanterden kitap silmek için kullanılacak olan DeleteBookCommand ve Handler tipi eklendi.

## Gün 5 - Dependency Injection yürütücü, Mail gönderici ve CSV dosya üretici sınıflarının yazılması.

- Librarian.Application projesi için bir dependency injection sınıfı eklendi.
- Librarian.Shared projesine Services isimli klasör açıldı ve içine CsvBuilder, EmailService sınıfları eklendi. 
- Mail gönderimi sırasından From ve DisplayName gibi kısımları çalışma zamanının sahibi uygulamadan almamız gerekebilir. Bu nedenle appSettings için MailSettings isimli bir sınıf Librarian.Domain projesindeki Settings klasörü altına eklendi. 
 
CsvBuilder için yardımcı CsvHelper, EmailService içinde MailKit ve MimeKit paketlerini Infrastructure katmanındaki Librarian.Shared projesine

```bash
dotnet add package CsvHelper
dotnet add package MailKit
dotnet add package MimeKit

# tabii birde Dependency Injection tarafı için yine Librarian.Shared projesine aşağıdaki paketi ekliyoruz.
dotnet add package Microsoft.Extensions.Options.ConfigurationExtensions
```

Servisler eklendikten sonra pek tabii bunları tüketecek uygulama kimse onun DI servislerine yardımcı olması için aynen Application projesinde olduğu gibi DependencyInjection isimli bir sınıfı Shared projesine de ekliyoruz.

__Bu arada şu ana kadar yazdığımız servisler çalışıyor mu hiç test etmedim. Keşke Unit Test'leri yazark ilerleseydim dediğim noktadayım :|__

## Gün 6 - Web API Projesindeki Controller'ların Tamamlanması ve Diğer

- Presentation katmanındaki Librarian.WebApi projesinde bulunan BooksController sınıfı, MediatR ve doğal olarak CQRS kullanır hale getirildi.
- Startup.cs içerisinde Core katmanındaki Application ve ve Infrastructure katmanındaki Shared kütüphanelerinde tanımladığımız Dependency Injection metotları konuşlandırıldı.
- Mail ayarları için appSettings.json dosyasında MailSettings alanı açıldı ve özellikleri eklendi.
- Entity Framework Context'inin ele alındığı Infrastructure.Data propjesine de Dependency Injection ayarları için sınıf eklendi.
- Kısa bir test yapılıp Swagger arabirimi üstünden yeni bir kitap eklenip listesi çekildi.
- CSV Export işlemi için WebApi tarafına ExportController sınıfı eklendi.

Sonrasında uygulamayı çalıştırıp Swagger arabirimi üstünden birkaç test yapılabilir. Birkaç kitap eklenip listesi çekilmeye çalışışabilir.

![./Assets/screenshot_2.png](./Assets/screenshot_2.png)

![./Assets/screenshot_3.png](./Assets/screenshot_3.png)

![./Assets/screenshot_5.png](./Assets/screenshot_5.png)

## Gün 7 - Serilog Entegrasyonu ve Yapısal Log'lamaya Geçiş

Logları yapısal (structured) tutmak için Web API tarafında ekler yapıldı.

```bash
# Gerekli Nuget paketleri yüklenir
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Settings.Configuration
dotnet add package Serilog.Exceptions
dotnet add package Serilog.Formatting.Compact
dotnet add package Serilog.Enrichers.Environment
dotnet add package Serilog.Enrichers.Process
dotnet add package Serilog.Enrichers.Thread
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Sinks.SQLite
```

- Librarian.WebApi, Program.cs içerisindeki Main metoduna loglama için gerekli kodlar eklendi.

Log mekanizması eklendikten sonra uygulamayı çalıştırıp Swagger ile birkaç test yapıldığında Logs klasör altındaki SQLite ve JSON dosya içeriklerinin log kayıtları ile doldurulduğunun görülmesi lazım.

![./Assets/screenshot_4.png](./Assets/screenshot_4.png)

## Gün 8 - Cache Yapısının Kurgulanması ve Redis Entegrasyonu

In-Memory cache yerine kitabın da anlattığı üzere dağıtık cache sistemlerinden redis'i kullanmayı tercih ettim. Sistem redis kurmayacağım ancak docker imajından yararlanabilirm.

```bash
docker run --name izmir -p 6379:6379 -d redis

#Container'ı çalıştırdıktan sonra Docker Desktop üstünden terminal ekranını açıp izleyen komutu vermekte yarar var.
redis-cli ping

# Bize PONG cevabı dönmeli

# Core katmanındaki Librarian.Application projesine redis kullanımı için gerekli Nuget paketleri yüklenir.
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis
dotnet add package Microsoft.Extensions.Configuration

# Cache için JSON serileştirmesi kullanabiliriz
dotnet add package Newtonsoft.Json
```

- Redis servislerinin DI tarafına dahil edilmesi için Librarian.Application projesindeki DependencyInjection sınıfında gerekli düzenlemeler yapıldı.
- Web Api projesinin appSettings.json dosyasına Redis sunucu adresi eklendi.
- Core katmanındaki Librarian.Application projesindeki GetBooksQueryHandler sınıfında cache stratejisi uygulandı.

Redis ayarları yapıldıktan sonra uygulamayı debug modda çalıştırıp incelemekte yarar var. İlk seferinde kitap listesini veri tabanından çekmeli, sonrakinde ise(belirlenen süre politikasına göre tabii) içeriği redis cache üstünden getirmeli.

__Kitap Redis içeriğini [Another Redis Desktop Manager](https://github.com/qishibo/AnotherRedisDesktopManager) isimli programla görebileceğimizi belirtmişti. Bende indirip denedim ve kullanışlı bir arabirimi olduğunu gördüm__

![./Assets/screenshot_1.png](./Assets/screenshot_1.png)

## Gün 09 - JWT Bazlı Güvenlik Politikasının Eklenmesi

Web Api tarafı için Token tabanlı bir doğrulama sistemi_(Token Based Authentication)_ eklemeye başladım. Servisleri kullanabilmek için talep gönderen tarafın geçerli bir token ile gelmesi gerekecek. Dolayısıyla önce kendini tanıtıp geçerli bir kullanıcı ise token almalı ve servis çağrısı yaparken bu token'ı header'a bearer tipte ekleyerek talepte bulunmalı. Bu sayede servislerimize yetkisiz erişimleri engellemiş olacağız.

```bash

# Infrastructure katmanına Librarian.Identity isimli Class Library eklendi.
cd infrastructure
dotnet new classlib -f net5.0 --name Librarian.Identity

cd ..
cd ..
dotnet sln add src\infrastructure\Librarian.Identity\Librarian.Identity.csproj

# Librarian.Idendity projesine Librarian.Application referans edilir
dotnet add reference ..\..\core\Librarian.Application\Librarian.Application.csproj

# Librarian.WebApi projesine Librarian.Identity projesi referans edilir.
dotnet add reference ..\..\infrastructure\Librarian.Identity\Librarian.Identity.csproj

# Librarian.Identity projesinde gerekli olan Nuget paketleri eklenir
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Authentication.OpenIdConnect
```

Sonrasında;

- Librarian.Domain projesine User sınıfı eklendi.
- Librarian.Application projesinde Dtos altına User klasörü açılıp AuthenticationRequest ve AuthenticationResponse sınıfları eklendi.
- Librarian.Application projesinde Common/Interfaces altına IUserService sözleşemesi eklendi.
- Librarian.Identity projesine AuthenticationSettings, JwtHandler, AuthorizeAttribute, UserService ve tabii ki DependencyInjection sınıfları ilave edildi.
- Librarian.WebApi projesine UserController isimli controller eklendi.

Gerekli geliştirmeler sonrası token almadan bir talep denersek aşağıdaki gibi HTTP 401 hatası alırız.

![./Assets/screenshot_6.png](./Assets/screenshot_6.png)

Swagger üstündeki denemelerde Token'ı talep içerisine eklemek için arabirime bir yardımcı eklenmiş. Bunun için Web API startup içindeki Swagger generator eklenen kısımda ilaveler söz konusu. Ayrıca JWT tarafını ele alan tipide Middleware katmanına ekledik. Buna göre uygulama tekrar çalıştırıldığında yetkilendirme ihtiyacı olan hizmetlerin kolayca kullanabilmesi için bir button çıktığını görmeliyiz.

![./Assets/screenshot_7.png](./Assets/screenshot_7.png)

Testler için Önce auth talebi ile geçerli bir token alınır ve sonrasında Authorize butonuna basarak gelen iletişim penceresine ilgili token girilerek servis çağrısı gerçekleştirilir. Denemelerden birkaç görüntü.

Token alma kısmı;
![./Assets/screenshot_8.png](./Assets/screenshot_8.png)

Authorize penceresinde token girme;
![./Assets/screenshot_9.png](./Assets/screenshot_9.png)

![./Assets/screenshot_10.png](./Assets/screenshot_10.png)

Token ile birlikte bir talep gönderme;

![./Assets/screenshot_11.png](./Assets/screenshot_11.png)

Testleri postman aracıyla da yapabiliriz elbette.

![./Assets/screenshot_12.png](./Assets/screenshot_12.png)

![./Assets/screenshot_13.png](./Assets/screenshot_13.png)

## Gün 10 - Önyüz Uygulamasının Vue.js ile Geliştirilmesi

Kitaplığıma ait envanteri yöneten servis tarafı ve Clean Architecture çatısı hazır. Şimdi önyüz tarafını geliştirmeye başlıyorum. Kitap Vue üstünden ilerliyor. Windows tabanlı sistemime bunun için gerekli node.js ve npm paketlerini yükledikten sonra aşağıdaki komutla vue uygulamaları oluşturmak için gerekli şablonu kuruyorum.

```bash
npm install -g @vue/cli

# Presentation klasörü altındayken vue projesi oluşturulur
vue create librarian-app

# > Manually select features
# İzleyen kısımda yapılacak seçimler (Choose Vue Version, Babel, Router, Vuex, Linter/Formatter)
# Version olarak 2.x seçilir ki 3.x ile denemek lazım
# Use history mode for router sorusuna Y(Yes) cevabı verilir
# Ardından ESLint + Prettier seçilir
# List on save seçili bırakılır
# In dedicated config files seçili bırakılır

# Material Design yaklaşımına uygun olarak vuetify kütüphanesi eklendi
# librarian-app klasöründeyken aşağıdaki komut çalıştırılır
vue add vuetify

# Default(recommended) ile ilerlenir
# Aşağıdaki komut ile vue tarafının çalıştığından emin olunur
npm run serve
```

___Vue tarafını kodlamak için Visual Studio Code kullanabiliriz___

- librarian-vue projesinin src dizini altındaki components içersine NavigationBar.vue bileşeni eklendi
- Buna bağlı olarak navigasyon çubuğu kontrolü app.vue bileşenine monte edildi.
- Home ve About bileşenlerinin içerikleri düzenlendi.
- Views altında Dashboard isimli klasör açıldı ve içine BookList.vue, DefaultContent.vue ile index.vue bileşenleri eklendi. Ayrıca yeni sayfaların yönlendirme ayarlamaları için router klasöründeki index.js içinde bazı düzenlemeler yapıldı.

Vue tarafı ile Web API tarafının Single Page Application şablonu olarak tek birim halinde kullanmak için birtakım hazırlıklar yapılır. Burada VueCliMiddleware isimli Nuget paketinden de yararlanılır.

```bash
dotnet add package VueCliMiddleware
```

Proje dosyasında ve Startup içerisinde yapılan değişiklikler sonrası Web API projesi başlatıldığında _npm run serve_ komutunun da otomatik çalıştırıldığı, gerekli node build işlemlerinin yapıldığı ve localhost:5001 adresinden vue arayüzüne ulaşıldığı görülebilir.

![./Assets/screenshot_14.png](./Assets/screenshot_14.png)

Vue uygulamasının web api tarafı ile iletişiminde axios paketinden yararlanılmakta. Bunun için librarian-app klasöründe iken npm paketi kurulur.

```bash
npm i axios
```

- Axios nesnesini ayağa kaldırması için src altında api isimli klasör açıldı ve içine api-config.js ile book-service.js dosyaları eklendi.
- BookList isimli vue bileşeninin içeriği servis çağıracak ve kitapların listesini getirecek şekilde değiştirildi.

Bu noktada web api tarafını _dotnet run_ ile çalıştırabiliriz. Öncesinde BooksController'ın Authorize niteliğini kapatmamız gerekecektir nitekim vue tarafına henüz token based bir authentication mekanizması ekli değil. Sonrasında aşağıdakine benzer bir ekran görüntüsü elde etme şansımız var. 

![./Assets/screenshot_15.png](./Assets/screenshot_15.png)

Önemli konulardan biriside state yönetimi. Nesne durumlarını kullanmak isteyen ortak bileşenler olduğunda bunu store mekanizması üstünden yönetebiliyoruz. Vuex yapısı da işi oldukça kolaylaştırıyor.

- store klasörü zaten var. Altında book isimli bir dizin açıldı.
- book klasörü altına services, action-types, actions, state, getters, index javascript dosyaları eklendi.
- store klasöründeki index.js içeriği değiştirildi.
- views\dashboard altındaki BookList.vue içeriği değiştirildi.

İşler yolunda gitti ve aşağıdaki sonuçları almaya başardım.

![./Assets/screenshot_16.png](./Assets/screenshot_16.png)

Sırada Delete işlemi için yapılacaklar var.

- store/book/services.js'e listeden kitap silmek için kullanacağımız delete fonksiyonu eklendi.
- action-types.js'e DELETE_BOOK değeri eklendi.
- action.js'e silme işlemi için removeBookAction fonksiyonu eklendi.
- mutations REMOVE_BOOK action için güncellendi. 
- BookList.vue bileşenine silme fonksiyonu eklendi.
- components altına BookListCard isimli vue bileşeni eklendi.
- dashboard altındaki DefaultContent içeriği değiştirildi ve BookListCard bileşeni kullanılır hale getirildi.

Bu işlemler sonrası Dashboard...

![./Assets/screenshot_17.png](./Assets/screenshot_17.png)

![./Assets/screenshot_18.png](./Assets/screenshot_18.png)

![./Assets/screenshot_19.png](./Assets/screenshot_19.png)

Şimdi yeni kitap eklemek için neler yaptığımıza bakalım.

- store/book altındaki services.js'e Post çağrısı yapan fonksiyon eklendi.
- action-types olarak yeni bir sabit eklendi.
- actions.js içerisine kitap ekleme aksiyonu ilave edildi.
- mutations.js kitap ekleme aksiyonu için güncellendi.
- components klasörüne AddBookForm.vue bileşeni eklendi.
- Kitap ekleme işini üstlenen bileşen DefaultContent sayfasına dahil edildi.

![./Assets/screenshot_20.png](./Assets/screenshot_20.png)

## Gün 11 - Vue Tarafında Authentication Mekanizmasının Uygulanması

Servis tarafında geçici süreliğine Authorization kısmını kapatmıştık. Gerçek senaryoda ön yüzün servislerle konuşurken JWT Token'a ihtiyacı olacak. Dolayısıyla bir Authentication mekanizması da gerekiyor. Bu destek için yine store altındaki aktörlerle state tutarak hareket edilmiş ama öncesinde vue tarafında jwt kullanımı için bir kütüphane yüklenmesi gerekiyor.

```bash
npm i jsonwebtoken
```

Sonrasında yapılanlar şöyle;

- root altında auth isimli klasör açıldı ve authService ile bodyguard modülleri eklendi.
- Benzer şekilde store klasöründe auth isimli başka bir alt dizin açıldı.
- store/auth dizineaction-types.js, actions.js, state.js, mutations.js, getters.js, index.js dosyaları eklendi. 
- store altındaki index.js dosyasında authModule bildirimi yapıldı. 
- Ayrıca middleware tarafında uygulama üzerindeki geçişlerde devreye girecek authentication modüle router altındaki index.js'e ilave edildi. Router tarafında kullanıcı doğrulamasına ihtiyacı olan yerler için gerekli meta tanımlamaları da yapıldı.
- Login sayfası için root üstündeki auth klasörü altına views isimli yeni bir klasör açıldı ve içine Login.vue ile ContinueAs bileşenleri eklendi.
- NavigationBar bileşeni değiştirildi ve dashboard kısmına geçilebilmesi için login olunma şartı eklendi.
- Denemelerden önce Web API tarafında geçici olarak kaldırılan Authorize nitelikleri yeniden etkinleştirilmeli.

![./Assets/screenshot_22.png](./Assets/screenshot_22.png)

Tabii şu aşamada login başarılı olacak ancak örneğin kitap listesi gelmeyecek ve HTTP 401 Not Authorized hatası alınacaktır.

![./Assets/screenshot_23.png](./Assets/screenshot_23.png)

Nitekim henüz header tarafında bearer token bilgisini ekleyerek talep göndermedik. Bunun için,

- authService modülüne getToken ve logOut fonksiyonları eklendi.
- api klasörüne madMax.js isimli modül eklendi. Bu modül Interceptor görevini üstlenecek ve request'lerin header kısmına gerekli jwt token'ı ekleyecek fonksiyonelliği sunacak.
- Interceptor'ü devreye almak için api-config.js içerisinde gerekli bildirim yapıldı.

![./Assets/screenshot_24.png](./Assets/screenshot_24.png)

Login ve logout işlemleri tamam ve sırada autologin özelliğinin entegrasyonu var. Yani local storage'e konan token bilgisi zaten varsa bunun geçerli olup olmadığını kontrol etmek ve geçerli ise kullanıcıyı kabul edip uygulamada gezinmesine izin vermek.

- authService modülüne isLocalStorageTokenValid ve getUsernameFromToken fonksiyonları eklendi.
- router altındaki index modülünde yer alan login router'u için beforeEnter isimli bir kontrol _(kitapta guard hook diye geçiyor)_ eklendi. Ayrıca continue-as isimli yeni bir route eklendi. local token geçerli ise next çağrısı ile bu route'a yönleniliyor olacak.
- actionTypes'a yeni aksiyon tipi eklendi ve actions modülünde local storage kontrolüne göre çalışan bir fonksiyon yazıldı. Tabii action tipi mutations kısmına da dahil edildi.
- bodyguard servisine local storage'da token olup olmadığını kontrol eden kod parçası eklendi.
- ContinueAs isimli vue bileşeni eklendi. dashboard klasöründeki index sayfası düzenlendi.

![./Assets/screenshot_25.png](./Assets/screenshot_25.png)

![./Assets/screenshot_26.png](./Assets/screenshot_26.png)

## Gün 12 - Validation Kontrollerinin Eklenmesi

Kütüphane için erişim yetkisi olacak kişi oluşturmak amacıyla bir formum var. Girdilerin bir takım kontrollerden geçmesi lazım. Vue tarafında bu işi kolaylaştırması adına vuelidate paketinden yararlanabileceğiz.

```bash
# librarian-app altında çalıştırabiliriz
npm i vuelidate
```

- Var olan plugins klasörü altında vuelidate.js isimli bir dosya oluşturuldu.
- Projede vuelidate paketine ait fonksiyonları kullanabilmek için gerekli bildirim main.js içinde yapıldı.
- src dizininde validators isimli bir klasör açıldı ve içinde index.js eklendi.
- auth/views altındaki Login bileşeninde doğrulama kontrollerinin kullanılması için değişikliler yapıldı. _computed ve validations fonksiyonlarındaki tanımlamalar ile kontrolleri v-model, input, blur error_messages özelliklerine dikkat_
- Yeni bir kitabın eklendiği form içinde vuelidate entegrasyonu yapıldı. _Epey uğraştırdı. data ve validations içerisindeki veri tiplerinin aynı isimde olmasına dikkat. Ayrıca button'ları sayfada validasyon hatası varsa engellemek için :disabled="$v.$anyError" kullanıldı_

Login kısmı için aşağıdakine benzer bir sonuç elde edildi.

![./Assets/screenshot_27.png](./Assets/screenshot_27.png)

![./Assets/screenshot_29.png](./Assets/screenshot_29.png)

Bu da yeni kitap eklerken doğrulama hataları oluşursa elde edilen sonuç. Yine de henüz çözemediğim sorunlar var. email kontrolü düzgün çalışmıyor ve Login popup'ındaki username ile password alanları yeni kullanıcı ekleme kısmındaki kontrollere de bağlanmış görünüyor.

![./Assets/screenshot_28.png](./Assets/screenshot_28.png)

## Gün 13 - SQL Server Göçü

Kitabın bu bölümünde entegrasyon testleri için SQL Server'a geçiş tercih ediliyor. Docker imajını kullanmayı tercih ettim. Bu amaçla Librarian.Data projesinde bir bir docker-compose.yaml dosyası oluşturuldu.

```bash
#aynı klasörde
docker-compose up
```

![./Assets/screenshot_30.png](./Assets/screenshot_30.png)

- Veritabanı bağlantısı için WebApi projesinin appSettings dosyasına yeni değer eklendi.
- Librarian.Data projesine Microsoft.EntityFrameworkCore.SqlServer paketi eklendi.

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

- Librarian.Data projesindeki DependencyInjection sınıfı SqlServer kullanacak şekilde değiştirildi.
- Artık veriyi SQL Server tarafında oluşturmamız gerektiğinden yeni bir migration plana ihtiyacımız var. Var olan Migration klasörü silindi ve yeni plan çalıştırıldı.

```bash
dotnet ef migrations add InitialCreate --startup-project ..\..\presentation\Librarian.WebApi
dotnet ef database update --startup-project ..\..\presentation\Librarian.WebApi
```

SQLite'ı terk edip SQL Server'a göç ettiğimiz için verileri de uçurmuş olduk. Seeding tekniği ile örnek verileri ekleyebiliriz.

- Librarian.Data projoesindeki Contexts klasörüne LibrarianDbContextSeed sınıfı eklendi.
- WebApi projesindeki program.cs Seed fonksiyonunu kullanacak şekilde düzenlendi.

Bu işlem sonrasında uygulama yeniden başlatıldığında SQL Server'a örnek verilerin eklendiğinin görülmüş olması lazım. _(SQL Server Docker Container'ının çalıştığından emin olun)_

## Gün 14 - Unit Test ile Entegrasyon Testlerinin Yazılması

Test fonksiyonları için xUnit kullanılıyor. Birim ve entegrasyon testleri için src klasörü ile aynı seviyede yer alan tests isimli yeni bir dizin kullanılıyor. Bu klasörde terminal üstünden yapılanlar,

```bash
# Entegrasyon testlerini WebApi projesindeki fonksiyonellikler için tasarlayacağız.

# xunit odaklı entegrasyon testleri için bir proje oluşturulur
dotnet new xunit --name Librarian.IntegrationTests
cd Librarian.IntegrationTests

# WebApi projesi referans edilir
dotnet add reference ..\..\src\presentation\Librarian.WebApi\Librarian.WebApi.csproj

# Ardından kabul kriterlerini yazmakta kullanılan FluentAssertions, mocking için Moq ve test veritabanını temiz bir konuma çekmek için Respawn isimli nuget paketleri eklenir.
dotnet add package FluentAssertions
dotnet add package Moq
dotnet add package Respawn

# Birim testleri ise core katmanındaki application projesindeki fonksiyonlar için yazacağız.
# Bunun için Librarian.UnitTests isimli yeni bir proje açılır. Yine tests klasörü altında.
cd ..
dotnet new xunit --name Librarian.UnitTests
cd Librarian.UnitTests
# Birim testler için Librarian.Application projesi referans edilir.
dotnet add reference ..\..\src\core\Librarian.Application\Librarian.Application.csproj
# Kabul kriterlerini şık bir stilde yazmak için yine FluentAssertions paketini kullanacağız
dotnet add package FluentAssertions

# root klasöre çıkıp solution'a bu projeleri ekliyoruz.
cd ..
cd ..
dotnet sln add tests\Librarian.IntegrationTests\Librarian.IntegrationTests.csproj
dotnet sln add tests\Librarian.UnitTests\Librarian.UnitTests.csproj
```

Sonuçta aşağıdaki gibi bir çıktı oluşmalı.

![./Assets/screenshot_31.png](./Assets/screenshot_31.png)

Kitap birim test örnekleri için ValidationException ve AutoMapper tiplerinin olduğu senaryoları ele almış.

- Librarian.UnitTests projesinde Common isimli bir klasör açılır. _(core katmanındaki Application projesindeki Common klasörüne karşılık. Yani test tarafında da benzer klasör yapısı oluşturulur)_ Exceptions ve Mappings isimli iki klasör daha açılır.
- Book tipinden BookDTO'ya dönüşümü test eden MappingTests sınıfı eklendi.
- Exceptions klasörü altına ValidationException tipi için iki basit test içeren ValidationExceptionTests sınıfı eklendi.

Birim testleri test projesinin komut satırından _dotnet test_ ile veya Visual Studio arabirimindeki Test Explorer yardımıyla çalıştırabiliriz.

![./Assets/screenshot_32.png](./Assets/screenshot_32.png)

Birim testler genellikle veri tabanı, servis, dosya girdi çıktı işlemleri, mesaj kuyrukları gibi bağımlılıkların bulunmadı/kullanılmadığı en küçük birimin testi olarak düşünülebilirler. Diğer yandan bağımlılıkların olduğu hallerde entegrasyon testleri ön plana çıkmaktadır. Nitekim uygulamayı harici bağımlılıkları ile birlikte test etmemiz gereken senaryolar da vardır. Entegrasyon testleri bağımlılıkları dolayısıyla(dosya, veritabanı, servis haberleşmesi, mesajlaşma vs) daha uzun sürede çalışabilirler. Örnek uygulamada bol miktarda veritabanı işlemi söz konusu.

- WebApi projesindeki appsettings.json aynen Librarion.IntegrationTest projesinde de oluşturulur. Sadece veritabanı adı değiştirilir, nitekim bu veritabanı sadece test için kullanılacak.
- DbFixture isimli sınıf eklendi. Test çalışma zamanı için gerekli ortam hazırlıklarını üstlenir _(Web Host ortamının mock'lanması, DI servis yapısının hazırlanması, scope oluşturulması gibi)_ ve bazı statik fonksiyonellikler _(Db'ye nesne ekleme, Mediator'a mesaj gönderme gibi)_ sağlar.
- Tüm testlerin DbFixture'u zahmetsizce kullanması için DbCollection sınıfı eklendi.
- Books isimli bir dizin oluşturuldu ve altına Commands ve Queries isimli iki alt klasör eklendi. Tam da core katmanı, Apllication projesindeki klasör hiyerarşisine uygun olacak şekilde. 
- CreateBookTests, UpdateBookTests, DeleteBookTests ve GetBooksTests sınıfları eklendi.

Kitabın da belirttiği sonuçların benzerlerini elde etmeyi başardım.

![./Assets/screenshot_33.png](./Assets/screenshot_33.png)
