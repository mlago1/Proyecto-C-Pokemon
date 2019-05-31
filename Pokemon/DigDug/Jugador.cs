using System;
using System.Collections.Generic;
using System.IO;
using static Ataque;

public class Jugador : Sprite
{
    protected string nombre;
    protected string genero;
    protected int dinero, pokemonsDiferentesCapturados, velocidad;
    protected int tiempoJugado;
    public bool Hablando { get; set; } 
    protected List<Bestia> equipo, caja;
    protected Dictionary<Objeto,int> mochila;

    public Jugador(string nombre, string genero)
    {
        this.velocidad = 5;
        this.width = 34;
        this.height = 48;
        this.nombre = nombre;
        this.genero = genero;
        this.x = 512;
        this.y = 450;
        this.dinero = 0;
        this.pokemonsDiferentesCapturados = 1;
        this.tiempoJugado = 0;
        if (genero == "Hombre")
            CargarHombre();
        else
            CargarMujer();
        Hablando = false;
        equipo = new List<Bestia>();
        caja = new List<Bestia>();
        mochila = new Dictionary<Objeto, int>();
    }

    public Jugador()
    {
        this.velocidad = 10;
        Hablando = false;
        equipo = new List<Bestia>();
        caja = new List<Bestia>();
        mochila = new Dictionary<Objeto, int>();
    }

    public string GetNombre()
    {
        return nombre;
    }

    public List<Bestia> GetEquipo()
    {
        return equipo;
    }

    public List<Bestia> GetCaja()
    {
        return caja;
    }

    public Dictionary<Objeto, int> GetMochila()
    {
        return mochila;
    }

    public string GetGenero()
    {
        return genero;
    }

    public int GetDinero()
    {
        return dinero;
    }

    public void SetDinero(int dinero)
    {
        this.dinero = dinero;
    }

    public int GetVelocidad()
    {
        return velocidad;
    }

    public int GetPokemonsDiferentesCapturados()
    {
        return pokemonsDiferentesCapturados;
    }

    public void SetPokemonsDiferentesCapturados(int pokemonsDiferentesCapturados)
    {
        this.pokemonsDiferentesCapturados = pokemonsDiferentesCapturados;
    }

    public int GetTiempoJugado() 
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

    public void guardarJugador(string partida, ref Sprite fondo,
        ref Sprite dialogo, int scrollX, int scrollY)
    {
        try
        {
            SdlHardware.DrawHiddenImage(new Image("data/cargando.png"), 0, 0);
            SdlHardware.ShowHiddenScreen();
            StreamWriter escribir = new StreamWriter(partida);
            escribir.WriteLine(nombre);
            escribir.WriteLine(genero);
            escribir.WriteLine(dinero);
            escribir.WriteLine(pokemonsDiferentesCapturados);
            escribir.WriteLine(0);
            escribir.WriteLine(this.x);
            escribir.WriteLine(this.y);
            escribir.WriteLine(fondo.x);
            escribir.WriteLine(fondo.y);
            escribir.WriteLine(dialogo.x);
            escribir.WriteLine(dialogo.y);
            escribir.WriteLine(scrollX);
            escribir.WriteLine(scrollY);

            foreach (Bestia bestia in equipo)
            {
                escribir.Write(bestia.image.nombre + ";" + bestia.GetNombre() + ";" +
                    bestia.GetNivel() + ";" + bestia.GetVida() + ";" + bestia.GetMaxVida() + ";");

                for(int i = 0; i < bestia.GetAtaques().Count ; i++)
                {
                    escribir.Write(bestia.GetAtaques()[i].nombre);
                    escribir.Write(":" + bestia.GetAtaques()[i].tipo);
                    escribir.Write(":" + bestia.GetAtaques()[i].poder);
                    if(i < bestia.GetAtaques().Count - 1)
                    {
                        escribir.Write("_");
                    }
                }
                escribir.WriteLine();
            }
            escribir.Close();
            escribir = new StreamWriter(partida+"_caja.txt");
            foreach (Bestia bestia in caja)
            {
                escribir.Write(bestia.image.nombre + ";" + bestia.GetNombre() + ";" +
                    bestia.GetNivel() + ";" + bestia.GetVida() + ";" + bestia.GetMaxVida() + ";");

                for (int i = 0; i < bestia.GetAtaques().Count; i++)
                {
                    escribir.Write(bestia.GetAtaques()[i].nombre);
                    escribir.Write(":" + bestia.GetAtaques()[i].tipo);
                    escribir.Write(":" + bestia.GetAtaques()[i].poder);
                    if (i < bestia.GetAtaques().Count - 1)
                    {
                        escribir.Write("_");
                    }
                }
                escribir.WriteLine();
            }
            escribir.Close();
            escribir = new StreamWriter(partida + "_mochila.txt");
            foreach (KeyValuePair<Objeto,int> kp in mochila)
            {
                if(kp.Key.GetType().Name == "Pocion")
                    escribir.WriteLine(kp.Key.GetType().Name + ";" +
                        kp.Key.Nombre + ";" + ((Pocion)kp.Key).hpRecuperados + ";" + kp.Value);
            }
            escribir.Close();
        }
        catch(Exception e)
        {
            Menu.Error();
        }
    }

    public void cargarJugador(string partida, ref Sprite fondo, ref Sprite dialogo)
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
            pokemonsDiferentesCapturados = Convert.ToInt32(leer.ReadLine());
            tiempoJugado = Convert.ToInt32(leer.ReadLine());
            this.x = Convert.ToInt32(leer.ReadLine());
            this.y = Convert.ToInt32(leer.ReadLine());
            fondo.x = Convert.ToInt32(leer.ReadLine());
            fondo.y = Convert.ToInt32(leer.ReadLine());
            dialogo.x = Convert.ToInt32(leer.ReadLine());
            dialogo.y = Convert.ToInt32(leer.ReadLine());
            SdlHardware.startX = Convert.ToInt16(leer.ReadLine());
            SdlHardware.startY = Convert.ToInt16(leer.ReadLine());

            string linea = leer.ReadLine();
            int i = 0;
            while (linea != null)
            {
                string[] cortar = linea.Split(';');
                equipo.Add(new Bestia(
                    cortar[1], cortar[0]));
                equipo[i].SetNivel(Convert.ToInt32(cortar[2]));
                equipo[i].SetVida(Convert.ToInt32(cortar[3]));
                equipo[i].SetMaxVida(Convert.ToInt32(cortar[4]));
                string[] auxCortar = cortar[5].Split('_');
                foreach (string s in auxCortar)
                {
                    string[] auxAuxCortar = s.Split(':');
                    equipo[i].GetAtaques().Add(new ataque(auxAuxCortar[0],
                        auxAuxCortar[1], Convert.ToInt32(auxAuxCortar[2])));
                }
                linea = leer.ReadLine();
                i++;
            }

            leer.Close();

            leer = new StreamReader(partida+"_caja.txt");

            linea = leer.ReadLine();
            i = 0;
            while (linea != null)
            {
                string[] cortar = linea.Split(';');
                caja.Add(new Bestia(
                    cortar[1], cortar[0]));
                caja[i].SetNivel(Convert.ToInt32(cortar[2]));
                caja[i].SetVida(Convert.ToInt32(cortar[3]));
                caja[i].SetMaxVida(Convert.ToInt32(cortar[4]));
                string[] auxCortar = cortar[5].Split('_');
                foreach (string s in auxCortar)
                {
                    string[] auxAuxCortar = s.Split(':');
                    caja[i].GetAtaques().Add(new ataque(auxAuxCortar[0],
                        auxAuxCortar[1], Convert.ToInt32(auxAuxCortar[2])));
                }
                linea = leer.ReadLine();
                i++;
            }

            leer.Close();

            leer = new StreamReader(partida + "_mochila.txt");
            linea = leer.ReadLine();
            while (linea != null)
            {
                string[] cortar = linea.Split(';');
                switch(cortar[0])
                {
                    case "Pocion":
                        mochila.Add(new Pocion(cortar[1],
                            Convert.ToInt32(cortar[2])),Convert.ToInt32(cortar[3])); break;
                }
                linea = leer.ReadLine();
            }

            leer.Close();

        }
        catch (Exception e)
        {
            Menu.Error();
        }
    }
}
