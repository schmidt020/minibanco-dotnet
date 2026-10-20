namespace MiniBanco.ConsoleApp.Models;

public class ContaPoupanca : Conta
{
    public double TaxaRendimentoMensal { get; }
    public int DiaAniversario { get; }

    public ContaPoupanca(string titular, int numero, decimal saldo, double taxaRendimentoMensal, int diaAniversario)
        : base(titular, numero, saldo)
    {
        this.TaxaRendimentoMensal = taxaRendimentoMensal;
        this.DiaAniversario = diaAniversario;
    }

    public override void AplicarFechamentoMensal()
    {
        decimal rendimento = Math.Round(Saldo * (decimal)TaxaRendimentoMensal / 100, 2);

        if (rendimento > 0)
        {
            Depositar(rendimento, "Rendimento mensal");
        }
    }
}