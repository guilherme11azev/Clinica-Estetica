markdown# 🏥 ClinicaEstetica API

API RESTful para gerenciamento completo de agendamentos de uma clínica de estética, onde biomédicos realizam procedimentos estéticos em clientes. O sistema controla a agenda, evita conflitos de horário e acompanha o status de cada atendimento.

---

## 🚀 Tecnologias utilizadas

- [.NET 10](https://dotnet.microsoft.com/) — Framework principal
- [ASP.NET Core Web API](https://learn.microsoft.com/aspnet/core) — Criação da API REST
- [Entity Framework Core](https://learn.microsoft.com/ef/core/) — ORM com Code First
- [SQLite](https://www.sqlite.org/) — Banco de dados leve, sem necessidade de instalação
- [Swagger / OpenAPI](https://swagger.io/) — Documentação interativa da API

---

## 🏗️ Arquitetura
ClinicaEstetica/
├── Controllers/   → Recebe e responde requisições HTTP
├── Services/      → Regras de negócio e validações
├── Repositories/  → Acesso ao banco de dados
├── Models/        → Entidades do banco
├── DTOs/          → Objetos de transferência de dados
└── Data/          → DbContext e configuração do banco

---

## ⚙️ Como executar o projeto

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio Code](https://code.visualstudio.com/)

### Passo a passo

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/ClinicaEstetica.git
cd ClinicaEstetica

# Restaure as dependências
dotnet restore

# Aplique as migrations e crie o banco de dados
dotnet ef database update

# Execute o projeto
dotnet run
```

Acesse a documentação interativa em:
http://localhost:5178/swagger

---

## 📋 Endpoints da API

### Biomédicos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/biomedicos` | Lista todos os biomédicos |
| GET | `/api/biomedicos/{id}` | Busca biomédico por ID |
| GET | `/api/biomedicos/{id}/agenda` | Lista agenda do biomédico por período |
| POST | `/api/biomedicos` | Cadastra novo biomédico |
| PUT | `/api/biomedicos/{id}` | Atualiza biomédico |
| DELETE | `/api/biomedicos/{id}` | Remove biomédico |

### Clientes

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/clientes` | Lista todos os clientes |
| GET | `/api/clientes/{id}` | Busca cliente por ID |
| GET | `/api/clientes/{id}/historico` | Histórico de agendamentos do cliente |
| POST | `/api/clientes` | Cadastra novo cliente |
| PUT | `/api/clientes/{id}` | Atualiza cliente |
| DELETE | `/api/clientes/{id}` | Remove cliente |

### Procedimentos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/procedimentos` | Lista todos os procedimentos |
| GET | `/api/procedimentos/{id}` | Busca procedimento por ID |
| POST | `/api/procedimentos` | Cadastra novo procedimento |
| PUT | `/api/procedimentos/{id}` | Atualiza procedimento |
| DELETE | `/api/procedimentos/{id}` | Remove procedimento |

### Agendamentos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/agendamentos` | Lista com filtros por data, biomédico, cliente e status |
| GET | `/api/agendamentos/{id}` | Busca agendamento por ID |
| GET | `/api/agendamentos/resumo-dia` | Resumo do dia com totalizadores |
| POST | `/api/agendamentos` | Cria agendamento com validação de conflito |
| PATCH | `/api/agendamentos/{id}/confirmar` | Confirma agendamento |
| PATCH | `/api/agendamentos/{id}/concluir` | Conclui agendamento |
| PATCH | `/api/agendamentos/{id}/cancelar` | Cancela agendamento |

---

## 🔄 Fluxo de status do agendamento
Agendado → Confirmado → Concluído
↓
Cancelado

---

## 📏 Regras de negócio

- Biomédico inativo não pode receber novos agendamentos
- Biomédico não pode ter dois agendamentos no mesmo horário
- Cliente não pode ter dois agendamentos no mesmo horário
- O sistema considera a duração do procedimento na verificação de conflito
- Agendamento deve ser criado para data e hora futura
- Cancelamento exige no mínimo 2 horas de antecedência
- Fluxo de status controlado — não é possível pular ou reverter etapas

---

## 🧠 Conceitos aplicados

- **Relacionamentos complexos no EF Core** — Agendamento conecta Biomédico, Cliente e Procedimento simultaneamente
- **Regras de conflito de horário** — verificação de sobreposição de intervalos nas queries
- **Fluxo de status controlado** — transições validadas na camada de Service
- **Data Annotations** — validações declarativas nos DTOs
- **Filtros combinados** — queries dinâmicas com parâmetros opcionais
- **Arquitetura em camadas** — Controllers → Services → Repositories
- **Padrão Repository** com interfaces para desacoplamento
- **Entity Framework Core** com Code First e Migrations
- **DTOs** para controle do contrato da API
- **Injeção de dependência** nativa do .NET
- **Programação assíncrona** com async/await
- **Verbos HTTP semânticos** — uso de PATCH para atualizações parciais de status
