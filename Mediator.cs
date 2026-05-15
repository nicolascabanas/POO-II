using System;
using System.Collections.Generic;

public interface ISalaChat
{
    void EnviarMensagem(string mensagem, Usuario remetente);
    void RegistrarUsuario(Usuario usuario);
}

public abstract class Usuario
{
    protected ISalaChat _mediador;
    public string Nome { get; private set; }

    public Usuario(string nome)
    {
        Nome = nome;
    }

    public void SetMediador(ISalaChat mediador)
    {
        _mediador = mediador;
    }

    public void Enviar(string mensagem)
    {
        _mediador.EnviarMensagem(mensagem, this);
    }

    public abstract void Receber(string mensagem, string deQuem);
}

public class Participante : Usuario
{
    public Participante(string nome) : base(nome) { }

    public override void Receber(string mensagem, string deQuem)
    {
        Console.WriteLine($"   [{Nome}] recebeu de {deQuem}: {mensagem}");
    }
}

public class ChatServer : ISalaChat
{
    private List<Usuario> _usuarios = new List<Usuario>();

    public void RegistrarUsuario(Usuario usuario)
    {
        _usuarios.Add(usuario);
        usuario.SetMediador(this);
    }

    public void EnviarMensagem(string mensagem, Usuario remetente)
    {
        foreach (var u in _usuarios)
        {
            if (u != remetente)
            {
                u.Receber(mensagem, remetente.Nome);
            }
        }
    }
}

class Program
{
    static void Main()
    {
        ChatServer servidor = new ChatServer();

        var alice = new Participante("Alice");
        var bob = new Participante("Bob");
        var caio = new Participante("Caio");

        servidor.RegistrarUsuario(alice);
        servidor.RegistrarUsuario(bob);
        servidor.RegistrarUsuario(caio);

        Participante[] participantes = { alice, bob, caio };

        while (true)
        {
            foreach (var user in participantes)
            {
                Console.Write($"\n{user.Nome}, digite: ");
                string msg = Console.ReadLine();

                if (msg?.ToLower() == "sair") return;

                if (!string.IsNullOrEmpty(msg))
                {
                    user.Enviar(msg);
                }
            }
        }
    }
}