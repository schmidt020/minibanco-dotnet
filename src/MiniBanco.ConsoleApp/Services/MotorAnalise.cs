using MiniBanco.ConsoleApp.Models;

namespace MiniBanco.ConsoleApp.Services;

public static class MotorAnalise
{
    private const int PontuacaoMaximaPorTransacao = 10;

    public static int ObterPontuacao(string tipo)
    {
        switch (tipo)
        {
            case "Deposito":
                return 10;
            case "Saque":
                return 5;
            case "Tarifa":
                return 0;
            default:
                return 0;
        }
    }

    public static int CalcularPontuacaoObtida(Conta conta)
    {
        int total = 0;

        foreach (Transacao transacao in conta.Extrato)
        {
            total += ObterPontuacao(transacao.Tipo);
        }

        return total;
    }

    public static int CalcularPontuacaoMaxima(Conta conta)
    {
        int total = 0;

        for (int i = 0; i < conta.Extrato.Count; i++)
        {
            total += PontuacaoMaximaPorTransacao;
        }

        return total;
    }

    public static double CalcularPercentual(Conta conta)
    {
        int obtida = CalcularPontuacaoObtida(conta);
        int maxima = CalcularPontuacaoMaxima(conta);

        if (maxima == 0)
        {
            return 0;
        }

        return (double)obtida / maxima * 100;
    }

    public static string Classificar(double percentual)
    {
        if (percentual >= 90)
        {
            return "Conta Saudável";
        }
        else if (percentual >= 60)
        {
            return "Conta em Atenção";
        }
        else
        {
            return "Conta em Risco";
        }
    }

    public static string ObterAcaoCorporativa(double percentual)
    {
        if (percentual >= 90)
        {
            return "Oferecer investimentos e avaliar aumento de limite.";
        }
        else if (percentual >= 60)
        {
            return "Contato da gerência para orientação financeira e revisão de tarifas.";
        }
        else
        {
            return "Bloquear aumento de limite e abrir análise de crédito.";
        }
    }

    public static List<Transacao> ListarPorTipo(Conta conta, string tipo)
    {
        List<Transacao> encontradas = new List<Transacao>();

        foreach (Transacao transacao in conta.Extrato)
        {
            if (transacao.Tipo == tipo)
            {
                encontradas.Add(transacao);
            }
        }

        return encontradas;
        
    }
      public static List<Transacao> ListarSaquesComChequeEspecial(Conta conta)
    {
        decimal saldo = conta.Saldo;

        foreach (Transacao transacao in conta.Extrato)
        {
            if (transacao.Tipo == "Deposito")
            {
                saldo -= transacao.Valor;
            }
            else
            {
                saldo += transacao.Valor;
            }
        }

        List<Transacao> encontradas = new List<Transacao>();

        foreach (Transacao transacao in conta.Extrato)
        {
            if (transacao.Tipo == "Deposito")
            {
                saldo += transacao.Valor;
            }
            else
            {
                saldo -= transacao.Valor;

                if (transacao.Tipo == "Saque" && saldo < 0)
                {
                    encontradas.Add(transacao);
                }
            }
        }

        return encontradas;
    }
    public static List<string> GerarRecomendacoes(Conta conta)
    {
        List<string> recomendacoes = new List<string>();

        int tarifas = ListarPorTipo(conta, "Tarifa").Count;
        int saques = ListarPorTipo(conta, "Saque").Count;

        if (tarifas > 0)
        {
            recomendacoes.Add("Revisar o pacote de tarifas e avaliar isenção ou migração para um pacote de serviços.");
        }

        if (saques > 0)
        {
            recomendacoes.Add("Acompanhar as retiradas registradas e entrar em contato com o cliente.");
        }

        if (conta.Saldo < 0)
        {
            recomendacoes.Add("Conta usando limite do cheque especial. Orientar o cliente e avaliar outro tipo de conta.");
        }

        if (recomendacoes.Count == 0)
        {
            recomendacoes.Add("Nenhuma pendência identificada. Oferecer produtos de investimento ao cliente.");
        }

        return recomendacoes;
    }
}