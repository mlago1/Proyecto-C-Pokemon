using System;
using static Ataque;

class Combate : Menu
{
    Jugador prota, rival;
    Image fondo_opciones;
    Font font35;
    Bestia salvaje, seleccionado;
    bool turno;
    Random r;

    public Combate(Jugador prota, Bestia salvaje)
    {
        r = new Random();
        bg = new Image("data/fondo_combate.png");
        font24 = new Font("data/Joystix.ttf", 25);
        font35 = new Font("data/Joystix.ttf", 35);
        continuar = true;
        turno = true;
        posicionFlecha = 520;
        seleccion = 1;
        this.prota = prota;
        seleccionado = prota.GetEquipo()[0];
        this.salvaje = salvaje;
        salvaje.MoveTo(600, 70);
        seleccionado.MoveTo(100, 350);
        fondo_opciones = new Image("data/dialogo_combate.png");
    }

    public override void DibujarInterfaz()
    {
        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        DibujarInterfazSalvaje();
        DibujarInterfazSeleccionado(seleccionado);
        SdlHardware.DrawHiddenImage(fondo_opciones, 0, 500);
    }

    private void DibujarInterfazSalvaje()
    {
        salvaje.DrawOnHiddenScreen();
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

    private void DibujarInterfazSeleccionado(Bestia seleccionado)
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
            short altura = 520;
            foreach (ataque a in seleccionado.GetAtaques())
            {
                SdlHardware.WriteHiddenText(a.nombre,
                            150, altura,
                            0, 0, 0,
                            font24);
                altura += 50;
            }
            SdlHardware.ShowHiddenScreen();
            altura = 500;
        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        SdlHardware.Pause(100);
    }

    private void TuTurno()
    {
        do
        {
            DibujarInterfaz();
            SdlHardware.WriteHiddenText("Atacar",
                150, 520,
                0xC0, 0xC0, 0xC0,
                font24);

            SdlHardware.WriteHiddenText("Cambiar Pokemon",
                150, 570,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Mochila",
                150, 620,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Huir",
                150, 670,
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
                    posicionFlecha = 520;
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
                    posicionFlecha = 670;
                }
                else
                {
                    seleccion--;
                    posicionFlecha -= 50;
                }
            }
        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        SdlHardware.Pause(100);

        switch (seleccion)
        {
            case 0: TusAtaques(); break;
            case 3:
                continuar = false;
                SdlHardware.Pause(60); break;
                //TO DO
        }
    }

    private void RivalAtaca()
    {
        int indice = r.Next(0, salvaje.GetAtaques().Count);
        do
        {
            DibujarInterfaz();
            
            SdlHardware.WriteHiddenText(salvaje.GetNombre() + " enemigo usó: ",
                        100, 540,
                        0, 0, 0,
                        font35);
            SdlHardware.WriteHiddenText(salvaje.GetAtaques()[indice].nombre,
                        100, 570,
                        0, 0, 0,
                        font35);
            SdlHardware.ShowHiddenScreen();
        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        SdlHardware.Pause(100);
    }

    public void Run()
    {
        do
        {
            if (turno)
            {
                TuTurno();
                SdlHardware.ShowHiddenScreen();
            }
            else
            {
                RivalAtaca();
                SdlHardware.ShowHiddenScreen();
            }
            turno = turno ? false : true;
            SdlHardware.Pause(40);
        } while (continuar);
    }
}
