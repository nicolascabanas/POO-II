// AmazonForest.cs - v1: Sem Flyweight (problema de memória)
using System;
using System.Collections.Generic;
using System.Linq;

// Cada árvore carrega TODOS os dados — incluindo os repetidos por espécie
class Arvore
{
    // Estado intrínseco (poderia ser compartilhado, mas não é aqui)
    private string especie;
    private string cor;
    private string textura;
    private byte[] imagemCasca;     // ~500KB por árvore!
    private byte[] imagemFolhas;    // ~300KB por árvore!

    // Estado extrínseco (único por árvore)
    private double latitude;
    private double longitude;
    private double altura;
    private int idade;

    public Arvore(string especie, string cor, string textura,
                  double latitude, double longitude, double altura, int idade)
    {
        this.especie = especie;
        this.cor = cor;
        this.textura = textura;
        this.imagemCasca = new byte[500_000]; // simulando 500KB
        this.imagemFolhas = new byte[300_000]; // simulando 300KB
        this.latitude = latitude;
        this.longitude = longitude;
        this.altura = altura;
        this.idade = idade;
    }

    public long GetMemoriaBytes() =>
        imagemCasca.Length + imagemFolhas.Length
        + especie.Length + cor.Length + textura.Length
        + sizeof(double) * 3 + sizeof(int);

    public override string ToString() =>
        $"[{especie}] lat={latitude:F4} lon={longitude:F4} alt={altura:F1}m idade={idade}anos";
}

class FlorestaSemFlyweight
{
    private List<Arvore> arvores = new();

    public void PlantarArvore(string especie, string cor, string textura,
                               double lat, double lon, double alt, int idade)
        => arvores.Add(new Arvore(especie, cor, textura, lat, lon, alt, idade));

    public long CalcularMemoriaTotal() => arvores.Sum(a => a.GetMemoriaBytes());
    public int TotalArvores() => arvores.Count;
}

class AmazonForest
{
    static readonly string[] Especies = { "Castanheira", "Seringueira", "Açaizeiro", "Mogno", "Andiroba" };
    static readonly Random rng = new();

    static void Main()
    {
        int totalArvores = 100_000;

        Console.WriteLine("=== COMMIT 1: Sem Flyweight ===\n");

        var floresta = new FlorestaSemFlyweight();

        for (int i = 0; i < totalArvores; i++)
        {
            string esp = Especies[i % Especies.Length];
            floresta.PlantarArvore(
                esp, $"verde-{esp}", $"rugosa-{esp}",
                -3.0 + rng.NextDouble() * 6,
                -73.0 + rng.NextDouble() * 20,
                5 + rng.NextDouble() * 40,
                rng.Next(1, 200)
            );
        }

        long memoriaBytes = floresta.CalcularMemoriaTotal();
        Console.WriteLine($"Árvores plantadas : {floresta.TotalArvores():N0}");
        Console.WriteLine($"Memória total     : {memoriaBytes:N0} bytes");
        Console.WriteLine($"                  = {memoriaBytes / 1_000_000.0:F2} MB");
        Console.WriteLine($"Média por árvore  : {(double)memoriaBytes / floresta.TotalArvores():F0} bytes");
        Console.WriteLine("\n⚠️  Problema: cada árvore duplica imagens de casca e folhas!");
        Console.WriteLine("   Mesmo sendo da mesma espécie, nada é compartilhado.");
    }
}