namespace MiniBanco.ConsoleApp.Models;

public class Transacao
{
    public string Descricao { get; }
    public string Tipo { get; }
    public decimal Valor { get; }

    public Transacao(string descricao, string tipo, decimal valor)
    {
        if (tipo != "Deposito" && tipo != "Saque" && tipo != "Tarifa")
        {
            throw new ArgumentException("Tipo de transacao invalido. Use Deposito, Saque ou Tarifa.");
        }

        if (valor <= 0)
        {
            throw new ArgumentException("O valor da transacao deve ser maior que zero.");
        }

        Descricao = descricao;
        Tipo = tipo;
        Valor = valor;
    }
}