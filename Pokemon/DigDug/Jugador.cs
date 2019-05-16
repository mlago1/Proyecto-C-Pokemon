using System;
using System.Collections.Generic;
using System.IO;

class Jugador : Sprite
{
    protected string nombre;
    protected string genero;
    protected int dinero;
    protected int tiempoJugado;
    //protected List<Pokemons> equipo; /Futuro/

    public Jugador(string nombre, string genero)
    {
        this.nombre = nombre;
        this.genero = genero;
        this.dinero = 0;
        this.tiempoJugado = 0;
        if (genero == "Hombre")
            CargarHombre();
        else
            CargarMujer();
    }

    public Jugador()
    {
        
    }
    void CargarMujer()
    {
        LoadSequence(RIGHT,
            new string[] { "data/prota_mujer/femaleRightBase.png",
                "data/prota_mujer/femaleRight1.png",
                "data/prota_mujer/femaleRightBase.png",
                "data/prota_mujer/femaleRight2.png"});
        LoadSequence(LEFT,
            new string[] { "data/prota_mujer/femaleLeftBase.png",
                "data/prota_mujer/femaleLeft1.png",
                "data/prota_mujer/femaleLeftBase.png",
                "data/prota_mujer/femaleLeft2.png"});
        LoadSequence(UP,
            new string[] { "data/prota_mujer/femaleUpBase.png",
                "data/prota_mujer/femaleUp1.png",
                "data/prota_mujer/femaleUpBase.png",
                "data/prota_mujer/femaleUp2.png"});
        LoadSequence(DOWN,
            new string[] { "data/prota_mujer/femaleDownBase.png",
                "data/prota_mujer/femaleDown1.png",
                "data/prota_mujer/femaleDownBase.png",
                "data/prota_mujer/femaleDown2.png"});

        currentDirection = LEFT;
    }

    void CargarHombre()
    {
        LoadSequence(RIGHT,
            new string[] { "data/prota_hombre/maleRightBase.png",
                "data/prota_hombre/maleRight1.png",
                "data/prota_hombre/maleRightBase.png",
                "data/prota_hombre/maleRight2.png"});
        LoadSequence(LEFT,
            new string[] { "data/prota_hombre/maleLeftBase.png",
                "data/prota_hombre/maleLeft1.png",
                "data/prota_hombre/maleLeftBase.png",
                "data/prota_hombre/maleLeft2.png"});
        LoadSequence(UP,
            new string[] { "data/prota_hombre/maleUpBase.png",
                "data/prota_hombre/maleUp1.png",
                "data/prota_hombre/maleUpBase.png",
                "data/prota_hombre/maleUp2.png"});
        LoadSequence(DOWN,
            new string[] { "data/prota_hombre/maleDownBase.png",
                "data/prota_hombre/maleDown1.png",
                "data/prota_hombre/maleDownBase.png",
                "data/prota_hombre/maleDown2.png"});

        currentDirection = LEFT;
    }

    public void guardarJugador(string partida)
    {
        try
        {
            StreamWriter escribir = new StreamWriter(partida);
            escribir.WriteLine(nombre);
            escribir.WriteLine(genero);
            escribir.WriteLine(dinero);
            escribir.WriteLine(0);
            escribir.Close();
        }
        catch(Exception e)
        {

        }
    }

    public void cargarJugador(string partida)
    {
        try
        {
            StreamReader leer = new StreamReader(partida);
            nombre = leer.ReadLine();
            genero = leer.ReadLine();
            if (genero == "Hombre")
                CargarHombre();

            dinero = Convert.ToInt32(leer.ReadLine());
            tiempoJugado = Convert.ToInt32(leer.ReadLine());
            leer.Close();
        }
        catch (Exception e)
        {

        }
    }

    public bool PuedeMoverse (Jugador prota, List<Arbol> arboles, List<Edificio> edificios)
    {
        bool puede = true;
        foreach (Sprite arbol in arboles)
        {
            if (prota.CollisionsWith(arbol))
                puede = false;
        }
        foreach (Sprite edificio in edificios)
        {
            if (prota.CollisionsWith(edificio))
                puede = false;
        }
        return puede;
    }
}
