using System;
using System.IO;
using System.Collections.Generic;

class Juego
{
    protected Jugador protagonista;
    protected List<Npc> npcs;
    protected bool bucle;
    protected Image fondo;
    protected Mapa mapa;
    Font font18;
    protected bool dibujarDialogo;

    public Juego(Jugador protagonista, Mapa mapa)
    {
        bucle = true;
        this.protagonista = protagonista;
        this.protagonista.MoveTo(512, 450);
        font18 = new Font("data/Joystix.ttf", 18);
        fondo = new Image("data/fondo_juego.jpg");
        this.mapa = mapa;
        npcs = new List<Npc>();
        CargarNpc();
        dibujarDialogo = false;
    }

    public void CargarNpc()
    {
        Random r = new Random(); //PROVISIONAL
        npcs.Add(new Npc("data/prota_hombre/maleDownBase.png"));
        npcs.Add(new Npc("data/prota_mujer/femaleDownBase.png"));
        foreach (Npc npc in npcs)
        {
            npc.MoveTo(r.Next(400, 600), r.Next(400, 600));
            npc.Dialogo.Add("Hola");
            npc.Dialogo.Add("Adios");
            npc.Dialogo.Add("Que tal");
        }
    }

    public Bestia CargarPokemonSalvaje()
    {
        Bestia devolver = null;
        try
        {
            string[] leer = File.ReadAllLines("data/pokemons/lista_pokemon.txt"); //Provisional, cargar todo en un principio
            int indice = new Random().Next(0,leer.Length);
            devolver = new Bestia(leer[indice].Split(';')[0], leer[indice].Split(';')[1]);
        }
        catch(Exception e)
        {

        }
        return devolver;
    }

    public void DibujarJuego()
    {
        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(fondo, 0, 0);

        foreach (Arbol arbol in mapa.Arboles)
        {
            arbol.DrawOnHiddenScreen();
        }
        foreach (Edificio edificio in mapa.Edificios)
        {
            edificio.DrawOnHiddenScreen();
        }
        foreach (Hierba hierba in mapa.Hierbas)
        {
            hierba.DrawOnHiddenScreen();
        }
        protagonista.DrawOnHiddenScreen();
        foreach (Npc npc in npcs)
        {
            npc.DrawOnHiddenScreen();
            if (npc.Hablando)
            {
                SdlHardware.DrawHiddenImage(
                    new Image("data/fondo_dialogo.png"), 0, 500);
                SdlHardware.WriteHiddenText(
                    npc.Dialogo[npc.IndiceDialogo],
                    50, 550,
                    0, 0, 0,
                    font18);
            }
        }

        SdlHardware.ShowHiddenScreen();
    }

    public void MoverMundo(int X, int Y)
    {
        foreach (Arbol arbol in mapa.Arboles)
        {
            arbol.MoveTo(arbol.GetX() + X, arbol.GetY() + Y);
        }
        foreach (Edificio edificio in mapa.Edificios)
        {
            edificio.MoveTo(edificio.GetX() + X, edificio.GetY() + Y);
        }
        foreach (Hierba hierba in mapa.Hierbas)
        {
            hierba.MoveTo(hierba.GetX() + X, hierba.GetY() + Y);
        }
        foreach (Npc npc in npcs)
        {
            npc.MoveTo(npc.GetX() + X, npc.GetY() + Y);
        }
    }

    private void BuscarCombateSalvaje()
    {
        foreach (Hierba hierba in mapa.Hierbas)
        {
            if(protagonista.CollisionsWith(hierba))
            {
                if (new Random().Next(1, 100) <= 5 ? true : false)
                {
                    Combate combate = new Combate(
                        protagonista,CargarPokemonSalvaje());
                    combate.Run();
                }
            }
        }
    }

    public void ComprobarTeclas()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            foreach (Npc npc in npcs)
            {
                if (protagonista.CollisionsWith(npc))
                {
                    switch (protagonista.currentDirection)
                    {
                        //PROVISIONAL
                        case Sprite.DOWN:
                            npc.ChangeDirection(Sprite.UP);
                            break;
                        case Sprite.UP:
                            npc.ChangeDirection(Sprite.DOWN);
                            break;
                        case Sprite.LEFT:
                            npc.ChangeDirection(Sprite.RIGHT);
                            break;
                        case Sprite.RIGHT:
                            npc.ChangeDirection(Sprite.LEFT);
                            break;
                    }

                    npc.Hablando = true;
                    dibujarDialogo = true;
                    protagonista.Hablando = true;
                }
            }
            SdlHardware.Pause(100);
        }
        foreach (Npc npc in npcs)
        {
            if (npc.Hablando)
            {
                if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
                {
                    if (npc.IndiceDialogo < npc.Dialogo.Count - 1)
                    {
                        npc.IndiceDialogo++;
                    }
                    else
                    {
                        npc.Hablando = false;
                        protagonista.Hablando = false;
                        npc.IndiceDialogo = 0;
                    }
                    SdlHardware.Pause(40);
                }
            }
        }
        if (!protagonista.Hablando)
        {
            if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
            {
                if (protagonista.PuedeMoverse(
                        protagonista, mapa.Arboles, mapa.Edificios, npcs))
                    MoverMundo(0, -10);
                else
                    MoverMundo(0, 20);
                BuscarCombateSalvaje();
                protagonista.ChangeDirection(Sprite.DOWN);
                protagonista.NextFrame();
            }
            else if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
            {
                if (protagonista.PuedeMoverse(
                        protagonista, mapa.Arboles, mapa.Edificios, npcs))
                    MoverMundo(0, 10);
                else
                    MoverMundo(0, -20);
                BuscarCombateSalvaje();
                protagonista.ChangeDirection(Sprite.UP);
                protagonista.NextFrame();
            }
            else if (SdlHardware.KeyPressed(SdlHardware.KEY_LEFT))
            {
                if (protagonista.PuedeMoverse(
                        protagonista, mapa.Arboles, mapa.Edificios, npcs))
                    MoverMundo(10, 0);
                else
                    MoverMundo(-20, 0);
                BuscarCombateSalvaje();
                protagonista.ChangeDirection(Sprite.LEFT);
                protagonista.NextFrame();
            }
            else if (SdlHardware.KeyPressed(SdlHardware.KEY_RIGHT))
            {
                if (protagonista.PuedeMoverse(
                        protagonista, mapa.Arboles, mapa.Edificios, npcs))
                    MoverMundo(-10, 0);
                else
                    MoverMundo(20, 0);
                BuscarCombateSalvaje();
                protagonista.ChangeDirection(Sprite.RIGHT);
                protagonista.NextFrame();
            }
            else if (SdlHardware.KeyPressed(SdlHardware.KEY_M))
            {
                MenuJugador mj = new MenuJugador(protagonista, mapa);
                mj.Run();
            }
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_K))
        {
            SdlHardware.Pause(100);
            Pokemon.Run();
        }
    }

    public void Run()
    {
        do
        {
            DibujarJuego();
            ComprobarTeclas();
            SdlHardware.Pause(40);
        } while (bucle);
    }
}

