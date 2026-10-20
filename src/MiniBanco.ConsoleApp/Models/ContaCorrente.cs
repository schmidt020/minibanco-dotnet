using MiniBanco.ConsoleApp.Exceptions;

namespace MiniBanco.ConsoleApp.Models;

public class ContaCorrente : Conta
{
    private const decimal TarifaManutencao = 10.00m;

    public decimal LimiteChequeEspecial { get; }

    public ContaCorrente(string titular, int numero, decimal saldo, decimal limiteChequeEspecial)
        : base(titular, numero, saldo)
    {
        this.LimiteChequeEspecial = limiteChequeEspecial;
    }

    public override void Sacar(decimal valor)
    {
        if (valor > Saldo + LimiteChequeEspecial)
        {
            throw new SaldoInsuficienteException("Saque acima do saldo somado ao limite do cheque especial.");
        }

        RegistrarTransacao("Saque", "Saque", valor);
        Saldo -= valor;
    }

    public override void AplicarFechamentoMensal()
    {
        RegistrarTransacao("Tarifa de manutenção", "Tarifa", TarifaManutencao);
        Saldo -= TarifaManutencao;
    }
}