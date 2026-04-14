// Usei IA para tirar dúvidas e organizar a estrutura dos padrões
using System;

// --- CONFIGURAÇÕES DO SISTEMA (SINGLETON) ---
class DadosSistema
{
    public string nomeApp = "Notificador Escolar";
    public int limiteEnvio = 5;
    public int enviosRealizados = 0;

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

// Interface padrão para qualquer tipo de envio
interface IMensagem
{
    void Disparar(string conteudo);
}

// --- CLASSES DE ENVIO DO SISTEMA ---
class EnvioEmail : IMensagem
{
    public void Disparar(string conteudo)
    {
        Console.WriteLine("-> Saída via E-mail: " + conteudo);
    }
}

class EnvioSMS : IMensagem
{
    public void Disparar(string conteudo)
    {
        Console.WriteLine("-> Saída via SMS: " + conteudo);
    }
}

// --- INTEGRAÇÃO COM WHATSAPP (ADAPTADOR) ---

class WhatsAppSDK
{
    public void enviarMensagemZap(string txt)
    {
        Console.WriteLine("-> Saída via WhatsApp (SDK Externo): " + txt);
    }
}

class AdaptadorWhatsApp : IMensagem
{
    private WhatsAppSDK _sdk;
    public AdaptadorWhatsApp(WhatsAppSDK sdk)
    {
        _sdk = sdk;
    }

    public void Disparar(string conteudo)
    {
        _sdk.enviarMensagemZap(conteudo);
    }
}

// --- SEGURANÇA E CONTROLE (PROXY) ---
class MensagemProxy : IMensagem
{
    private IMensagem _objetoReal;
    private DadosSistema _config;

    public MensagemProxy(IMensagem objetoReal)
    {
        _objetoReal = objetoReal;
        _config = DadosSistema.ObterInstancia();
    }

    public void Disparar(string conteudo)
    {
        if (_config.enviosRealizados < _config.limiteEnvio)
        {
            _objetoReal.Disparar(conteudo);
            _config.enviosRealizados++;
            Console.WriteLine("[LOG] Envios realizados agora: " + _config.enviosRealizados + "/" + _config.limiteEnvio);
        }
        else
        {
            Console.WriteLine("[ERRO] Bloqueio do Proxy: Limite de envios excedido!");
        }
    }
}

// --- GERADOR DE AVISOS (FACTORY) ---
class CriadorAviso
{
    public static IMensagem Gerar(string opcao)
    {
        IMensagem servico = null;

        if (opcao == "1") servico = new EnvioEmail();
        if (opcao == "2") servico = new EnvioSMS();
        if (opcao == "3") servico = new AdaptadorWhatsApp(new WhatsAppSDK());

        if (servico != null)
        {
            return new MensagemProxy(servico);
        }
        return null;
    }
}

// --- PROGRAMA PRINCIPAL ---
class Program
{
    static void Main()
    {
        DadosSistema config = DadosSistema.ObterInstancia();
        Console.WriteLine("=== " + config.nomeApp + " ===");
        Console.WriteLine("Limite configurado: " + config.limiteEnvio + " envios.\n");

        // Testando Email (Opção 1)
        IMensagem m1 = CriadorAviso.Gerar("1");
        if (m1 != null)
        {
            m1.Disparar("Alerta de reunião de pais");
        }

        // Testando SMS (Opção 2)
        IMensagem m2 = CriadorAviso.Gerar("2");
        if (m2 != null)
        {
            m2.Disparar("Sua nota foi postada");
        }

        // Testando WhatsApp Adaptado (Opção 3)
        IMensagem m3 = CriadorAviso.Gerar("3");
        if (m3 != null)
        {
            m3.Disparar("Boleto disponível para pagamento");

            // Testando o limite do Proxy (Forçando mais envios)
            Console.WriteLine("\n--- Testando limite do Proxy ---");
            m3.Disparar("Teste limite 4");
            m3.Disparar("Teste limite 5");
            m3.Disparar("Teste limite 6 (Deve bloquear)");
        }

        Console.WriteLine("\nFim do programa.");
    }
}
