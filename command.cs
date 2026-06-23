using System;
using System.Collections.Generic;

public class Pessoa
{
    public int Id
    {
        get; 
        set;
    }
    public string Nome
    {
        get; 
        set;
    }

    // CONSTRUTOR
    // quando o seu NewCommand for executado, 
    // ele vai fazer uma chamada do tipo new Pessoa(1, "Fulano")
    public Pessoa (int id, string nome)
    {
        Id = id;
        Nome = nome;
    }
    // => - formata a informacao e devolve a string
    // deixa a info pronta para ser impressa
    public override string ToString() => $"(ID: {Id}) {Nome}";
}

// RECEPTOR
// tem o conhecimento sobre inserir, deletar, buscar, etc, pessoas
public class BancoPessoas
{
    // Chave eh um int (ID da pessoa) 
    // e o Valor eh o objeto Pessoa inteiro
    // 
    // O private garante que ninguem fora dessa classe consiga baguncar a lista
    private Dictionary<int, Pessoa> banco = new Dictionary <int, Pessoa>();

    // adiciona uma pessoa nova
    public void Inserir(Pessoa p)
    {
        banco[p.Id] = p;
        Console.WriteLine($"Pessoa Inserida: {p}");
    }

    public void Deletar(int id)
    {
        // O metodo .Remove(id) do dicionario faz duas coisas ao mesmo tempo: 
        // tenta deletar a chave 
        // e retorna um valor booleano (true se conseguiu deletar, false se a chave nao existia)
        if (banco.Remove(id)){
            Console.WriteLine($"Pessoa ID: {id} deletada!");
        }
        else {
            Console.WriteLine($"ERRO ao deletar ID: {id}. Pessoa não encontrada.");
        }
    }

    public void ListarTodos()
    {
        Console.WriteLine($"\n -- Pessoas Presentes no Banco de Dados --");
        foreach(var p in banco.Values)
        {
            Console.WriteLine(p);
        }
    }

    public void Buscar(int id)
    {
        // o metodo TryGetValue vai procurar a chave
        // se ele encontrar, ele joga o resultado diretamente dentro dessa nova variavel Pessoa p,
        // se nao achar, erro
        if (banco.TryGetValue(id, out Pessoa p))
            Console.WriteLine($"Pessoa encontrada: {p}");
        else
            Console.WriteLine($"-> Erro: Pessoa {id} não encontrada.");
    }
}



public interface ICommand
{
    void Execute (Object arg);
}

public class NewCommand : ICommand
{
    private BancoPessoas db;
    public NewCommand (BancoPessoas db)
    {
        this.db = db;
    }

    public void Execute(Object arg)
    {
        Object[] args = (Object[])arg;
        int id = (int)args[0];
        string nome = (string)args[1];

        db.Inserir(new Pessoa(id, nome));
    }
}

public class DeleteCommand : ICommand
{
    private BancoPessoas db;
    public DeleteCommand (BancoPessoas db)
    {
        this.db = db;
    }

    public void Execute(Object arg)
    {
        int id = (int)arg;
        db.Deletar(id);
    }
}

public class AllCommand : ICommand{
    private BancoPessoas db;
    public AllCommand(BancoPessoas db) => this.db = db;

    public void Execute(object arg)
    {
        db.ListarTodos();
    }

}

public class GetCommand : ICommand{
    private BancoPessoas db;
    public GetCommand(BancoPessoas db) => this.db = db; 

    public void Execute(object arg){
        int id = (int)arg;
        db.Buscar(id);
    }
}

//Invoker
public class Executor{
    private BancoPessoas db = new BancoPessoas();
    private Dictionary<string, ICommand> cmds = new Dictionary<string, ICommand>();
    public Executor() {
        // aq registra os comandos do pool
        cmds["new"] = new NewCommand(db);
        cmds["delete"] = new DeleteCommand(db);
        cmds["all"] = new AllCommand(db);
        cmds["get"] = new GetCommand(db);
    }
    public void Servico(string cmd, object arg = null) //mêtodo que recebe o pedido da main e aciona o comando correto
    {
        if (cmds.TryGetValue(cmd.ToLower(), out ICommand comando)){
            comando.Execute(arg);
        } 
        else{
            Console.WriteLine($"-> Comando '{cmd}' não reconhecido");
        }
    }
}
//CLIENTE
class Program{
    static void Main(string[] args){
        Executor executor = new Executor();
        Console.WriteLine("Iniciando operações via Main...\n");
        //criando pessoas (passamos os argumentos como um array de objetos)
        executor.Servico("new", new object[] { 1, "Hulk" });
        executor.Servico("new", new object[] { 2, "HOmem de Ferro" });
        executor.Servico("new", new object[] { 3, "Carlos" });
        executor.Servico("all");
        //buscando a pessoa de ID 2 
        executor.Servico("get", 2);
        //deletando a pessoa de ID 1
        executor.Servico("delete", 1);
        //listnado dnv pra ver se deletou
        executor.Servico("all");
    }
}

