![CI Pipeline](https://github.com/najumattos/vitrine-semi-joias/actions/workflows/ci.yml/badge.svg)

# :alembic: Testes

Este projeto adota uma estratégia de testes em duas camadas para equilibrar velocidade, isolamento e confiança na entrega.

Os testes unitários validam regras de negócio e serviços em isolamento, com dependências simuladas por `Moq`. Já os testes de integração sobem a aplicação em memória com `WebApplicationFactory`, interceptam o `DbContext` original e usam o `Entity Framework Core In-Memory Database` para executar cenários mais próximos do comportamento real do sistema, sem poluir o banco definitivo.

## Estratégia de Testes

### Testes Unitários

Os testes unitários são usados para verificar comportamentos específicos de serviços e regras de negócio sem depender de infraestrutura externa.

Neste projeto, eles são indicados para cenários como:

- validação de cálculos e regras do `CartService`;
- verificação de fluxos do `ProductService`;
- simulação de repositórios, serviços auxiliares e dependências de terceiros com `Moq`.

O objetivo é obter feedback rápido e preciso sobre falhas de lógica, com baixo custo de execução.

### Testes de Integração

Os testes de integração validam o fluxo completo entre aplicação, controllers, persistência e pipeline HTTP.

Nesta aplicação, eles utilizam:

- `WebApplicationFactory` para subir o servidor em memória;
- substituição do `DbContext` real por `UseInMemoryDatabase`;
- requisições HTTP reais de `GET` e `POST`;
- captura de `AntiforgeryToken` e cookies de sessão;
- `IAsyncLifetime` do xUnit para preparar e limpar o ambiente de teste.

Os fluxos de autenticação mais sensíveis, como **esqueci minha senha** e **redefinição de senha**, também são cobertos. Nesses cenários, o serviço de e-mail é substituído por mock usando `NSubstitute` e as asserções são feitas com `FluentAssertions` para garantir o comportamento fim a fim sem depender de SMTP real.

Esse nível de teste é mais próximo do uso real da aplicação e ajuda a validar a integração entre as camadas.

## Diferença de Escopo

Os dois tipos de teste se complementam, mas não têm o mesmo foco:

- **Unitários**: verificam uma unidade de código isolada, com dependências mockadas, priorizando velocidade e diagnóstico fino de falhas.
- **Integração**: verificam a comunicação entre componentes reais da aplicação, priorizando confiança no comportamento fim a fim dentro do processo de execução.

Na prática, os testes unitários protegem a lógica interna, enquanto os testes de integração confirmam que a aplicação continua funcionando corretamente quando suas partes interagem.

## Como Executar os Testes Localmente

Execute os comandos na raiz da solução, em um terminal do seu ambiente.

### 1. Rodar a suíte completa

```bash
dotnet test .\VitrineSemiJoias.sln
```

### 2. Rodar apenas os testes unitários

```bash
dotnet test .\Tests\VitrineSemiJoias.UnitTests\VitrineSemiJoias.UnitTests.csproj
```

### 3. Rodar apenas os testes de integração

```bash
dotnet test .\Tests\VitrineSemiJoias.IntegrationTests\VitrineSemiJoias.IntegrationTests.csproj
```

### 4. Rodar uma suíte específica com filtro

Quando quiser restringir a execução a uma classe ou método, use `--filter`:

```bash
dotnet test .\Tests\VitrineSemiJoias.UnitTests\VitrineSemiJoias.UnitTests.csproj --filter "FullyQualifiedName~ProductServiceTests"
```

```bash
dotnet test .\Tests\VitrineSemiJoias.IntegrationTests\VitrineSemiJoias.IntegrationTests.csproj --filter "FullyQualifiedName~AuthIntegrationTests"
```

## CI/CD

A qualidade do projeto é garantida automaticamente por uma esteira no GitHub Actions. Em cada Pull Request direcionado para `main`, o pipeline compila a solução e executa os testes em passos separados para unidades e integração.

Essa divisão é importante porque isola falhas por camada, reduz o tempo de diagnóstico e impede que mudanças incorretas cheguem à branch principal sem validação.
