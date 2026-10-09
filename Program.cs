using System;

// ===================== INTERFACES =====================

interface ICurable
{
    void Curar();
}

interface IDefendible
{
    void Defender();
}

// ===================== CLASE BASE ABSTRACTA =====================

abstract class Personaje
{
    // Propiedades compartidas por todos los personajes (no se repiten en las hijas)
    public string Nombre { get; private set; }

    // Solo se puede modificar desde la clase Personaje o sus derivadas
    // (desde fuera solo se puede leer)
    public int Vida { get; protected set; }

    public int Nivel { get; private set; }

    public Personaje(string nombre, int vida, int nivel)
    {
        Nombre = nombre;

        // La vida inicial debe estar entre 1 y 100
        if (vida < 1)
        {
            vida = 1;
        }
        else if (vida > 100)
        {
            vida = 100;
        }
        Vida = vida;

        // El nivel mínimo es 1
        if (nivel < 1)
        {
            nivel = 1;
        }
        Nivel = nivel;
    }

    // Método común: lo heredan todos los personajes
    public void MostrarEstado()
    {
        Console.WriteLine("Nombre: " + Nombre);
        Console.WriteLine("Vida: " + Vida);
        Console.WriteLine("Nivel: " + Nivel);
    }

    // Método abstracto: no define CÓMO se ataca, cada hija lo implementa
    public abstract void Atacar();

    public void RecibirDanio(int cantidad)
    {
        Vida = Vida - cantidad;

        // La vida nunca puede ser menor que 0
        if (Vida < 0)
        {
            Vida = 0;
        }

        Console.WriteLine(Nombre + " recibe " + cantidad + " puntos de daño.");
        Console.WriteLine("Vida restante: " + Vida);

        if (Vida == 0)
        {
            Console.WriteLine(Nombre + " ha sido derrotado.");
        }
    }
}

// ===================== CLASES DERIVADAS =====================

class Guerrero : Personaje, IDefendible
{
    public int Fuerza { get; private set; }

    public Guerrero(string nombre, int vida, int nivel, int fuerza)
        : base(nombre, vida, nivel)
    {
        Fuerza = fuerza;
    }

    public override void Atacar()
    {
        Console.WriteLine(Nombre + " ataca con su espada causando " + Fuerza + " puntos de daño.");
    }

    public void Defender()
    {
        Console.WriteLine(Nombre + " levanta su escudo y se prepara para defenderse.");
    }
}

class Mago : Personaje, ICurable
{
    public int Mana { get; private set; }

    public Mago(string nombre, int vida, int nivel, int mana)
        : base(nombre, vida, nivel)
    {
        Mana = mana;
    }

    public override void Atacar()
    {
        // El daño del hechizo depende del maná del mago
        Console.WriteLine(Nombre + " lanza un hechizo causando " + Mana + " puntos de daño.");
    }

    public void Curar()
    {
        int vidaAntes = Vida;

        Vida = Vida + 20;

        // La vida nunca debe superar 100
        if (Vida > 100)
        {
            Vida = 100;
        }

        Console.WriteLine(Nombre + " utiliza magia de curación.");
        Console.WriteLine("Vida recuperada: " + (Vida - vidaAntes));
        Console.WriteLine("Vida actual: " + Vida);
    }
}

class Arquero : Personaje
{
    public int Flechas { get; private set; }

    public Arquero(string nombre, int vida, int nivel, int flechas)
        : base(nombre, vida, nivel)
    {
        Flechas = flechas;
    }

    public override void Atacar()
    {
        // No puede disparar si no tiene flechas
        if (Flechas <= 0)
        {
            Console.WriteLine(Nombre + " no tiene flechas para disparar.");
            return;
        }

        Flechas = Flechas - 1;
        Console.WriteLine(Nombre + " dispara una flecha causando 25 puntos de daño.");
        Console.WriteLine("Flechas restantes: " + Flechas);
    }
}

// ===================== PROGRAMA PRINCIPAL =====================

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Guerrero kratos = new Guerrero("Kratos", 100, 5, 30);
        Mago merlin = new Mago("Merlín", 80, 6, 40);
        Arquero legolas = new Arquero("Legolas", 90, 4, 3);

        // Polimorfismo: una sola colección para objetos de tipos diferentes
        Personaje[] personajes = { kratos, merlin, legolas };

        Console.WriteLine("===== PERSONAJES =====");
        Console.WriteLine();

        foreach (Personaje personaje in personajes)
        {
            personaje.MostrarEstado();
            Console.WriteLine();
            personaje.Atacar();   // cada objeto ejecuta SU propia versión
            Console.WriteLine();
            Console.WriteLine("--------------------");
            Console.WriteLine();
        }

        Console.WriteLine("===== HABILIDADES =====");
        Console.WriteLine();

        // Guerrero: Defender (IDefendible)
        kratos.Defender();
        Console.WriteLine();

        // Mago: recibe daño y luego se cura (ICurable)
        merlin.RecibirDanio(30);
        Console.WriteLine();
        merlin.Curar();
        Console.WriteLine();

        // Arquero: ataques repetidos hasta quedarse sin flechas
        legolas.Atacar();
        Console.WriteLine();
        legolas.Atacar();
        Console.WriteLine();
        legolas.Atacar();   // intento sin flechas
        Console.WriteLine();

        // Un personaje es derrotado
        legolas.RecibirDanio(100);
    }
}