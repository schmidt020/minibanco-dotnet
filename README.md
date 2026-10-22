# MiniBanco.ConsoleApp: Motor de Gestão de Contas Bancárias

Aplicação de console em C# que gerencia contas bancárias de diferentes tipos, registra movimentações, aplica o fechamento mensal, calcula o índice de saúde financeira de cada conta e emite um relatório com recomendações para a gerência.

## Tecnologias e conceitos

- C# e .NET (Console Application)
- Programação Orientada a Objetos: classes, encapsulamento, herança, `virtual`/`override` e polimorfismo
- Coleções com `List<T>` e laços tradicionais (`foreach` e `for`), sem LINQ
- Tratamento de exceções com `try`, `catch` e `throw`
- Versionamento com Git e GitHub

## Como executar

Pré-requisito: .NET SDK instalado.

```
git clone https://github.com/schmidt020/minibanco-dotnet.git
cd minibanco-dotnet
dotnet run --project src/MiniBanco.ConsoleApp
```

## Estrutura do projeto

```
minibanco-dotnet/
├── src/
│   └── MiniBanco.ConsoleApp/
│       ├── Program.cs
│       ├── Models/
│       │   ├── Transacao.cs
│       │   ├── Conta.cs
│       │   ├── ContaCorrente.cs
│       │   ├── ContaPoupanca.cs
│       │   └── ContaSalario.cs
│       ├── Exceptions/
│       │   └── SaldoInsuficienteException.cs
│       ├── Services/
│       │   └── MotorAnalise.cs
│       └── MiniBanco.ConsoleApp.csproj
└── README.md
```

## Menu principal

| Opção | Ação |
|---|---|
| 1 | Abrir nova conta (corrente, poupança ou salário) |
| 2 | Registrar movimentação (depósito ou saque) |
| 3 | Aplicar fechamento mensal em todas as contas |
| 4 | Exibir relatório das contas |
| 0 | Sair |

## Tipos de conta

| Tipo | Atributos próprios | Saque | Fechamento mensal |
|---|---|---|---|
| Conta Corrente | Limite de cheque especial | Permitido até saldo + limite | Tarifa de manutenção de R$ 10,00 |
| Conta Poupança | Taxa de rendimento mensal (%) e dia de aniversário | Até o saldo disponível | Depósito de rendimento (saldo × taxa / 100) |
| Conta Salário | Empregador e limite de saques mensais | Saques acima do limite geram tarifa de R$ 5,00 | Zera a contagem de saques do mês |

## Análise da conta

Cada transação recebe uma pontuação: Depósito vale 10 pontos, Saque vale 5 e Tarifa vale 0.

O índice de saúde é calculado por:

```
Percentual = (PontuaçãoObtida / PontuaçãoMáxima) × 100
```

A pontuação máxima é o total de transações multiplicado por 10.

| Percentual | Classificação | Ação |
|---|---|---|
| 90% a 100% | Conta Saudável | Oferecer investimentos e avaliar aumento de limite |
| 60% a 89% | Conta em Atenção | Contato da gerência para orientação financeira e revisão de tarifas |
| 0% a 59% | Conta em Risco | Bloquear aumento de limite e abrir análise de crédito |

## Exemplo de execução

Cenário usado: uma Conta Corrente (Ana Souza, saldo inicial de R$ 1000,00 e limite de R$ 500,00) e uma Conta Poupança (Bia Lima, saldo inicial de R$ 2000,00 e rendimento de 0,5% ao mês), com as movimentações cadastradas pelo menu e o fechamento mensal aplicado uma vez. Saída da opção 4 (Exibir Relatório das Contas):

```
====================================================================
        MINIBANCO .NET - MOTOR DE GESTÃO DE CONTAS
====================================================================

[1/2] PROCESSANDO CONTA
--------------------------------------------------------------------
> DADOS DA CONTA:
  - Tipo: Conta Corrente
  - Titular: Ana Souza | Conta: 1001
  - Atributo Específico: Limite Cheque Especial: R$ 500,00
  - Saldo Atual: R$ -310,00

> MOVIMENTAÇÕES REGISTRADAS (5 TRANSAÇÕES):
  [OK] Salário ---------------------- Deposito: R$ 2000,00 (10 pts)
  [OK] Transferência recebida ------- Deposito: R$ 300,00 (10 pts)
  [ ! ] Saque ------------------------ Saque: R$ 800,00 (5 pts)
  [ ! ] Saque ------------------------ Saque: R$ 2800,00 (5 pts)
  [ X ] Tarifa de manutenção ---------- Tarifa: R$ 10,00 (0 pts)

> RESUMO DA PONTUAÇÃO:
  - Pontuação Atingida: 30 de 50 pontos possíveis
  - Índice de Saúde: 60,0%
  - Classificação Final: [ CONTA EM ATENÇÃO ]
  - Ação Corporativa: Contato da gerência para orientação financeira e revisão de tarifas.

> RELATÓRIO DE PENDÊNCIAS E RECOMENDAÇÕES DA GERÊNCIA:
  🔴 ITENS CRÍTICOS (TARIFAS):
    - Tarifa de manutenção: Avaliar isenção ou migração para pacote de serviços.

  🟡 ITENS DE ATENÇÃO (SAQUES):
    - Saque de R$ 2800,00: Conta usando limite do cheque especial. Entrar em contato com o cliente.
--------------------------------------------------------------------

[2/2] PROCESSANDO CONTA
--------------------------------------------------------------------
> DADOS DA CONTA:
  - Tipo: Conta Poupança
  - Titular: Bia Lima | Conta: 2001
  - Atributo Específico: Rendimento: 0,5% ao mês | Aniversário: dia 10
  - Saldo Atual: R$ 2814,00

> MOVIMENTAÇÕES REGISTRADAS (3 TRANSAÇÕES):
  [OK] Depósito em dinheiro --------- Deposito: R$ 500,00 (10 pts)
  [OK] Transferência recebida ------- Deposito: R$ 300,00 (10 pts)
  [OK] Rendimento mensal ------------ Deposito: R$ 14,00 (10 pts)

> RESUMO DA PONTUAÇÃO:
  - Pontuação Atingida: 30 de 30 pontos possíveis
  - Índice de Saúde: 100,0%
  - Classificação Final: [ CONTA SAUDÁVEL ]
  - Ação Corporativa: Oferecer investimentos e avaliar aumento de limite.

> RELATÓRIO DE PENDÊNCIAS E RECOMENDAÇÕES DA GERÊNCIA:
  🟢 Nenhuma pendência identificada. Oferecer produtos de investimento ao cliente.
--------------------------------------------------------------------

====================================================================
        FIM DO PROCESSAMENTO DE CONTAS
====================================================================
```
## Tratamento de exceções

O menu trata os erros sem encerrar o programa:

- `SaldoInsuficienteException`: saque acima do permitido
- `ArgumentException`: valor ou tipo de transação inválido
- `FormatException`: texto digitado onde se esperava um número