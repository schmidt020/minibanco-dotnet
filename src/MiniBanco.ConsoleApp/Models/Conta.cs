using MiniBanco.ConsoleApp.Exceptions;

namespace MiniBanco.ConsoleApp.Models;

public abstract class Conta
{
    public string Titular { get; }
    public int Numero { get; }
    public decimal Saldo { get; protected set; }
    public List<Transacao> Extrato { get; }

    protected Conta(string titular, int numero, decimal saldo)
    {
        this.Titular = titular;
        this.Numero = numero;
        this.Saldo = saldo;
        this.Extrato = new List<Transacao>();
    }

    public void RegistrarTransacao(string descricao, string tipo, decimal valor)
    {
        Transacao transacao = new Transacao(descricao, tipo, valor);
        Extrato.Add(transacao);
    }

    public void Depositar(decimal valor, string descricao)
    {
        RegistrarTransacao(descricao, "Deposito", valor);
        Saldo += valor;
    }

    public virtual void Sacar(decimal valor)
    {
        if (valor > Saldo)
        {
            throw new SaldoInsuficienteException("Saldo insuficiente para realizar o saque.");
        }

        RegistrarTransacao("Saque", "Saque", valor);
        Saldo -= valor;
    }

    public virtual void AplicarFechamentoMensal()
    {
    }
}