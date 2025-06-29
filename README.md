# Duil – Gestão de Encomendas e Produção

[![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core/)
[![Entity Framework](https://img.shields.io/badge/Entity_Framework-6DB33F?style=for-the-badge&logo=ef&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)

## Descrição

Projeto desenvolvido no âmbito da unidade curricular de **Desenvolvimento Web**, do 2.º ano da Licenciatura em Engenharia Informática – ESTT - IPT.

A aplicação tem como objetivo gerir encomendas, clientes, utilizadores, peças e fábricas de uma empresa de exportação. A aplicação foi desenvolvida com o objetivo de criar uma solução que resolvesse a logistica atual dessa empresa.
Para tal, a aplicação dispões de uma interface intuitiva e que suporta duas línguas (Português de Portugal e Inglês). 
É também facilitado o processo de pesquisa dos modelos e implementou-se Signal R para uma melhor comunicação entre Cliente e Funcionário.


---

## Autores

| Nº | Nome                                      |
|----|-------------------------------------------|
| 19254 | Maria Eduarda Marques Galamba          |
| 24871 | Cláudio Miguel De Almeida Vaz Jorge    |

---

## Tecnologias e Ferramentas

- [.NET 8.0 (ASP.NET MVC)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Bootstrap 5](https://getbootstrap.com/)
- [Bootstrap Icons](https://icons.getbootstrap.com/)
- [SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-8.0)
- [Chart.js](https://www.chartjs.org/)
- [MailTrap](https://mailtrap.io/)
- [Swagger (Swashbuckle)](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

---

## Funcionalidades

- CRUD de **Clientes**, **Fábricas**, **Peças**, **Utilizadores** e **Encomendas**
- Gestão de **roles** (Admin / Cliente)
- Seleção de idioma (**Português** / **Inglês**)
- Envio de emails de autenticação via SMTP
- Comunicação em tempo real com SignalR
- API RESTful com documentação via Swagger
- Integração com gráficos (Chart.js)
- Autenticação JWT na API
- Pesquisa e paginação em tabelas

---

## Como Executar

1. Clonar o repositório:
   ```bash
   git clone https://github.com/teu-utilizador/teu-repositorio.git
   (https://github.com/ClaudioJorge24871/Duil-App)

2. Restaurar pacotes NuGet  
   No Visual Studio, acede ao **Gerenciador de Pacotes NuGet** e clica em `Restaurar`.

3. Aplicar as migrações e criar a base de dados  
   No `Package Manager Console`, executa o seguinte comando:

   ```powershell
   Update-Database

