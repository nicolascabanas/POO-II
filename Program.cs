//Utilizei IA para correção, comparação e deixar claro os conceitos para o meu entedimento

using System;

class DadosSistema
{
    public string nomeApp = "Notificador Escolar";
    public string urlServidor = "smtp.provedor.com";
    public int limiteEnvio = 5;

    private static DadosSistema unico;

    private DadosSistema() { }

    public static DadosSistema ObterInstancia()
    {
        if (unico == null)
        {
            unico = new DadosSistema();
        }
        return unico;
    }
}

interface IMensagem
{
    void Disparar(string conteudo);
}

class EnvioEmail : IMensagem
{
    public void Disparar(string conteudo)
    {
        Console.WriteLine("Via E-mail: " + conteudo);
    }
}

class EnvioSMS : IMensagem
{
    public void Disparar(string conteudo)
    {
        Console.WriteLine("Via SMS: " + conteudo);
    }
}

class EnvioPush : IMensagem
{
    public void Disparar(string conteudo)
    {
        Console.WriteLine("Via Push: " + conteudo);
    }
}

class CriadorAviso
{
    public static IMensagem Gerar(string opcao)
    {
        if (opcao == "1") return new EnvioEmail();
        if (opcao == "2") return new EnvioSMS();
        if (opcao == "3") return new EnvioPush();
        return null;
    }
}

class Program
{
    static void Main()
    {
        DadosSistema info = DadosSistema.ObterInstancia();
        Console.WriteLine("Sistema: " + info.nomeApp);

        IMensagem msg = CriadorAviso.Gerar("1");

        if (msg != null)
        {
            msg.Disparar("Mensagem de teste do meu exercicio.");
        }

        Console.WriteLine("\n--- Status dos Testes ---");

        DadosSistema info2 = DadosSistema.ObterInstancia();
        if (info == info2)
        {
            Console.WriteLine("Singleton OK: Mesma instancia encontrada.");
        }

        if (msg is EnvioEmail)
        {
            Console.WriteLine("Factory OK: Objeto de Email criado.");
        }
    }
}