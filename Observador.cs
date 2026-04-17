//utilizei IA para facilitar alguns passos, bem como entender a ideia geral do padrão e corrigir erros

using System;
using System.Collections.Generic;

namespace ExercicioAmazonia
{
    // Interface para os interessados (Observadores)
    public interface IObservador
    {
        void ReceberAviso(EntidadeMonitorada posto);
    }

    // Classe base para os postos (Sujeitos)
    public abstract class EntidadeMonitorada
    {
        protected List<IObservador> listaInteressados = new List<IObservador>();

        public void Registrar(IObservador obs)
        {
            if (!listaInteressados.Contains(obs))
                listaInteressados.Add(obs);
        }

        public void NotificarGeral()
        {
            foreach (var item in listaInteressados)
            {
                item.ReceberAviso(this);
            }
        }

        public List<IObservador> PegarInteressados() => listaInteressados;
    }

    // Posto de coleta nos estados
    public class PostoDados : EntidadeMonitorada
    {
        public string NomeEstado { get; set; }
        public double TemperaturaAgua { get; set; }
        public double Ph { get; set; }
        public double Umidade { get; set; }

        public PostoDados(string nome, double t, double p, double u)
        {
            NomeEstado = nome;
            TemperaturaAgua = t;
            Ph = p;
            Umidade = u;
        }

        public void AtualizarLeituras(double t, double p, double u)
        {
            this.TemperaturaAgua = t;
            this.Ph = p;
            this.Umidade = u;

            Console.WriteLine($"\n[SISTEMA] Alteração detectada no posto: {NomeEstado}");
            NotificarGeral();
        }

        public void ImprimirStatus()
        {
            Console.WriteLine($"Posto {NomeEstado.PadRight(10)} | Temp: {TemperaturaAgua}°C | pH: {Ph} | Umidade: {Umidade}%");
        }
    }

    
    public class Faculdade : IObservador
    {
        public string NomeFaculdade { get; set; }

        public Faculdade(string nome)
        {
            NomeFaculdade = nome;
        }

        public void ReceberAviso(EntidadeMonitorada entidade)
        {
           
            if (entidade is PostoDados p)
            {
                Console.WriteLine($"   -> {NomeFaculdade} avisa: Mudança em {p.NomeEstado}! Novos Dados: [T: {p.TemperaturaAgua}°C | pH: {p.Ph} | UR: {p.Umidade}%]");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Criando a lista de postos 
            var listaPostos = new List<PostoDados>();
            listaPostos.Add(new PostoDados("Pará", 30.0, 7.0, 80.0));
            listaPostos.Add(new PostoDados("Amazonas", 28.0, 6.5, 90.0));
            listaPostos.Add(new PostoDados("Acre", 26.0, 6.2, 85.0));
            listaPostos.Add(new PostoDados("Amapá", 29.5, 7.1, 82.0));
            listaPostos.Add(new PostoDados("Rondônia", 27.0, 6.8, 87.0));
            listaPostos.Add(new PostoDados("Roraima", 31.0, 7.0, 75.0));
            listaPostos.Add(new PostoDados("Tocantins", 33.0, 7.2, 45.0));

            // Instanciando faculdades
            var unifesp = new Faculdade("UNIFESP");
            var unb = new Faculdade("UnB");
            var ufrj = new Faculdade("UFRJ");
            var ufsc = new Faculdade("UFSC");
            var usp = new Faculdade("USP");

            // demonstrando os interesses
            listaPostos[1].Registrar(usp);      // Amazonas
            listaPostos[1].Registrar(unb);
            listaPostos[0].Registrar(ufrj);     // Pará
            listaPostos[0].Registrar(unifesp);
            listaPostos[2].Registrar(ufsc);     // Acre
            listaPostos[2].Registrar(usp);
            listaPostos[6].Registrar(unb);      // Tocantins

            // Saída inicial
            Console.WriteLine(" STATUS INICIAL DOS POSTOS NA AMAZÔNIA ");
            foreach (var p in listaPostos) p.ImprimirStatus();

            Console.WriteLine("\n MAPA DE INTERESSES (QUEM MONITORIA QUEM) ");
            foreach (var posto in listaPostos)
            {
                var interessados = posto.PegarInteressados();
                if (interessados.Count > 0)
                {
                    Console.Write($"O estado [{posto.NomeEstado}] é vigiado por: ");
                    foreach (Faculdade f in interessados) Console.Write($"{f.NomeFaculdade} ");
                    Console.WriteLine();
                }
            }
            Console.WriteLine("===============================================");

            // Simulando as mudanças nos postos
            listaPostos[1].AtualizarLeituras(29.2, 6.8, 94.0); // Amazonas
            listaPostos[0].AtualizarLeituras(32.1, 6.9, 78.0); // Pará
            listaPostos[6].AtualizarLeituras(35.5, 7.1, 35.0); // Tocantins

            Console.WriteLine("\nFim do monitoramento.");
        }
    }
}