# scaling-robot (Bir Clean Architecture Macerası)

Clean Architecture'ın basit uygulamalı bir örneğini yapmaya çalışacağım. Asp.Net Core üstünde ilerlemeyi düşünüyorum. .Net 5 tabanlı olmasına özen göstereceğim. Frontend tarafta React veya Vue.Js kullanabilirim. Fikir olarak çok sıradan ama bilindik bir senaryo üstünde ilerleyebilirim. Çalışma odamdaki kitapların sayısı arttı ve hangi kitap hangi rafta bulmakta zorlanıyorum. Kitaplığımı kayıt altına alacağım basit bir uygulama geliştirebilirim. Scaling-Robot isminin kitaplıkla bir ilgisi yok. Github önerdi, hoşuma gitti ;)

## Çalışma Logları

Projede ilerledikçe neler yaptığımı gün bazında kayıt altına almak iyi bir fikir olabilir.

## 21 Ağustos 2021 Cumartesi

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