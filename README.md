# RuData API
RuData API - .NET-библиотека, предназначенная для взаимодействия с сервером EFIR.DataHub от Interfax (RuData). 
RuData API позволяет получить справочные и исторические данные по различным финансовым инструментам.

## Зависимости

- Microsoft.AspNet.WebApi.Client
- Efir.DataHub.Models ([подробнее](https://developer.efir-net.ru/NuGetFeed))

`Efir.DataHub.Models` является внешним NuGet-пакетом, поэтому для разрешения проблем с зависимостями проекта проще использовать .NET CLI: 
```
dotnet add package Microsoft.AspNet.WebApi.Client
dotnet add package Efir.DataHub.Models --source http://developer.efir-net.ru/nuget
```

## Использование

```csharp
var credentials = EfirClient.GetCredentialsFromFile("MyCredentils.json");
using EfirClient efir = new EfirClient(credentials);

await efir.LoginAsync();

var secinfo = await efir.GetSecurityData("RU000A105DL4");

Console.WriteLine(secinfo.nickname);
Console.WriteLine(secinfo.endmtydate);
Console.WriteLine(secinfo.fintoolid);
```

## Файл с параметрами авторизации

Для хранения данных для авторизации используется json-файл со следующим содержанием:
```json
{
  "Url": "https://dh2.efir-net.ru/v2",
  "Login": "my-login",
  "Password": "my-password"
}
```