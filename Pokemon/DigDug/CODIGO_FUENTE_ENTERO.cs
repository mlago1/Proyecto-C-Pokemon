using System;

public class Ataque
{
    public struct ataque
    {
        public string nombre;
        public string tipo;
        public int poder;

        public ataque(string nombre, string tipo, int poder)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.poder = poder;
        }
    }
}
﻿using System;
using System.IO;

public class Juego
{
    Sound bgSound;
    protected Jugador protagonista;
    protected bool bucle;
    protected Sprite fondo, dialogo;
    protected Mapa mapa;
    Font font18;
    protected bool dibujarDialogo;
    Random r;
    int viejoX, viejoY, viejoFondoX, viejoFondoY, viejoDialogoX, viejoDialogoY;
    public short viejoScrollX, viejoScrollY;

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
        bgSound = new Sound("data/sonidos/fondo.mp3");
        bgSound.BackgroundPlay();
    }

    private Bestia CargarPokemonSalvaje()
    {
        Bestia devolver = null;
        try
        {
            string[] leer = File.ReadAllLines("data/pokemons/lista_pokemon.txt"); 
            int indice = r.Next(0,leer.Length);
            SdlHardware.Pause(100);
            devolver = new Bestia(leer[indice].Split(';')[0],
                leer[indice].Split(';')[1]);
            devolver.CargarAtaques();
        }
        catch(Exception e)
        {
            Menu.Error();
        }
        return devolver;
    }

    private void DibujarJuego()
    {
        bool dibujarDialogo = false;
        Npc npcHablando = null;

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
                dibujarDialogo = true;
                npcHablando = npc;
            }
        }

        if(dibujarDialogo && npcHablando.Dialogo.Count > 0)
        {
            dialogo.DrawOnHiddenScreen();
            SdlHardware.WriteHiddenText(
                npcHablando.Dialogo[npcHablando.IndiceDialogo],
                Convert.ToInt16(dialogo.x + 50), Convert.ToInt16(dialogo.y + 50),
                0, 0, 0,
                font18);
        }

        SdlHardware.ShowHiddenScreen();
    }

    private void Moverse(int X, int Y, byte direccion)
    {
        protagonista.MoveTo(protagonista.x + X, protagonista.y + Y);
        fondo.MoveTo(fondo.x + X, fondo.y + Y);
        dialogo.MoveTo(dialogo.x + X, dialogo.y + Y);
        SdlHardware.ScrollTo(Convert.ToInt16(SdlHardware.startX - X),
            Convert.ToInt16(SdlHardware.startY - Y));
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
                    bgSound.StopMusic();
                    combate.Run();
                    bgSound.BackgroundPlay();
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
                    bgSound.StopMusic();
                    new Sound("data/sonidos/menu_jugador.mp3").PlayOnce();
                    pc.Run(ref protagonista, viejoScrollX, viejoScrollY);
                    bgSound.BackgroundPlay();
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
                        if (npc.GetType().Name == "Enfermera")
                        {
                            foreach (Bestia b in protagonista.GetEquipo())
                            {
                                b.SetVida(b.GetMaxVida());
                            }
                        }
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
                bgSound.StopMusic();
                new Sound("data/sonidos/menu_jugador.mp3").PlayOnce();
                new MenuJugador(protagonista, fondo, dialogo, this).Run();
                bgSound.BackgroundPlay();
            }
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
            SdlHardware.Pause(40);
        } while (bucle);
    }
}

﻿public class Pokemon
{
    public static void Main()
    {
        Run();
    }

    public static void Run()
    {
        bool fullScreen = false;
        SdlHardware.Init(1024, 768, 24, fullScreen);
        Bienvenida b = new Bienvenida();
        b.Run();
        Instrucciones i = new Instrucciones();
        i.Run();
        MenuPartidas m = new MenuPartidas();
        m.Run();
    }
}


﻿using System.IO;
using System;
using System.Collections.Generic;

class BorrarPartida : Menu
{ 
    protected bool partidaElegida;
    protected string nombrePartida;

    public BorrarPartida()
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 20);
        seleccion = 1;
        posicionFlecha = 200;
        partidaElegida = false;
        CargarNombres(ref listaNombres);
        try
        {
            maxOpciones = listaNombres.Count;
        }
        catch (Exception e)
        {
            maxOpciones = 0;
        }
    }

    private void BorradoConExito()
    {
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("Partida borrada con éxito",
                100, 50,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa <-- para volver atrás",
                100, 100,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.ShowHiddenScreen();
        } while (!SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE));
        SdlHardware.Pause(100);
    }

    private void BorrarPartidaSeleccionada()
    {
        File.Delete("partidas/" + nombrePartida + ".txt");
        File.Delete("partidas/" + nombrePartida + ".txt_caja.txt");
        File.Delete("partidas/" + nombrePartida + ".txt_mochila.txt");
        List<string> leerPartidas = new List<string>(File.ReadAllLines("partidas/listaPartidas.txt"));
        leerPartidas.Remove(nombrePartida);
        File.WriteAllLines("partidas/listaPartidas.txt", leerPartidas);
        SdlHardware.Pause(100);
        BorradoConExito();
        MenuPartidas m = new MenuPartidas();
        m.Run();
    }

    public override void DibujarInterfaz()
    {
        if (maxOpciones == 0)
        {
            SinPartidas();
            return;
        }
        else
        {
            do
            {
                SdlHardware.ClearScreen();
                SdlHardware.DrawHiddenImage(bg, 0, 0);
                SdlHardware.WriteHiddenText("Elige una partida",
                    100, 50,
                    0xC0, 0xC0, 0xC0,
                    font24);
                SdlHardware.WriteHiddenText("Pulsa <-- para volver",
                    100, 100,
                    0xC0, 0xC0, 0xC0,
                    font24);

                short altura = 200;
                foreach (string s in listaNombres)
                {
                    if (s.Length > 0)
                    {
                        SdlHardware.WriteHiddenText(s,
                        100, altura,
                        0xC0, 0xC0, 0xC0,
                        font24);
                        altura += 100;
                    }
                    else
                        SinPartidas();
                }

                SdlHardware.WriteHiddenText("<--",
                        500, posicionFlecha,
                        0xC0, 0xC0, 0xC0,
                        font24);

                SdlHardware.ShowHiddenScreen();

                if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
                {
                    if (seleccion == maxOpciones)
                    {
                        seleccion = 1;
                        posicionFlecha = 200;
                    }
                    else
                    {
                        seleccion++;
                        posicionFlecha += 100;
                    }
                }
                if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
                {
                    if (seleccion == 1)
                    {
                        seleccion = maxOpciones;
                        posicionFlecha = posicionFlecha = Convert.ToInt16(200 + (100 * (maxOpciones - 1)));
                    }
                    else
                    {
                        seleccion--;
                        posicionFlecha -= 100;
                    }
                }
                SdlHardware.ShowHiddenScreen();
                if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
                {
                    partidaElegida = true;
                    nombrePartida = listaNombres[seleccion - 1];
                    SdlHardware.Pause(100);
                }
                if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE))
                {
                    SdlHardware.Pause(100);
                    new MenuPartidas().Run();
                }
                SdlHardware.Pause(40);
            } while (!partidaElegida);
        }
    }

    public void Run()
    {
            DibujarInterfaz();
            BorrarPartidaSeleccionada();
    }
}
﻿using System;
using Tao.Sdl;

public class Sound
{

    private IntPtr internalPointer;

    public Sound(string fileName)
    {
        internalPointer = SdlMixer.Mix_LoadMUS(fileName);
    }

    // To play a song at a particular time
    public void PlayOnce()
    {
        SdlMixer.Mix_PlayMusic(internalPointer, 1);
    }

    // To continuously play song (background music)
    public void BackgroundPlay()
    {
        SdlMixer.Mix_PlayMusic(internalPointer, -1);
    }

    // To stop the music
    public void StopMusic()
    {
        SdlMixer.Mix_HaltMusic();
    }
}
﻿using System;
using System.Collections.Generic;
using static Ataque;

class Combate : Menu
{
    Sound bgSound;
    Jugador prota;
    Image fondo_opciones;
    Juego juego;
    Font font35;
    Bestia salvaje, seleccionado;
    bool turno, capturando;
    Random r;
    int cantidadMinima, cantidadMaxima;

    public Combate(ref Jugador prota, Bestia salvaje, Juego juego)
    {
        r = new Random();
        bg = new Image("data/fondo_combate.png");
        font24 = new Font("data/Joystix.ttf", 25);
        font35 = new Font("data/Joystix.ttf", 35);
        continuar = true;
        capturando = false;
        turno = true;
        posicionFlecha = 560;
        seleccion = 0;
        this.prota = prota;
        seleccionado = ObtenerPokemonDisponible();
        this.salvaje = salvaje;
        this.juego = juego;
        salvaje.MoveTo(600, 70);
        fondo_opciones = new Image("data/dialogo_combate.png");
        bgSound = new Sound("data/sonidos/combate.mp3");
        cantidadMinima = 500;
        cantidadMaxima = 700;
    }

    public override void DibujarInterfaz()
    {
        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        DibujarInterfazSalvaje();
        DibujarInterfazSeleccionado();
        SdlHardware.DrawHiddenImage(fondo_opciones, 0, 534);
    }

    private void AbrirMochila(ref bool accionRealizada)
    {
        int seleccionMochila = 0;
        int aux = 0;
        int maxOpcionesMochila = prota.GetMochila().Count - 1;
        int posicionFlechaMochila = 300;

        bool objetoElegido = false ;
        Objeto objetoSeleccionado = null;

        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(new Image("data/menu_partidas.png"), 0, 0);
            if (prota.GetMochila().Count == 0)
            {
                SdlHardware.WriteHiddenText("Tu mochila está vacia",
                        100, 150,
                        0xC0, 0xC0, 0xC0,
                        font24);
                SdlHardware.WriteHiddenText("Pulsa <-- para volver ",
                        100, 200,
                        0xC0, 0xC0, 0xC0,
                        font24);
                SdlHardware.WriteHiddenText("al combate",
                        100, 250,
                        0xC0, 0xC0, 0xC0,
                        font24);
            }
            else
            {
                SdlHardware.WriteHiddenText("Pulsa Espacio para consumir un objeto",
                        80, 150,
                        0xC0, 0xC0, 0xC0,
                        font24);
                SdlHardware.WriteHiddenText("Pulsa <-- para volver al combate",
                        80, 200,
                        0xC0, 0xC0, 0xC0,
                        font24);
                SdlHardware.WriteHiddenText("-->",
                        30, Convert.ToInt16(posicionFlechaMochila),
                        0xC0, 0xC0, 0xC0,
                        font24);
                int altura = 300;
                foreach (KeyValuePair<Objeto, int> kp in prota.GetMochila())
                {
                    SdlHardware.WriteHiddenText(kp.Key.Nombre + " x" + kp.Value,
                        100, Convert.ToInt16(altura),
                        0xC0, 0xC0, 0xC0,
                        font35);
                    altura += 50;

                }
                altura = 300;

                if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
                {
                    if (seleccionMochila == maxOpcionesMochila)
                    {
                        seleccionMochila = 0;
                        posicionFlechaMochila = 300;
                    }
                    else
                    {
                        seleccionMochila++;
                        posicionFlechaMochila += 50;
                    }
                }

                if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
                {
                    if (seleccionMochila == 0)
                    {
                        seleccionMochila = maxOpcionesMochila;
                        posicionFlechaMochila = 300 + (maxOpcionesMochila * 50);
                    }
                    else
                    {
                        seleccionMochila--;
                        posicionFlechaMochila -= 50;
                    }
                }

                if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
                {
                    objetoElegido = true;
                    int i = 0;
                    foreach (KeyValuePair<Objeto, int> kp in prota.GetMochila())
                    {
                        if(i == seleccionMochila)
                        {
                            objetoSeleccionado = kp.Key;
                            aux = kp.Value - 1;
                            break;
                        }
                        i++;
                    }
                    
                    SdlHardware.Pause(100);
                }
            }
            SdlHardware.ShowHiddenScreen();
            SdlHardware.Pause(60);
        } while (!objetoElegido &&
            !SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE));
        SdlHardware.Pause(100);

        if(objetoElegido)
        {
            prota.GetMochila().Remove(objetoSeleccionado);
            if (aux > 0)
                prota.GetMochila().Add(objetoSeleccionado, aux);
            do
            {
                DibujarInterfaz();

                SdlHardware.WriteHiddenText(prota.GetNombre() + " usó " +
                    objetoSeleccionado.Nombre,
                            100, 560,
                            0xC0, 0xC0, 0xC0,
                            font35);
                SdlHardware.ShowHiddenScreen();
            } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
            SdlHardware.Pause(100);
            if (objetoSeleccionado.GetType().Name == "Pocion")
            {
                seleccionado.SetVida(
                    seleccionado.GetVida() + ((Pocion)objetoSeleccionado).hpRecuperados
                        <= seleccionado.GetMaxVida() ? seleccionado.GetVida() +
                        ((Pocion)objetoSeleccionado).hpRecuperados : seleccionado.GetMaxVida() );
            }
            accionRealizada = true;
        }
        SdlHardware.Pause(100);
    }

    private Bestia ObtenerPokemonDisponible()
    {
        bool puedeCambiar = false;
        Bestia cambiar = null;
        foreach (Bestia b in prota.GetEquipo())
        {
            if (b.GetVida() > 0 && !puedeCambiar)
            {
                cambiar = b;
                cambiar.MoveTo(100, 350);
                puedeCambiar = true;
            }
        }

        if (!puedeCambiar)
        {
            PerderCombate();
        }
        return cambiar;
    }

    private void PerderCombate()
    {
        do
        {
            DibujarInterfaz();

            SdlHardware.WriteHiddenText("A " + prota.GetNombre() + " ya no le quedan",
                        100, 560,
                        0xC0, 0xC0, 0xC0,
                        font35);
            SdlHardware.WriteHiddenText("más pokemons",
                        100, 590,
                        0xC0, 0xC0, 0xC0,
                        font35);
            SdlHardware.ShowHiddenScreen();
        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        foreach (Bestia b in prota.GetEquipo())
        {
            b.SetVida(b.GetMaxVida());
        }
        continuar = false;
        SdlHardware.Pause(60);
    }

    private void GanarCombate()
    {
        do
        {
            DibujarInterfaz();

            SdlHardware.WriteHiddenText(salvaje.GetNombre() + " enemigo se ha debilitado",
                        80, 560,
                        0xC0, 0xC0, 0xC0,
                        font35);
            SdlHardware.WriteHiddenText("¡Has ganado!",
                        80, 590,
                        0xC0, 0xC0, 0xC0,
                        font35);
            SdlHardware.ShowHiddenScreen();
        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        SdlHardware.Pause(100);
        int dineroGanado = r.Next(cantidadMinima,cantidadMaxima);
        do
        {
            DibujarInterfaz();

            SdlHardware.WriteHiddenText("Obtienes " + dineroGanado + " PokeDólares",
                        80, 560,
                        0xC0, 0xC0, 0xC0,
                        font35);
            SdlHardware.ShowHiddenScreen();
        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        prota.SetDinero(prota.GetDinero() + dineroGanado);
        continuar = false;
        SdlHardware.Pause(60);
    }

    private void DibujarInterfazSalvaje()
    {
        if (capturando)
        {
            SdlHardware.DrawHiddenImage(new Image("data/pokeball.png"), 700, 250);
        }
        else
        {
            salvaje.DrawOnHiddenScreen();
        }
        SdlHardware.WriteHiddenText(salvaje.GetNombre(),
                        100, 100,
                        0, 0, 0,
                        font24);
        SdlHardware.WriteHiddenText("Lvl: " + salvaje.GetNivel().ToString(),
                        100, 130,
                        0, 0, 0,
                        font24);
        SdlHardware.WriteHiddenText("HP: " + salvaje.GetVida().ToString(),
                        100, 160,
                        0, 0, 0,
                        font24);
    }

    private void DibujarInterfazSeleccionado()
    {
        seleccionado.DrawOnHiddenScreen();

        SdlHardware.WriteHiddenText(seleccionado.GetNombre(),
                        650, 400,
                        0, 0, 0,
                        font24);
        SdlHardware.WriteHiddenText("Lvl: " + seleccionado.GetNivel().ToString(),
                        650, 430,
                        0, 0, 0,
                        font24);
        SdlHardware.WriteHiddenText("HP: " + seleccionado.GetVida().ToString(),
                        650, 460,
                        0, 0, 0,
                        font24);
    }

    private void TusAtaques()
    {
        do
        {
            DibujarInterfaz();
            short altura = 560;
            foreach (ataque a in seleccionado.GetAtaques())
            {
                SdlHardware.WriteHiddenText(a.nombre,
                            150, altura,
                            0xC0, 0xC0, 0xC0,
                            font24);
                altura += 50;
            }
            altura = 560;
            SdlHardware.WriteHiddenText("-->",
               50, Convert.ToInt16(posicionFlecha),
               0xC0, 0xC0, 0xC0,
               font24);
            SdlHardware.ShowHiddenScreen();

            if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
            {
                if (seleccion == seleccionado.GetAtaques().Count - 1)
                {
                    seleccion = 0;
                    posicionFlecha = 560;
                }
                else
                {
                    seleccion++;
                    posicionFlecha += 50;
                }
            }
            if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
            {
                if (seleccion == 0)
                {
                    seleccion = seleccionado.GetAtaques().Count - 1;
                    posicionFlecha = Convert.ToInt16(560 + (
                        (seleccionado.GetAtaques().Count - 1) * 50));
                }
                else
                {
                    seleccion--;
                    posicionFlecha -= 50;
                }
            }
            SdlHardware.Pause(40);

        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        SdlHardware.Pause(100);

        do
        {
            DibujarInterfaz();

            SdlHardware.WriteHiddenText(seleccionado.GetNombre() + " usó: ",
                        100, 560,
                        0xC0, 0xC0, 0xC0,
                        font35);
            SdlHardware.WriteHiddenText(seleccionado.GetAtaques()[seleccion].nombre,
                        100, 590,
                        0xC0, 0xC0, 0xC0,
                        font35);
            SdlHardware.ShowHiddenScreen();
        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        SdlHardware.Pause(100);
        salvaje.SetVida(
            (salvaje.GetVida() - seleccionado.GetAtaques()[seleccion].poder) >= 0 ?
               (salvaje.GetVida() - seleccionado.GetAtaques()[seleccion].poder) : 0);
    }

    private void CapturarPokemon() 
    {
        SdlHardware.Pause(100);
        capturando = true;
        bool haEscapado = false;
        int tickBall = 1;
        string cadenaPuntos = "";
        do
        {
            DibujarInterfaz();
            SdlHardware.WriteHiddenText("Capturando a " + salvaje.GetNombre(),
                        100, 560,
                        0xC0, 0xC0, 0xC0,
                        font35);
            for (int i = 0; i < tickBall; i++)
                cadenaPuntos += ". ";

            SdlHardware.WriteHiddenText(cadenaPuntos,
                        100, 590,
                        0xC0, 0xC0, 0xC0,
                        font35);

            SdlHardware.ShowHiddenScreen();

            if (r.Next(1, 100) >= 70 ? true : false)
            {
                haEscapado = true;
            }
            else
            {
                tickBall++;
            }
            if (tickBall == 3)
                capturando = false;
        } while (capturando && !haEscapado);

        if(haEscapado)
        {
            do
            {
                DibujarInterfaz();
                SdlHardware.WriteHiddenText("¡El pokemon ha escapado!",
                            100, 560,
                            0xC0, 0xC0, 0xC0,
                            font35);
                SdlHardware.ShowHiddenScreen();
            } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
            capturando = false;
        }
        else
        {
            PokemonCapturado();
        }
    }

    private void PokemonCapturado()
    {
        do
        {
            DibujarInterfaz();
            SdlHardware.WriteHiddenText("¡Has capturado a " + salvaje.GetNombre() + "!",
                        100, 560,
                        0xC0, 0xC0, 0xC0,
                        font35);
            SdlHardware.ShowHiddenScreen();
        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        SdlHardware.Pause(100);

        bool yaLoTiene = false;
        foreach (Bestia b in prota.GetEquipo())
        {
            if (b.GetNombre() == salvaje.GetNombre())
                yaLoTiene = true;
        }
        foreach(Bestia b in prota.GetCaja())
        {
            if (b.GetNombre() == salvaje.GetNombre())
                yaLoTiene = true;
        }

        if(!yaLoTiene)
            prota.SetPokemonsDiferentesCapturados(
                prota.GetPokemonsDiferentesCapturados() + 1);

        if (prota.GetEquipo().Count == 6)
        {
            do
            {
                DibujarInterfaz();
                SdlHardware.WriteHiddenText(salvaje.GetNombre() + " fue enviado ",
                            100, 560,
                            0xC0, 0xC0, 0xC0,
                            font35);
                SdlHardware.WriteHiddenText("al pc",
                            100, 610,
                            0xC0, 0xC0, 0xC0,
                            font35);
                SdlHardware.ShowHiddenScreen();
            } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));

            prota.GetCaja().Add(salvaje);
        }
        else
        {
            prota.GetEquipo().Add(salvaje);
        }

        capturando = false;
        continuar = false;
    }

    private void DibujarTuTurno()
    {
        SdlHardware.WriteHiddenText("Atacar",
                150, 560,
                0xC0, 0xC0, 0xC0,
                font24);

        SdlHardware.WriteHiddenText("Capturar Pokemon",
            150, 610,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Mochila",
            150, 660,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Huir",
            150, 710,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("-->",
            50, Convert.ToInt16(posicionFlecha),
            0xC0, 0xC0, 0xC0,
            font24);
    }

    private void TuTurno()
    {
        bool accion = false;

        do
        {
            DibujarInterfaz();
            DibujarTuTurno();
            SdlHardware.ShowHiddenScreen();

            if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
            {
                if (seleccion == 3)
                {
                    seleccion = 0;
                    posicionFlecha = 560;
                }
                else
                {
                    seleccion++;
                    posicionFlecha += 50;
                }
            }
            if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
            {
                if (seleccion == 0)
                {
                    seleccion = 3;
                    posicionFlecha = 710;
                }
                else
                {
                    seleccion--;
                    posicionFlecha -= 50;
                }
            }
            SdlHardware.Pause(40);
        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        SdlHardware.Pause(100);

        switch (seleccion)
        {
            case 0: TusAtaques(); break;
            case 1: CapturarPokemon(); break;
            case 2: AbrirMochila(ref accion);
                if(!accion)
                    TuTurno(); break;
            case 3: ComprobarPoderHuir(); break;
        }
    }

    private void ComprobarPoderHuir()
    {
        if (r.Next(1, 100) >= 50 ? true : false)
        {
            do
            {
                DibujarInterfaz();
                SdlHardware.WriteHiddenText("¡No has podido escapar!",
                            100, 560,
                            0xC0, 0xC0, 0xC0,
                            font35);
                SdlHardware.ShowHiddenScreen();
            } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        }
        else
        {
            do
            {
                DibujarInterfaz();
                SdlHardware.WriteHiddenText("Escapaste sin problemas",
                            100, 560,
                            0xC0, 0xC0, 0xC0,
                            font35);
                SdlHardware.ShowHiddenScreen();
            } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
            continuar = false;
        }
    }

    private void RivalAtaca()
    {
        int indice = r.Next(0, salvaje.GetAtaques().Count);
        do
        {
            DibujarInterfaz();

            SdlHardware.WriteHiddenText(salvaje.GetNombre() + " enemigo usó: ",
                        100, 560,
                        0xC0, 0xC0, 0xC0,
                        font35);
            SdlHardware.WriteHiddenText(salvaje.GetAtaques()[indice].nombre,
                        100, 590,
                        0xC0, 0xC0, 0xC0,
                        font35);
            SdlHardware.ShowHiddenScreen();
        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        SdlHardware.Pause(100);
        seleccionado.SetVida(
            (seleccionado.GetVida() - salvaje.GetAtaques()[indice].poder) >= 0 ?
               (seleccionado.GetVida() - salvaje.GetAtaques()[indice].poder) : 0);
    }

    private void ComprobarVidas()
    {
        if (seleccionado.GetVida() == 0)
        {
            do
            {
                DibujarInterfaz();
                SdlHardware.WriteHiddenText(seleccionado.GetNombre() + " se ha debilitado",
                            100, 560,
                            0xC0, 0xC0, 0xC0,
                            font35);
                SdlHardware.ShowHiddenScreen();
            } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
            SdlHardware.Pause(100);
            seleccionado = ObtenerPokemonDisponible();
        }
        if (salvaje.GetVida() == 0)
            GanarCombate();
    }

    public void Run()
    {
        bgSound.BackgroundPlay();
        do
        {
            if (turno)
                TuTurno();
            else
                RivalAtaca();

            ComprobarVidas();
            turno = turno ? false : true;
            SdlHardware.Pause(40);
        } while (continuar);
        SdlHardware.ScrollTo(juego.viejoScrollX, juego.viejoScrollY);
        bgSound.StopMusic();
    }
}
﻿public class Edificio : Sprite
{
    public Edificio(string nombre)
        : base(nombre)
    {
        startX = -1;
        startY = -1;
        width = 300;
        height = 300;
        visible = true;
    }
}

﻿using System;

class CargarPartida : Menu
{   
    protected bool partidaElegida;
    protected string nombrePartida;
    Mapa cargarMapa;

    public CargarPartida()
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 20);
        seleccion = 1;
        posicionFlecha = 200;
        partidaElegida = false;
        cargarMapa = new Mapa();
        cargarMapa.CargarMapa("data/mapa.txt");
        CargarNombres(ref listaNombres);
        try
        {
            maxOpciones = listaNombres.Count;
        }
        catch(Exception e)
        {
            maxOpciones = 0;
        }
    }

    public override void DibujarInterfaz()
    {
        if (maxOpciones == 0)
        {
            SinPartidas();
        }
        else
        {
            do
            {
                SdlHardware.ClearScreen();
                SdlHardware.DrawHiddenImage(bg, 0, 0);
                SdlHardware.WriteHiddenText("Elige una partida",
                    100, 50,
                    0xC0, 0xC0, 0xC0,
                    font24);
                SdlHardware.WriteHiddenText("Pulsa <-- para volver",
                    100, 100,
                    0xC0, 0xC0, 0xC0,
                    font24);

                short altura = 200;
                foreach (string s in listaNombres)
                {
                    if (s.Length > 0)
                    {
                        SdlHardware.WriteHiddenText(s,
                        100, altura,
                        0xC0, 0xC0, 0xC0,
                        font24);
                        altura += 100;
                    }
                    else
                        SinPartidas();
                }

                SdlHardware.WriteHiddenText("<--",
                        500, posicionFlecha,
                        0xC0, 0xC0, 0xC0,
                        font24);

                SdlHardware.ShowHiddenScreen();

                if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
                {
                    if (seleccion == maxOpciones)
                    {
                        seleccion = 1;
                        posicionFlecha = 200;
                    }
                    else
                    {
                        seleccion++;
                        posicionFlecha += 100;
                    }
                }
                if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
                {
                    if (seleccion == 1)
                    {
                        seleccion = maxOpciones;
                        posicionFlecha = Convert.ToInt16(200 + (100 * (maxOpciones - 1)));
                    }
                    else
                    {
                        seleccion--;
                        posicionFlecha -= 100;
                    }
                }
                SdlHardware.ShowHiddenScreen();
                if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
                {
                    partidaElegida = true;
                    nombrePartida = listaNombres[seleccion - 1];
                    SdlHardware.Pause(100);
                }
                if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE))
                {
                    SdlHardware.Pause(100);
                    new MenuPartidas().Run();
                }
                SdlHardware.Pause(40);
            } while (!partidaElegida);
        }
    }
    public void Run()
    {
            DibujarInterfaz();
            Jugador nuevoJugador = new Jugador();
            nuevoJugador.cargarJugador("partidas/"+ nombrePartida + ".txt", ref fondo, ref dialogo);
            Juego j = new Juego(nuevoJugador, fondo, dialogo);
            j.Run();
        
    }
}
﻿/**
 * Font.cs - To hide SDL TTF font handling
 * 
 * Changes:
 * 0.01, 24-jul-2013: Initial version, based on SdlMuncher 0.02
 */

using System;
using Tao.Sdl;

class Font
{
    private IntPtr internalPointer;

    public Font(string fileName, short sizePoints)
    {
        Load(fileName, sizePoints);
    }

    public void Load(string fileName, short sizePoints)
    {
        internalPointer = SdlTtf.TTF_OpenFont(fileName, sizePoints);
        if (internalPointer == IntPtr.Zero)
            SdlHardware.FatalError("Font not found: " + fileName);
    }

    public IntPtr GetPointer()
    {
        return internalPointer;
    }
}

﻿public class Hierba : Sprite
{
    public Hierba(string nombre)
        : base(nombre)
    {
        startX = -1;
        startY = -1;
        width = 40;
        height = 40;
        visible = true;
    }
}
﻿/**
 * Image.cs - To hide SDL image handling
 * 
 * Changes:
 * 0.01, 24-jul-2013: Initial version, based on SdlMuncher 0.02
 */



using System;
using Tao.Sdl;


public class Image
{
    private IntPtr internalPointer;
    public string nombre { get; set; }

    public Image(string fileName)  // Constructor
    {
        nombre = fileName;
        Load(fileName);
    }

    public void Load(string fileName)
    {
        internalPointer = SdlImage.IMG_Load(fileName);
        if (internalPointer == IntPtr.Zero)
            SdlHardware.FatalError("Image not found: " + fileName);
    }


    public IntPtr GetPointer()
    {
        return internalPointer;
    }
}

﻿class InformacionJugador : Menu
{
    Jugador prota;

    public InformacionJugador(Jugador prota)
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 30);
        continuar = true;
        this.prota = prota;
    }

    public void DetectarTeclas()
    {
        SdlHardware.ShowHiddenScreen();
        if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE))
        {
            SdlHardware.Pause(100);
            continuar = false;
        }
    }

    public override void DibujarInterfaz()
    {

        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        SdlHardware.WriteHiddenText("Nombre: " + prota.GetNombre(),
            100, 50,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Genero: " + prota.GetGenero(),
            100, 100,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Dinero: " + prota.GetDinero() + " PokeDólares",
            100, 150,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Nº Pokemons diferentes ",
            100, 200,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("atrapados: " + prota.GetPokemonsDiferentesCapturados(),
            100, 250,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Medallas: 0", 
           100, 300,
           0xC0, 0xC0, 0xC0,
           font24);
        SdlHardware.WriteHiddenText("Tiempo Jugado: " + prota.GetTiempoJugado(),
           100, 350,
           0xC0, 0xC0, 0xC0,
           font24);

        SdlHardware.WriteHiddenText("Pulsa <-- para salir",
                100, 450,
                0xC0, 0xC0, 0xC0,
                font24);

        SdlHardware.ShowHiddenScreen();

        SdlHardware.Pause(40);


    }
    public void Run()
    {
        do
        {
            DibujarInterfaz();
            DetectarTeclas();
        } while (continuar);
    }
}
﻿class Instrucciones : Menu
{
    public Instrucciones()
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 20);
        continuar = true;
    }

    public void DetectarTeclas()
    {
        SdlHardware.ShowHiddenScreen();
        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            SdlHardware.Pause(100);
            continuar = false;
        }
    }

    public override void DibujarInterfaz()
    {

        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        SdlHardware.WriteHiddenText("Instrucciones: ",
            100, 50,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("-Pulsa las flechas de control",
            100, 150,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("para moverte / seleccionar opciones",
            100, 200,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("-Pulsa Espacio para hablar con ",
            100, 300,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Personajes/Interactuar en general",
            100, 350,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("-Pulsa M para acceder al menú ",
            100, 450,
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.WriteHiddenText("Pulsa Espacio para cerrar esto",
                100, 600,
                0xC0, 0xC0, 0xC0,
                font24);

        SdlHardware.ShowHiddenScreen();

        SdlHardware.Pause(40);


    }
    public void Run()
    {
        do
        {
            DibujarInterfaz();
            DetectarTeclas();
        } while (continuar);
    }
}
﻿using System.Collections.Generic;

public class Npc : Sprite
{
    public List<string> Dialogo { get; set; }
    public int IndiceDialogo { get; set; }
    public bool Hablando { get; set; }

    public Npc(string[][] direcciones)
    {
        width = 34;
        height = 48;
        Dialogo = new List<string>();
        IndiceDialogo = 0;
        Hablando = false;
        LoadSequence(RIGHT, direcciones[0]);
        LoadSequence(LEFT, direcciones[1]);
        LoadSequence(UP, direcciones[2]);
        LoadSequence(DOWN, direcciones[3]);
    }
}
﻿public class Arbol : Sprite
{
    public Arbol(string nombre)
        : base(nombre)
    {
        startX = -1;
        startY = -1;
        width = 36;
        height = 46;
        visible = true;
    }
}
﻿using System.Collections.Generic;
using System.IO;
using System;

abstract class Menu
{
    protected Image bg;
    protected Font font24;
    protected int seleccion, maxOpciones, scrollX, scrollY;
    protected short posicionFlecha;
    protected List<string> listaNombres;
    protected bool continuar;
    protected Sprite fondo, dialogo;

    public Menu()
    {
        fondo = new Sprite("data/fondo_juego.jpg");
        dialogo = new Sprite("data/fondo_dialogo.png");
        scrollX = scrollY = 0;
        dialogo.MoveTo(0,550);
    }

    protected void SinPartidas()
    {
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("No hay partidas disponibles",
                50, 100,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa <-- para volver al menu principal",
                50, 300,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.ShowHiddenScreen();
        }
        while (!SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE));
        SdlHardware.Pause(100);
        Pokemon.Run();
    }

    protected void CargarNombres(ref List<string> listaNombres)
    {
        try
        {
            listaNombres = new List<string>(File.ReadAllLines("partidas/listaPartidas.txt"));
        }
        catch (Exception e)
        {
            Menu.Error();
        }

        for(int i = 0; i < listaNombres.Count; i++)
        {
            if (listaNombres[i].Length < 0)
                listaNombres.RemoveAt(i);
        }
    }

    public virtual void DibujarInterfaz()
    {

    }

    public static void Error()
    {
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(new Image("data/menu_partidas.png"), 0, 0);
            SdlHardware.WriteHiddenText("Error Fatal",
                    100, 50,
                    0xC0, 0xC0, 0xC0,
                    new Font("data/Joystix.ttf", 24));
            SdlHardware.WriteHiddenText("Pulsa Espacio para volver",
                    100, 100,
                    0xC0, 0xC0, 0xC0,
                    new Font("data/Joystix.ttf", 24));
            SdlHardware.WriteHiddenText("a la pantalla principal",
                    100, 150,
                    0xC0, 0xC0, 0xC0,
                    new Font("data/Joystix.ttf", 24));
            SdlHardware.ShowHiddenScreen();

        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        SdlHardware.Pause(100);
        Pokemon.Run();
    }
}
﻿using System;

public class Pc : Sprite
{
    Jugador prota;
    Font font24;
    Image bg;
    int seleccion, maxOpciones;
    short posicionFlecha;
    bool fallo;
    string mensajeError;

    public Pc(string nombreImagen)
        : base(nombreImagen)
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 20);
        seleccion = 1;
        maxOpciones = 2;
        posicionFlecha = 200;
        fallo = false;
    }

    private void DibujarInterfaz()
    {

        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        SdlHardware.WriteHiddenText("Pc de " + prota.GetNombre(),
            100, 50,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Pulsa <-- para volver",
            200, 100,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Sacar Pokemon",
            100, 200,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Dejar Pokemon",
            100, 250,
            0xC0, 0xC0, 0xC0,
            font24);
        if (fallo)
        {
            SdlHardware.WriteHiddenText(mensajeError,
            100, 400,
            0xC0, 0xC0, 0xC0,
            font24);
        }
        SdlHardware.WriteHiddenText("<--",
                400, posicionFlecha,
                0xC0, 0xC0, 0xC0,
                font24);

        SdlHardware.ShowHiddenScreen();

        SdlHardware.Pause(40);
    }

    private void SacarPokemon()
    {
        int indicePc = 0;
        bool continuar = true;

        if(prota.GetCaja().Count == 0)
        {
            do
            {
                SdlHardware.ClearScreen();
                SdlHardware.DrawHiddenImage(bg, 0, 0);
                SdlHardware.WriteHiddenText("No hay pokemons en tu PC",
                                100, 50,
                                0xC0, 0xC0, 0xC0,
                                font24);
                SdlHardware.WriteHiddenText("Pulsa <-- para volver atrás",
                    100, 100,
                    0xC0, 0xC0, 0xC0,
                    font24);
                SdlHardware.ShowHiddenScreen();
            } while (!SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE));
            return;
        }

        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("Caja",
                100, 50,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa <-- para volver atrás",
                100, 100,
                0xC0, 0xC0, 0xC0,
                font24);

            SdlHardware.WriteHiddenText("Usa Arriba o Abajo para moverte",
                100, 150,
                0xC0, 0xC0, 0xC0,
                font24);

            SdlHardware.WriteHiddenText((indicePc + 1)+"/"+prota.GetCaja().Count,
                100, 200,
                0xC0, 0xC0, 0xC0,
                font24);

            SdlHardware.WriteHiddenText(prota.GetCaja()[indicePc].GetNombre(),
                        150, 250,
                        0xC0, 0xC0, 0xC0,
                        font24);

            SdlHardware.DrawHiddenImage(prota.GetCaja()[indicePc].image, 768, 150);
            SdlHardware.WriteHiddenText("Lvl: " + prota.GetCaja()[indicePc].GetNivel(),
               550, 250,
               0xC0, 0xC0, 0xC0,
               font24);
            SdlHardware.WriteHiddenText("Hp: " + prota.GetCaja()[indicePc].GetVida(),
               550, 300,
               0xC0, 0xC0, 0xC0,
               font24);
            SdlHardware.ShowHiddenScreen();

            if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
            {
                if (indicePc == prota.GetCaja().Count - 1)
                {
                    indicePc = 0;
                }
                else
                {
                    indicePc++;
                }
            }
            if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
            {
                if (indicePc == 0)
                {
                    indicePc = prota.GetCaja().Count - 1;
                }
                else
                {
                    indicePc--;
                }
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
            {
                SdlHardware.Pause(100);
                prota.GetEquipo().Add(prota.GetCaja()[indicePc]);
                prota.GetCaja().RemoveAt(indicePc);
                continuar = false;
            }

            if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE))
            {
                continuar = false;
            }

            SdlHardware.Pause(100);
        } while (continuar);
    }

    private void DejarPokemon()
    {
        int indiceEquipo = 0;
        int posicionFlecha = 150;
        bool continuar = true;

        do
        {
            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("Equipo",
                100, 50,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa <-- para volver atrás",
                100, 100,
                0xC0, 0xC0, 0xC0,
                font24);
            short altura = 150;
            foreach (Bestia b in prota.GetEquipo())
            {
                SdlHardware.WriteHiddenText(b.GetNombre(),
                            150, altura,
                            0xC0, 0xC0, 0xC0,
                            font24);
                altura += 50;
            }
            altura = 150;
            SdlHardware.WriteHiddenText("-->",
               40, Convert.ToInt16(posicionFlecha),
               0xC0, 0xC0, 0xC0,
               font24);
            SdlHardware.DrawHiddenImage(prota.GetEquipo()[indiceEquipo].image, 768, 150);
            SdlHardware.WriteHiddenText("Lvl: " + prota.GetEquipo()[indiceEquipo].GetNivel(),
               550, 150,
               0xC0, 0xC0, 0xC0,
               font24);
            SdlHardware.WriteHiddenText("Hp: " + prota.GetEquipo()[indiceEquipo].GetVida(),
               550, 200,
               0xC0, 0xC0, 0xC0,
               font24);
            SdlHardware.ShowHiddenScreen();

            if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
            {
                if (indiceEquipo == prota.GetEquipo().Count - 1)
                {
                    indiceEquipo = 0;
                    posicionFlecha = 150;
                }
                else
                {
                    indiceEquipo++;
                    posicionFlecha += 50;
                }
            }
            if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
            {
                if (indiceEquipo == 0)
                {
                    indiceEquipo = prota.GetEquipo().Count - 1;
                    posicionFlecha = Convert.ToInt16(150 + ((prota.GetEquipo().Count - 1) * 50));
                }
                else
                {
                    indiceEquipo--;
                    posicionFlecha -= 50;
                }
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
            {
                SdlHardware.Pause(100);
                prota.GetCaja().Add(prota.GetEquipo()[indiceEquipo]);
                prota.GetEquipo().RemoveAt(indiceEquipo);
                continuar = false;
            }

            if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE))
            {
                continuar = false;
            }

            SdlHardware.Pause(100);
        } while (continuar);
    }

    private void DetectarTeclas()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
        {
            if (seleccion == maxOpciones)
            {
                seleccion = 1;
                posicionFlecha = 200;
            }
            else
            {
                seleccion++;
                posicionFlecha += 50;
            }
        }
        if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
        {
            if (seleccion == 1)
            {
                seleccion = maxOpciones;
                posicionFlecha = 250;
            }
            else
            {
                seleccion--;
                posicionFlecha -= 50;
            }
        }
        SdlHardware.ShowHiddenScreen();
        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            SdlHardware.Pause(100);
            switch (seleccion)
            {
                case 1:
                    if (prota.GetEquipo().Count == 6)
                    {
                        fallo = true;
                        mensajeError = "Ya tienes 6 pokemon";
                    }
                    else
                    {
                        fallo = false;
                        SacarPokemon();
                    }
                    break;
                case 2:
                    if (prota.GetEquipo().Count == 1)
                    {
                        fallo = true;
                        mensajeError = "No puedes tener 0 pokemon";
                    }
                    else
                    {
                        fallo = false;
                        DejarPokemon();
                    }
                    break;
            }
            SdlHardware.Pause(100);
        }
    }

    public void Run(ref Jugador prota, short scrollX, short scrollY)
    {
        this.prota = prota;

        do
        {
            DibujarInterfaz();
            DetectarTeclas();
            SdlHardware.Pause(60);
        } while (!SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE));
        SdlHardware.ScrollTo(scrollX, scrollX);
    }
}
﻿using System;
using System.Collections.Generic;

class MenuJugador : Menu
{
    Jugador prota;
    Juego juego;

    public MenuJugador(Jugador prota, Sprite fondo, Sprite dialogo, Juego juego)
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 30);
        seleccion = 1;
        posicionFlecha = 200;
        maxOpciones = 7;
        continuar = true;
        this.prota = prota;
        this.fondo = fondo;
        this.dialogo = dialogo;
        this.juego = juego;
    }

    private void GuardadoCompletado()
    {
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("La partida se ha guardado",
                100, 50,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("satisfactoriamente",
                100, 100,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa Espacio para salir de ",
                100, 200,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("la partida",
                100, 250,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa <-- para volver atrás",
                100, 400,
                0xC0, 0xC0, 0xC0,
                font24);

            if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
            {
                SdlHardware.Pause(100);
                Pokemon.Run();
            }

            SdlHardware.ShowHiddenScreen();

        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC) &&
            !SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE));
    }

    public void DetectarTeclas()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
        {
            if (seleccion == maxOpciones)
            {
                seleccion = 1;
                posicionFlecha = 200;
            }
            else
            {
                seleccion++;
                posicionFlecha += 50;
            }
        }
        if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
        {
            if (seleccion == 1)
            {
                seleccion = maxOpciones;
                posicionFlecha = 500;
            }
            else
            {
                seleccion--;
                posicionFlecha -= 50;
            }
        }
        SdlHardware.ShowHiddenScreen();
        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            
            switch(seleccion)
            {
                case 1:;Pokedex p = new Pokedex(prota); p.Run(); break;
                case 2: EquipoJugador ej = new EquipoJugador(prota); ej.Run() ; break;
                case 3:VerMochila(); break;
                case 4: InformacionJugador ij = new InformacionJugador(prota); ij.Run() ; break;
                case 5: prota.guardarJugador("partidas/" + prota.GetNombre() +
                    ".txt", ref fondo, ref dialogo, juego.viejoScrollX, juego.viejoScrollY);
                    GuardadoCompletado(); break;
                case 6: Instrucciones i = new Instrucciones() ; i.Run() ; break;
                case 7: continuar = false; ; break;
            }  
            SdlHardware.Pause(100);
        }
        if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE))
        {
            SdlHardware.Pause(100);
            continuar = false;
        }
    }

    private void VerMochila()
    {
        int actual = 0;
        int maxOpciones = prota.GetMochila().Count - 1;
        do
        {
            SdlHardware.ClearScreen();
            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("Mochila",
                100, 50,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa <-- para volver atrás",
                100, 100,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Pulsa ARRIBA o ABAJO para ", 
                100, 200,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("pasar objetos", 
                100, 250,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText((actual + 1) + "/" + prota.GetMochila().Count,
                100, 300,
                0xC0, 0xC0, 0xC0,
                font24);
            int i = 0;
            foreach (KeyValuePair<Objeto,int> kp in prota.GetMochila())
            {
                if (i == actual)
                {
                    SdlHardware.WriteHiddenText(kp.Key.Nombre + " x" + kp.Value,
                        100, 350,
                        0xC0, 0xC0, 0xC0,
                        font24);
                }
                i++;
            }
            SdlHardware.ShowHiddenScreen();
            if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
            {
                if (actual == maxOpciones)
                    actual = 0;
                else
                    actual++;
            }
            if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
            {
                if (actual == 0)
                    actual = maxOpciones;
                else
                    actual--;
            }
            SdlHardware.Pause(100);
        } while (!SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE));
    }

    public override void DibujarInterfaz()
    {

        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        SdlHardware.WriteHiddenText("Menú",
            100, 50,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Pulsa <-- para volver",
            100, 100,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Pokedex",
            100, 200,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Equipo",
            100, 250,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Mochila",
            100, 300,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Entrenador",
            100, 350,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Guardar Partida",
            100, 400,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Instrucciones",
            100, 450,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Volver a la partida",
            100, 500,
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.WriteHiddenText("-->",
                30, posicionFlecha,
                0xC0, 0xC0, 0xC0,
                font24);

        SdlHardware.ShowHiddenScreen();

        SdlHardware.Pause(40);
    }
    public void Run()
    {
        do
        {
            DibujarInterfaz();
            DetectarTeclas();
        } while (continuar);
        SdlHardware.ScrollTo(juego.viejoScrollX,juego.viejoScrollY);
    }
}
﻿/**
 * SdlHardware.cs - Hides SDL library
 * 
 * Nacho Cabanes, 2013
 * 
 * Changes:
 * 0.01, 24-jul-2013: Initial version, based on SdlMuncher 0.14
 */

using System.IO;
using System.Threading;
using Tao.Sdl;
using System;

class SdlHardware
{
    static IntPtr hiddenScreen;
    static short width, height;

    public static short startX, startY; // For Scroll

    static bool isThereJoystick;
    static IntPtr joystick;

    static int mouseClickLapse;
    static int lastMouseClick;


    public static void Init(short w, short h, int colors, bool fullScreen)
    {
        width = w;
        height = h;

        int flags = Sdl.SDL_HWSURFACE | Sdl.SDL_DOUBLEBUF | Sdl.SDL_ANYFORMAT;
        if (fullScreen)
            flags |= Sdl.SDL_FULLSCREEN;
        Sdl.SDL_Init(Sdl.SDL_INIT_EVERYTHING);
        hiddenScreen = Sdl.SDL_SetVideoMode(
            width,
            height,
            colors,
            flags);

        Sdl.SDL_Rect rect2 =
            new Sdl.SDL_Rect(0, 0, (short)width, (short)height);
        Sdl.SDL_SetClipRect(hiddenScreen, ref rect2);

        SdlTtf.TTF_Init();

        // Joystick initialization
        isThereJoystick = true;
        if (Sdl.SDL_NumJoysticks() < 1)
            isThereJoystick = false;

        if (isThereJoystick)
        {
            joystick = Sdl.SDL_JoystickOpen(0);
            if (joystick == IntPtr.Zero)
                isThereJoystick = false;
        }

        SdlMixer.Mix_OpenAudio(22050,
             (short)SdlMixer.MIX_DEFAULT_FORMAT, 2, 1024);

        // Time lapse between two consecutive mouse clicks,
        // so that they are not too near
        mouseClickLapse = 10;
        lastMouseClick = Sdl.SDL_GetTicks();
    }

    public static void ClearScreen()
    {
        Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, width, height);
        Sdl.SDL_FillRect(hiddenScreen, ref origin, 0);
    }

    public static void DrawHiddenImage(Image image, int x, int y)
    {
        drawHiddenImage(image.GetPointer(), x + startX, y + startY);
    }

    public static void ShowHiddenScreen()
    {
        Sdl.SDL_Flip(hiddenScreen);
    }

    public static bool KeyPressed(int c)
    {
        bool pressed = false;
        Sdl.SDL_PumpEvents();
        Sdl.SDL_Event myEvent;
        Sdl.SDL_PollEvent(out myEvent);
        int numkeys;
        byte[] keys = Tao.Sdl.Sdl.SDL_GetKeyState(out numkeys);
        if (keys[c] == 1)
            pressed = true;
        return pressed;
    }

    public static void Pause(int milisegundos)
    {
        Thread.Sleep(milisegundos);
    }

    public static int GetWidth()
    {
        return width;
    }

    public static int GetHeight()
    {
        return height;
    }

    public static void FatalError(string text)
    {
        StreamWriter sw = File.AppendText("errors.log");
        sw.WriteLine(text);
        sw.Close();
        Console.WriteLine(text);
        Environment.Exit(1);
    }

    public static void WriteHiddenText(string txt,
        short x, short y, byte r, byte g, byte b, Font f)
    {
        Sdl.SDL_Color color = new Sdl.SDL_Color(r, g, b);
        IntPtr textoComoImagen = SdlTtf.TTF_RenderText_Solid(
            f.GetPointer(), txt, color);
        if (textoComoImagen == IntPtr.Zero)
            Environment.Exit(5);

        Sdl.SDL_Rect origen = new Sdl.SDL_Rect(0, 0, width, height);
        Sdl.SDL_Rect dest = new Sdl.SDL_Rect(
            (short)(x + startX), (short)(y + startY),
            width, height);

        Sdl.SDL_BlitSurface(textoComoImagen, ref origen,
            hiddenScreen, ref dest);
    }

    // Scroll Methods

    public static void ResetScroll()
    {
        startX = startY = 0;
    }

    public static void ScrollTo(short newStartX, short newStartY)
    {
        startX = newStartX;
        startY = newStartY;
    }

    public static void ScrollHorizontally(short xDespl)
    {
        startX += xDespl;
    }

    public static void ScrollVertically(short yDespl)
    {
        startY += yDespl;
    }

    // Joystick methods

    /** JoystickPressed: returns TRUE if
        *  a certain button in the joystick/gamepad
        *  has been pressed
        */
    public static bool JoystickPressed(int boton)
    {
        if (!isThereJoystick)
            return false;

        if (Sdl.SDL_JoystickGetButton(joystick, boton) > 0)
            return true;
        else
            return false;
    }

    /** JoystickMoved: returns TRUE if
        *  the joystick/gamepad has been moved
        *  up to the limit in any direction
        *  Then, int returns the corresponding
        *  X (1=right, -1=left)
        *  and Y (1=down, -1=up)
        */
    public static bool JoystickMoved(out int posX, out int posY)
    {
        posX = 0; posY = 0;
        if (!isThereJoystick)
            return false;

        posX = Sdl.SDL_JoystickGetAxis(joystick, 0);  // Leo valores (hasta 32768)
        posY = Sdl.SDL_JoystickGetAxis(joystick, 1);
        // Normalizo valores
        if (posX == -32768) posX = -1;  // Normalizo, a -1, +1 o 0
        else if (posX == 32767) posX = 1;
        else posX = 0;
        if (posY == -32768) posY = -1;
        else if (posY == 32767) posY = 1;
        else posY = 0;

        if ((posX != 0) || (posY != 0))
            return true;
        else
            return false;
    }


    /** JoystickMovedRight: returns TRUE if
        *  the joystick/gamepad has been moved
        *  completely to the right
        */
    public static bool JoystickMovedRight()
    {
        if (!isThereJoystick)
            return false;

        int posX = 0, posY = 0;
        if (JoystickMoved(out posX, out posY) && (posX == 1))
            return true;
        else
            return false;
    }

    /** JoystickMovedLeft: returns TRUE if
        *  the joystick/gamepad has been moved
        *  completely to the left
        */
    public static bool JoystickMovedLeft()
    {
        if (!isThereJoystick)
            return false;

        int posX = 0, posY = 0;
        if (JoystickMoved(out posX, out posY) && (posX == -1))
            return true;
        else
            return false;
    }


    /** JoystickMovedUp: returns TRUE if
        *  the joystick/gamepad has been moved
        *  completely upwards
        */
    public static bool JoystickMovedUp()
    {
        if (!isThereJoystick)
            return false;

        int posX = 0, posY = 0;
        if (JoystickMoved(out posX, out posY) && (posY == -1))
            return true;
        else
            return false;
    }


    /** JoystickMovedDown: returns TRUE if
        *  the joystick/gamepad has been moved
        *  completely downwards
        */
    public static bool JoystickMovedDown()
    {
        if (!isThereJoystick)
            return false;

        int posX = 0, posY = 0;
        if (JoystickMoved(out posX, out posY) && (posY == 1))
            return true;
        else
            return false;
    }


    /** GetMouseX: returns the current X
        *  coordinate of the mouse position
        */
    public static int GetMouseX()
    {
        int posX = 0, posY = 0;
        Sdl.SDL_PumpEvents();
        Sdl.SDL_GetMouseState(out posX, out posY);
        return posX;
    }


    /** GetMouseY: returns the current Y
        *  coordinate of the mouse position
        */
    public static int GetMouseY()
    {
        int posX = 0, posY = 0;
        Sdl.SDL_PumpEvents();
        Sdl.SDL_GetMouseState(out posX, out posY);
        return posY;
    }


    /** MouseClicked: return TRUE if
        *  the (left) mouse button has been clicked
        */
    public static bool MouseClicked()
    {
        int posX = 0, posY = 0;

        Sdl.SDL_PumpEvents();

        // To avoid two consecutive clicks
        int now = Sdl.SDL_GetTicks();
        if (now - lastMouseClick < mouseClickLapse)
            return false;

        // Ahora miro si realmente hay pulsación
        if ((Sdl.SDL_GetMouseState(out posX, out posY) & Sdl.SDL_BUTTON(1)) == 1)
        {
            lastMouseClick = now;
            return true;
        }
        else
            return false;
    }


    // Private (auxiliar) methods

    private static void drawHiddenImage(IntPtr image, int x, int y)
    {
        Sdl.SDL_Rect origin = new Sdl.SDL_Rect(0, 0, width, height);
        Sdl.SDL_Rect dest = new Sdl.SDL_Rect((short)x, (short)y,
            width, height);
        Sdl.SDL_BlitSurface(image, ref origin, hiddenScreen, ref dest);
    }


    // Alternate key definitions

    public static int KEY_ESC = Sdl.SDLK_ESCAPE;
    public static int KEY_SPC = Sdl.SDLK_SPACE;
    public static int KEY_A = Sdl.SDLK_a;
    public static int KEY_B = Sdl.SDLK_b;
    public static int KEY_C = Sdl.SDLK_c;
    public static int KEY_D = Sdl.SDLK_d;
    public static int KEY_E = Sdl.SDLK_e;
    public static int KEY_F = Sdl.SDLK_f;
    public static int KEY_G = Sdl.SDLK_g;
    public static int KEY_H = Sdl.SDLK_h;
    public static int KEY_I = Sdl.SDLK_i;
    public static int KEY_J = Sdl.SDLK_j;
    public static int KEY_K = Sdl.SDLK_k;
    public static int KEY_L = Sdl.SDLK_l;
    public static int KEY_M = Sdl.SDLK_m;
    public static int KEY_N = Sdl.SDLK_n;
    public static int KEY_O = Sdl.SDLK_o;
    public static int KEY_P = Sdl.SDLK_p;
    public static int KEY_Q = Sdl.SDLK_q;
    public static int KEY_R = Sdl.SDLK_r;
    public static int KEY_S = Sdl.SDLK_s;
    public static int KEY_T = Sdl.SDLK_t;
    public static int KEY_U = Sdl.SDLK_u;
    public static int KEY_V = Sdl.SDLK_v;
    public static int KEY_W = Sdl.SDLK_w;
    public static int KEY_X = Sdl.SDLK_x;
    public static int KEY_Y = Sdl.SDLK_y;
    public static int KEY_Z = Sdl.SDLK_z;
    public static int KEY_1 = Sdl.SDLK_1;
    public static int KEY_2 = Sdl.SDLK_2;
    public static int KEY_3 = Sdl.SDLK_3;
    public static int KEY_4 = Sdl.SDLK_4;
    public static int KEY_5 = Sdl.SDLK_5;
    public static int KEY_6 = Sdl.SDLK_6;
    public static int KEY_7 = Sdl.SDLK_7;
    public static int KEY_8 = Sdl.SDLK_8;
    public static int KEY_9 = Sdl.SDLK_9;
    public static int KEY_0 = Sdl.SDLK_0;
    public static int KEY_UP = Sdl.SDLK_UP;
    public static int KEY_DOWN = Sdl.SDLK_DOWN;
    public static int KEY_RIGHT = Sdl.SDLK_RIGHT;
    public static int KEY_LEFT = Sdl.SDLK_LEFT;
    public static int KEY_RETURN = Sdl.SDLK_RETURN;
}

﻿using System;
using System.Collections.Generic;
using System.IO;

public class Mapa
{
     public List<Arbol> Arboles { get; set; }
     public List<Edificio> Edificios { get; set; }
     public List<Hierba> Hierbas { get; set; }
     public List<Npc> Npcs { get; set; }
     public List<Pc> Pcs { get; set; }
     Random r;

    public Mapa()
    {
        Arboles = new List<Arbol>();
        Edificios = new List<Edificio>();
        Hierbas = new List<Hierba>();
        Npcs = new List<Npc>();
        Pcs = new List<Pc>();
        r = new Random();
    }

    private Npc CargarNpc()
    {
        Npc devolver = null;
        int numFrasesAnyadir = 3;
        try
        {
            string[] leer = File.ReadAllLines("data/npcs/lista_npc.txt");
            string[] leer2 = File.ReadAllLines("data/npcs/lista_frases.txt");

            int indice = r.Next(0, leer.Length);
            devolver = new Npc(new string[4][] {
                new string[] {leer[indice].Split(';')[0] },
                new string[] {leer[indice].Split(';')[1] },
                new string[] {leer[indice].Split(';')[2] },
                new string[] {leer[indice].Split(';')[3] } });

            for(int i = 0; i < numFrasesAnyadir; i++)
            {
                indice = r.Next(0, leer2.Length);
                if (!devolver.Dialogo.Contains(leer2[indice]))
                    devolver.Dialogo.Add(leer2[indice]);
            }
            
        }
        catch (Exception e)
        {
            Menu.Error();
        }
        return devolver;
    }

    public void CargarMapa(string nombre)
    {
        List<string> lineas;
        int actualX, actualY;

        try
        {
            lineas = new List<string>(File.ReadAllLines(nombre));
        }
        catch (Exception e)
        {
            lineas = new List<string>();
        }
        Sprite aux = new Sprite();

        for (int i = 0; i < lineas.Count; i++)
        {
            string linea = lineas[i];
            actualY = i * aux.height;

            for (int j = 0; j < linea.Length; j++)
            {
                actualX = j * aux.width;
                
                switch (linea[j])
                {
                    case 'A':
                        Arbol a = new Arbol("data/tree.png");
                        a.MoveTo(actualX, actualY);
                        Arboles.Add(a);
                        break;
                       
                    case 'E':
                        Edificio e = new Edificio("data/casa_amarilla.png");
                        e.MoveTo(actualX , actualY );
                        Edificios.Add(e);
                        break;

                    case 'H':
                        Hierba h = new Hierba("data/hierba_alta.png");
                        h.MoveTo(actualX, actualY);
                        Hierbas.Add(h);
                        break;

                    case 'N':
                        Npc n = CargarNpc();
                        n.MoveTo(actualX, actualY);
                        Npcs.Add(n);
                        break;

                    case 'F':
                        Enfermera ef = new Enfermera(
                            new string[4][] {
                            new string[] {"data/npcs/enfermera/enfermeraRightBase.png"},
                            new string[] {"data/npcs/enfermera/enfermeraLeftBase.png"},
                            new string[] {"data/npcs/enfermera/enfermeraUpBase.png"},
                            new string[] {"data/npcs/enfermera/enfermeraDownBase.png"}});
                        ef.Dialogo.Add("Tus pokemons han sido curados");
                        ef.MoveTo(actualX, actualY);
                        Npcs.Add(ef);
                        break;

                    case 'P':
                        Pc p = new Pc("data/pc.png");
                        p.MoveTo(actualX, actualY);
                        Pcs.Add(p);
                        break;
                }

            }
        }
    }
}﻿using System.IO;
using System;

class NuevaPartida : Menu
{
    bool nombreIntroducido;
    bool generoIntroducido;
    int indiceGenero;
    string nombrePJ,genero, errorNombre;
    Image bg2;
    Mapa cargarMapa;

    public NuevaPartida()
    {
        nombreIntroducido = false;
        generoIntroducido = false;
        bg = new Image("data/menu_partidas.png");
        bg2 = new Image("data/menu_seleccion_sprite.jpg");
        font24 = new Font("data/Joystix.ttf", 30);
        string[] leer = File.ReadAllLines("partidas/listaPartidas.txt");
        nombrePJ = " ";
        indiceGenero = 0;
        posicionFlecha = 300;
        genero = "Hombre";
        CargarNombres(ref listaNombres);
        errorNombre = "El nombre no puede estar vacio";
        cargarMapa = new Mapa();
        cargarMapa.CargarMapa("data/mapa.txt");
    }

    public void IntroducirTecla()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_A))
        {
            nombrePJ += "a";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_B))
        {
            nombrePJ += "b";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_C))
        {
            nombrePJ += "c";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_D))
        {
            nombrePJ += "d";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_E))
        {
            nombrePJ += "e";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_F))
        {
            nombrePJ += "f";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_G))
        {
            nombrePJ += "g";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_H))
        {
            nombrePJ += "h";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_I))
        {
            nombrePJ += "i";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_J))
        {
            nombrePJ += "j";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_K))
        {
            nombrePJ += "k";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_L))
        {
            nombrePJ += "l";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_M))
        {
            nombrePJ += "m";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_N))
        {
            nombrePJ += "n";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_O))
        {
            nombrePJ += "o";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_P))
        {
            nombrePJ += "p";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_Q))
        {
            nombrePJ += "q";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_R))
        {
            nombrePJ += "r";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_S))
        {
            nombrePJ += "s";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_T))
        {
            nombrePJ += "t";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_U))
        {
            nombrePJ += "u";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_V))
        {
            nombrePJ += "v";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_W))
        {
            nombrePJ += "w";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_X))
        {
            nombrePJ += "x";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_Y))
        {
            nombrePJ += "y";
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_Z))
        {
            nombrePJ += "z";
        }
        else if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE) && nombrePJ.Length > 1)
        {
            nombrePJ = nombrePJ.Substring(0, nombrePJ.Length - 1);
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            if (nombrePJ.Length == 1)
            {
                errorNombre = "El nombre no puede estar vacio";
            }
            else if (listaNombres.Contains(nombrePJ.Trim()))
            {
                errorNombre = "El nombre ya existe";
            }
            else
                nombreIntroducido = true;

            
        }
        SdlHardware.Pause(1);
    }

    public void CrearPartida()
    {
        nombrePJ = nombrePJ.Trim();
        Jugador nuevoJugador = new Jugador(nombrePJ, genero);
        try
        {
            string[] leer = File.ReadAllLines("data/pokemons/lista_pokemon.txt");
            int r = new Random().Next(1, leer.Length);
            nuevoJugador.GetEquipo().Add(new Bestia(leer[r].Split(';')[0], leer[r].Split(';')[1]));
            nuevoJugador.GetEquipo()[0].CargarAtaques();
        }
        catch (Exception e)
        {
            Menu.Error();
        }
        nuevoJugador.GetMochila().Add(new Pocion("Pocion", 50), 3);
        nuevoJugador.GetMochila().Add(new Pocion("SuperPocion", 100), 3);
        nuevoJugador.GetMochila().Add(new Pocion("HiperPocion", 150), 3);
        nuevoJugador.guardarJugador("partidas/" + nombrePJ + ".txt", ref fondo,
            ref dialogo, scrollX, scrollY);
        StreamWriter escribir = new StreamWriter("partidas/listaPartidas.txt", true);
        escribir.WriteLine(nombrePJ);
        escribir.Close();
        Juego j = new Juego(nuevoJugador, fondo, dialogo);
        j.Run();
    }

    public void IntroducirNombre()
    {
        do
        {
            if (!nombreIntroducido)
            {
                SdlHardware.ClearScreen();
                SdlHardware.DrawHiddenImage(bg, 0, 0);
                SdlHardware.WriteHiddenText("Introduce tu nombre. ",
                    100, 100,
                    0xC0, 0xC0, 0xC0,
                    font24);

                SdlHardware.WriteHiddenText("Pulsa espacio al terminar.",
                    50, 200,
                    0xC0, 0xC0, 0xC0,
                    font24);
                SdlHardware.WriteHiddenText(nombrePJ,
                    100, 300,
                    0xC0, 0xC0, 0xC0,
                    font24);
                SdlHardware.WriteHiddenText(errorNombre,
                    30, 400,
                    0xC0, 0xC0, 0xC0,
                    font24);
                SdlHardware.ShowHiddenScreen();
                IntroducirTecla();
            }
            SdlHardware.Pause(100);
        } while (!nombreIntroducido);
    }

    public void SeleccionGenero()
    {
        do
        {
            if (!generoIntroducido)
            {
                SdlHardware.ClearScreen();
                SdlHardware.DrawHiddenImage(bg2, 0, 0);
                SdlHardware.WriteHiddenText("<--",
                        500, posicionFlecha,
                        0xC0, 0xC0, 0xC0,
                        font24);
                SdlHardware.ShowHiddenScreen();
                if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
                {
                    if (indiceGenero == 0)
                        indiceGenero++;
                    else
                        indiceGenero = 0;
                    posicionFlecha = posicionFlecha == 300 ?
                        Convert.ToInt16(posicionFlecha + 250) : Convert.ToInt16(300);
                    SdlHardware.Pause(1);
                }
                if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
                {
                    if(indiceGenero == 1)
                        indiceGenero--;
                    else
                        indiceGenero = 1;
                    posicionFlecha = posicionFlecha == 550 ?
                        Convert.ToInt16(posicionFlecha - 250) : Convert.ToInt16(550);
                    SdlHardware.Pause(1);
                }
                if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
                {
                    generoIntroducido = true;
                    SdlHardware.Pause(1);
                }
                genero = indiceGenero == 0 ? "Hombre" : "Mujer";
            }
            SdlHardware.Pause(100);
        } while (!generoIntroducido);
    }

    public void Run()
    {
        IntroducirNombre();
        SeleccionGenero();
        CrearPartida();
    }
}
﻿using System;
using System.Collections.Generic;
using System.IO;

public class Bestia : Sprite
{
    protected string nombre;
    protected int nivel,vida,maxVida;
    List<Ataque.ataque> ataques;
    Random r = new Random();
    public Bestia(string nombre, string nombreImagen)
        : base (nombreImagen)
    {
        this.nombre = nombre;
        nivel = new Random().Next(1,100);
        vida = nivel * 2;
        maxVida = vida;
        width = 256;
        height = 282;
        ataques = new List<Ataque.ataque>();
    }

    public string GetNombre()
    {
        return nombre;
    }

    public List<Ataque.ataque> GetAtaques()
    {
        return ataques;
    }

    public void SetNombre(string nombre)
    {
        this.nombre = nombre;
    }

    public int GetNivel()
    {
        return nivel;
    }

    public void SetNivel(int nivel)
    {
        this.nivel = nivel;
    }

    public int GetVida()
    {
        return vida;
    }

    public void SetVida(int vida)
    {
        this.vida = vida;
    }

    public int GetMaxVida()
    {
        return maxVida;
    }

    public void SetMaxVida(int maxVida)
    {
        this.maxVida = maxVida;
    }

    public void CargarAtaques()
    {
        int numAtaquesMax = 4;
        try
        {
            string[] leer = File.ReadAllLines("data/pokemons/lista_ataques.txt");
            for (int i = 0; i < numAtaquesMax; i++)
            {
                int indiceRandom = r.Next(0, leer.Length);
                string[] cortarLinea = leer[indiceRandom].Split(';');
                bool control = true;
                foreach (Ataque.ataque a in this.ataques)
                {
                    if(a.nombre == cortarLinea[0])
                    {
                        control = false;
                    }
                }
                if(control)
                    this.ataques.Add(
                        new Ataque.ataque(cortarLinea[0],cortarLinea[1],
                        Convert.ToInt32(cortarLinea[2])));
            }
        }catch(Exception e)
        {
            Menu.Error();
        }
    }
}
﻿public class Objeto
{
    public string Nombre { get; set; }

    public Objeto(string Nombre)
    {
        this.Nombre = Nombre;
    }


}
﻿


using System.Collections.Generic;
/**
* Sprite.cs - A basic graphic element to inherit from
* 
* Changes:
* 0.01, 24-jul-2013: Initial version, based on SdlMuncher 0.12
*/
public class Sprite
{
    public int x { get; set; }
    public int y { get; set; }
    protected int startX, startY;
    public int width { get; set; }
    public int height { get; set; }
    protected int xSpeed, ySpeed;
    protected bool visible;
    public Image image;
    public Image[][] sequence;
    protected bool containsSequence;
    protected int currentFrame;

    protected byte numDirections = 4;
    public byte currentDirection { get; set; }
    public const byte RIGHT = 0;
    public const byte LEFT = 1;
    public const byte DOWN = 2;
    public const byte UP = 3;

    public Sprite()
    {
        startX = -1;
        startY = -1;
        width = 32;
        height = 32;
        visible = true;
        sequence = new Image[numDirections][];
        currentDirection = RIGHT;
    }

    public Sprite(string imageName)
        : this()
    {
        LoadImage(imageName);
    }

    public Sprite(string[] imageNames)
        : this()
    {
        LoadSequence(imageNames);
    }

    public void LoadImage(string name)
    {
        image = new Image(name);
        containsSequence = false;
    }

    public void LoadSequence(byte direction, string[] names)
    {
        int amountOfFrames = names.Length;
        sequence[direction] = new Image[amountOfFrames];
        for (int i = 0; i < amountOfFrames; i++)
            sequence[direction][i] = new Image(names[i]);
        containsSequence = true;
        currentFrame = 0;
    }

    public void LoadSequence(string[] names)
    {
        LoadSequence(RIGHT, names);
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetSpeedX()
    {
        return xSpeed;
    }

    public int GetSpeedY()
    {
        return ySpeed;
    }

    public bool IsVisible()
    {
        return visible;
    }

    public void MoveTo(int newX, int newY)
    {
        x = newX;
        y = newY;
        if (startX == -1)
        {
            startX = x;
            startY = y;
        }
    }

    public void SetSpeed(int newXSpeed, int newYSpeed)
    {
        xSpeed = newXSpeed;
        ySpeed = newYSpeed;
    }

    public void Show()
    {
        visible = true;
    }

    public void Hide()
    {
        visible = false;
    }

    public virtual void Move()
    {
        // To be redefined in subclasses
    }

    public void DrawOnHiddenScreen()
    {
        if (!visible)
            return;

        if (containsSequence)
            SdlHardware.DrawHiddenImage(
                sequence[currentDirection][currentFrame], x, y);
        else
            SdlHardware.DrawHiddenImage(image, x, y);
    }

    public void NextFrame()
    {
        currentFrame++;
        if (currentFrame >= sequence[currentDirection].Length)
            currentFrame = 0;
    }

    public void ChangeDirection(byte newDirection)
    {
        if (!containsSequence) return;
        if (currentDirection != newDirection)
        {
            currentDirection = newDirection;
            currentFrame = 0;
        }

    }

    public bool CollisionsWith(Sprite otherSprite)
    {
        return (visible && otherSprite.IsVisible() &&
            CollisionsWith(otherSprite.GetX(),
                otherSprite.GetY(),
                otherSprite.GetX() + otherSprite.GetWidth(),
                otherSprite.GetY() + otherSprite.GetHeight()));
    }

    public bool CollisionsWith(int xStart, int yStart, int xEnd, int yEnd)
    {
        if (visible &&
                (x < xEnd) &&
                (x + width > xStart) &&
                (y < yEnd) &&
                (y + height > yStart)
                )
            return true;
        return false;
    }

    public void Restart()
    {
        x = startX;
        y = startY;
    }


}


﻿using System;
using System.Collections.Generic;
using System.IO;

class Pokedex : Menu
{
    Jugador prota;
    Dictionary<int, string> pokedex;
    Dictionary<string, Image> dexImagenes;
    Font select;

    public Pokedex(Jugador prota)
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 25);
        select = new Font("data/Joystix.ttf", 40);
        continuar = true;
        this.prota = prota;
        seleccion = 0;
        pokedex = new Dictionary<int, string>();
        dexImagenes = new Dictionary<string, Image>();
        CargarPokedex();
    }

    private void CargarPokedex()
    {
        try
        {
            string[] leer = File.ReadAllLines("data/pokemons/lista_pokemon.txt");
            for(int i = 0; i < leer.Length; i++)
            {
                pokedex.Add(i,leer[i].Split(';')[0]);
                dexImagenes.Add(leer[i].Split(';')[0], new Image(leer[i].Split(';')[1]));
            }

        }
        catch (Exception e)
        {
            Menu.Error();
        }
    }

    public void DetectarTeclas()
    {
        if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE))
        {
            SdlHardware.Pause(100);
            continuar = false;
        }
    }

    public override void DibujarInterfaz()
    {
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        SdlHardware.WriteHiddenText("Pokedex",
            100, 50,
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.WriteHiddenText("Usa ARRIBA O ABAJO para desplazarte",
            100, 100,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText((seleccion + 1)+" de "+pokedex.Count,
            100, 150,
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.WriteHiddenText(pokedex[seleccion],
           100, 250,
           0xC0, 0xC0, 0xC0,
           select);
        SdlHardware.DrawHiddenImage(dexImagenes[pokedex[seleccion]],400, 250);

        SdlHardware.WriteHiddenText("Pulsa <-- para salir",
                100, 600,
                0xC0, 0xC0, 0xC0,
                font24);

        SdlHardware.ShowHiddenScreen();

        if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
        {
            if(seleccion == pokedex.Count - 1)
            {
                seleccion = 0;
            }
            else
            {
                seleccion++;
            }
        }
        if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
        {
            if (seleccion == 0)
            {
                seleccion = pokedex.Count - 1;
            }
            else
            {
                seleccion--;
            }
        }

        SdlHardware.Pause(40);


    }
    public void Run()
    {
        do
        {
            DibujarInterfaz();
            DetectarTeclas();
        } while (continuar);
    }
}
﻿using System;

class EquipoJugador : Menu
{
    Jugador prota;

    public EquipoJugador(Jugador prota)
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 30);
        continuar = true;
        this.prota = prota;
        posicionFlecha = 150;
    }

    public void DetectarTeclas()
    {
        if (SdlHardware.KeyPressed(Tao.Sdl.Sdl.SDLK_BACKSPACE))
        {
            SdlHardware.Pause(100);
            continuar = false;
        }
    }

    public override void DibujarInterfaz()
    {
        SdlHardware.DrawHiddenImage(bg,0,0);
        SdlHardware.WriteHiddenText("Equipo",
            100, 50,
            0xC0, 0xC0, 0xC0,
            font24);
        short altura = 150;
        foreach (Bestia b in prota.GetEquipo())
        {
            SdlHardware.WriteHiddenText(b.GetNombre(),
                        150, altura,
                        0xC0, 0xC0, 0xC0,
                        font24);
            altura += 50;
        }
        altura = 150;
        SdlHardware.WriteHiddenText("-->",
           40, Convert.ToInt16(posicionFlecha),
           0xC0, 0xC0, 0xC0,
           font24);
        SdlHardware.DrawHiddenImage(prota.GetEquipo()[seleccion].image,768,150);
        SdlHardware.WriteHiddenText("Lvl: " + prota.GetEquipo()[seleccion].GetNivel(),
           550, 150,
           0xC0, 0xC0, 0xC0,
           font24);
        SdlHardware.WriteHiddenText("Hp: " + prota.GetEquipo()[seleccion].GetVida(),
           550, 200,
           0xC0, 0xC0, 0xC0,
           font24);

        SdlHardware.WriteHiddenText("Pulsa <-- para salir",
                100, 450,
                0xC0, 0xC0, 0xC0,
                font24);

        SdlHardware.ShowHiddenScreen();

        if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
        {
            if (seleccion == prota.GetEquipo().Count - 1)
            {
                seleccion = 0;
                posicionFlecha = 150;
            }
            else
            {
                seleccion++;
                posicionFlecha += 50;
            }
        }
        if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
        {
            if (seleccion == 0)
            {
                seleccion = prota.GetEquipo().Count - 1;
                posicionFlecha = Convert.ToInt16(150 + ((prota.GetEquipo().Count - 1) * 50));
            }
            else
            {
                seleccion--;
                posicionFlecha -= 50;
            }
        }

        SdlHardware.Pause(40);


    }
    public void Run()
    {
        do
        {
            DibujarInterfaz();
            DetectarTeclas();
        } while (continuar);
    }
}﻿class Enfermera : Npc
{
    public Enfermera(string[][] direcciones)
        : base(direcciones)
    {

    }
}
﻿public class Pocion : Objeto
{
    public int hpRecuperados { get; set; }

    public Pocion(string nombre, int hpRecuperados)
        :base(nombre)
    {
        this.hpRecuperados = hpRecuperados;
    }
}
﻿class Bienvenida : Menu
{
    public Bienvenida()
    {
        bg = new Image("data/pantalla_principal.jpg");
        font24 = new Font("data/Joystix.ttf", 35);
    }

    public void Run()
    {
        Sound bgSound = new Sound("data/sonidos/pantalla_titulo.mp3");
        bgSound.BackgroundPlay();
        do
        {
            SdlHardware.ClearScreen();

            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("Pulsa Espacio para",
                100, 550,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Continuar",
                200, 600,
                0xC0, 0xC0, 0xC0,
                font24);

            SdlHardware.ShowHiddenScreen();

            SdlHardware.Pause(1);

        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        bgSound.StopMusic();
        SdlHardware.Pause(100);
    }
}


﻿using System;
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
﻿using System;

class MenuPartidas : Menu
{
    public MenuPartidas()
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 35);
        seleccion = 0;
        posicionFlecha = 100;
        maxOpciones = 2;
        continuar = true;
    }

    public override void DibujarInterfaz()
    {
        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        SdlHardware.WriteHiddenText("Nueva Partida",
            100, 100,
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.WriteHiddenText("Cargar Partida",
            100, 200,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Borrar Partida",
            100, 300,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("<--",
            600, Convert.ToInt16(posicionFlecha),
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.ShowHiddenScreen();
    }

    private void DetectarTeclas()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
        {
            if (seleccion == maxOpciones)
            {
                seleccion = 0;
                posicionFlecha = 100;
            }
            else
            {
                seleccion++;
                posicionFlecha += 100;
            }
        }
        if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
        {
            if (seleccion == 0)
            {
                seleccion = maxOpciones;
                posicionFlecha = Convert.ToInt16(100 + (maxOpciones * 100));
            }
            else
            {
                seleccion--;
                posicionFlecha -= 100;
            }
        }
        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            SdlHardware.Pause(100);
            switch (seleccion)
            {
                case 0: NuevaPartida np = new NuevaPartida(); np.Run(); break;
                case 1:
                    CargarPartida cp = new CargarPartida(); cp.Run()
                 ; break;
                case 2:
                    BorrarPartida bp = new BorrarPartida(); bp.Run()
                 ; break;
            }
        }
    }

    public void Run()
    {
        do
        {
            DibujarInterfaz();
            DetectarTeclas();
            SdlHardware.Pause(100);
        } while (continuar);
    }

}

