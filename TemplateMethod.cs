using System;
using System.Collections.Generic;

// classe base abstrata que define o esqueleto do algoritmo
abstract class SortPalavras{
    // esse eh o template method, nao mexe nele nas subclasses
    public void rodar(){
        List<string> lista = pegarPalavras();
        organizar(lista);
        mostrar(lista);
    }

    // palavras fixas, poderia vir de um arquivo mas ta bom por enquanto
    protected List<string> pegarPalavras(){
        return new List<string> { "nicolas", "marcelo", "estevam", "de", "Jesus", "Cabanas", "nome" };
    }

    // cada subclasse implementa do seu jeito
    protected abstract void organizar(List<string> lst);

    // so printa, nao muda
    protected void mostrar(List<string> lst){
        Console.WriteLine(GetType().Name + " => " + string.Join(" , ", lst));
    }
}

// ordena normal alfabetico basico
class SortNormal : SortPalavras{
    protected override void organizar(List<string> lst){
        lst.Sort(); // sort padrao mesmo
    }
}

// ordena pela ultima letra de cada palavra
class SortUltimaLetra : SortPalavras{
    protected override void organizar(List<string> lst){
        // pega o ultimo char de cada string e compara
        lst.Sort((x, y) => x[x.Length - 1].CompareTo(y[y.Length - 1]));
    }
}

// ordena pelo tamnanho da string (menor pra maior)
class SortTamanho : SortPalavras{
    protected override void organizar(List<string> lst){
        lst.Sort((x, y) => x.Length - y.Length); // diferenca de tamanho ja serve de comparador
    }
}

class Program{
    static void Main(string[] args){
        // testa os tres tipos
        SortPalavras[] arr = { new SortNormal(), new SortUltimaLetra(), new SortTamanho() };

        foreach (SortPalavras s in arr)
            s.rodar();
    }
}