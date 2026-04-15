using System;

// REQUISITO: Interface base para garantir a substituição e o polimorfismo
public interface IBebida
{
    string GetDescricao();
    double GetCusto();
}

// REQUISITO: Classes Base (Componentes Concretos)
public class Expresso : IBebida
{
    public string GetDescricao() => "Café Expresso";
    public double GetCusto() => 5.00;
}

public class Cappuccino : IBebida
{
    public string GetDescricao() => "Cappuccino";
    public double GetCusto() => 8.50;
}

public class Cha : IBebida
{
    public string GetDescricao() => "Chá";
    public double GetCusto() => 4.00;
}

// REQUISITO: Padrão Decorator utilizado para adicionar responsabilidades dinamicamente
// Esta classe base do decorador mantém uma referência (composição) para um objeto IBebida.
public abstract class BebidaDecorator : IBebida
{
    protected IBebida _bebida;

    public BebidaDecorator(IBebida bebida)
    {
        _bebida = bebida;
    }

    // Delega a chamada inicial para o objeto que está sendo decorado
    public virtual string GetDescricao() => _bebida.GetDescricao();
    public virtual double GetCusto() => _bebida.GetCusto();
}

// REQUISITO: Demonstrar flexibilidade e extensibilidade na composição
// Adicionais são implementados como decoradores concretos.
public class Leite : BebidaDecorator
{
    public Leite(IBebida bebida) : base(bebida) { }

    // REQUISITO: Calcula corretamente a descrição final e seu custo total (composição de valores)
    public override string GetDescricao() => _bebida.GetDescricao() + ", Leite";
    public override double GetCusto() => _bebida.GetCusto() + 1.50;
}

public class Chantilly : BebidaDecorator
{
    public Chantilly(IBebida bebida) : base(bebida) { }

    public override string GetDescricao() => _bebida.GetDescricao() + ", Chantilly";
    public override double GetCusto() => _bebida.GetCusto() + 2.00;
}

public class Canela : BebidaDecorator
{
    public Canela(IBebida bebida) : base(bebida) { }

    public override string GetDescricao() => _bebida.GetDescricao() + ", Canela";
    public override double GetCusto() => _bebida.GetCusto() + 0.50;
}

public class CaldaChocolate : BebidaDecorator
{
    public CaldaChocolate(IBebida bebida) : base(bebida) { }

    public override string GetDescricao() => _bebida.GetDescricao() + ", Calda de Chocolate";
    public override double GetCusto() => _bebida.GetCusto() + 1.20;
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("--- Sistema de Cafeteria (Padrão Decorator) ---\n");

        // REQUISITO: "O cliente deve poder montar bebidas personalizadas de forma dinâmica"
        // REQUISITO: "Permita combinar múltiplos adicionais sem alterar as classes base"

        // Caso 1: Uma bebida simples com um adicional
        IBebida pedido1 = new Expresso();
        pedido1 = new Leite(pedido1);

        Console.WriteLine($"Pedido 1: {pedido1.GetDescricao()}");
        Console.WriteLine($"Custo Total: R$ {pedido1.GetCusto():F2}\n");

        // REQUISITO: "Sem explosão no número de classes" 
        // Note que combinou 3 níveis sem precisar de uma classe 'CappuccinoComChantillyECalda'
        IBebida pedido2 = new Cappuccino();
        pedido2 = new Chantilly(pedido2);
        pedido2 = new CaldaChocolate(pedido2);
        pedido2 = new Canela(pedido2);

        Console.WriteLine($"Pedido 2: {pedido2.GetDescricao()}");
        Console.WriteLine($"Custo Total: R$ {pedido2.GetCusto():F2}");
    }
}