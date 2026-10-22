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

## Tratamento de exceções

O menu trata os erros sem encerrar o programa:

- `SaldoInsuficienteException`: saque acima do permitido
- `ArgumentException`: valor ou tipo de transação inválido
- `FormatException`: texto digitado onde se esperava um número