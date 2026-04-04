# GAPIM - Generic API Mauricio 🚀

![.NET Version](https://img.shields.io/badge/.NET-10.0-blue.svg)
![Status](https://img.shields.io/badge/Status-Active-success.svg)
![License](https://img.shields.io/badge/License-MIT-green.svg)

**GAPIM** é uma biblioteca .NET projetada para acelerar drasticamente o desenvolvimento de APIs REST. Ela fornece uma infraestrutura genérica completa (Controller, Service e Repository) para que você possa criar endpoints CRUD completos em questão de segundos, mantendo uma arquitetura limpa e escalável.

## ✨ Features

* **CRUD Instantâneo:** Herde de um único Controller e ganhe endpoints `GET`, `POST`, `PUT` e `DELETE` automaticamente.
* **Mapeamento Automático:** Integração nativa com `AutoMapper` para separar Entidades de Banco de Dados (`TEntity`) dos Contratos de API (`TRequest`, `TResponse`).
* **Banco em Memória Seguro:** Contém um `MemoryContext` baseado em `ConcurrentDictionary`, garantindo *thread-safety* e alta performance para prototipagem.
* **Swagger/OpenAPI Integrado:** Configuração simplificada para expor a documentação da sua API com poucas linhas de código.
* **Performance Otimizada:** Suporte de ponta a ponta a `CancellationToken` para evitar processamentos fantasmas.
* **Pronto para .NET 10:** Código moderno, limpo e com suporte a *Nullable Reference Types*.

---

## 📦 Instalação

Atualmente, você pode instalar o pacote via gerenciador de pacotes NuGet apontando para o seu diretório local ou servidor NuGet interno:

```bash
dotnet add package GAPIM.GenericAPIMauricio --version 1.0.0
````

-----

## 🚀 Getting Started

Criar uma API completa com a GAPIM é extremamente simples. Veja como configurar o seu projeto:

### 1\. Configurando o `Program.cs`

Registre os serviços essenciais da GAPIM e ative o Swagger:

```csharp
using GAPIM_GenericAPIMauricio.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 1. Registra a GAPIM e o AutoMapper automaticamente
builder.Services.AddGAPIM(options =>
{
    options.Name = "Minha API Incrível";
    options.Description = "API gerada utilizando a biblioteca GAPIM.";
    options.Version = "v1";
});

var app = builder.Build();

// 2. Ativa o Swagger configurado pela GAPIM
if (app.Environment.IsDevelopment())
{
    app.UseGAPIMSwagger();
}

app.MapControllers();
app.Run();
```

### 2\. Criando suas Entidades e DTOs

Crie sua entidade herdando de `BaseEntity`. Em seguida, defina os seus DTOs de entrada (Request) e saída (Response).
*(Não se esqueça de criar o `Profile` do AutoMapper para ensinar a biblioteca como converter os objetos).*

```csharp
using GAPIM_GenericAPIMauricio.Entities;
using AutoMapper;

// Entidade de Domínio
public class Produto : BaseEntity
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
}

// Contratos (DTOs)
public class ProdutoRequest { public string Nome { get; set; } public decimal Preco { get; set; } }
public class ProdutoResponse { public Guid Id { get; set; } public string Nome { get; set; } public decimal Preco { get; set; } }

// Configuração do AutoMapper
public class ProdutoProfile : Profile
{
    public ProdutoProfile()
    {
        CreateMap<ProdutoRequest, Produto>();
        CreateMap<Produto, ProdutoResponse>();
    }
}
```

### 3\. A Mágica: Seu Controller

Crie um controller e herde de `GenericController`. **Apenas isso\!** Sua API já possui todos os endpoints REST operacionais.

```csharp
using GAPIM_GenericAPIMauricio.Controllers;
using GAPIM_GenericAPIMauricio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MinhaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : GenericController<Produto, ProdutoRequest, ProdutoResponse>
{
    public ProdutoController(IGenericService<Produto, ProdutoRequest, ProdutoResponse> service) 
        : base(service)
    {
    }
}
```

-----

## 🛠 Extensibilidade (Sobrescrevendo Comportamentos)

A GAPIM foi construída pensando no mundo real. Todos os métodos do `GenericController` são `virtual`. Se você precisar de uma regra de negócio específica para a criação de um Produto, basta sobrescrever o método:

```csharp
[HttpPost]
public override async Task<ActionResult<ProdutoResponse>> AddAsync([FromBody] ProdutoRequest request, CancellationToken cancellationToken)
{
    if (request.Preco <= 0)
    {
        return BadRequest(new { Message = "O preço deve ser maior que zero!" });
    }
    
    // Chama o comportamento padrão da biblioteca se a validação passar
    return await base.AddAsync(request, cancellationToken);
}
```

-----

## 🤝 Contribuição

Contribuições são sempre bem-vindas\! Se você encontrou um bug, tem uma ideia para uma nova feature ou deseja melhorar a documentação, sinta-se à vontade para abrir uma *Issue* ou enviar um *Pull Request*.

## 📄 Licença

Este projeto está sob a licença [MIT](https://www.google.com/search?q=LICENSE).
