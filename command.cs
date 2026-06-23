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
    public override string ToString() => $"ID: {Id} --> {Nome}";
}





public interface ICommand
{
    void Execute (Object arg);
}





public class NewCommand : ICommand
{
    private Database db;
    public NewCommand (Database db)
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
    private Database db;
    public DeleteCommand (Database db)
    {
        this.db = db;
    }

    public void Execute(Object arg)
    {
        int id = (int)arg;
        db.Deletar(id);
    }
}

public class Allcommand : ICommand{
    private Database db;
    public AllCommand(Database db) => this.db = db;

    public void Execute(object arg)
    {
        db.ListarTodos();
    }

}

public class GetCommand : ICommand{
    private Database db;
    public GetCommand(DataBase db) => this.db = db; 

    public void Execute(object arg){
        int id = (int)arg;
        db.Buscar(id);
    }
}
