using System;
using System.Collections.Generic;

namespace ExercicioAmazonia
{
    // O que é o Callback: É basicamente um método que a gente cria, mas passa pra outro objeto executar.
    // Onde ele tá: É esse método ReceberAviso. 
    // Pra que funciona: Serve como um "telefone para contato". A faculdade implementa ele, 
    // mas quem vai usar (chamar) é o Posto quando quiser enviar os dados.
    public interface IObservador
    {
        void ReceberAviso(EntidadeMonitorada posto);
    }

    // Classe base que guarda a lista de quem quer receber os avisos
    public abstract class EntidadeMonitorada
    {
        protected List<IObservador> listaInteressados = new List<IObservador>();

        // É aqui que a faculdade passa o "contato" dela (o callback) pro Posto
        public void Registrar(IObservador obs)
        {
            if (!listaInteressados.Contains(obs))
                listaInteressados.Add(obs);
        }

        // Onde usei Inversão de Controle (IoC): Exatamente nesse método NotificarGeral.
        // Pra que serve: Serve pra tirar da Faculdade a obrigação de ficar toda hora perguntando "mudou algo?".
        // Relação com o código: O controle do fluxo inverteu. Não é o cliente (Faculdade) que puxa os dados. 
        // É o framework (EntidadeMonitorada) que toma o controle, roda o loop e empurra os dados chamando os callbacks.
        public void NotificarGeral()
        {
            foreach (var item in listaInteressados)
            {
                // PRINCÍPIO DE HOLLYWOOD AQUI: "Nós chamaremos você".
                // O posto está ativamente pegando a lista de contatos e "ligando de volta" 
                // para cada faculdade através do callback, em vez de deixar elas perguntarem.

                // Mudança após leitura do material (Estratégia PUSH): 
                // Em vez de fazer o observador ter que buscar o dado, eu já passo o 'this' (o posto inteiro) 
                // no callback. Assim a faculdade já recebe tudo de uma vez.
                item.ReceberAviso(this);
            }
        }

        public List<IObservador> PegarInteressados() => listaInteressados;
    }

    public class PostoDados : EntidadeMonitorada
    {
        // Mudança após leitura do material (Encapsulamento):
        // Coloquei 'private set' porque o material fala que o Modelo (Posto) deve proteger seu estado.
        // Ninguém de fora pode mudar essas variáveis direto, só usando o método AtualizarLeituras.
        public string NomeEstado { get; private set; }
        public double TemperaturaAgua { get; private set; }
        public double Ph { get; private set; }
        public double Umidade { get; private set; }

        public PostoDados(string nome, double t, double p, double u)
        {
            NomeEstado = nome;
            TemperaturaAgua = t;
            Ph = p;
            Umidade = u;
        }

        public void AtualizarLeituras(double t, double p, double u)
        {
            // Mudança após leitura do material (Consistência):
            // O material de Eventos diz que só devemos notificar se algo realmente mudar.
            // Então coloquei esse 'if' pra não ficar disparando a IoC à toa se os dados forem os mesmos.
            if (this.TemperaturaAgua != t || this.Ph != p || this.Umidade != u)
            {
                this.TemperaturaAgua = t;
                this.Ph = p;
                this.Umidade = u;

                Console.WriteLine($"\n[SISTEMA] Alteração detectada no posto: {NomeEstado}");

                // Dispara a Inversão de Controle
                NotificarGeral();
            }
        }

        public void ImprimirStatus()
        {
            Console.WriteLine($"Posto {NomeEstado.PadRight(10)} | Temp: {TemperaturaAgua}°C | pH: {Ph} | Umidade: {Umidade}%");
        }
    }

    public class Faculdade : IObservador
    {
        public string NomeFaculdade { get; private set; }

        public Faculdade(string nome)
        {
            NomeFaculdade = nome;
        }

        // Aqui é a Faculdade implementando o Callback.
        // PRINCÍPIO DE HOLLYWOOD AQUI: "Não nos ligue". 
        // Note que não tem nenhum método aqui do tipo "VerificarTemperaturaDoPosto()". 
        // A faculdade fica 100% passiva, só de boa esperando o Posto decidir chamar esse método.
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
            var listaPostos = new List<PostoDados>();
            listaPostos.Add(new PostoDados("Pará", 30.0, 7.0, 80.0));
            listaPostos.Add(new PostoDados("Amazonas", 28.0, 6.5, 90.0));
            listaPostos.Add(new PostoDados("Acre", 26.0, 6.2, 85.0));
            listaPostos.Add(new PostoDados("Amapá", 29.5, 7.1, 82.0));
            listaPostos.Add(new PostoDados("Rondônia", 27.0, 6.8, 87.0));
            listaPostos.Add(new PostoDados("Roraima", 31.0, 7.0, 75.0));
            listaPostos.Add(new PostoDados("Tocantins", 33.0, 7.2, 45.0));

            var unifesp = new Faculdade("UNIFESP");
            var unb = new Faculdade("UnB");
            var ufrj = new Faculdade("UFRJ");
            var ufsc = new Faculdade("UFSC");
            var usp = new Faculdade("USP");

            // Registrando os Callbacks
            listaPostos[1].Registrar(usp);      // Amazonas
            listaPostos[1].Registrar(unb);
            listaPostos[0].Registrar(ufrj);     // Pará
            listaPostos[0].Registrar(unifesp);
            listaPostos[2].Registrar(ufsc);     // Acre
            listaPostos[2].Registrar(usp);
            listaPostos[6].Registrar(unb);      // Tocantins

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

            // Simulando alterações - isso aqui aciona automaticamente a Inversão de Controle
            listaPostos[1].AtualizarLeituras(29.2, 6.8, 94.0); // Amazonas
            listaPostos[0].AtualizarLeituras(32.1, 6.9, 78.0); // Pará
            listaPostos[6].AtualizarLeituras(35.5, 7.1, 35.0); // Tocantins

            Console.WriteLine("\nFim do monitoramento.");
        }
    }
}