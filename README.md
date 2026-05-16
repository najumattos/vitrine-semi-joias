# Vitrine Semi Jóias 💎

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![AutoMapper](https://img.shields.io/badge/AutoMapper-0078D4?style=for-the-badge&logoColor=white)
![MVC](https://img.shields.io/badge/MVC%20Pattern-0078D4?style=for-the-badge&logoColor=white)
![SQLServer](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)

## 📋 Descrição Geral

**Vitrine Semi Jóias** é uma aplicação web moderna desenvolvida em **ASP.NET Core 8 MVC** que implementa um sistema profissional de gerenciamento de catálogo de semi-jóias. O projeto utiliza arquitetura em camadas com separação clara de responsabilidades, padrões de projeto consolidados e tecnologias de ponta para garantir escalabilidade, manutenibilidade e segurança.

A aplicação oferece funcionalidades completas de CRUD para produtos, gerenciamento de imagens no servidor, autenticação segura com criptografia BCrypt e um painel administrativo intuitivo.

---

## 🏗️ Arquitetura e Padrões de Projeto

O projeto foi estruturado seguindo princípios **SOLID** e padrões consolidados de desenvolvimento, promovendo uma arquitetura em camadas bem definida:

### 📐 Estrutura de Camadas

```
┌─────────────────────────────────────┐
│  Views (Apresentação)               │
└──────────────────┬──────────────────┘
                   │
┌──────────────────▼──────────────────┐
│  Controllers (Orquestração)         │
└──────────────────┬──────────────────┘
                   │
┌──────────────────▼──────────────────┐
│  Services (Lógica de Negócio)       │
│  + DTOs (Transferência de Dados)    │
└──────────────────┬──────────────────┘
                   │
┌──────────────────▼──────────────────┐
│  Repositories (Acesso a Dados)      │
└──────────────────┬──────────────────┘
                   │
┌──────────────────▼──────────────────┐
│  Models + DbContext (Persistência)  │
└─────────────────────────────────────┘
```

### 🔄 Padrões Implementados

#### **1. Repository Pattern**
- **Responsabilidade**: Abstração do acesso a dados
- **Benefício**: Facilita testes unitários e troca de tecnologia de persistência
- **Localização**: Pasta `Repository/` com interfaces em `Repository/Interfaces/`
- **Exemplo**: `IProductRepository` implementa operações genéricas e específicas de produtos

#### **2. Service Layer Pattern**
- **Responsabilidade**: Encapsulamento da lógica de negócio
- **Benefício**: Reutilização de código, validações centralizadas e separação de responsabilidades
- **Localização**: Pasta `Services/` com interfaces em `Services/Interfaces/`
- **Exemplo**: `ProductService` orquestra operações de produto, validações e transformações

#### **3. Data Transfer Objects (DTOs)**
- **Responsabilidade**: Transferência de dados entre camadas sem expor models internos
- **Benefício**: Segurança de dados, controle granular de informações expostas
- **Localização**: Pasta `DTOs/`
- **Exemplo**: `ProductDto` encapsula apenas dados públicos do produto

#### **4. ViewModels**
- **Responsabilidade**: Preparação de dados específicos para as views
- **Benefício**: Lógica de apresentação centralizada, views mais simples
- **Localização**: Pasta `ViewModels/`
- **Exemplo**: `ProductViewModel` contém dados formatados para exibição

#### **5. Dependency Injection (DI)**
- **Responsabilidade**: Gerenciamento de dependências em tempo de execução
- **Benefício**: Código flexível, testável e desacoplado
- **Implementação**: Configurado em `Configurations/DependencyInjectionConfig.cs`

#### **6. AutoMapper Integration**
- **Responsabilidade**: Mapeamento automático entre objetos (Models ↔ DTOs ↔ ViewModels)
- **Benefício**: Elimina código boilerplate repetitivo, reduz erros de mapeamento manual
- **Fluxo**: 
  ```
  Model → AutoMapper → DTO → AutoMapper → ViewModel → View
  ```

---

## ✨ Funcionalidades Técnicas

- 🛍️ **CRUD Completo de Produtos** - Criar, ler, atualizar e deletar produtos com validações robustas
- 📸 **Gerenciamento de Imagens** - Upload de arquivos de imagem com armazenamento seguro no servidor
- 🗑️ **Exclusão de Arquivos Físicos** - Remoção automática de imagens quando produtos são deletados
- 🔐 **Autenticação Segura** - Sistema de autenticação customizado com criptografia BCrypt
- 👤 **Controle de Acesso** - Proteção de rotas administrativas com verificação de permissões
- 📊 **Catálogo de Produtos** - Visualização pública do catálogo de semi-jóias com filtros
- 💾 **Persistência de Dados** - Entity Framework Core com migrations automáticas
- 🎨 **Interface Responsiva** - Bootstrap para design moderno e compatível com dispositivos móveis
- ✔️ **Validação de Dados** - Validações no servidor e cliente (jQuery Validation)
- 📝 **Configuração por Perfil** - Suporte para ambientes Development e Production

---

## 🚀 Como Executar o Projeto

### Pré-requisitos

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download)
- **SQL Server** (ou LocalDB) - Incluído no Visual Studio
- **Git** - Para clonar o repositório

### Passo 1: Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/vitrine-semi-joias.git
cd vitrine-semi-joias
```

### Passo 2: Restaurar Dependências

```bash
dotnet restore
```

### Passo 3: Aplicar Migrations do Banco de Dados

```bash
dotnet ef database update
```

Se receber erro sobre EF Core não estar instalado:
```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

### Passo 4: Executar a Aplicação

```bash
dotnet run
```

A aplicação estará disponível em: `https://localhost:7000` ou `http://localhost:5000`

### Desenvolvimento com Watch Mode (Opcional)

Para reiniciar automaticamente ao salvar mudanças:

```bash
dotnet watch run
```

---

## 📁 Estrutura de Pastas

```
vitrine-semi-joias/
├── Controllers/          # Controladores MVC
├── Models/              # Modelos de domínio
├── DTOs/                # Data Transfer Objects
├── ViewModels/          # Modelos para Views
├── Services/            # Lógica de negócio
├── Repository/          # Padrão Repository
├── Data/                # Configuração do DbContext e Entidades
├── Migrations/          # Migrations do Entity Framework
├── Views/               # Templates Razor
├── wwwroot/             # Arquivos estáticos (CSS, JS, imagens)
├── Common/              # Utilitários compartilhados
├── Configurations/      # Configurações de DI e AutoMapper
├── Enums/               # Enumerações
└── Properties/          # Configurações do projeto
```

---

## 🔑 Credenciais Padrão

Uma conta administrador padrão é criada automaticamente nas migrations:

- **Email**: `admin@vitrine.com`
- **Senha**: `Admin@123` (alterar na primeira execução)

---

## 📝 Notas Importantes

- ✅ Certifique-se de que a string de conexão em `appsettings.json` está configurada corretamente
- ✅ As imagens carregadas são armazenadas em `wwwroot/img/`
- ✅ Configure as variáveis de ambiente para segurança em produção
- ✅ O projeto usa HashAlgorithm BCrypt para segurança de senhas

---

## 📚 Tecnologias Utilizadas

| Tecnologia | Descrição |
|-----------|-----------|
| **.NET 8** | Framework principal para desenvolvimento web |
| **ASP.NET Core MVC** | Padrão arquitetural Model-View-Controller |
| **Entity Framework Core** | ORM para acesso a dados |
| **SQL Server** | Banco de dados relacional |
| **AutoMapper** | Mapeamento automático entre objetos |
| **Bootstrap 5** | Framework CSS responsivo |
| **jQuery** | Biblioteca JavaScript |
| **BCrypt** | Hashing seguro de senhas |

---

## 🤝 Contribuições

Contribuições são bem-vindas! Por favor, faça um fork do projeto e envie um pull request com suas melhorias.

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

---

## 📧 Contato

Para dúvidas ou sugestões, entre em contato através das issues do repositório.