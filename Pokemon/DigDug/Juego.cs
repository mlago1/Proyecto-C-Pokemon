using System;
using System.IO;

public class Juego
{
    protected Jugador protagonista;
    protected bool bucle;
    protected Sprite fondo, dialogo;
    protected Mapa mapa;
    Font font18;
    protected bool dibujarDialogo;
    Random r;
    int viejoX, viejoY, viejoFondoX, viejoFondoY, viejoDialogoX, viejoDialogoY;
    public short viejoScrollX, viejoScrollY, nuevoScrollX, nuevoScrollY;

    public Juego(Jugador protagonista, Sprite fondo, Sprite dialogo)
    {
        r = new Random();
        bucle = true;
        this.protagonista = protagonista;
        font18 = new Font("data/Joystix.ttf", 18);
        this.fondo = fondo;
        this.dialogo = dialogo;
        mapa = new Mapa();
        mapa.CargarMapa("data/mapa.txt");
        dibujarDialogo = false;
    }

    private Bestia CargarPokemonSalvaje()
    {
        Bestia devolver = null;
        try
        {
            string[] leer = File.ReadAllLines("data/pokemons/lista_pokemon.txt"); 
            int indice = r.Next(0,leer.Length);
            SdlHardware.Pause(100);
            devolver = new Bestia(leer[indice].Split(';')[0], leer[indice].Split(';')[1]);
            devolver.CargarAtaques();
        }
        catch(Exception e)
        {

        }
        return devolver;
    }

    private void DibujarJuego()
    {
        SdlHardware.ClearScreen();
        fondo.DrawOnHiddenScreen();
        foreach (Hierba hierba in mapa.Hierbas)
        {
            hierba.DrawOnHiddenScreen();
        }
        protagonista.DrawOnHiddenScreen();
        foreach (Arbol arbol in mapa.Arboles)
        {
            arbol.DrawOnHiddenScreen();
        }
        foreach (Edificio edificio in mapa.Edificios)
        {
            edificio.DrawOnHiddenScreen();
        }
        foreach (Pc pc in mapa.Pcs)
        {
            pc.DrawOnHiddenScreen();
        }


        foreach (Npc npc in mapa.Npcs)
        {
            npc.DrawOnHiddenScreen();
            if (npc.Hablando)
            {
                dialogo.DrawOnHiddenScreen();
                SdlHardware.WriteHiddenText(
                    npc.Dialogo[npc.IndiceDialogo],
                    Convert.ToInt16(dialogo.x + 50), Convert.ToInt16(dialogo.y + 50),
                    0, 0, 0,
                    font18);
            }
        }

        SdlHardware.ShowHiddenScreen();
    }

    private void Moverse(int X, int Y, byte direccion)
    {
        protagonista.MoveTo(protagonista.x + X, protagonista.y + Y);
        fondo.MoveTo(fondo.x + X, fondo.y + Y);
        dialogo.MoveTo(dialogo.x + X, dialogo.y + Y);
        SdlHardware.ScrollTo(Convert.ToInt16(SdlHardware.startX - X), Convert.ToInt16(SdlHardware.startY - Y));
        protagonista.ChangeDirection(direccion);
        protagonista.NextFrame();
    }

    private void InvertirCoordenadas(bool invertir)
    {
        if(invertir)
        {
            protagonista.x = viejoX;
            protagonista.y = viejoY;
            fondo.x = viejoFondoX;
            fondo.y = viejoFondoY;
            dialogo.x = viejoDialogoX;
            dialogo.y = viejoDialogoY;
            SdlHardware.startX = viejoScrollX;
            SdlHardware.startY = viejoScrollY;
        }
        else
        {
            viejoX = protagonista.x;
            viejoY = protagonista.y;
            viejoFondoX = fondo.x;
            viejoFondoY = fondo.y;
            viejoDialogoX = dialogo.x;
            viejoDialogoY = dialogo.y;
            viejoScrollX = SdlHardware.startX;
            viejoScrollY = SdlHardware.startY;
        }
    }

    private void BuscarCombateSalvaje()
    {
        foreach (Hierba hierba in mapa.Hierbas)
        {
            if(protagonista.CollisionsWith(hierba))
            {
                if (r.Next(1, 100) <= 5 ? true : false)
                {
                    Combate combate = new Combate(
                        ref protagonista,CargarPokemonSalvaje(), this);
                    SdlHardware.ResetScroll();
                    combate.Run();
                }
            }
        }
    }

    private void GestionarConversaciones()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            foreach (Pc pc in mapa.Pcs)
            {
                if (protagonista.CollisionsWith(pc))
                {
                    SdlHardware.ResetScroll();
                    SdlHardware.Pause(100);
                    pc.Run(ref protagonista, viejoScrollX, viejoScrollY);
                }
            }
            foreach (Npc npc in mapa.Npcs)
            {
                if (protagonista.CollisionsWith(npc))
                {
                    switch (protagonista.currentDirection)
                    {
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
        foreach (Npc npc in mapa.Npcs)
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
    }

    private void ComprobarTeclas()
    {
        if (!protagonista.Hablando)
        {
            InvertirCoordenadas(false);

            if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
            {
                Moverse(0,protagonista.GetVelocidad(), Sprite.DOWN);
                BuscarCombateSalvaje();
            }
            else if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
            {
                Moverse(0, -protagonista.GetVelocidad(), Sprite.UP);
                BuscarCombateSalvaje();
            }
            else if (SdlHardware.KeyPressed(SdlHardware.KEY_LEFT))
            {
                Moverse(-protagonista.GetVelocidad(), 0, Sprite.LEFT);
                BuscarCombateSalvaje();
            }
            else if (SdlHardware.KeyPressed(SdlHardware.KEY_RIGHT))
            {
                Moverse(protagonista.GetVelocidad(), 0, Sprite.RIGHT);
                BuscarCombateSalvaje();
            }
            else if (SdlHardware.KeyPressed(SdlHardware.KEY_M))
            {
                SdlHardware.ResetScroll();
                MenuJugador mj = new MenuJugador(protagonista, fondo, dialogo, this);
                mj.Run();
            }
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_K))
        {
            SdlHardware.ResetScroll();
            SdlHardware.Pause(100);
            Pokemon.Run();
        }
    }

    private void DetectarColisiones()
    {
        bool colision = false;

        foreach (Arbol arbol in mapa.Arboles)
            if (protagonista.CollisionsWith(arbol))
                colision = true;

        foreach (Edificio edificio in mapa.Edificios)
            if (protagonista.CollisionsWith(edificio))
                colision = true;

        foreach (Pc pc in mapa.Pcs)
            if (protagonista.CollisionsWith(pc))
                colision = true;

        foreach (Npc npc in mapa.Npcs)
            if (protagonista.CollisionsWith(npc))
                colision = true;

        if(colision)
            InvertirCoordenadas(true);
    }

    public void Run()
    {
        do
        {
            DibujarJuego();
            ComprobarTeclas();
            GestionarConversaciones();
            DetectarColisiones();
            nuevoScrollX = SdlHardware.startX;
            nuevoScrollY = SdlHardware.startY;
            SdlHardware.Pause(40);
        } while (bucle);
    }
}

