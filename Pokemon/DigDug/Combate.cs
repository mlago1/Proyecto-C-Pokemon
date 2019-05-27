using System;
using static Ataque;

class Combate : Menu
{
    Jugador prota, rival;
    Image fondo_opciones;
    Juego juego;
    Font font35;
    Bestia salvaje, seleccionado;
    bool turno, capturando;
    Random r;

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
    }

    public override void DibujarInterfaz()
    {
        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        DibujarInterfazSalvaje();
        DibujarInterfazSeleccionado();
        SdlHardware.DrawHiddenImage(fondo_opciones, 0, 534);
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
                    posicionFlecha = Convert.ToInt16(560 + ((seleccionado.GetAtaques().Count - 1) * 50));
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
            {
                cadenaPuntos += ". ";
            }
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
            prota.SetPokemonsDiferentesCapturados(prota.GetPokemonsDiferentesCapturados()+1);
        }
    }

    private void TuTurno()
    {
        do
        {
            DibujarInterfaz();
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
            case 2: Console.WriteLine("TO DO"); break;
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
        SdlHardware.ScrollTo(juego.nuevoScrollX, juego.nuevoScrollY);
    }
}
