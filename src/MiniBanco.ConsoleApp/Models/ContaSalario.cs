using MiniBanco.ConsoleApp.Exceptions;

namespace MiniBanco.ConsoleApp.Models;

public class ContaSalario : Conta
{
    private const decimal TarifaSaqueExcedente = 5.00m;

    private int saquesNoMes;

    public string Empregador { get; }
    public int LimiteSaquesMensais { get; }

    public ContaSalario(string titular, int numero, decimal saldo, string empregador, int limiteSaquesMensais)
        : base(titular, numero, saldo)
    {
        this.Empregador = empregador;
        this.LimiteSaquesMensais = limiteSaquesMensais;
        this.saquesNoMes = 0;
    }

    public override void Sacar(decimal valor)
    {
        bool cobraTarifa = saquesNoMes + 1 > LimiteSaquesMensais;
        decimal totalDoSaque = cobraTarifa ? valor + TarifaSaqueExcedente : valor;

        if (totalDoSaque > Saldo)
        {
            throw new SaldoInsuficienteException("Saldo insuficiente para o saque e a tarifa de saque excedente.");
        }

        base.Sacar(valor);
        saquesNoMes++;

        if (cobraTarifa)
        {
            RegistrarTransacao("Tarifa de saque excedente", "Tarifa", TarifaSaqueExcedente);
            Saldo -= TarifaSaqueExcedente;
        }
    }

    public override void AplicarFechamentoMensal()
    {
        saquesNoMes = 0;
    }
}