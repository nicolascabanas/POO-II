using System;

namespace HomeTheaterSystem
{
    // --- SUBSISTEMAS ---
    public class TV { public void ligar() => Console.WriteLine("TV -> Ligada"); }
    public class Projetor { public void ligar() => Console.WriteLine("Projetor -> Ligado"); }
    public class Receiver { public void configurarAudio() => Console.WriteLine("Receiver -> Áudio configurado"); }
    public class PlayerMidia { public void iniciar() => Console.WriteLine("Player -> Reprodução iniciada"); }
    public class SistemaSom { public void ajustarVolume(int v) => Console.WriteLine($"Som -> Volume em {v}"); }
    public class LuzAmbiente { public void ajustar() => Console.WriteLine("Luzes -> Ambiente dimerizado"); }

    // --- FACHADA ---
    public class HomeTheaterFacade
    {
        // A fachada detém as referências para os subsistemas (Redução de Acoplamento)
        private TV tv = new TV();
        private Projetor projetor = new Projetor();
        private Receiver receiver = new Receiver();
        private PlayerMidia player = new PlayerMidia();
        private SistemaSom som = new SistemaSom();
        private LuzAmbiente luz = new LuzAmbiente();

        // REQUISITO: Operação assistirFilme()
        public void assistirFilme()
        {
            Console.WriteLine("\n--- [Operação: Assistir Filme] ---");
            luz.ajustar();
            projetor.ligar();
            tv.ligar();
            receiver.configurarAudio();
            som.ajustarVolume(15);
            player.iniciar();
            Console.WriteLine("----------------------------------");
        }

        // REQUISITO: Operação ouvirMusica()
        public void ouvirMusica()
        {
            Console.WriteLine("\n--- [Operação: Ouvir Música] ---");
            luz.ajustar();
            receiver.configurarAudio();
            som.ajustarVolume(20);
            player.iniciar();
            Console.WriteLine("---------------------------------");
        }

        // REQUISITO: Encapsular desligamento
        public void desligarSistema()
        {
            Console.WriteLine("\n--- [Encerrando tudo...] ---");
            Console.WriteLine("Todos os dispositivos foram desligados.");
        }
    }

    // --- CLIENTE ---
    class Program
    {
        static void Main(string[] args)
        {
            // O cliente agora interage APENAS com a fachada
            HomeTheaterFacade fachada = new HomeTheaterFacade();

           
            fachada.assistirFilme();
            fachada.ouvirMusica();

            fachada.desligarSistema();
        }
    }
}
