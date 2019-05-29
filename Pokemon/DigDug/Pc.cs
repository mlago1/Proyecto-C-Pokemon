using System;

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
