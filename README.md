# Cloud Games Store - Users Service

Microsserviço responsável pelo gerenciamento de usuários da plataforma FIAP Cloud Games Store, incluindo autenticação e cadastro.

## Tecnologias

- .NET 8  
- Entity Framework Core  
- SQL Server  
- Bcrypt + JWT  
- Docker  
- Application Insights  

## Arquitetura

- Clean Architecture  
- Domain-Driven Design (DDD)  
- Result Pattern

## Como executar localmente com Docker Compose

1. Certifique-se de ter o **Docker** em execução.  
2. No diretório do projeto, rode:
   ```bash
   docker-compose up --build
   ```
3. Vai subir a aplicação + Banco SQL Server
4. Pra encerrar, rode:
   ```bash
   docker-compose down
   ```
