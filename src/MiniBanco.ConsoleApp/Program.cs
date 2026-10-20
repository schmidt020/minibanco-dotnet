using System.Globalization;
using System.Text;
using MiniBanco.ConsoleApp.Exceptions;
using MiniBanco.ConsoleApp.Models;
using MiniBanco.ConsoleApp.Services;

namespace MiniBanco.ConsoleApp;

public class Program
{
    private const string LinhaDupla = "====================================================================";
    private const string LinhaSimples = "--------------------------------------------------------------------";

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

        List<Conta> contas = new List<Conta>();
        string opcao = "";

        while (opcao != "0")
        {
            ExibirMenu();
            opcao = LerTexto("Escolha uma opção: ");

            try
            {
                switch (opcao)
                {
                    case "1":
                        AbrirConta(contas);
                        break;
                    case "2":
                        RegistrarMovimentacao(contas);
                        break;
                    case "3":
                        AplicarFechamento(contas);
                        break;
                    case "4":
                        ExibirRelatorioGeral(contas);
                        break;
                    case "0":
                        Console.WriteLine("Encerrando o MiniBanco. Até logo!");
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Escolha 1, 2, 3, 4 ou 0.");
                        break;
                }
            }
            catch (SaldoInsuficienteException ex)
            {
                Console.WriteLine($"Operação não realizada: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Dado inválido: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Entrada inválida: digite um número no formato correto (exemplo: 1500,50).");
            }
        }
    }

    private static void ExibirMenu()
    {
        Console.WriteLine();
        Console.WriteLine(LinhaDupla);
        Console.WriteLine("        MINIBANCO .NET - MENU PRINCIPAL");
        Console.WriteLine(LinhaDupla);
        Console.WriteLine("1 - Abrir Nova Conta");
        Console.WriteLine("2 - Registrar Movimentação");
        Console.WriteLine("3 - Aplicar Fechamento Mensal");
        Console.WriteLine("4 - Exibir Relatório das Contas");
        Console.WriteLine("0 - Sair");
        Console.WriteLine(LinhaSimples);
    }

    private static string LerTexto(string mensagem)
    {
        Console.Write(mensagem);
        return Console.ReadLine() ?? "";
    }

    private static int LerInt(string mensagem)
    {
        return int.Parse(LerTexto(mensagem));
    }

    private static decimal LerDecimal(string mensagem)
    {
        return decimal.Parse(LerTexto(mensagem));
    }

    private static double LerDouble(string mensagem)
    {
        return double.Parse(LerTexto(mensagem));
    }

    private static Conta? BuscarConta(List<Conta> contas, int numero)
    {
        for (int i = 0; i < contas.Count; i++)
        {
            if (contas[i].Numero == numero)
            {
                return contas[i];
            }
        }

        return null;
    }

    private static void AbrirConta(List<Conta> contas)
    {
        Console.WriteLine("Tipo de conta:");
        Console.WriteLine("1 - Conta Corrente");
        Console.WriteLine("2 - Conta Poupança");
        Console.WriteLine("3 - Conta Salário");
        string tipo = LerTexto("Escolha o tipo: ");

        if (tipo != "1" && tipo != "2" && tipo != "3")
        {
            Console.WriteLine("Tipo de conta inválido.");
            return;
        }

        string titular = LerTexto("Titular: ");

        if (string.IsNullOrWhiteSpace(titular))
        {
            throw new ArgumentException("O nome do titular não pode ficar vazio.");
        }

        int numero = LerInt("Número da conta: ");

        if (BuscarConta(contas, numero) != null)
        {
            throw new ArgumentException("Já existe uma conta com esse número.");
        }

        decimal saldoInicial = LerDecimal("Saldo inicial: R$ ");

        if (saldoInicial < 0)
        {
            throw new ArgumentException("O saldo inicial não pode ser negativo.");
        }

        Conta novaConta;

        if (tipo == "1")
        {
            decimal limite = LerDecimal("Limite do cheque especial: R$ ");
            novaConta = new ContaCorrente(titular, numero, saldoInicial, limite);
        }
        else if (tipo == "2")
        {
            double taxa = LerDouble("Taxa de rendimento mensal (%): ");
            int diaAniversario = LerInt("Dia do aniversário: ");
            novaConta = new ContaPoupanca(titular, numero, saldoInicial, taxa, diaAniversario);
        }
        else
        {
            string empregador = LerTexto("Empregador: ");
            int limiteSaques = LerInt("Limite de saques mensais: ");
            novaConta = new ContaSalario(titular, numero, saldoInicial, empregador, limiteSaques);
        }

        contas.Add(novaConta);
        Console.WriteLine($"Conta {numero} aberta com sucesso para {titular}.");
    }

    private static void RegistrarMovimentacao(List<Conta> contas)
    {
        int numero = LerInt("Número da conta: ");
        Conta? conta = BuscarConta(contas, numero);

        if (conta == null)
        {
            Console.WriteLine("Conta não encontrada.");
            return;
        }

        Console.WriteLine("Operação:");
        Console.WriteLine("1 - Depósito");
        Console.WriteLine("2 - Saque");
        string operacao = LerTexto("Escolha a operação: ");

        if (operacao != "1" && operacao != "2")
        {
            Console.WriteLine("Operação inválida.");
            return;
        }

        decimal valor = LerDecimal("Valor: R$ ");

        if (operacao == "1")
        {
            string descricao = LerTexto("Descrição do depósito: ");

            if (string.IsNullOrWhiteSpace(descricao))
            {
                descricao = "Depósito";
            }

            conta.Depositar(valor, descricao);
            Console.WriteLine($"Depósito realizado. Saldo atual: R$ {conta.Saldo:F2}");
        }
        else
        {
            conta.Sacar(valor);
            Console.WriteLine($"Saque realizado. Saldo atual: R$ {conta.Saldo:F2}");
        }
    }

    private static void AplicarFechamento(List<Conta> contas)
    {
        if (contas.Count == 0)
        {
            Console.WriteLine("Nenhuma conta aberta até o momento.");
            return;
        }

        foreach (Conta conta in contas)
        {
            conta.AplicarFechamentoMensal();
        }

        Console.WriteLine($"Fechamento mensal aplicado em {contas.Count} conta(s).");
    }

    private static void ExibirRelatorioGeral(List<Conta> contas)
    {
        if (contas.Count == 0)
        {
            Console.WriteLine("Nenhuma conta aberta até o momento.");
            return;
        }

        Console.WriteLine(LinhaDupla);
        Console.WriteLine("        MINIBANCO .NET - MOTOR DE GESTÃO DE CONTAS");
        Console.WriteLine(LinhaDupla);

        for (int i = 0; i < contas.Count; i++)
        {
            Console.WriteLine();
            Console.WriteLine($"[{i + 1}/{contas.Count}] PROCESSANDO CONTA");
            Console.WriteLine(LinhaSimples);
            ExibirRelatorioConta(contas[i]);
            Console.WriteLine(LinhaSimples);
        }

        Console.WriteLine();
        Console.WriteLine(LinhaDupla);
        Console.WriteLine("        FIM DO PROCESSAMENTO DE CONTAS");
        Console.WriteLine(LinhaDupla);
    }

    private static void ExibirRelatorioConta(Conta conta)
    {
        Console.WriteLine("> DADOS DA CONTA:");
        Console.WriteLine($"  - Tipo: {ObterNomeTipo(conta)}");
        Console.WriteLine($"  - Titular: {conta.Titular} | Conta: {conta.Numero}");
        Console.WriteLine($"  - Atributo Específico: {ObterAtributoEspecifico(conta)}");
        Console.WriteLine($"  - Saldo Atual: R$ {conta.Saldo:F2}");
        Console.WriteLine();

        Console.WriteLine($"> MOVIMENTAÇÕES REGISTRADAS ({conta.Extrato.Count} TRANSAÇÕES):");

        foreach (Transacao transacao in conta.Extrato)
        {
            string marcador = ObterMarcador(transacao.Tipo);
            string descricao = (transacao.Descricao + " ").PadRight(30, '-');
            int pontos = MotorAnalise.ObterPontuacao(transacao.Tipo);

            Console.WriteLine($"  {marcador} {descricao} {transacao.Tipo}: R$ {transacao.Valor:F2} ({pontos} pts)");
        }

        Console.WriteLine();

        int obtida = MotorAnalise.CalcularPontuacaoObtida(conta);
        int maxima = MotorAnalise.CalcularPontuacaoMaxima(conta);
        double percentual = MotorAnalise.CalcularPercentual(conta);
        string classificacao = MotorAnalise.Classificar(percentual);
        string acao = MotorAnalise.ObterAcaoCorporativa(percentual);

        Console.WriteLine("> RESUMO DA PONTUAÇÃO:");
        Console.WriteLine($"  - Pontuação Atingida: {obtida} de {maxima} pontos possíveis");
        Console.WriteLine($"  - Índice de Saúde: {percentual:F1}%");
        Console.WriteLine($"  - Classificação Final: [ {classificacao.ToUpper()} ]");
        Console.WriteLine($"  - Ação Corporativa: {acao}");
        Console.WriteLine();

        Console.WriteLine("> RELATÓRIO DE PENDÊNCIAS E RECOMENDAÇÕES DA GERÊNCIA:");
        ExibirPendencias(conta);
    }

    private static void ExibirPendencias(Conta conta)
    {
        List<Transacao> tarifas = MotorAnalise.ListarPorTipo(conta, "Tarifa");
        List<Transacao> saques = MotorAnalise.ListarPorTipo(conta, "Saque");
        List<string> recomendacoes = MotorAnalise.GerarRecomendacoes(conta);

        if (tarifas.Count == 0 && saques.Count == 0)
        {
            foreach (string recomendacao in recomendacoes)
            {
                Console.WriteLine($"  🟢 {recomendacao}");
            }

            return;
        }

        if (tarifas.Count > 0)
        {
            Console.WriteLine("  🔴 ITENS CRÍTICOS (TARIFAS):");

            foreach (Transacao tarifa in tarifas)
            {
                Console.WriteLine($"    - {tarifa.Descricao} (R$ {tarifa.Valor:F2}): avaliar isenção ou renegociação com o cliente.");
            }

            Console.WriteLine();
        }

        if (saques.Count > 0)
        {
            Console.WriteLine("  🟡 ITENS DE ATENÇÃO (SAQUES):");

            foreach (Transacao saque in saques)
            {
                Console.WriteLine($"    - Saque de R$ {saque.Valor:F2}: acompanhar a retirada.");
            }

            Console.WriteLine();
        }

        Console.WriteLine("  RECOMENDAÇÕES DA GERÊNCIA:");

        foreach (string recomendacao in recomendacoes)
        {
            Console.WriteLine($"    - {recomendacao}");
        }
    }

    private static string ObterNomeTipo(Conta conta)
    {
        if (conta is ContaCorrente)
        {
            return "Conta Corrente";
        }

        if (conta is ContaPoupanca)
        {
            return "Conta Poupança";
        }

        if (conta is ContaSalario)
        {
            return "Conta Salário";
        }

        return "Conta";
    }

    private static string ObterAtributoEspecifico(Conta conta)
    {
        if (conta is ContaCorrente corrente)
        {
            return $"Limite Cheque Especial: R$ {corrente.LimiteChequeEspecial:F2}";
        }

        if (conta is ContaPoupanca poupanca)
        {
            return $"Rendimento: {poupanca.TaxaRendimentoMensal}% ao mês | Aniversário: dia {poupanca.DiaAniversario}";
        }

        if (conta is ContaSalario salario)
        {
            return $"Empregador: {salario.Empregador} | Limite de saques mensais: {salario.LimiteSaquesMensais}";
        }

        return "-";
    }

    private static string ObterMarcador(string tipo)
    {
        switch (tipo)
        {
            case "Deposito":
                return "[OK]";
            case "Saque":
                return "[ ! ]";
            case "Tarifa":
                return "[ X ]";
            default:
                return "[ ? ]";
        }
    }
}