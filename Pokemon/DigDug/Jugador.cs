using System;
using System.Collections.Generic;
using System.IO;

public class Jugador : Sprite
{
    protected string nombre;
    protected string genero;
    protected int dinero;
    protected int tiempoJugado;
    public bool Hablando { get; set; } //SUBIR A SUPERCLASE
    protected List<Bestia> equipo;

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
        Hablando = false;
        equipo = new List<Bestia>();
    }

    public Jugador()
    {
        Hablando = false;
        equipo = new List<Bestia>();
    }

    public string GetNombre()
    {
        return nombre;
    }

    public List<Bestia> GetEquipo()
    {
        return equipo;
    }

    public string GetGenero()
    {
        return genero;
    }

    public int GetDinero()
    {
        return dinero;
    }

    public int GetTiempoJugado() //PROVISIONAL
    {
        return tiempoJugado;
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

    public void guardarJugador(string partida, ref Mapa cargarMapa)
    {
        try
        {
            SdlHardware.DrawHiddenImage(new Image("data/cargando.png"), 0, 0);
            SdlHardware.ShowHiddenScreen();
            StreamWriter escribir = new StreamWriter(partida);
            escribir.WriteLine(nombre);
            escribir.WriteLine(genero);
            escribir.WriteLine(dinero);
            escribir.WriteLine(0);
            foreach (Arbol arbol in cargarMapa.Arboles)
            {
                escribir.WriteLine(arbol.x);
                escribir.WriteLine(arbol.y);
            }
            foreach (Edificio edificio in cargarMapa.Edificios)
            {
                escribir.WriteLine(edificio.x);
                escribir.WriteLine(edificio.y);
            }
            foreach (Hierba hierba in cargarMapa.Hierbas)
            {
                escribir.WriteLine(hierba.x);
                escribir.WriteLine(hierba.y);
            }
            foreach (Bestia bestia in equipo)
            {
                escribir.WriteLine(bestia.image.nombre);
                escribir.WriteLine(bestia.GetNombre());
                escribir.WriteLine(bestia.GetNivel());
                escribir.WriteLine(bestia.GetVida());
            }
            escribir.Close();
        }
        catch(Exception e)
        {

        }
    }

    public void cargarJugador(string partida, ref Mapa cargarMapa)
    {
        try
        {
            SdlHardware.DrawHiddenImage(new Image("data/cargando.png"), 0, 0);
            SdlHardware.ShowHiddenScreen();
            StreamReader leer = new StreamReader(partida);
            nombre = leer.ReadLine();
            genero = leer.ReadLine();
            if (genero == "Hombre")
                CargarHombre();
            else
                CargarMujer();

            dinero = Convert.ToInt32(leer.ReadLine());
            tiempoJugado = Convert.ToInt32(leer.ReadLine());
            foreach (Arbol arbol in cargarMapa.Arboles)
            {
                arbol.x = Convert.ToInt32(leer.ReadLine());
                arbol.y = Convert.ToInt32(leer.ReadLine());
            }
            foreach (Edificio edificio in cargarMapa.Edificios)
            {
                edificio.x = Convert.ToInt32(leer.ReadLine());
                edificio.y = Convert.ToInt32(leer.ReadLine());
            }
            foreach (Hierba hierba in cargarMapa.Hierbas)
            {
                hierba.x = Convert.ToInt32(leer.ReadLine());
                hierba.y = Convert.ToInt32(leer.ReadLine());
            }
            string linea = leer.ReadLine();
            int i = 0;
            while(linea != null)
            {
                equipo.Add(new Bestia(leer.ReadLine(),linea));
                equipo[i].SetNivel(Convert.ToInt32(leer.ReadLine()));
                equipo[i].SetVida(Convert.ToInt32(leer.ReadLine()));
                linea = leer.ReadLine();
                i++;
            }

            leer.Close();
        }
        catch (Exception e)
        {

        }
    }

    public bool PuedeMoverse (Jugador prota, List<Arbol> arboles, List<Edificio> edificios, List<Npc> npcs)
    {
        bool puede = true;
        foreach (Arbol arbol in arboles)
        {
            if (prota.CollisionsWith(arbol))
                puede = false;
        }
        foreach (Edificio edificio in edificios)
        {
            if (prota.CollisionsWith(edificio))
                puede = false;
        }
        foreach (Npc npc in npcs)
        {
            if (prota.CollisionsWith(npc))
                puede = false;
        }
        return puede;
    }
}
